#pragma once

#include "SkinControl.h"
#include "SkinImageMap.h"
#include "SkinDraggableControl.h"

class CSkinImageControl : public CSkinDraggableControl
{
public:
	CSkinImageControl(void);
	virtual ~CSkinImageControl(void);

	SET_CLASS_NAME(_T("CSkinImageControl"), CSkinDraggableControl)

	static HRGN BitmapToRegion(HBITMAP hBmp, COLORREF cTransparentColor, COLORREF cTolerance);

	void SetImageMap(LPCTSTR pszImageFile);
	void SetImageMap(CSkinImageMap * pImageMap);
	void SetImageRect(RECT * prtImage = NULL);
	CSkinImageMap * GetImageMap();
	void SetTransparent(BOOL bTransparent);
	void SetTransparentColor(COLORREF crTransparent);
	void SetStretch(BOOL bStretch);

protected:
	static COLORREF GetDCColor(COLORREF crLogical);

	virtual void DrawControl(HDC hDC);
	virtual BOOL ApplyXML(XMLNS::IXMLDOMNodePtr spNode);

	CSkinImageMap * m_pImageMap;
	RECT m_rtImage;
	BOOL m_bTransparent;
	COLORREF m_crTransparent;
	BOOL m_bStretch;
};
