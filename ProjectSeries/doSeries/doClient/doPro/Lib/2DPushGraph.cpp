/* ===================================================================

C2DPushGraph Control (2DPushGraph.h and 2DPushGraph.cpp)

Author:  Stuart Konen
Contact: skonen@gmail.com (Job information welcome)

Description: A push graph control similiar to the graph control located
in Microsoft's Task Manager.

====================================================================*/


#include "stdafx.h"
#include "2DPushGraph.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define PUSHGRAPH_MAX(num, max, type) \
((num > max) ? (type)(max) : (type)(num))

#define PUSHGRAPH_MIN(num, min, type) \
((num < min) ? (type)(min) : (type)(num))


/* Self-Register */
BOOL C2DPushGraph::m_bRegistered = C2DPushGraph::RegisterClass(); 


/////////////////////////////////////////////////////////////////////////////
// C2DPushGraph

C2DPushGraph::C2DPushGraph()
{
	m_nMaxCoords       = -1;
	m_nMoveOffset      = 0;
	m_nPeekOffset      = 0;
	m_bStylesModified  = false;
	m_usLineInterval   = 2;

	SetPeekRange(0, 100);
	
	SetLabelForMax("100%");
	SetLabelForMin("0%");

	SetGridSize(15);	
	

	SetBGColor  ( RGB(0, 0, 0)     );
	SetGridColor( RGB(0, 150, 0)   );
	SetTextColor( RGB(255, 255, 0) );	
}


// ===================================================================

C2DPushGraph::~C2DPushGraph()
{
	while (m_aLines.GetSize())
	{
		delete m_aLines[0];
		m_aLines.RemoveAt(0);
	}
}


// ===================================================================

BOOL C2DPushGraph::RegisterClass()
{
	/* Static function to automatically register this class */

	WNDCLASS wc;
	
	ZeroMemory(&wc, sizeof(WNDCLASS));
                                 
    wc.lpfnWndProc   = ::DefWindowProc;
    wc.hInstance     = (HINSTANCE)::GetModuleHandle(NULL);        
    wc.hbrBackground = (HBRUSH)(COLOR_BTNFACE + 1);                
    wc.lpszClassName = _T("C2DPushGraph");	

    return AfxRegisterClass(&wc);
}


BEGIN_MESSAGE_MAP(C2DPushGraph, CWnd)
	//{{AFX_MSG_MAP(C2DPushGraph)
	ON_WM_ERASEBKGND()
	ON_WM_PAINT()
	ON_WM_SIZING()
	ON_WM_SIZE()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()


// ===================================================================

bool C2DPushGraph::CreateFromStatic( UINT nStaticID, CWnd* pParent )
{
	CStatic  wndStatic;

	if (pParent == NULL || !wndStatic.SubclassDlgItem(nStaticID, pParent))
	{
		return false;
	}
	

	/* Get the static windows rectangle and transform it into
	parent client coordinates, instead of screen coordinates */

	CRect    rectStatic;

	wndStatic.GetWindowRect(&rectStatic);
	pParent->ScreenToClient(&rectStatic);

		
	if ( !CreateEx( wndStatic.GetExStyle(), 
		            NULL, NULL, WS_CHILD | WS_VISIBLE | wndStatic.GetStyle(),
		            rectStatic, pParent, nStaticID, NULL))
	{		
		wndStatic.DestroyWindow();
		return false;
	}
	
	
	wndStatic.DestroyWindow();
	return true;
}


// ==========================================================================
// Functions for Setting and Retrieving attributes
// ==========================================================================


COLORREF C2DPushGraph::GetGridColor() const
{
	return m_crGridColor;
}


// ===================================================================

void C2DPushGraph::SetGridColor( COLORREF crColor )
{
	m_crGridColor = crColor;
}


// ===================================================================

COLORREF C2DPushGraph::GetTextColor() const
{
	return m_crTextColor;
}


// ===================================================================

void  C2DPushGraph::SetTextColor( COLORREF crColor )
{
	m_crTextColor = crColor;
}


// ===================================================================

COLORREF C2DPushGraph::GetBGColor() const
{
	return m_crBGColor;
}


// ===================================================================

void C2DPushGraph::SetBGColor( COLORREF crColor )
{
	m_crBGColor = crColor;
}


// ===================================================================

int C2DPushGraph::GetGridSize() const
{
	return m_nGridSize;
}


// ===================================================================

