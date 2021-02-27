#include "StdAfx.h"
#include "SkinWindow.h"
#include "commctrl.h"
#include "SkinImageControl.h"

CSkinWindow::CSkinWindow(void)
{
	m_hWnd = NULL;
	m_OldProc = NULL;
	m_bDoubleBuffered = FALSE;
	m_pCapturedControl = NULL;
	m_hToolTipWnd = NULL;
	m_bSupportDoubleClick = FALSE;
	m_pOverControl = NULL;
	m_bMouseEnter = FALSE;
	m_bUseDefaultPaint = TRUE;
	m_bClientSize = FALSE;
	m_nMinWidth = -1;
	m_nMinHeight = -1;
	m_nMaxWidth = -1;
	m_nMaxHeight = -1;
	m_bRedraw = TRUE;
	m_bClipChildren = FALSE;
}

CSkinWindow::~CSkinWindow(void)
{
	Detach();
}

HWND CSkinWindow::Attach(HWND hWnd, BOOL bRelocate)
{
	HWND hRetWnd = Detach();

	m_hWnd = hWnd;
	m_OldProc = (WNDPROC)GetWindowLong(hWnd, GWL_WNDPROC);
	SetProp(hWnd, _T("sswptr"), (HANDLE)this);

	RECT rtClient;
	RECT rtWindow;

	GetClientRect(hWnd, &rtClient);
	GetWindowRect(hWnd, &rtWindow);
	int nBorderWidth = 0;
	int nBorderHeight = 0;
	if(m_bClientSize)
	{
		nBorderWidth = (rtWindow.right - rtWindow.left) - rtClient.right;
		nBorderHeight = (rtWindow.bottom - rtWindow.top) - rtClient.bottom;
		m_rtControl.right += nBorderWidth;
		m_rtControl.bottom += nBorderHeight;
	}

	if(IsChildWindow())
	{
		if(bRelocate)
		{
			RECT rtControl;
			GetAbsoluteRect(&rtControl);
			SetWindowPos(hWnd, NULL, rtControl.left, rtControl.top,
				rtControl.right - rtControl.left, rtControl.bottom - rtControl.top, SWP_NOZORDER);
		} else
		{
			POINT pt = {rtWindow.left, rtWindow.top};
			ScreenToClient(::GetParent(hWnd), &pt);
			m_rtControl = rtWindow;
			OffsetRect(&m_rtControl, -pt.x, -pt.y);

			CSkinControl * pControl = GetParent();
			if(pControl && !pControl->IsWindowControl())
			{
				RECT rtParent;
				pControl->GetAbsoluteRect(&rtParent);
				OffsetRect(&m_rtControl, -rtParent.left, -rtParent.top);
			}				
		}
	} else
	{
		SetUseDefaultPaint(FALSE);
		SetControlRect(&rtWindow);
	}

	SetClipChildren(m_bClipChildren);
	SetSupportDoubleClick(m_bSupportDoubleClick);
	
	SetWindowLong(hWnd, GWL_WNDPROC, (LONG)InternalWndProc);
	SetConstraints(m_nMinWidth, m_nMinHeight, m_nMaxWidth, m_nMaxHeight);

	m_hToolTipWnd = CreateWindowEx(WS_EX_TOPMOST, TOOLTIPS_CLASS, NULL,
			WS_POPUP | TTS_NOPREFIX | TTS_ALWAYSTIP, CW_USEDEFAULT,
			CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, NULL, NULL,
			GetModuleHandle(NULL), NULL);

	TrackMouse(TME_LEAVE);

	CSkinControl * pControl = m_pFirstChild;
	while(pControl)
	{
		NotifyParentWindowAttach(this, pControl);
		pControl = pControl->GetNextSibling();
	}

	return hRetWnd;
}

CSkinWindow * CSkinWindow::AddDlgCtlItem(int nDlgCtlID, int nAnchors)
{
	if(m_hWnd == NULL)
		return NULL;

	HWND hWnd = GetDlgItem(m_hWnd, nDlgCtlID);
	if(hWnd == NULL)
		return NULL;

	CSkinWindow * pWindow = new CSkinWindow();
	pWindow->SetAnchors(nAnchors);
	pWindow->SetAutoDelete(TRUE);
	pWindow->Attach(hWnd, FALSE);
	AddChildControl(pWindow);
	return pWindow;
}

