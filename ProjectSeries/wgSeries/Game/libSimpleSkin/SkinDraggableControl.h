#pragma once

#include "SkinControl.h"

class CSkinDraggableControl  : public CSkinControl
{
public:
	CSkinDraggableControl(void);
	virtual ~CSkinDraggableControl(void);

	SET_CLASS_NAME(_T("CSkinDraggableControl"), CSkinControl)

	void SetDraggable(BOOL bDraggable);
	BOOL GetDraggable();

	virtual LRESULT NotifyMessage(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL & bHandled);
	virtual BOOL ApplyXML(XMLNS::IXMLDOMNodePtr spNode);

protected:
	BOOL m_bDraggable;

};