void C2DPushGraph::SetGridSize( unsigned short usWidthAndHeight )
{
	m_nGridSize = PUSHGRAPH_MIN(usWidthAndHeight, 3, int);
}


// ===================================================================

int C2DPushGraph::GetMaxPeek() const
{
	return m_nMaxPeek;
}


// ===================================================================

void C2DPushGraph::SetMaxPeek( int nMax )
{
	m_nMaxPeek = nMax;
}


// ===================================================================

LPCTSTR C2DPushGraph::GetLabelForMax() const
{
	 return (LPCTSTR)m_strMaxLabel;
}


// ===================================================================

void C2DPushGraph::SetLabelForMax( LPCTSTR lpszLabel )
{
	 m_strMaxLabel = lpszLabel;
}


// ===================================================================

int C2DPushGraph::GetMinPeek() const
{
	return m_nMinPeek;
}


// ===================================================================

void C2DPushGraph::SetMinPeek( int nMin )
{	
	if (nMin < 0)
	{
		m_nPeekOffset = 0 - nMin;		
	}
	else
	{
		m_nPeekOffset = 0;
	}

	m_nMinPeek = nMin;
}


// ===================================================================

LPCTSTR C2DPushGraph::GetLabelForMin() const
{
	 return (LPCTSTR)m_strMinLabel;
}


// ===================================================================

void C2DPushGraph::SetLabelForMin( LPCTSTR lpszLabel )
{
	 m_strMinLabel = lpszLabel;
}


// ===================================================================

void C2DPushGraph::SetPeekRange( int nMin, int nMax )
{
	ASSERT(nMin < nMax);

	SetMinPeek(nMin);
	SetMaxPeek(nMax);
}


// ===================================================================

unsigned short C2DPushGraph::GetInterval() const
{
	return m_usLineInterval;
}


// ===================================================================

void C2DPushGraph::SetInterval( unsigned short usInterval )
{
	m_usLineInterval = usInterval;
	m_nMaxCoords = -1; // Forces reset
}


// ===================================================================

bool C2DPushGraph::AddLine( UINT uiLineID, COLORREF crColor )
{
	PushGraphLine *pNewLine = new PushGraphLine(uiLineID);

	if (!pNewLine)
	{
		ASSERT(pNewLine && "Out of memory");
		AfxMessageBox("Memory Over");
		return false;
	}

	pNewLine->crLine = crColor;
	m_aLines.Add(pNewLine);

	return true;
}


// ===================================================================

void C2DPushGraph::RemoveLine( UINT uiLineID )
{
	for (int n = m_aLines.GetSize()-1; n >= 0; --n)
	{
		if (m_aLines[n]->uiID == uiLineID)
		{
			delete m_aLines[n];
			m_aLines.RemoveAt(n);

			--n; // Because we removed a line
		}
	}	
}


// ===================================================================

bool C2DPushGraph::SetLineColor( COLORREF crColor, UINT uiLineID )
{
	PushGraphLine *pLine = internal_LineFromID(uiLineID);
	
	if (pLine == NULL)
	{
		return false; 
	}
	
	pLine->crLine = crColor;
	return true;
}


// ===================================================================

COLORREF C2DPushGraph::GetLineColor( UINT uiLineID )
{
	PushGraphLine *pLine = internal_LineFromID(uiLineID);
	return (pLine) ? pLine->crLine : RGB(0, 0, 0);
}


// ===================================================================

bool C2DPushGraph::Push( int nMagnitude, UINT uiLineID )
{
	PushGraphLine *pLine = internal_LineFromID(uiLineID);
	
	if (pLine == NULL)
	{
		return false; 
	}

	
	/* Now add the magnitude (push point) to the array of push points, but
	first restrict it to the peek bounds */

	if (nMagnitude > m_nMaxPeek)
	{
		nMagnitude = m_nMaxPeek;
	}
	else if (nMagnitude < m_nMinPeek)
	{
		nMagnitude = m_nMinPeek;
	}
	
	nMagnitude -= m_nMinPeek;
	nMagnitude += m_nPeekOffset;
	
	pLine->aMagnitudes.Add(nMagnitude);
	return true;
}


// ===================================================================

void C2DPushGraph::ShowAsBar( UINT uiLineID, bool bAsBar )
{
	PushGraphLine *pLine = internal_LineFromID(uiLineID);
	
	if (pLine)
	{
		pLine->bShowAsBar = bAsBar;
	}
}


// ===================================================================

