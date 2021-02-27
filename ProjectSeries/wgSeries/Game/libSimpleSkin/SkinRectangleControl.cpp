#include "StdAfx.h"
#include "SkinRectangleControl.h"

CSkinRectangleControl::CSkinRectangleControl(void)
{
	m_crLine = RGB(0, 0, 0);
	m_crBrush = RGB(255, 255, 255);
	m_bHollow = FALSE;
	m_sizeRound.cx = 0;
	m_sizeRound.cy = 0;
	m_nLineWidth = 1;
	m_nGridUse = 0;
}

CSkinRectangleControl::~CSkinRectangleControl(void)
{
}

void CSkinRectangleControl::SetLineColor(COLORREF crLine)
{
	if(m_crLine == crLine)
		return;
	m_crLine = crLine;
	Invalidate();
}

void CSkinRectangleControl::SetLineWidth(int nWidth)
{
	if(m_nLineWidth == nWidth)
		return;

	m_nLineWidth = nWidth;
	Invalidate();
}

void CSkinRectangleControl::SetBrushColor(COLORREF crBrush)
{
	if(m_crBrush == crBrush)
		return;
	m_crBrush = crBrush;
	Invalidate();
}

void CSkinRectangleControl::SetHollowBrush(BOOL bHollow)
{
	if(m_bHollow == bHollow)
		return;
	m_bHollow = bHollow;
	Invalidate();
}

void CSkinRectangleControl::DrawControl(HDC hDC)
{
	HBRUSH hBrush = NULL;
	HBRUSH hOldBrush = NULL;
	if(!m_bHollow)
	{
		hBrush = CreateSolidBrush(m_crBrush);
		hOldBrush = (HBRUSH)SelectObject(hDC, hBrush);
	}

	HPEN hPen = NULL;
	hPen = CreatePen(PS_SOLID, m_nLineWidth, m_crLine);
	HPEN hOldPen = (HPEN)SelectObject(hDC, hPen);

	int nLeftTopPenWidth = m_nLineWidth >> 1;
	int nRightBottomPenWidth = (m_nLineWidth - 1) >> 1;
	
	if(m_nGridUse == 0)
	{
		if(m_sizeRound.cx == m_sizeRound.cy == 0)
		{
			Rectangle(hDC, nLeftTopPenWidth, nLeftTopPenWidth,
				m_rtControl.right - m_rtControl.left - nRightBottomPenWidth,
				m_rtControl.bottom - m_rtControl.top - nRightBottomPenWidth);
		} else
		{
			RoundRect(hDC, nLeftTopPenWidth, nLeftTopPenWidth,
				m_rtControl.right - m_rtControl.left - nRightBottomPenWidth,
				m_rtControl.bottom - m_rtControl.top - nRightBottomPenWidth,
				m_sizeRound.cx, m_sizeRound.cy);
		}
	}

	SelectObject(hDC, hOldPen);
	DeleteObject(hPen);
	if(hOldBrush)
	{
		SelectObject(hDC, hOldBrush);
		DeleteObject(hOldBrush);
	}
	if(hBrush)
		DeleteObject(hBrush);

}

void CSkinRectangleControl::SetRoundSize(int cx, int cy)
{
	if(m_sizeRound.cx == cx && m_sizeRound.cy == cy)
		return;

	m_sizeRound.cx = cx;
	m_sizeRound.cy = cy;
	Invalidate();
}

BOOL CSkinRectangleControl::ApplyXML(XMLNS::IXMLDOMNodePtr spNode)
{
//	<rectangle line_color='000000' brush_color='ffffff' hollow='y'
//		round_width='5' round_height='5' line_width='2'/>
	if(!CSkinDraggableControl::ApplyXML(spNode))
		return FALSE;

	BOOL bValue;
	COLORREF crValue;
	int nValue;
	XMLNS::IXMLDOMNodePtr spAttr;

	// line_color
	spAttr = spNode->attributes->getNamedItem(L"line_color");
	if(spAttr)
	{
		if(!StringToColor(spAttr->Gettext(), &crValue))
			return FALSE;
		SetLineColor(crValue);
	}

	// brush_color
	spAttr = spNode->attributes->getNamedItem(L"brush_color");
	if(spAttr)
	{
		if(!StringToColor(spAttr->Gettext(), &crValue))
			return FALSE;
		SetBrushColor(crValue);
	}

	// hollow
	spAttr = spNode->attributes->getNamedItem(L"hollow");
	if(spAttr)
	{
		if(!StringToBOOL(spAttr->Gettext(), &bValue))
			return FALSE;
		SetHollowBrush(bValue);
	}

	// round_width
	int nRoundWidth = 0;
	spAttr = spNode->attributes->getNamedItem(L"round_width");
	if(spAttr)
	{
		if(!StringToInt(spAttr->Gettext(), &nValue))
			return FALSE;
		nRoundWidth = nValue;
	}

	// round_height
	int nRoundHeight = 0;
	spAttr = spNode->attributes->getNamedItem(L"round_height");
	if(spAttr)
	{
		if(!StringToInt(spAttr->Gettext(), &nValue))
			return FALSE;
		nRoundHeight = nValue;
	}
	SetRoundSize(nRoundWidth, nRoundHeight);

	// round_height
	spAttr = spNode->attributes->getNamedItem(L"line_width");
	if(spAttr)
	{
		if(!StringToInt(spAttr->Gettext(), &nValue))
			return FALSE;
		SetLineWidth(nValue);
	}

	// grid use
	spAttr = spNode->attributes->getNamedItem(L"griduse");
	if(spAttr)
	{
		if(!StringToInt(spAttr->Gettext(), &nValue))
			return FALSE;
		m_nGridUse = nValue;
	}
	return TRUE;
}