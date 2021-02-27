#pragma once

#include "SkinDraggableControl.h"

class CSkinRectangleControl : public CSkinDraggableControl
{
public:
	CSkinRectangleControl(void);
	virtual ~CSkinRectangleControl(void);

	SET_CLASS_NAME(_T("CSkinRectangleControl"), CSkinDraggableControl)

	void SetLineColor(COLORREF crLine);
	void SetLineWidth(int nWidth);
	void SetBrushColor(COLORREF crBrush);
	void SetHollowBrush(BOOL bHollow);
	void SetRoundSize(int cx, int cy);

	virtual void DrawControl(HDC hDC);
	virtual BOOL ApplyXML(XMLNS::IXMLDOMNodePtr spNode);

protected:
	COLORREF m_crLine;
	COLORREF m_crBrush;
	BOOL m_bHollow;
	SIZE m_sizeRound;
	int m_nLineWidth;
	int m_nGridUse;
};