void C2DPushGraph::Update()
{
	int nGreatest = 0;  // Largest push point count
	
	
	for (int n=0; n < m_aLines.GetSize(); ++n)
	{
		if (nGreatest < m_aLines[n]->aMagnitudes.GetSize())
		{
			nGreatest = m_aLines[n]->aMagnitudes.GetSize();
		}
	}

	if (nGreatest >= m_nMaxCoords)
	{
		m_nMoveOffset = 
			(m_nMoveOffset - (((nGreatest-m_nMaxCoords)+1)*m_usLineInterval))
			% m_nGridSize;	
	}

	RedrawWindow();
}


// ===================================================================

void C2DPushGraph::OnPaint() 
{	
	CPaintDC paintDC(this);	
	

	/* First we create the back buffer */
	CDC &dc = internal_InitBackBuffer(paintDC);
	

	/* Ensure we don't draw out of our client rectangle */
	
	CRect &rectClient = getClientRect();
	dc.IntersectClipRect(&rectClient);	
	

	/* Fill the background */
	dc.FillSolidRect(&rectClient, m_crBGColor);


	dc.SetBkMode(TRANSPARENT);
	


	if (m_bShowMinMax)
	{
		/* Show maximum and minimum labels */

		internal_DrawMinMax(dc, rectClient);
	}

	if (m_bShowGrid)
	{
		/* Show the grid overlay */

		internal_DrawGrid(dc, rectClient);
	}


	internal_DrawLines(dc, rectClient);
	internal_FreeBackBuffer(paintDC);	


	if (m_bStylesModified)
	{
		/* Reset styles if resized */

		GetParent()->ModifyStyle(WS_CLIPCHILDREN, 0);
		ModifyStyle(WS_CLIPSIBLINGS, 0);
		m_bStylesModified = false;
	}
//	ReleaseDC(&dc);			// CDC release
//	ReleaseDC(&paintDC);	// PDC release
}


// ===================================================================

CDC& C2DPushGraph::internal_InitBackBuffer(CPaintDC &dc)
{
	/* Create the offscreen DC and associated bitmap */

	m_dcBack.CreateCompatibleDC(&dc);
	m_bmBack.CreateCompatibleBitmap(&dc, getClientRect().Width(),
	         getClientRect().Height());

	m_pOldBitmap = m_dcBack.SelectObject(&m_bmBack);
	return m_dcBack;
}


// ===================================================================

void C2DPushGraph::internal_FreeBackBuffer(CPaintDC &dc)
{
	/* Copy the offscreen buffer to the onscreen CPaintDC.
	Then free the back buffer objects. */

	dc.BitBlt(getClientRect().left, getClientRect().top, 
              getClientRect().Width(),  getClientRect().Height(),
              &m_dcBack, getClientRect().left, getClientRect().top,
			  SRCCOPY);

	dc.SelectObject(m_pOldBitmap);

	m_bmBack.DeleteObject();
	m_dcBack.DeleteDC();
}


// ===================================================================

PushGraphLine* C2DPushGraph::internal_LineFromID( UINT uiLineID )
{
	/* Find the corresponding line to the passed ID */

	for (int n = m_aLines.GetSize()-1; n >= 0; --n)
	{
		if (m_aLines[n]->uiID == uiLineID)
		{
			return m_aLines[n];			
		}
	}

	return NULL;
}


// ===================================================================

void C2DPushGraph::internal_DrawMinMax( CDC &dc, CRect& rect)
{

	CSize MaxSize = dc.GetTextExtent(m_strMaxLabel);
	CSize MinSize = dc.GetTextExtent(m_strMinLabel);
	int CapSize = (m_strCaptionName.GetLength()*13) + 20;

	int nTextWidth = 
		((MaxSize.cx > MinSize.cx) ? MaxSize.cx : MinSize.cx) + CapSize + 6;

	dc.SetTextColor(RGB(150,150,150));											// CaptionColor
	CFont newFont, *oldFont;
	newFont.CreateFont(20,10,0,0, FW_DONTCARE, FALSE, FALSE, FALSE, DEFAULT_CHARSET, OUT_DEFAULT_PRECIS,
						CLIP_DEFAULT_PRECIS, DEFAULT_QUALITY, DEFAULT_PITCH, "HY¿±¼­M");
	oldFont = dc.SelectObject(&newFont);
	dc.TextOut(2, 2, m_strCaptionName);
	dc.SetTextColor(RGB(230, 230, 230));											// PercentColor
	dc.TextOut(2, rect.Height()-MinSize.cy-10, m_strPercent);
	dc.SelectObject(oldFont);
	newFont.DeleteObject();

	// DataName
	dc.SetTextColor(m_crTextColor);

	dc.TextOut(nTextWidth/2-(MaxSize.cx/2) + CapSize/2 + 6, 2, m_strMaxLabel);	// MaxName
	dc.TextOut(nTextWidth/2-(MinSize.cx/2) + CapSize/2 + 6,						// MinName
		rect.Height()-MinSize.cy-2, m_strMinLabel);

	/* Draw the bordering line */

	CPen penBorder(PS_SOLID, 1, m_crGridColor);
	CPen *pOldPen = dc.SelectObject(&penBorder);

	dc.MoveTo(nTextWidth + 6, 0);
	dc.LineTo(nTextWidth + 6, rect.Height());


	dc.SelectObject(pOldPen);
	penBorder.DeleteObject();


	/* Offset the graph rectangle so it doesn't overlap the labels */

	rect.left = nTextWidth + 6;
}


