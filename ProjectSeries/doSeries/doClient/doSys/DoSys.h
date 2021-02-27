// DoSys.h : main header file for the DOSYS application
//

#if !defined(AFX_DOSYS_H__3E98D519_5D9B_45EB_A458_CAE7124B11B7__INCLUDED_)
#define AFX_DOSYS_H__3E98D519_5D9B_45EB_A458_CAE7124B11B7__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CDoSysApp:
// See DoSys.cpp for the implementation of this class
//

class CDoSysApp : public CWinApp
{
public:
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	CDoSysApp();
	int m_bCheck;				// Always Top Menu
	int m_bAuto;				// Auto Start

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDoSysApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	//{{AFX_MSG(CDoSysApp)
	afx_msg void OnAppAbout();
	afx_msg void OnAppAuto();
	afx_msg void OnUpdateAppAuto(CCmdUI* pCmdUI);
	afx_msg void OnViewTop();
	afx_msg void OnUpdateViewTop(CCmdUI* pCmdUI);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DOSYS_H__3E98D519_5D9B_45EB_A458_CAE7124B11B7__INCLUDED_)
