#pragma once

#include "SkinResource.h"

class CSkinImageMap : public CSkinResource
{
public:
	CSkinImageMap(STRING & szKey, LPCTSTR pszFileName);

	HDC GetBitmapDC();
	const DIBSECTION & GetDibInfo();

	void Draw(RECT * prtSrc, HDC hDstDC, RECT * prtDst);
	void DrawTransparent(RECT * prtSrc, HDC hDstDC, RECT * prtDst, COLORREF crTransparent);
	void DrawAlphaBlend(RECT * prtSrc, HDC hDstDC, RECT * prtDst, int nAlpha);
	HBITMAP GetHandle();

	static void GenerateResourceKey(LPCTSTR pszFileName, STRING & szKey);

	static HBITMAP LoadImage(LPCTSTR pszFileName);
	static void HIMETRICtoDP(HDC hDC, LPSIZE lpSize);

protected:
	~CSkinImageMap(void);

	HBITMAP m_hBitmap;
	HBITMAP m_hOldBitmap;
	HDC m_hDC;
	DIBSECTION m_DibInfo;
};
