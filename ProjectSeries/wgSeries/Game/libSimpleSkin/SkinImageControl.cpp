#include "StdAfx.h"
#include "SkinImageControl.h"
#include "SkinWindow.h"
#include "SkinFileReader.h"

CSkinImageControl::CSkinImageControl(void)
{
	m_pImageMap = NULL;
	m_bTransparent = FALSE;
	m_crTransparent = RGB(255, 0, 255);
	m_bStretch = TRUE;
}

CSkinImageControl::~CSkinImageControl(void)
{
	if(m_pImageMap)
		sg_pResourceMngr->ReleaseResource(m_pImageMap);
}

void CSkinImageControl::SetImageMap(LPCTSTR pszImageFile)
{
	STRING szFullPath;
	if(m_pSkinFileReader)
	{
		m_pSkinFileReader->GetFullFilePath(pszImageFile, &szFullPath);
		pszImageFile = szFullPath.c_str();
	}

	CSkinImageMap * pImageMap = sg_pResourceMngr->QueryImageMap(pszImageFile);
	if(pImageMap)
	{
		SetImageMap(pImageMap);
		sg_pResourceMngr->ReleaseResource(pImageMap);
	}
}

void CSkinImageControl::SetImageMap(CSkinImageMap * pImageMap)
{
	if(m_pImageMap)
		sg_pResourceMngr->ReleaseResource(m_pImageMap);

	m_pImageMap = pImageMap;
	sg_pResourceMngr->AddRefResource(pImageMap);
	Invalidate();
}

void CSkinImageControl::SetImageRect(RECT * prtImage)
{
	if(prtImage == NULL)
	{
		if(m_pImageMap)
		{
			const DIBSECTION & info = m_pImageMap->GetDibInfo();
			SetRect(&m_rtImage, 0, 0, info.dsBmih.biWidth, info.dsBmih.biHeight);
		}
	} else
		m_rtImage = *prtImage;
}

CSkinImageMap * CSkinImageControl::GetImageMap()
{
	return m_pImageMap;
}

void CSkinImageControl::DrawControl(HDC hDC)
{
	if(m_pImageMap == NULL)
		CSkinDraggableControl::DrawControl(hDC);
	else
	{
		RECT rtDst;
		int nControlWidth = m_rtControl.right - m_rtControl.left;
		int nControlHeight = m_rtControl.bottom - m_rtControl.top;
		if(m_bStretch)
			SetRect(&rtDst, 0, 0, nControlWidth, nControlHeight);
		else
		{
			int nImageWidth = m_rtImage.right - m_rtImage.left;
			int nImageHeight = m_rtImage.bottom - m_rtImage.top;
			SetRect(&rtDst, 0, 0, nImageWidth, nImageHeight);
		}

		if(m_bTransparent)
			m_pImageMap->DrawTransparent(&m_rtImage, hDC, &rtDst, m_crTransparent);
		else
			m_pImageMap->Draw(&m_rtImage, hDC, &rtDst);
	}
}

void CSkinImageControl::SetTransparent(BOOL bTransparent)
{
	if(m_bTransparent == bTransparent)
		return;

	m_bTransparent = bTransparent;
	Invalidate();
}

void CSkinImageControl::SetTransparentColor(COLORREF crTransparent)
{
	if(m_crTransparent == crTransparent)
		return;

	m_crTransparent = crTransparent;
	Invalidate();
}

void CSkinImageControl::SetStretch(BOOL bStretch)
{
	if(m_bStretch == bStretch)
		return;
	m_bStretch = bStretch;
	Invalidate();
}

COLORREF CSkinImageControl::GetDCColor(COLORREF crLogical)
{
	COLORREF crRet;

	HDC hDC = GetDC(NULL);
	HDC hColorTestDC = CreateCompatibleDC(hDC);
	HBITMAP hColorTestBitmap = CreateCompatibleBitmap(hDC, 1, 1);
	HBITMAP hOldBitmap = (HBITMAP)SelectObject(hColorTestDC, hColorTestBitmap);
	SetPixel(hColorTestDC, 0, 0, crLogical);
	crRet = GetPixel(hColorTestDC, 0, 0);
	SelectObject(hColorTestDC, hOldBitmap);
	DeleteObject(hColorTestBitmap);
	DeleteDC(hColorTestDC);
	ReleaseDC(NULL, hDC);

	return crRet;
}

