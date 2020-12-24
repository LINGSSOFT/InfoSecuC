#include "StdAfx.h"
#include "SkinControl.h"
#include "SkinWindow.h"
#include "SkinImageControl.h"
#include "SkinFileReader.h"

CSkinControl::CSkinControl(void)
{
	memset(&m_rtControl, 0, sizeof(RECT));
	m_pData = NULL;
	m_pParent = NULL;
	m_nChildCount = 0;
	m_pFirstChild = NULL;
	m_pLastChild = NULL;
	m_pNextSibling = NULL;
	m_pPrevSibling = NULL;
	m_bAutoDelete = FALSE;
	m_nAnchors = ANK_LEFT | ANK_TOP;
	m_bEnabled = TRUE;
	m_pEvents = NULL;
	m_pszToolTip = NULL;
	m_pszName = NULL;
	m_pSkinFileReader = NULL;
	m_bVisible = TRUE;
}

CSkinControl::~CSkinControl(void)
{
	RemoveAllChildren();
	if(m_pszToolTip)
		delete []m_pszToolTip;
	if(m_pszName)
		delete []m_pszName;
}

void CSkinControl::DrawControl(HDC hDC)
{
	int nWidth = m_rtControl.right - m_rtControl.left;
	int nHeight = m_rtControl.bottom - m_rtControl.top;
	Rectangle(hDC, 0, 0, nWidth, nHeight);
}

void CSkinControl::SetAutoDelete(BOOL bAutoDelete)
{
	m_bAutoDelete = bAutoDelete;
}

BOOL CSkinControl::GetAutoDelete()
{
	return m_bAutoDelete;
}

int CSkinControl::GetChildCount()
{
	return m_nChildCount;
}

void CSkinControl::AddChildControl(CSkinControl * pControl)
{
	InsertChildControl(m_pLastChild, pControl);
}

void CSkinControl::InsertChildControl(CSkinControl * pPrevControl, CSkinControl * pControl)
{
	if(pControl->m_pParent == this)
		return;

	pControl->m_pParent = this;
	if(pPrevControl)
	{
		if(pPrevControl->m_pNextSibling) // between
		{
			pControl->m_pNextSibling = pPrevControl->m_pNextSibling;
			pPrevControl->m_pNextSibling->m_pPrevSibling = pControl;
		} else // Last
		{
			pControl->m_pNextSibling = NULL;
			m_pLastChild = pControl;
		}
		pControl->m_pPrevSibling = pPrevControl;
		pPrevControl->m_pNextSibling = pControl;
	} else // First
	{
		if(m_pFirstChild)
		{
			pControl->m_pNextSibling = m_pFirstChild;
			m_pFirstChild->m_pPrevSibling = pControl;
		} else // Empty
		{
			m_pLastChild = pControl;
			pControl->m_pNextSibling = NULL;
		}
		m_pFirstChild = pControl;
		pControl->m_pPrevSibling = NULL;
	}
}

void CSkinControl::RemoveChildControl(CSkinControl * pControl)
{
	if(pControl->m_pParent != this)
		return;

	if(pControl->m_pPrevSibling)
	{
		if(pControl->m_pNextSibling) // between
		{
			pControl->m_pPrevSibling->m_pNextSibling = pControl->m_pNextSibling;
			pControl->m_pNextSibling->m_pPrevSibling = pControl->m_pPrevSibling;
		} else // Last
		{
			pControl->m_pPrevSibling->m_pNextSibling = NULL;
			m_pLastChild = pControl->m_pPrevSibling;
		}
	} else // First
	{
		if(pControl->m_pNextSibling)
		{
			pControl->m_pNextSibling->m_pPrevSibling = NULL;
			m_pFirstChild = pControl->m_pNextSibling;
		} else // Empty
		{
			m_pFirstChild = m_pLastChild = NULL;
		}
	}
	pControl->m_pParent = NULL;
	pControl->m_pPrevSibling = NULL;
	pControl->m_pNextSibling = NULL;
	m_nChildCount--;

	if(pControl->m_bAutoDelete)
		delete pControl;
}

CSkinControl * CSkinControl::GetFirstChild()
{
	return m_pFirstChild;
}

CSkinControl * CSkinControl::GetLastChild()
{
	return m_pLastChild;
}