// ===================================================================

void C2DPushGraph::internal_DrawLines( CDC &dc, CRect& rect)
{
	CPen penLine;
	CPen *pOldPen = NULL;

	int nGreatest = 0;
	

	if (m_nMaxCoords == -1)
	{
		/* Maximum push points not yet calculated */

		m_nMaxCoords = (rect.Width() / m_usLineInterval) + 2
		               + (rect.Width()%m_usLineInterval ? 1 : 0);

		if (m_nMaxCoords <= 0)
		{
			m_nMaxCoords = 1;
		}
	}


	for (int n=0; n < m_aLines.GetSize(); ++n)
	{		
		if (nGreatest < m_aLines[n]->aMagnitudes.GetSize())
		{
			nGreatest = m_aLines[n]->aMagnitudes.GetSize();
		}
	}

	if (nGreatest == 0)
	{		
		return; // No lines to draw
	}

	for (int n = 0; n < m_aLines.GetSize(); ++n)
	{
		/* If the line has less push points than the line with the greatest
		number of push points, new push points are appended with
		the same magnitude as the previous push point. If no push points
		exist for the line, one is added with the least magnitude possible. */
		
		PushGraphLine *pLine = m_aLines[n];
			
		if (!pLine->aMagnitudes.GetSize())
		{
			pLine->aMagnitudes.Add(m_nMinPeek);
		}

		while (pLine->aMagnitudes.GetSize() < nGreatest)
		{
			pLine->aMagnitudes.Add(
				pLine->aMagnitudes[pLine->aMagnitudes.GetSize()-1]);
		}


		while (m_aLines[n]->aMagnitudes.GetSize() >= m_nMaxCoords)
		{
			m_aLines[n]->aMagnitudes.RemoveAt(0);
		}

		if (!m_aLines[n]->aMagnitudes.GetSize())
		{
			/* No push points to draw */
			return;			
		}


		/* Now prepare to draw the line or bar */

		penLine.CreatePen(PS_SOLID, 1, m_aLines[n]->crLine);
		pOldPen = dc.SelectObject(&penLine);


		if (pLine->bShowAsBar)
		{
			dc.MoveTo(rect.left, rect.Height());
		}
		else
		{
			dc.MoveTo(rect.left, nGreatest == 1 ? rect.Height() : 
		    rect.Height()-(pLine->aMagnitudes[0] * 
		    rect.Height()/(m_nMaxPeek-m_nMinPeek)));	
		}
		

		for (int n2 = 0; n2 < pLine->aMagnitudes.GetSize(); ++n2)
		{
			if (pLine->bShowAsBar)
			{

				/* The line is set to be shown as a bar graph, so
				first we get the bars rectangle, then draw the bar */

				CRect rectBar;

				rectBar.left   = rect.left + (n2*(m_usLineInterval)) + 1;
				rectBar.right  = rectBar.left + GetInterval() - 1;
				rectBar.bottom = rect.Height();
				rectBar.top    = rect.Height() -
				                 (pLine->aMagnitudes[n2] * rect.Height() /
				                 (m_nMaxPeek-m_nMinPeek));				

				internal_DrawBar(dc, rectBar, *pLine);
			}

			else
			{
				/* Draw a line */

				dc.LineTo(rect.left + (n2*m_usLineInterval),
				rect.Height() -
				(pLine->aMagnitudes[n2] * rect.Height() /
				(m_nMaxPeek-m_nMinPeek)) );

				/*
				dc.FillSolidRect(rect.left + (n2*m_usLineInterval) - 2,
					rect.Height() -
				(pLine->aMagnitudes[n2] * rect.Height() /
				(m_nMaxPeek-m_nMinPeek)) - 2, 4, 4, m_aLines[n]->crLine); */

			}

		}

		dc.SelectObject(pOldPen);	
		penLine.DeleteObject(); // Free Pen 
	}

}