void CSkinWindow::NotifyParentWindowAttach(CSkinWindow * pWindow, CSkinControl * pControl)
{
	if(pControl->IsWindowControl())
		return;
	pControl->OnParentWindowAttached(pWindow);

	CSkinControl * pChildControl = pControl->GetFirstChild();
	while(pChildControl)
	{
		NotifyParentWindowAttach(pWindow, pChildControl);
		pChildControl = pChildControl->GetNextSibling();
	}
}

HWND CSkinWindow::Detach()
{
	if(m_hWnd == NULL)
		return NULL;

	RemoveProp(m_hWnd, _T("sswptr"));
	SetWindowLong(m_hWnd, GWL_WNDPROC, (LONG)m_OldProc);
	HWND hRet = m_hWnd;
	m_hWnd = NULL;

	DeleteToolTip();
	DestroyWindow(m_hToolTipWnd);
	m_hToolTipWnd = NULL;

	return hRet;
}

HWND CSkinWindow::GetHWND()
{
	return m_hWnd;
}

LRESULT __stdcall CSkinWindow::InternalWndProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	CSkinWindow * pWnd = (CSkinWindow *)GetProp(hWnd, _T("sswptr"));
	WNDPROC OldProc = pWnd->m_OldProc;

	BOOL bHandled = FALSE;
	LRESULT lRet = pWnd->WndProc(uMsg, wParam, lParam, bHandled);
	if(bHandled)
		return lRet;
	return CallWindowProc(OldProc, hWnd, uMsg, wParam, lParam);
}

LRESULT CSkinWindow::WndProc(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL & bHandled)
{
	switch(uMsg)
	{
	case WM_PAINT:
		return OnPaint(wParam, lParam, bHandled);

	case WM_WINDOWPOSCHANGED:
		return OnWindowPosChanged(wParam, lParam, bHandled);

	case WM_SIZE:
		return OnSize(wParam, lParam, bHandled);

	case WM_GETMINMAXINFO:
		return OnGetMinMaxInfo(wParam, lParam, bHandled);

	case WM_DESTROY:
		Detach();
		break;

	case WM_ERASEBKGND:
		return OnEraseBkGnd(wParam, lParam, bHandled);

	case WM_MOUSELEAVE:
		return OnMouseLeave(wParam, lParam, bHandled);

	default:
		if(uMsg >= WM_MOUSEFIRST && uMsg <= WM_MOUSELAST)
			return OnMouseMessage(uMsg, wParam, lParam, bHandled);
		break;
	}

	return 0;
}

void CSkinWindow::SetRedraw(BOOL bRedraw)
{
	m_bRedraw = bRedraw;
}

BOOL CSkinWindow::GetRedraw()
{
	return m_bRedraw;
}


LRESULT CSkinWindow::OnPaint(WPARAM wParam, LPARAM lParam, BOOL & bHandled)
{
	if(m_bUseDefaultPaint)
		return 0;

	PAINTSTRUCT ps;
	HDC hDC = BeginPaint(m_hWnd, &ps);

	if(m_bRedraw)
	{
		RECT rtUpdate;
		if(GetUpdateRect(m_hWnd, &rtUpdate, FALSE) == FALSE)
			GetClientRect(m_hWnd, &rtUpdate);

		HDC hDrawDC = hDC;
		HDC hMemDC = NULL;
		HBITMAP hBitmap = NULL;
		HBITMAP hOldBitmap = NULL;
		int nOffsetX = 0;
		int nOffsetY = 0;
		int cx = rtUpdate.right - rtUpdate.left;
		int cy = rtUpdate.bottom - rtUpdate.top;

		if(m_bDoubleBuffered)
		{
			hMemDC = CreateCompatibleDC(hDC);
			hBitmap = CreateCompatibleBitmap(hDC, cx, cy);
			hOldBitmap = (HBITMAP)SelectObject(hMemDC, hBitmap);
			hDrawDC = hMemDC;
			nOffsetX = rtUpdate.left;
			nOffsetY = rtUpdate.top;
		}

		DrawChildControls(hDrawDC, &rtUpdate, nOffsetX, nOffsetY);

		if(m_bDoubleBuffered)
		{
			BitBlt(hDC, rtUpdate.left, rtUpdate.top, cx, cy, hMemDC, 0, 0, SRCCOPY);
			SelectObject(hMemDC, hOldBitmap);
			DeleteObject(hBitmap);
			DeleteDC(hMemDC);
		}

	}
	EndPaint(m_hWnd, &ps);

	bHandled = TRUE;
	return 0;
}

