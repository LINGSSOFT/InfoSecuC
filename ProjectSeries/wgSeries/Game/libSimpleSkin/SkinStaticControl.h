#pragma once

#include "SkinControl.h"
#include "SkinFont.h"
#include "SkinDraggableControl.h"

class CSkinStaticControl : public CSkinDraggableControl
{
public:
	CSkinStaticControl(void);
	virtual ~CSkinStaticControl(void);

	SET_CLASS_NAME(_T("CSkinStaticControl"), CSkinDraggableControl)

	void SetText(LPCTSTR pszText);
	LPCTSTR GetText();
	void SetColor(COLORREF crFont);
	COLORREF GetColor();
	void SetAutoSize(BOOL bAutoSize);
	void SetDrawTextFlag(DWORD dwFlag);
	DWORD GetDrawTextFlag();
	void SetFont(LPCTSTR pszFontName, int nFontSize, DWORD dwFontStyle);
	void SetFont(CSkinFont * pFont);
	CSkinFont * GetFont();
	int GetTextWidth();
	void CalcSize();

protected:
	virtual void DrawControl(HDC hDC);
	virtual BOOL ApplyXML(XMLNS::IXMLDOMNodePtr spNode);
	virtual void OnParentWindowAttached(CSkinWindow * pWindow);

	CSkinFont * m_pFont;
	CSkinFont * m_pFont2;
	COLORREF m_crFont;
	BOOL m_bAutoSize;
	TCHAR * m_pszText;
	DWORD m_dwFlag;
//	BOOL m_bNeedToRecalc;
};
