#pragma once

#include "SkinControl.h"

class CSkinWindow : public CSkinControl
{
public:
	CSkinWindow(void);
	virtual ~CSkinWindow(void);

	SET_CLASS_NAME(_T("CSkinWindow"), CSkinControl)

	HWND Attach(HWND hWnd, BOOL bRelocate);
	HWND Detach();
	HWND GetHWND();
	CSkinWindow * AddDlgCtlItem(int nDlgCtlID, int nAnchors);
	void SetDoubleBuffered(BOOL bDoubleBuffered);
	BOOL IsChildWindow();
	void SetClipChildren(BOOL bClipChildrent);
	void SetSupportDoubleClick(BOOL bSupport);
	void SetUseDefaultPaint(BOOL bDefault);
	void SetConstraints(int nMinWidth, int nMinHeight, int nMaxWidth, int nMaxHeight);
	void GetConstraints(RECT * prtConstraints);
	void SetRedraw(BOOL bRedraw);
	BOOL GetRedraw();

	CSkinControl * SetControlCapture(CSkinControl * pControl);
	CSkinControl * GetControlCapture();
	BOOL ReleaseControlCapture();

protected:
	virtual BOOL ApplyXML(XMLNS::IXMLDOMNodePtr spNode);

	void DrawChildControls(HDC hDC, RECT * prtUpdate, int nOffsetX, int nOffsetY);
	void AddToolTip(LPCTSTR pszToolTip, RECT * prtToolTip = NULL);
	void AddToolTip(CSkinControl * pControl);
	void DeleteToolTip();
	void SetOverControl(CSkinControl * pControl);
	void TrackMouse(DWORD dwFlag);
	void NotifyParentWindowAttach(CSkinWindow * pWindow, CSkinControl * pControl);

	static LRESULT __stdcall InternalWndProc(HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam);
	virtual LRESULT WndProc(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL & bHandled);

	virtual LRESULT OnMouseMessage(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL & bHandled);
	virtual LRESULT OnPaint(WPARAM wParam, LPARAM lParam, BOOL & bHandled);
	virtual LRESULT OnWindowPosChanged(WPARAM wParam, LPARAM lParam, BOOL & bHandled);
	virtual LRESULT OnSize(WPARAM wParam, LPARAM lParam, BOOL & bHandled);
	virtual LRESULT OnEraseBkGnd(WPARAM wParam, LPARAM lParam, BOOL & bHandled);
	virtual LRESULT OnMouseLeave(WPARAM wParam, LPARAM lParam, BOOL & bHandled);
	virtual LRESULT OnGetMinMaxInfo(WPARAM wParam, LPARAM lParam, BOOL & bHandled);

	HWND m_hWnd;
	WNDPROC m_OldProc;
	BOOL m_bDoubleBuffered;
	BOOL m_bClipChildren;
	CSkinControl * m_pCapturedControl;
	CSkinControl * m_pOverControl;
	HWND m_hToolTipWnd;
	BOOL m_bSupportDoubleClick;
	BOOL m_bMouseEnter;
	BOOL m_bUseDefaultPaint;
	BOOL m_bClientSize;
	int m_nMinWidth;
	int m_nMinHeight;
	int m_nMaxWidth;
	int m_nMaxHeight;
	BOOL m_bRedraw;

};