CSkinControl * CSkinControl::GetNextSibling()
{
	return m_pNextSibling;
}

CSkinControl * CSkinControl::GetPrevSibling()
{
	return m_pPrevSibling;
}

CSkinControl * CSkinControl::FindControl(LPCTSTR pszName, CSkinControl * pAfter)
{
	if(m_pFirstChild == NULL)
		return NULL;

	CSkinControl * pNext;
	if(pAfter)
		pNext = GetNextControl(pAfter);
	else
		pNext = m_pFirstChild;

	while(pNext)
	{
		if(pNext == this)
			return NULL;

		if(_tcscmp(pNext->GetName(), pszName) == 0)
			return pNext;

		pNext = GetNextControl(pNext);
	}
	return NULL;
}

CSkinControl * CSkinControl::GetParent()
{
	return m_pParent;
}

CSkinWindow * CSkinControl::GetParentWindow()
{
	CSkinControl * pParent = m_pParent;
	while(pParent)
	{
		if(pParent->IsWindowControl())
			return (CSkinWindow *)pParent;
		pParent = pParent->GetParent();
	}
	return NULL;
}

CSkinControl * CSkinControl::GetNextControl(CSkinControl * pControl)
{
	CSkinControl * pNext = pControl->GetFirstChild();
	if(pNext)
		return pNext;
	pNext = pControl->GetNextSibling();
	if(pNext)
		return pNext;
	pNext = pControl->GetParent();
	if(pNext == NULL)
		return NULL;
	pNext = pNext->GetNextSibling();
	return pNext;
}

CSkinControl * CSkinControl::GetControlFromPoint(POINT pt, BOOL bEnableOnly)
{
	CSkinControl * pRet = NULL;
	CSkinControl * pControl = m_pLastChild;
	RECT rtControl;
	while(pControl)
	{
		pControl->GetControlRect(&rtControl);
		if(PtInRect(&rtControl, pt) && (!bEnableOnly || pControl->m_bEnabled) && pControl->m_bVisible)
		{
			POINT ptControl = {pt.x - rtControl.left, pt.y - rtControl.top};
			pRet = pControl->GetControlFromPoint(ptControl, bEnableOnly);
			if(pRet == NULL)
				pRet = pControl;
			break;
		}
		pControl = pControl->GetPrevSibling();
	}
	return pRet;
}

void CSkinControl::SetControlRect(const RECT * prt)
{
	if(EqualRect(&m_rtControl, prt))
		return;

	int cx = prt->right - prt->left;
	int cy = prt->bottom - prt->top;
	int old_cx = m_rtControl.right - m_rtControl.left;
	int old_cy = m_rtControl.bottom - m_rtControl.top;
	int dx = cx - old_cx;
	int dy = cy - old_cy;

	Invalidate();
	m_rtControl = *prt;
	Invalidate();

	if(IsWindowControl())
	{
		// Window의 경우 OnSize에서 Child의 크기 조절..
		CSkinWindow * pWindow = (CSkinWindow *)this;
		HWND hWnd = pWindow->GetHWND();
		if(hWnd != NULL)
		{
			RECT rtWindow;
			GetAbsoluteRect(&rtWindow);
			SetWindowPos(hWnd, NULL, rtWindow.left, rtWindow.top, cx, cy, SWP_NOZORDER);
		}
	}

	CSkinControl * pControl = m_pFirstChild;
	while(pControl)
	{
		pControl->ApplyAnchors(dx, dy);
		pControl = pControl->m_pNextSibling;
	}
}

void CSkinControl::GetControlRect(RECT * prt)
{
	*prt = m_rtControl;
}

void CSkinControl::GetAbsoluteRect(RECT * prt)
{
	*prt = m_rtControl;
	RECT rtParent;
	CSkinControl * pParent = m_pParent;
	while(pParent)
	{
		if(pParent->IsWindowControl())
			break;
		pParent->GetControlRect(&rtParent);
		OffsetRect(prt, rtParent.left, rtParent.top);
		pParent = pParent->GetParent();
	}
}

