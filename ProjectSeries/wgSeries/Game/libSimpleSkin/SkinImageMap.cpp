#include "StdAfx.h"
#include "SkinImageMap.h"
#include <Objidl.h>
#include <ocidl.h>
#include <olectl.h>

#ifndef AC_SRC_ALPHA
#define AC_SRC_ALPHA			0x01
#endif

CSkinImageMap::CSkinImageMap(STRING & szKey, LPCTSTR pszFileName) :
	CSkinResource(szKey)
{
	m_hDC = NULL;
	m_hOldBitmap = NULL;
	m_hBitmap = LoadImage(pszFileName);
	if(m_hBitmap != NULL)
	{
		GetObject(m_hBitmap, sizeof(m_DibInfo), &m_DibInfo);
		HDC hScreenDC = ::GetDC(GetDesktopWindow());
		m_hDC = CreateCompatibleDC(hScreenDC);
		m_hOldBitmap = (HBITMAP)SelectObject(m_hDC, m_hBitmap);
	}
}

CSkinImageMap::~CSkinImageMap(void)
{
	if(m_hDC)
	{
		SelectObject(m_hDC, m_hOldBitmap);
		DeleteDC(m_hDC);
	}

	if(m_hBitmap)
		DeleteObject(m_hBitmap);
}

void CSkinImageMap::GenerateResourceKey(LPCTSTR pszFileName, STRING & szKey)
{
	szKey = _T("IMG:");
	szKey += pszFileName;
	MakeUpper(szKey);
}

void CSkinImageMap::HIMETRICtoDP(HDC hDC, LPSIZE lpSize)
{
	int _HIMETRIC_INCH = 2540;
	int nMapMode = GetMapMode(hDC);
	if (nMapMode < MM_ISOTROPIC && nMapMode != MM_TEXT)
	{
		SetMapMode(hDC, MM_HIMETRIC);
		LPtoDP(hDC, (POINT *)lpSize, 1);
		SetMapMode(hDC, nMapMode);
	} else
	{
		int cxPerInch, cyPerInch;
		cxPerInch = GetDeviceCaps(hDC, LOGPIXELSX);
		cyPerInch = GetDeviceCaps(hDC, LOGPIXELSY);
		lpSize->cx = MulDiv(lpSize->cx, cxPerInch, _HIMETRIC_INCH);
		lpSize->cy = MulDiv(lpSize->cy, cyPerInch, _HIMETRIC_INCH);
	}
}

HBITMAP CSkinImageMap::LoadImage(LPCTSTR pszFileName)
{
	HANDLE hFile = CreateFile(pszFileName, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);   
	if(hFile == INVALID_HANDLE_VALUE)
		return NULL;

	HRESULT hr;
	IStream *pStm = NULL;
	IPicture *pPic = NULL;

	hr = CreateStreamOnHGlobal(NULL, TRUE, &pStm);
	if(FAILED(hr))
		return NULL;

	// Loading...
	do
	{
		char buf[8192];
		long nSize = 0;

		DWORD dwRealRead = 0;
		DWORD dwRealWrite;
		while(TRUE) 
		{
			if(ReadFile(hFile, buf, 8192, &dwRealRead, NULL) == FALSE || dwRealRead == 0)
				break;

			nSize += dwRealRead;
			hr = pStm->Write(buf, dwRealRead, &dwRealWrite);
			if(FAILED(hr))
				break;
		}
		CloseHandle(hFile);
	
		if( FAILED(hr) )
			break;
		
		LARGE_INTEGER dlibMove;
		dlibMove.QuadPart = 0;
		hr = pStm->Seek(dlibMove, STREAM_SEEK_SET, NULL);
		if( FAILED(hr) )
			break;

		hr = OleLoadPicture(pStm, nSize, FALSE, IID_IPicture, (LPVOID*)&pPic);

		if(FAILED(hr) || pPic == NULL)
			break;

		HDC hDC = ::GetDC(NULL);
		HDC hMemDC = ::CreateCompatibleDC(hDC);

		OLE_XSIZE_HIMETRIC w;
		OLE_YSIZE_HIMETRIC h;
		pPic->get_Width(&w);
		pPic->get_Height(&h);

		SIZE cs = {w, h};
		HIMETRICtoDP(hDC, &cs);

		BITMAPINFOHEADER bmih;
		bmih.biSize = sizeof (BITMAPINFOHEADER);
		bmih.biWidth = cs.cx;
		bmih.biHeight = cs.cy;
		bmih.biPlanes = 1;
		bmih.biBitCount = 32;
		bmih.biCompression = BI_RGB;
		bmih.biSizeImage = (((cs.cx * 32) + 31) & (~31)) / 8;
		bmih.biXPelsPerMeter = 0;
		bmih.biYPelsPerMeter = 0;
		bmih.biClrUsed = 0;
		bmih.biClrImportant = 0;

		HBITMAP hBitmap = CreateDIBSection(hDC, (const BITMAPINFO*)&bmih, DIB_RGB_COLORS, NULL, NULL, 0) ;
		HBITMAP hOldBitmap = (HBITMAP)SelectObject(hMemDC, hBitmap);
		pPic->Render(hMemDC, 0, 0, cs.cx, cs.cy, 0, h - 1, w, - h, NULL);
		SelectObject(hMemDC, hOldBitmap);
		::DeleteDC(hMemDC);
		::ReleaseDC(NULL, hDC);
		pStm->Release();
		pPic->Release();

		return hBitmap;
	} while(false);

	pStm->Release();

	return NULL;
}

