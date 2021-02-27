#include "StdAfx.h"
#include "SkinDraggableControl.h"
#include "SkinWindow.h"

CSkinDraggableControl::CSkinDraggableControl(void)
{
	m_bDraggable = FALSE;
}

CSkinDraggableControl::~CSkinDraggableControl(void)
{
}

void CSkinDraggableControl::SetDraggable(BOOL bDraggable)
{
	m_bDraggable = bDraggable;
}

BOOL CSkinDraggableControl::GetDraggable()
{
	return m_bDraggable;
}

LRESULT CSkinDraggableControl::NotifyMessage(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL & bHandled)
{
	LRESULT lRet = CSkinControl::NotifyMessage(uMsg, wParam, lParam, bHandled);

	if(m_bDraggable && uMsg == WM_LBUTTONDOWN)
		SendMessage(GetParentWindow()->GetHWND(), WM_NCLBUTTONDOWN, (WPARAM)2, 0);
	return lRet;
}

BOOL CSkinDraggableControl::ApplyXML(XMLNS::IXMLDOMNodePtr spNode)
{
	if(!CSkinControl::ApplyXML(spNode))
		return FALSE;

	BOOL bValue;
	XMLNS::IXMLDOMNodePtr spAttr;
	spAttr = spNode->attributes->getNamedItem(L"draggable");
	if(spAttr)
	{
		if(!StringToBOOL(spAttr->Gettext(), &bValue))
			return FALSE;
		SetDraggable(bValue);
	}
	return TRUE;
}