// ===================================================================

void C2DPushGraph::internal_DrawBar( CDC &dc, CRect& rect, 
                                     PushGraphLine& rLine )
{
	COLORREF &crFill = rLine.crLine;
	GraphColor gcTopLeft, gcBottomRight;
		
	
	/* gcTopLeft is the left and top frame color, gcBottomRight
	is the right and bottom frame color */	


	if (GetRValue(crFill))
	{
		gcTopLeft.bRed     = PUSHGRAPH_MAX(GetRValue(crFill)+40, 255, BYTE);
		gcBottomRight.bRed = PUSHGRAPH_MIN(GetRValue(crFill) - 40, 0, BYTE);
	}

	if (GetBValue(crFill))
	{
		gcTopLeft.bBlue      = PUSHGRAPH_MAX(GetBValue(crFill)+40, 255, BYTE);
		gcBottomRight.bBlue  = PUSHGRAPH_MIN(GetBValue(crFill) - 40, 0, BYTE);
	}

	if (GetGValue(crFill))
	{
		gcTopLeft.bGreen     = PUSHGRAPH_MAX(GetGValue(crFill)+40, 255, BYTE);
		gcBottomRight.bGreen = PUSHGRAPH_MIN(GetGValue(crFill) - 40, 0, BYTE);
	}
			 

	dc.FillSolidRect(&rect, rLine.crLine);
	dc.Draw3dRect(&rect, gcTopLeft, gcBottomRight);
}


// ===================================================================

void C2DPushGraph::internal_DrawGrid( CDC &dc, CRect& rect)
{
	/* Draw the grid overlay. 
	
	We use rect.left as our x offset instead of zero because
	if m_bShowMinMax is true, then rect.left is set to the 
	first pixel after the labels. */

	CPen GridPen( PS_SOLID, 1, m_crGridColor );
	CPen *pOldPen = dc.SelectObject(&GridPen);


	for (int n = rect.Height()-1; n >= 0; n -= m_nGridSize)
	{
		dc.MoveTo(rect.left, n);
		dc.LineTo(rect.right, n);
	}
		
	for (int n = rect.left + m_nMoveOffset; n < rect.right; n += m_nGridSize)
	{
		if (n < rect.left)
		{
			continue;
		}

		dc.MoveTo(n, 0);
		dc.LineTo(n, rect.Height());
	}

	dc.SelectObject(pOldPen);
	GridPen.DeleteObject();
}


// ===================================================================

CRect& C2DPushGraph::getClientRect()
{
	static CRect rectClient;
	return (GetClientRect(&rectClient), rectClient);
}


// ===================================================================

BOOL C2DPushGraph::OnEraseBkgnd(CDC*) 
{
	return FALSE;
}


// ===================================================================

void C2DPushGraph::OnSizing(UINT fwSide, LPRECT pRect) 
{

	if (GetParent()->GetStyle() & ~WS_CLIPCHILDREN)
	{
		/* Eliminate flickering */

		m_bStylesModified = true;
		GetParent()->ModifyStyle(0, WS_CLIPCHILDREN);		
	}

	ModifyStyle(0, WS_CLIPSIBLINGS);


	CWnd::OnSizing(fwSide, pRect);		
	(m_nMaxCoords = -1, Invalidate());	
}


// ===================================================================

void C2DPushGraph::OnSize(UINT nType, int cx, int cy) 
{
	CWnd::OnSize(nType, cx, cy);	
	(m_nMaxCoords = -1, Invalidate());
}

// Caption User Making
void C2DPushGraph::SetCaption(LPCTSTR lpszLabel)
{
	m_strCaptionName = lpszLabel;
}
// Caption User Making
LPCTSTR C2DPushGraph::GetCaption() const
{
	return m_strCaptionName;
}

void C2DPushGraph::SetPercent(LPSTR lpszPercent)
{
	m_strPercent = lpszPercent;
}

LPCTSTR C2DPushGraph::GetPercent() const
{
	return m_strPercent;
}