HDC CSkinImageMap::GetBitmapDC()
{
	return m_hDC;
}

const DIBSECTION & CSkinImageMap::GetDibInfo()
{
	return m_DibInfo;
}

void CSkinImageMap::Draw(RECT * prtSrc, HDC hDstDC, RECT * prtDst)
{
	int nDstWidth = prtDst->right - prtDst->left;
	int nDstHeight = prtDst->bottom - prtDst->top;
	int nSrcWidth = prtSrc->right - prtSrc->left;
	int nSrcHeight = prtSrc->bottom - prtSrc->top;
		
	if(nDstWidth == nSrcWidth && nDstHeight == nSrcHeight)
		BitBlt(hDstDC, prtDst->left, prtDst->top, nDstWidth, nDstHeight, m_hDC, prtSrc->left, prtSrc->top, SRCCOPY);
	else
		StretchBlt(hDstDC, prtDst->left, prtDst->top, nDstWidth, nDstHeight, m_hDC, prtSrc->left, prtSrc->top, nSrcWidth, nSrcHeight, SRCCOPY);
}

void CSkinImageMap::DrawTransparent(RECT * prtSrc, HDC hDstDC, RECT * prtDst, COLORREF crTransparent)
{
	int nDstWidth = prtDst->right - prtDst->left;
	int nDstHeight = prtDst->bottom - prtDst->top;
	int nSrcWidth = prtSrc->right - prtSrc->left;
	int nSrcHeight = prtSrc->bottom - prtSrc->top;

	TransparentBlt(hDstDC, prtDst->left, prtDst->top, nDstWidth, nDstHeight, m_hDC, prtSrc->left, prtSrc->top, nSrcWidth, nSrcHeight, crTransparent);
}

void CSkinImageMap::DrawAlphaBlend(RECT * prtSrc, HDC hDstDC, RECT * prtDst, int nAlpha)
{
	int nDstWidth = prtDst->right - prtDst->left;
	int nDstHeight = prtDst->bottom - prtDst->top;
	int nSrcWidth = prtSrc->right - prtSrc->left;
	int nSrcHeight = prtSrc->bottom - prtSrc->top;

	BLENDFUNCTION bf;
	bf.AlphaFormat = AC_SRC_ALPHA;
	bf.BlendFlags = 0;
	bf.BlendOp = AC_SRC_OVER;
	bf.SourceConstantAlpha = nAlpha;

	AlphaBlend(hDstDC, prtDst->left, prtDst->top, nDstWidth, nDstHeight, m_hDC, prtSrc->left, prtSrc->top, nSrcWidth, nSrcHeight, bf);
}

HBITMAP CSkinImageMap::GetHandle()
{
	return m_hBitmap;
}