HRGN CSkinImageControl::BitmapToRegion(HBITMAP hBmp, COLORREF cTransparentColor, COLORREF cTolerance)
{
	cTransparentColor = GetDCColor(cTransparentColor);
	HRGN hRgn = NULL;

	if (hBmp)
	{
		// Create a memory DC inside which we will scan the bitmap conten
		HDC hMemDC = CreateCompatibleDC(NULL);
		if (hMemDC)
		{
			// Get bitmap siz
			BITMAP bm;
			GetObject(hBmp, sizeof(bm), &bm);

			// Create a 32 bits depth bitmap and select it into the memory DC
			BITMAPINFOHEADER RGB32BITSBITMAPINFO = {	
					sizeof(BITMAPINFOHEADER),	// biSize
					bm.bmWidth,					// biWidth;
					bm.bmHeight,				// biHeight;
					1,							// biPlanes;
					32,							// biBitCount
					BI_RGB,						// biCompression;
					0,							// biSizeImage;
					0,							// biXPelsPerMeter;
					0,							// biYPelsPerMeter;
					0,							// biClrUsed;
					0							// biClrImportant;
			};
			VOID * pbits32; 
			HBITMAP hbm32 = CreateDIBSection(hMemDC, (BITMAPINFO *)&RGB32BITSBITMAPINFO, DIB_RGB_COLORS, &pbits32, NULL, 0);
			if (hbm32)
			{
				HBITMAP holdBmp = (HBITMAP)SelectObject(hMemDC, hbm32);

				// Create a DC just to copy the bitmap into the memory D
				HDC hDC = CreateCompatibleDC(hMemDC);
				if (hDC)
				{
					// Get how many bytes per row we have for the bitmap bits (rounded up to 32 bits
					BITMAP bm32;
					GetObject(hbm32, sizeof(bm32), &bm32);
					while (bm32.bmWidthBytes % 4)
						bm32.bmWidthBytes++;

					// Copy the bitmap into the memory D
					HBITMAP holdBmp = (HBITMAP)SelectObject(hDC, hBmp);
					BitBlt(hMemDC, 0, 0, bm.bmWidth, bm.bmHeight, hDC, 0, 0, SRCCOPY);

					// For better performances, we will use the ExtCreateRegion() function to create th
					// region. This function take a RGNDATA structure on entry. We will add rectangles b
					// amount of ALLOC_UNIT number in this structure
					#define ALLOC_UNIT	100
					DWORD maxRects = ALLOC_UNIT;
					HANDLE hData = GlobalAlloc(GMEM_MOVEABLE, sizeof(RGNDATAHEADER) + (sizeof(RECT) * maxRects));
					RGNDATA *pData = (RGNDATA *)GlobalLock(hData);
					pData->rdh.dwSize = sizeof(RGNDATAHEADER);
					pData->rdh.iType = RDH_RECTANGLES;
					pData->rdh.nCount = pData->rdh.nRgnSize = 0;
					SetRect(&pData->rdh.rcBound, MAXLONG, MAXLONG, 0, 0);

					// Keep on hand highest and lowest values for the "transparent" pixel
					BYTE lr = GetRValue(cTransparentColor);
					BYTE lg = GetGValue(cTransparentColor);
					BYTE lb = GetBValue(cTransparentColor);
					BYTE hr = min(0xff, lr + GetRValue(cTolerance));
					BYTE hg = min(0xff, lg + GetGValue(cTolerance));
					BYTE hb = min(0xff, lb + GetBValue(cTolerance));

					// Scan each bitmap row from bottom to top (the bitmap is inverted vertically
					BYTE *p32 = (BYTE *)bm32.bmBits + (bm32.bmHeight - 1) * bm32.bmWidthBytes;
					for (int y = 0; y < bm.bmHeight; y++)
					{
						// Scan each bitmap pixel from left to righ
						for (int x = 0; x < bm.bmWidth; x++)
						{
							// Search for a continuous range of "non transparent pixels"
							int x0 = x;
							LONG *p = (LONG *)p32 + x;
							while (x < bm.bmWidth)
							{
								BYTE b = GetRValue(*p);
								if (b >= lr && b <= hr)
								{
									b = GetGValue(*p);
									if (b >= lg && b <= hg)
									{
										b = GetBValue(*p);
										if (b >= lb && b <= hb)
											// This pixel is "transparent"
											break;
									}
								}
								p++;
								x++;
							}

							if (x > x0)
							{
								// Add the pixels (x0, y) to (x, y+1) as a new rectangle in the regio
								if (pData->rdh.nCount >= maxRects)
								{
									GlobalUnlock(hData);
									maxRects += ALLOC_UNIT;
									hData = GlobalReAlloc(hData, sizeof(RGNDATAHEADER) + (sizeof(RECT) * maxRects), GMEM_MOVEABLE);
									pData = (RGNDATA *)GlobalLock(hData);
								}
								RECT *pr = (RECT *)&pData->Buffer;
								SetRect(&pr[pData->rdh.nCount], x0, y, x, y+1);
								if (x0 < pData->rdh.rcBound.left)
									pData->rdh.rcBound.left = x0;
								if (y < pData->rdh.rcBound.top)
									pData->rdh.rcBound.top = y;
								if (x > pData->rdh.rcBound.right)
									pData->rdh.rcBound.right = x;
								if (y+1 > pData->rdh.rcBound.bottom)
									pData->rdh.rcBound.bottom = y+1;
								pData->rdh.nCount++;

								// On Windows98, ExtCreateRegion() may fail if the number of rectangles is to
								// large (ie: > 4000). Therefore, we have to create the region by multiple steps
								if (pData->rdh.nCount == 2000)
								{
									HRGN h = ExtCreateRegion(NULL, sizeof(RGNDATAHEADER) + (sizeof(RECT) * maxRects), pData);
									if (hRgn)
									{
										CombineRgn(hRgn, hRgn, h, RGN_OR);
										DeleteObject(h);
									}
									else
										hRgn = h;
									pData->rdh.nCount = 0;
									SetRect(&pData->rdh.rcBound, MAXLONG, MAXLONG, 0, 0);
								}
							}
						}

						// Go to next row (remember, the bitmap is inverted vertically
						p32 -= bm32.bmWidthBytes;
					}

					// Create or extend the region with the remaining rectangle
					HRGN h = ExtCreateRegion(NULL, sizeof(RGNDATAHEADER) + (sizeof(RECT) * maxRects), pData);
					if (hRgn)
					{
						CombineRgn(hRgn, hRgn, h, RGN_OR);
						DeleteObject(h);
					}
					else
						hRgn = h;

					// Clean up
					GlobalUnlock(hData);
					GlobalFree(hData);
					SelectObject(hDC, holdBmp);
					DeleteDC(hDC);
				}

				DeleteObject(SelectObject(hMemDC, holdBmp));
			}

			DeleteDC(hMemDC);
		}	
	}

	return hRgn;
}

