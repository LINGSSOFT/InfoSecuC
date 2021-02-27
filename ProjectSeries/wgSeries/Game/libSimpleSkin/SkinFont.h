#pragma once

#include "SkinResource.h"

// Font Style Flags
#define FS_BOLD			(0x00000001)
#define FS_ITALIC		(0x00000002)
#define	FS_UNDERLINE	(0x00000004)
#define FS_STRIKEOUT	(0x00000008)


class CSkinFont : public CSkinResource
{
public:
	static void GenerateResourceKey(LPCTSTR pszFontName, int nFontSize, DWORD dwFontStyle, STRING & szKey);

	CSkinFont(STRING & szKey, LPCTSTR pszFontName, int nFontSize, DWORD dwFontStyle);

	HFONT GetHandle();
	DWORD GetFontStyle();
	int GetFontSize();
	const STRING & GetFontName();

protected:
	~CSkinFont(void);

	HFONT CreateFont(LPCTSTR pszFontName, int nFontSize, DWORD dwFontStyle);

	HFONT m_hFont;
	DWORD m_dwFontStyle;
	int m_nFontSize;
	STRING m_szFontName;
};
