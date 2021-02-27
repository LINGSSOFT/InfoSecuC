#include "StdAfx.h"
#include "SkinFont.h"

CSkinFont::CSkinFont(STRING & szKey, LPCTSTR pszFontName, int nFontSize, DWORD dwFontStyle) :
	CSkinResource(szKey)
{
	m_hFont = CreateFont(pszFontName, nFontSize, dwFontStyle);
	m_dwFontStyle = dwFontStyle;
	m_nFontSize = nFontSize;
	m_szFontName = pszFontName;
}

CSkinFont::~CSkinFont(void)
{
}

void CSkinFont::GenerateResourceKey(LPCTSTR pszFontName, int nFontSize, DWORD dwFontStyle, STRING & szKey)
{
	TCHAR szFontKey[MAX_PATH + 100];
	_stprintf_s(szFontKey, MAX_PATH + 100, _T("FNT:%s|%d|%u"), pszFontName, nFontSize, dwFontStyle);
	szKey = szFontKey;
	MakeUpper(szKey);
}

HFONT CSkinFont::CreateFont(LPCTSTR pszFontName, int nFontSize, DWORD dwFontStyle)
{
	nFontSize *= 10;

	LOGFONT lf;
	GetObject(GetStockObject(SYSTEM_FONT), sizeof(lf), &lf);
	_tcscpy_s(lf.lfFaceName, LF_FACESIZE, pszFontName);
	lf.lfCharSet = DEFAULT_CHARSET;
	lf.lfWidth = 0;

	if(dwFontStyle & FS_BOLD)
		lf.lfWeight |= FW_BOLD;
	else
		lf.lfWeight &= ~FW_BOLD;

	if(dwFontStyle & FS_ITALIC)
		lf.lfItalic = TRUE;
	else
		lf.lfItalic = FALSE;

	if(dwFontStyle & FS_UNDERLINE)
		lf.lfUnderline = TRUE;
	else
		lf.lfUnderline = FALSE;

	if(dwFontStyle & FS_STRIKEOUT)
		lf.lfStrikeOut = TRUE;
	else
		lf.lfStrikeOut = FALSE;

	HDC hDC = GetDC(NULL);
	lf.lfHeight = -MulDiv(nFontSize, GetDeviceCaps(hDC, LOGPIXELSY), 720);
	ReleaseDC(NULL, hDC);

	return CreateFontIndirect(&lf);
}

HFONT CSkinFont::GetHandle()
{
	return m_hFont;
}

DWORD CSkinFont::GetFontStyle()
{
	return m_dwFontStyle;
}

int CSkinFont::GetFontSize()
{
	return m_nFontSize;
}

const STRING & CSkinFont::GetFontName()
{
	return m_szFontName;
}