void CSkinWindow::DrawChildControls(HDC hDC, RECT * prtUpdate, int nOffsetX, int nOffsetY)
{
	RECT rtControl;
	RECT rtIntersect;
	CSkinControl * pControl = m_pFirstChild;
	while(pControl)
	{
		if(!pControl->IsWindowControl() && pControl->GetVisible())
		{
			pControl->GetAbsoluteRect(&rtControl);
			if(IntersectRect(&rtIntersect, prtUpdate, &rtControl))
			{
				RECT rtParent;
				RECT rtClip;

				CSkinControl * pParent = pControl->GetParent();
				if(pParent->IsWindowControl() && ((CSkinWindow *)pParent)->IsChildWindow() == FALSE)
					GetClientRect(((CSkinWindow *)pParent)->GetHWND(), &rtParent);
				else
					pParent->GetAbsoluteRect(&rtParent);

				if(IntersectRect(&rtClip, &rtParent, &rtControl))
				{
					HRGN hRgn = NULL;
					hRgn = CreateRectRgn(rtClip.left, rtClip.top, rtClip.right, rtClip.bottom);
					SelectClipRgn(hDC, hRgn);
					DeleteObject(hRgn);
					SetViewportOrgEx(hDC, rtControl.left - nOffsetX, rtControl.top - nOffsetY, NULL);
					pControl->DrawControl(hDC);
					if(hRgn)
						SelectClipRgn(hDC, NULL);
				}
			}
		}
		pControl = GetNextControl(pControl);
	}
	SetViewportOrgEx(hDC, 0, 0, NULL);
}

LRESULT CSkinWindow::OnWindowPosChanged(WPARAM wParam, LPARAM lParam, BOOL & bHandled)
{
	WINDOWPOS * pPos = (WINDOWPOS *)lParam;
	
	RECT rtWindow = {pPos->x, pPos->y, pPos->x + pPos->cx, pPos->y + pPos->cy};
	if(IsChildWindow())
	{
		RECT rtControl;
		GetAbsoluteRect(&rtControl);

		int nOffsetX = rtControl.left - m_rtControl.left;
		int nOffsetY = rtControl.top - m_rtControl.top;

		OffsetRect(&rtWindow, -nOffsetX, -nOffsetY);
	}

	SetControlRect(&rtWindow);

	return 0;
}

LRESULT CSkinWindow::OnSize(WPARAM wParam, LPARAM lParam, BOOL & bHandled)
{
	return 0;
}

void CSkinWindow::SetDoubleBuffered(BOOL bDoubleBuffered)
{
	if(m_bDoubleBuffered == bDoubleBuffered)
		return;

	m_bDoubleBuffered = bDoubleBuffered;
	if(m_hWnd)
		InvalidateRect(m_hWnd, NULL, !m_bDoubleBuffered);
}

LRESULT CSkinWindow::OnEraseBkGnd(WPARAM wParam, LPARAM lParam, BOOL & bHandled)
{
	if(m_bDoubleBuffered)
	{
		bHandled = TRUE;
		return 0;
	}
	return 0;
}

LRESULT CSkinWindow::OnMouseLeave(WPARAM wParam, LPARAM lParam, BOOL & bHandled)
{
	m_bMouseEnter = FALSE;
	SetOverControl(NULL);
	return 0;
}

LRESULT CSkinWindow::OnGetMinMaxInfo(WPARAM wParam, LPARAM lParam, BOOL & bHandled)
{
	MINMAXINFO * pmmi = (MINMAXINFO *)lParam;
	if(m_nMinWidth > 0)
		pmmi->ptMinTrackSize.x = m_nMinWidth;
	if(m_nMinHeight > 0)
		pmmi->ptMinTrackSize.y = m_nMinHeight;
	if(m_nMaxWidth > 0)
		pmmi->ptMaxTrackSize.x = m_nMaxWidth;
	if(m_nMaxHeight > 0)
		pmmi->ptMaxTrackSize.y = m_nMaxHeight;

	bHandled = TRUE;
	return 0;
}

BOOL CSkinWindow::IsChildWindow()
{
	if(m_hWnd == NULL)
		return TRUE;

	return (GetWindowLong(m_hWnd, GWL_STYLE) & WS_CHILD) != 0;
}


void CSkinWindow::SetClipChildren(BOOL bClipChildrent)
{
	if(m_hWnd)
	{
		LONG lStyle = GetWindowLong(m_hWnd, GWL_STYLE) & (~WS_CLIPCHILDREN);
		if(m_bClipChildren)
			lStyle |= WS_CLIPCHILDREN;

		SetWindowLong(m_hWnd, GWL_STYLE, lStyle);
	}
	m_bClipChildren = bClipChildrent;
}