void CSkinControl::ApplyAnchors(int dx, int dy)
{
	RECT rtControl = m_rtControl;

	if((m_nAnchors & ANK_LEFT) != 0)
	{
		if((m_nAnchors & ANK_RIGHT) != 0)
			rtControl.right += dx;
	} else
	{
		if((m_nAnchors & ANK_RIGHT) != 0)
			OffsetRect(&rtControl, dx, 0);
		else
			OffsetRect(&rtControl, dx / 2, 0);
	}

	if((m_nAnchors & ANK_TOP) != 0)
	{
		if((m_nAnchors & ANK_BOTTOM) != 0)
			rtControl.bottom += dy;
	} else
	{
		if((m_nAnchors & ANK_BOTTOM) != 0)
			OffsetRect(&rtControl, 0, dy);
		else
			OffsetRect(&rtControl, 0, dy / 2);
	}

	SetControlRect(&rtControl);
}

int CSkinControl::GetAnchors()
{
	return m_nAnchors;
}

void CSkinControl::SetAnchors(int nAnchors)
{
	if(m_nAnchors == nAnchors)
		return;
	m_nAnchors = nAnchors;
	ApplyAnchors(0, 0);
}


void * CSkinControl::GetData()
{
	return m_pData;
}

void CSkinControl::SetData(void * pData)
{
	m_pData = pData;
}

void CSkinControl::Invalidate()
{
	CSkinWindow * pWindow = GetParentWindow();
	if(pWindow == NULL)
		return;

	HWND hWnd = pWindow->GetHWND();
	if(hWnd == NULL)
		return;

	RECT rtControl;
	GetAbsoluteRect(&rtControl);
	InvalidateRect(hWnd, &rtControl, TRUE);
}

void CSkinControl::RemoveAllChildren()
{
	CSkinControl * pNext = m_pFirstChild;
	CSkinControl * pControl;

	while(pNext)
	{
		pControl = pNext;
		pNext = pNext->m_pNextSibling;
		RemoveChildControl(pControl);
	}
}

BOOL CSkinControl::IsWindowControl()
{
	return IsKindOf(_T("CSkinWindow")) && ((CSkinWindow *)this)->GetHWND() != NULL;
}

BOOL CSkinControl::GetEnabled()
{
	return m_bEnabled;
}

void CSkinControl::SetEnabled(BOOL bEnabled)
{
	if(m_bEnabled == bEnabled)
		return;

	m_bEnabled = bEnabled;
	Invalidate();
}

BOOL CSkinControl::GetVisible()
{
	return m_bVisible;
}

void CSkinControl::SetVisible(BOOL bVisible)
{
	if(m_bVisible == bVisible)
		return;

	m_bVisible = bVisible;
	Invalidate();
}

LRESULT CSkinControl::NotifyMessage(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL & bHandled)
{
	if(m_pEvents)
		return m_pEvents->OnSkinControlMessage(this, uMsg, wParam, lParam);

	return 0;
}

void CSkinControl::SetEvents(ISkinControlEvents * pEvents)
{
	m_pEvents = pEvents;
}

void CSkinControl::SetToolTip(LPCTSTR pszToolTip)
{
	if(m_pszToolTip)
		delete []m_pszToolTip;

	size_t nLen = _tcslen(pszToolTip) + 1; 
	m_pszToolTip = new TCHAR[nLen];
	_tcscpy_s(m_pszToolTip, nLen, pszToolTip);
}

LPCTSTR CSkinControl::GetToolTip()
{
	if(m_pszToolTip)
		return m_pszToolTip;
	else
		return _T("");
}

void CSkinControl::SetName(LPCTSTR pszName)
{
	if(m_pszName)
		delete m_pszName;
	size_t size = _tcslen(pszName) + 1;
	m_pszName = new TCHAR[size];
	_tcscpy_s(m_pszName, size, pszName);
}

LPCTSTR CSkinControl::GetName()
{
	if(m_pszName)
		return m_pszName;
	return _T("");
}

CSkinFileReader * CSkinControl::GetSkinFileReader()
{
	return m_pSkinFileReader;
}

void CSkinControl::SetSkinFileReader(CSkinFileReader * pReader)
{
	m_pSkinFileReader = pReader;
}

