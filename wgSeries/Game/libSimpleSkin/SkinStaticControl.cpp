#include "StdAfx.h"
#include "SkinStaticControl.h"
#include "ResourceManager.h"
#include "SkinWindow.h"

CSkinStaticControl::CSkinStaticControl(void)
{
	m_pszText = NULL;
	m_pFont = NULL;
	m_bAutoSize = TRUE;
	m_dwFlag = DT_LEFT | DT_SINGLELINE | DT_NOPREFIX;
	m_crFont = RGB(0, 0, 0);

	m_pFont2 = sg_pResourceMngr->QueryFont(_T("±¼¸²"), 9, FS_BOLD);
//	m_bNeedToRecalc = FALSE;
}

CSkinStaticControl::~CSkinStaticControl(void)
{
	if(m_pszText)
		delete []m_pszText;
	if(m_pFont)
		sg_pResourceMngr->ReleaseResource(m_pFont);

	sg_pResourceMngr->ReleaseResource(m_pFont2);
}

void CSkinStaticControl::SetText(LPCTSTR pszText)
{
	if(m_pszText)
		delete []m_pszText;

	size_t nSize = _tcslen(pszText) + 1;
	m_pszText = new TCHAR[nSize];
	_tcscpy_s(m_pszText, nSize, pszText);

	if(m_bAutoSize)
		CalcSize();
}

LPCTSTR CSkinStaticControl::GetText()
{
	if(m_pszText)
		return m_pszText;
	return _T("");
}

void CSkinStaticControl::SetColor(COLORREF crFont)
{
	if(m_crFont == crFont)
		return;
	m_crFont = crFont;
	Invalidate();
}

COLORREF CSkinStaticControl::GetColor()
{
	return m_crFont;
}

void CSkinStaticControl::SetAutoSize(BOOL bAutoSize)
{
	if(m_bAutoSize == bAutoSize)
		return;
	m_bAutoSize = bAutoSize;
	Invalidate();
}

void CSkinStaticControl::SetDrawTextFlag(DWORD dwFlag)
{
	m_dwFlag = dwFlag;
}

DWORD CSkinStaticControl::GetDrawTextFlag()
{
	return m_dwFlag;
}

void CSkinStaticControl::SetFont(LPCTSTR pszFontName, int nFontSize, DWORD dwFontStyle)
{
	CSkinFont * pFont = sg_pResourceMngr->QueryFont(pszFontName, nFontSize, dwFontStyle);
	if(pFont)
	{
		SetFont(pFont);
		sg_pResourceMngr->ReleaseResource(pFont);
	}
}

void CSkinStaticControl::SetFont(CSkinFont * pFont)
{
	if(m_pFont)
		sg_pResourceMngr->ReleaseResource(m_pFont);
	m_pFont = pFont;
	sg_pResourceMngr->AddRefResource(pFont);
	if(m_bAutoSize)
		CalcSize();
	else
		Invalidate();
}

CSkinFont * CSkinStaticControl::GetFont()
{
	return m_pFont;
}

void CSkinStaticControl::DrawControl(HDC hDC)
{
	if(m_pszText == NULL)
		return;

	//if(m_bNeedToRecalc)
	//	CalcSize();

	HFONT hTempFont = NULL;
	int nOldMode = SetBkMode(hDC, TRANSPARENT);
	COLORREF clOldTextColor = SetTextColor(hDC, m_crFont);
	HFONT hOldFont = NULL;
	if(m_pFont)
		hOldFont = (HFONT)SelectObject(hDC, m_pFont->GetHandle());

	RECT rtControl = {0, 0, m_rtControl.right - m_rtControl.left, m_rtControl.bottom - m_rtControl.top};
	DrawText(hDC, m_pszText, _tcslen(m_pszText), &rtControl, m_dwFlag);

	if(hOldFont)
		SelectObject(hDC, hOldFont);
	SetTextColor(hDC, clOldTextColor);
	SetBkMode(hDC, nOldMode);

//	m_bNeedToRecalc = FALSE;
}