CSkinControl * CSkinWindow::SetControlCapture(CSkinControl * pControl)
{
	CSkinControl * pRet = m_pCapturedControl;
	SetCapture(m_hWnd);
	m_pCapturedControl = pControl;
	return pRet;
}

CSkinControl * CSkinWindow::GetControlCapture()
{
	return m_pCapturedControl;
}

BOOL CSkinWindow::ReleaseControlCapture()
{
	if(m_pCapturedControl)
	{
		m_pCapturedControl = NULL;
		return ReleaseCapture();
	}

	return FALSE;
}

LRESULT CSkinWindow::OnMouseMessage(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL & bHandled)
{
	POINT pt = {(short)LOWORD(lParam), (short)HIWORD(lParam)};
	CSkinControl * pControl;
	if(m_pCapturedControl)
		pControl = m_pCapturedControl;
	else
		pControl = GetControlFromPoint(pt, TRUE);

	if(uMsg == WM_MOUSEMOVE && pControl != m_pOverControl)
	{
		if(!m_bMouseEnter)
		{
			m_bMouseEnter = TRUE;
			TrackMouse(TME_LEAVE);
		}

		SetOverControl(pControl);
		if(pControl == NULL)
			DeleteToolTip();
		else
			AddToolTip(pControl);
	}

	if(pControl)
	{
		RECT rtControl;
		pControl->GetControlRect(&rtControl);
		WORD wLowWord = (WORD)(pt.x - rtControl.left);
		WORD wHiWord = (WORD)(pt.y - rtControl.top);
		LPARAM lParamControl = (DWORD)wLowWord | ((DWORD)wHiWord << 16);
		return pControl->NotifyMessage(uMsg, wParam, lParamControl, bHandled);
	}
	
	return 0;
}

void CSkinWindow::AddToolTip(LPCTSTR pszToolTip, RECT * prtToolTip)
{
	if(m_hToolTipWnd == NULL)
		return;

	DeleteToolTip();

	TOOLINFO ti;
	ti.cbSize = sizeof(TOOLINFO);
	ti.uFlags = TTF_SUBCLASS;
	ti.hwnd = m_hWnd;
	ti.hinst = GetModuleHandle(NULL);
	ti.uId = 1;
	ti.lpszText = (LPTSTR)pszToolTip;
	if(prtToolTip == NULL)
		GetClientRect(m_hWnd, &ti.rect);
	else
		ti.rect = *prtToolTip;
	SendMessage(m_hToolTipWnd, TTM_ADDTOOL, 0, (LPARAM)(LPTOOLINFO)&ti);
}

void CSkinWindow::AddToolTip(CSkinControl * pControl)
{
	RECT rtControl;
	pControl->GetAbsoluteRect(&rtControl);
	AddToolTip(pControl->GetToolTip(), &rtControl);
}

void CSkinWindow::DeleteToolTip()
{
	if(m_hToolTipWnd == NULL)
		return;

	TOOLINFO ti;
	memset(&ti, 0, sizeof(ti));
	ti.cbSize = sizeof(TOOLINFO);
	ti.hwnd = m_hWnd;
	ti.uId = 1;
	if(SendMessage(m_hToolTipWnd, TTM_GETTOOLCOUNT, 0, 0))
		SendMessage(m_hToolTipWnd, TTM_DELTOOL, 0, (LPARAM)(LPTOOLINFO)&ti);
}

void CSkinWindow::SetOverControl(CSkinControl * pControl)
{
	if(m_pOverControl != NULL)
		m_pOverControl->OnMouseLeave();
	m_pOverControl = pControl;
	if(m_pOverControl != NULL)
		m_pOverControl->OnMouseEnter();
}

void CSkinWindow::SetSupportDoubleClick(BOOL bSupport)
{
	if(m_hWnd)
	{
		LONG lStyle = GetClassLong(m_hWnd, GCL_STYLE) & (~CS_DBLCLKS);
		if(bSupport)
			lStyle |= CS_DBLCLKS;
		::SetClassLong(m_hWnd, GCL_STYLE, lStyle);
	}
	m_bSupportDoubleClick = bSupport;
}

void CSkinWindow::TrackMouse(DWORD dwFlag)
{
	TRACKMOUSEEVENT evt;
	evt.cbSize = sizeof(evt);
	evt.dwFlags = dwFlag;
	evt.dwHoverTime = 0;
	evt.hwndTrack = m_hWnd;
	TrackMouseEvent(&evt);
}