BOOL CSkinControl::ApplyXML(XMLNS::IXMLDOMNodePtr spNode)
{
	XMLNS::IXMLDOMNodePtr spAttr;
	RECT rtControl = {0, 0, 0, 0};

	spAttr = spNode->attributes->getNamedItem(L"name");
	if(spAttr)
		SetName(spAttr->Gettext());

	spAttr = spNode->attributes->getNamedItem(L"left");
	if(spAttr)
		rtControl.left = rtControl.right = _wtoi(spAttr->Gettext());

	spAttr = spNode->attributes->getNamedItem(L"top");
	if(spAttr)
		rtControl.top = rtControl.bottom = _wtoi(spAttr->Gettext());

	spAttr = spNode->attributes->getNamedItem(L"width");
	if(spAttr)
		rtControl.right = rtControl.left + _wtoi(spAttr->Gettext());

	spAttr = spNode->attributes->getNamedItem(L"height");
	if(spAttr)
		rtControl.bottom = rtControl.top + _wtoi(spAttr->Gettext());

	spAttr = spNode->attributes->getNamedItem(L"tooltip");
	if(spAttr)
		SetToolTip(spAttr->Gettext());

	SetControlRect(&rtControl);

	spAttr = spNode->attributes->getNamedItem(L"anchors");
	if(spAttr)
	{
		int nAnchors;
		if(!StringToAnchors(spAttr->Gettext(), &nAnchors))
			return FALSE;
		SetAnchors(nAnchors);
	}

	return TRUE;
}

BOOL CSkinControl::StringToAnchors(LPCTSTR pszAnchors, int * pValue)
{
	int nRet = 0;
	int nLen = (int)_tcslen(pszAnchors);
	for(int i = 0; i < nLen; i++)
	{
		switch(pszAnchors[i])
		{
		case 'l':
		case 'L':
			nRet |= ANK_LEFT;
			break;
		case 't':
		case 'T':
			nRet |= ANK_TOP;
			break;
		case 'r':
		case 'R':
			nRet |= ANK_RIGHT;
			break;
		case 'b':
		case 'B':
			nRet |= ANK_BOTTOM;
			break;

		default:
			return FALSE;
		}
	}
	*pValue = nRet;
	return TRUE;
}

BOOL CSkinControl::StringToBOOL(LPCTSTR pszBool, BOOL * pValue)
{
	if(_tcslen(pszBool) != 1)
		return FALSE;

	if(pszBool[0] == 'y' || pszBool[0] == 'Y')
	{
		*pValue = TRUE;
		return TRUE;
	}

	if(pszBool[0] == 'n' || pszBool[0] == 'N')
	{
		*pValue = FALSE;
		return TRUE;
	}

	return FALSE;
}

BOOL CSkinControl::StringToInt(LPCTSTR pszInt, int * pValue)
{
	*pValue = _ttoi(pszInt);
	return TRUE;
}

BOOL CSkinControl::StringToColor(LPCTSTR pszColor, COLORREF * pValue)
{
	if(_tcslen(pszColor) != 6)
		return FALSE;
	
	COLORREF crRet = 0;
	BYTE n;
	TCHAR c;

	for(int i = 0; i < 3; i++)
	{
		n = 0;
		for(int j = 0; j < 2; j++)
		{
			c = pszColor[i * 2 + j];
			if(isdigit(c))
				c -= '0';
			else if(c >= 'A' && c <= 'F')
				c -= ('A' - 10);
			else if(c >= 'a' && c <= 'f')
				c -= ('a' - 10);
			else
				return FALSE;
			n = (n << 4) | c;
		}
		crRet |= (n << (i << 3));
	}
	*pValue = crRet;
	return TRUE;
}

BOOL CSkinControl::StringToFontStyle(LPCTSTR pszStyle, DWORD * pValue)
{
	DWORD dwRet = 0;
	TCHAR * pszDup = _tcsdup(pszStyle);
	TCHAR * pszContext = NULL;
	TCHAR * pszToken = _tcstok_s(pszDup, _T(" ,"), &pszContext);
	while(pszToken)
	{
		if(_tcsicmp(pszToken, _T("bold")) == 0)
			dwRet |= FS_BOLD;
		else if(_tcsicmp(pszToken, _T("italic")) == 0)
			dwRet |= FS_ITALIC;
		else if(_tcsicmp(pszToken, _T("underline")) == 0)
			dwRet |= FS_UNDERLINE;
		else if(_tcsicmp(pszToken, _T("strikeout")) == 0)
			dwRet |= FS_STRIKEOUT;
		else
			return FALSE;

		pszToken = _tcstok_s(NULL, _T(" ,"), &pszContext);
	}
	free(pszDup);

	*pValue = dwRet;
	return TRUE;
}