int CSkinStaticControl::GetTextWidth()
{
	if(m_pszText == NULL || GetParentWindow() == NULL)
		return 0;

	HWND hParent = GetParentWindow()->GetHWND();
	if(hParent == NULL)
		return 0;

	HDC hDC = GetDC(hParent);
	RECT rt = {0, 0, 0, 0};
	HFONT hOldFont = NULL;

	if(m_pFont)
		hOldFont = (HFONT)SelectObject(hDC, m_pFont->GetHandle());

	int nTextLen = _tcslen(m_pszText);
	DrawText(hDC, m_pszText, nTextLen, &rt, DT_CALCRECT | DT_SINGLELINE | DT_LEFT | DT_NOPREFIX);
	if((m_pFont && (m_pFont->GetFontStyle() & FS_ITALIC)) != 0  && nTextLen > 0)
	{
		ABC abc;
		if(GetCharABCWidths(hDC, m_pszText[0], m_pszText[0], &abc))
			rt.right += (abc.abcA + abc.abcA);

		if(GetCharABCWidths(hDC, m_pszText[nTextLen - 1], m_pszText[nTextLen - 1], &abc))
			rt.right += (abc.abcA - abc.abcC);
	}
	
	if(hOldFont)
		SelectObject(hDC, hOldFont);

	ReleaseDC(hParent, hDC);

	return rt.right - rt.left;
}

void CSkinStaticControl::CalcSize()
{
//	m_bNeedToRecalc = TRUE;

	if(m_pszText == NULL || GetParentWindow() == NULL)
		return;

	HWND hParent = GetParentWindow()->GetHWND();
	if(hParent == NULL)
		return;

	HDC hDC = GetDC(hParent);
	RECT rt = {0, 0, 0, 0};
	HFONT hOldFont = NULL;

	if(m_pFont)
		hOldFont = (HFONT)SelectObject(hDC, m_pFont->GetHandle());

	int nTextLen = _tcslen(m_pszText);
	DrawText(hDC, m_pszText, nTextLen, &rt, m_dwFlag | DT_CALCRECT);
	if((m_pFont && (m_pFont->GetFontStyle() & FS_ITALIC)) != 0 && nTextLen > 0)
	{
		ABC abc;
		if(GetCharABCWidths(hDC, m_pszText[0], m_pszText[0], &abc))
			rt.right += (abc.abcA + abc.abcA);

		if(GetCharABCWidths(hDC, m_pszText[nTextLen - 1], m_pszText[nTextLen - 1], &abc))
			rt.right += (abc.abcA - abc.abcC);
	}
	
	SetRect(&rt, m_rtControl.left, m_rtControl.top, m_rtControl.left + rt.right, m_rtControl.top + rt.bottom);
	SetControlRect(&rt);

	if(hOldFont)
		SelectObject(hDC, hOldFont);

	ReleaseDC(hParent, hDC);
}

void CSkinStaticControl::OnParentWindowAttached(CSkinWindow * pWindow)
{
	if(m_bAutoSize)
		CalcSize();
}

BOOL CSkinStaticControl::ApplyXML(XMLNS::IXMLDOMNodePtr spNode)
{
//				<static text='hello' auto_size='y' color='ffff00' size='9'
//					face='±¼¸²' style='bold, underline, italic, strike'
//					flags='end_ellipsis, word_wrap, single_line, vcenter'/>
	if(!CSkinDraggableControl::ApplyXML(spNode))
		return FALSE;

	// text
	XMLNS::IXMLDOMNodePtr spAttr;
	spAttr = spNode->attributes->getNamedItem(L"text");
	if(spAttr)
		SetText(spAttr->Gettext());

	// auto_size
	BOOL bValue;
	spAttr = spNode->attributes->getNamedItem(L"auto_size");
	if(spAttr)
	{
		if(!StringToBOOL(spAttr->Gettext(), &bValue))
			return FALSE;
		SetAutoSize(bValue);
	}

	// color
	COLORREF crValue;
	spAttr = spNode->attributes->getNamedItem(L"color");
	if(spAttr)
	{
		if(!StringToColor(spAttr->Gettext(), &crValue))
			return FALSE;
		SetColor(crValue);
	}

	// size
	int nFontSize = 9;
	spAttr = spNode->attributes->getNamedItem(L"size");
	if(spAttr)
	{
		if(!StringToInt(spAttr->Gettext(), &nFontSize))
			return FALSE;
	}

	// face
	STRING szFontFace = _T("±¼¸²");
	spAttr = spNode->attributes->getNamedItem(L"face");
	if(spAttr)
		szFontFace = spAttr->Gettext();

	// style
	DWORD dwFontStyle = 0;
	spAttr = spNode->attributes->getNamedItem(L"style");
	if(spAttr)
	{
		if(!StringToFontStyle(spAttr->Gettext(), &dwFontStyle))
			return FALSE;
	}
	SetFont(szFontFace.c_str(), nFontSize, dwFontStyle);

	// flags
	DWORD dwDrawTextFlag = DT_LEFT | DT_TOP | DT_SINGLELINE;
	spAttr = spNode->attributes->getNamedItem(L"flags");
	if(spAttr)
	{
		if(!StringToDTFlag(spAttr->Gettext(), &dwDrawTextFlag))
			return FALSE;
	}
	SetDrawTextFlag(dwDrawTextFlag);

	return TRUE;
}