BOOL CSkinImageControl::ApplyXML(XMLNS::IXMLDOMNodePtr spNode)
{
	if(!CSkinDraggableControl::ApplyXML(spNode))
		return FALSE;

	//<image file='test.jpg' mask='y' transparent='n'
	//	transparent_color='ff00ff' image_rect='0,0-50,50' stretch='y'/>

	// file
	STRING szFileName;
	XMLNS::IXMLDOMNodePtr spAttr;
	spAttr = spNode->attributes->getNamedItem(L"file");
	if(spAttr)
	{
		szFileName = spAttr->Gettext();
		SetImageMap(szFileName.c_str());
	}

	// image_rect
	RECT rtValue;
	spAttr = spNode->attributes->getNamedItem(L"image_rect");
	if(spAttr)
	{
		if(!StringToRect(spAttr->Gettext(), &rtValue))
			return FALSE;
		SetImageRect(&rtValue);
	} else
		SetImageRect(NULL);

	// transparent_color
	COLORREF crValue;
	spAttr = spNode->attributes->getNamedItem(L"transparent_color");
	if(spAttr)
	{
		if(!StringToColor(spAttr->Gettext(), &crValue))
			return FALSE;
		SetTransparentColor(crValue);
	}

	// transparent
	BOOL bValue;
	spAttr = spNode->attributes->getNamedItem(L"transparent");
	if(spAttr)
	{
		if(!StringToBOOL(spAttr->Gettext(), &bValue))
			return FALSE;
		SetTransparent(bValue);
	}

	// stretch
	spAttr = spNode->attributes->getNamedItem(L"stretch");
	if(spAttr)
	{
		if(!StringToBOOL(spAttr->Gettext(), &bValue))
			return FALSE;
		SetStretch(bValue);
	}


	return TRUE;
}