BOOL CSkinControl::StringToDTFlag(LPCTSTR pszFlag, DWORD * pValue)
{
	DWORD dwRet = 0;
	TCHAR * pszDup = _tcsdup(pszFlag);
	TCHAR * pszContext = NULL;
	TCHAR * pszToken = _tcstok_s(pszDup, _T(" ,"), &pszContext);
	while(pszToken)
	{
		if(_tcsicmp(pszToken, _T("left")) == 0)
			dwRet |= DT_LEFT;
		else if(_tcsicmp(pszToken, _T("right")) == 0)
			dwRet |= DT_RIGHT;
		else if(_tcsicmp(pszToken, _T("vcenter")) == 0)
			dwRet |= DT_VCENTER;
		else if(_tcsicmp(pszToken, _T("bottom")) == 0)
			dwRet |= DT_BOTTOM;
		else if(_tcsicmp(pszToken, _T("wordbreak")) == 0)
			dwRet |= DT_WORDBREAK;
		else if(_tcsicmp(pszToken, _T("singleline")) == 0)
			dwRet |= DT_SINGLELINE;
		else if(_tcsicmp(pszToken, _T("expandtabs")) == 0)
			dwRet |= DT_EXPANDTABS;
		else if(_tcsicmp(pszToken, _T("tabstop")) == 0)
			dwRet |= DT_TABSTOP;
		else if(_tcsicmp(pszToken, _T("noclip")) == 0)
			dwRet |= DT_NOCLIP;
		else if(_tcsicmp(pszToken, _T("externalleading")) == 0)
			dwRet |= DT_EXTERNALLEADING;
		else if(_tcsicmp(pszToken, _T("noprefix")) == 0)
			dwRet |= DT_NOPREFIX;
		else
			return FALSE;

		pszToken = _tcstok_s(NULL, _T(" ,"), &pszContext);
	}
	free(pszDup);

	*pValue = dwRet;
	return TRUE;
}

BOOL CSkinControl::StringToRect(LPCTSTR pszRect, RECT * pValue)
{
	BOOL bRet = FALSE;
	TCHAR * pszDup = _tcsdup(pszRect);
	TCHAR * pszContext = NULL;
	TCHAR * pszToken = _tcstok_s(pszDup, _T(", "), &pszContext);
	do
	{
		if(pszToken == NULL)
			break;
		int nLeft = _ttoi(pszToken);

		pszToken = _tcstok_s(NULL, _T(": "), &pszContext);
		if(pszToken == NULL)
			break;
		int nTop = _ttoi(pszToken);

		pszToken = _tcstok_s(NULL, _T(", "), &pszContext);
		if(pszToken == NULL)
			break;
		int nWidth = _ttoi(pszToken);

		pszToken = _tcstok_s(NULL, _T(", "), &pszContext);
		if(pszToken == NULL)
			break;
		int nHeight = _ttoi(pszToken);

		pszToken = _tcstok_s(NULL, _T(", "), &pszContext);
		if(pszToken != NULL)
			break;

		pValue->left = nLeft;
		pValue->top = nTop;
		pValue->right = nLeft + nWidth;
		pValue->bottom = nTop + nHeight;
		bRet = TRUE;
	} while(FALSE);

	free(pszDup);
	return bRet;
}

int CSkinControl::AnchorsToDirection(int nDirection)
{
	if(nDirection & ANK_LEFT)
	{
		if(nDirection & ANK_TOP)
			return HTTOPLEFT;
		else if(nDirection & ANK_BOTTOM)
			return HTBOTTOMLEFT;
		else
			return HTLEFT;
	} else if(nDirection & ANK_RIGHT)
	{
		if(nDirection & ANK_TOP)
			return HTTOPRIGHT;
		else if(nDirection & ANK_BOTTOM)
			return HTBOTTOMRIGHT;
		else
			return HTRIGHT;
	} else
	{
		if(nDirection == ANK_TOP)
			return HTTOP;
		else
			return HTBOTTOM;
	}
}