void CSkinWindow::SetConstraints(int nMinWidth, int nMinHeight, int nMaxWidth, int nMaxHeight)
{
	m_nMinWidth = nMinWidth;
	m_nMinHeight = nMinHeight;
	m_nMaxWidth = nMaxWidth;
	m_nMaxHeight = nMaxHeight;

	if(m_hWnd)
	{
		int nWidth = m_rtControl.right - m_rtControl.left;
		int nHeight = m_rtControl.bottom - m_rtControl.top;

		RECT rtNewControl = m_rtControl;
		if(m_nMinWidth > 0 && nWidth < m_nMinWidth)
			rtNewControl.right = rtNewControl.left + m_nMinWidth;
		if(m_nMinHeight > 0 && nHeight < m_nMinHeight)
			rtNewControl.bottom = rtNewControl.top + m_nMinHeight;
		if(m_nMaxWidth > 0 && nWidth > m_nMaxWidth)
			rtNewControl.right = rtNewControl.left + m_nMaxWidth;
		if(m_nMaxHeight > 0 && nHeight > m_nMaxHeight)
			rtNewControl.bottom = rtNewControl.top + m_nMaxHeight;

		SetControlRect(&rtNewControl);
	}
}

void CSkinWindow::GetConstraints(RECT * prtConstraints)
{
	SetRect(prtConstraints, m_nMinWidth, m_nMinHeight, m_nMaxWidth, m_nMaxHeight);
}

void CSkinWindow::SetUseDefaultPaint(BOOL bDefault)
{
	if(m_bUseDefaultPaint == bDefault)
		return;
	m_bUseDefaultPaint = bDefault;
	Invalidate();
}

BOOL CSkinWindow::ApplyXML(XMLNS::IXMLDOMNodePtr spNode)
{
	// <window double_click='y' double_buffering='y' use_default_paint='n'
	//		client_width='500' client_height='400' support_mask='n'/>
	if(!CSkinControl::ApplyXML(spNode))
		return FALSE;

	// double_click
	STRING szFileName;
	XMLNS::IXMLDOMNodePtr spAttr;
	BOOL bValue;
	spAttr = spNode->attributes->getNamedItem(L"double_click");
	if(spAttr)
	{
		if(!StringToBOOL(spAttr->Gettext(), &bValue))
			return FALSE;
		SetSupportDoubleClick(bValue);
	}

	spAttr = spNode->attributes->getNamedItem(L"double_buffering");
	if(spAttr)
	{
		if(!StringToBOOL(spAttr->Gettext(), &bValue))
			return FALSE;
		SetDoubleBuffered(bValue);
	}

	spAttr = spNode->attributes->getNamedItem(L"use_default_paint");
	if(spAttr)
	{
		if(!StringToBOOL(spAttr->Gettext(), &bValue))
			return FALSE;
		SetUseDefaultPaint(bValue);
	}

	spAttr = spNode->attributes->getNamedItem(L"client_size");
	if(spAttr)
	{
		if(!StringToBOOL(spAttr->Gettext(), &bValue))
			return FALSE;
		m_bClientSize = bValue;
	}

	int nMinWidth = -1;
	int nMinHeight = -1;
	int nMaxWidth = -1;
	int nMaxHeight = -1;
	spAttr = spNode->attributes->getNamedItem(L"min_width");
	if(spAttr)
	{
		if(!StringToInt(spAttr->Gettext(), &nMinWidth))
			return FALSE;
	}
	spAttr = spNode->attributes->getNamedItem(L"min_height");
	if(spAttr)
	{
		if(!StringToInt(spAttr->Gettext(), &nMinHeight))
			return FALSE;
	}
	spAttr = spNode->attributes->getNamedItem(L"max_width");
	if(spAttr)
	{
		if(!StringToInt(spAttr->Gettext(), &nMaxWidth))
			return FALSE;
	}
	spAttr = spNode->attributes->getNamedItem(L"max_height");
	if(spAttr)
	{
		if(!StringToInt(spAttr->Gettext(), &nMaxHeight))
			return FALSE;
	}
	SetConstraints(nMinWidth, nMinHeight, nMaxWidth, nMaxHeight);

	spAttr = spNode->attributes->getNamedItem("clip_children");
	if(spAttr)
	{
		if(!StringToBOOL(spAttr->Gettext(), &bValue))
			return FALSE;
		SetClipChildren(bValue);
	}

	return TRUE;

}
