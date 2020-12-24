// DoPro.h : main header file for the DOPRO application
//

#if !defined(AFX_DOPRO_H__13C8E0C2_C04A_4077_85DC_B38EFCC696FA__INCLUDED_)
#define AFX_DOPRO_H__13C8E0C2_C04A_4077_85DC_B38EFCC696FA__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CDoProApp:
// See DoPro.cpp for the implementation of this class
//

class CDoProApp : public CWinApp
{
public:
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	CDoProApp();
	int m_bCheck;				// Always Top Menu
	int m_bAuto;				// Auto Start

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDoProApp)
	public:
	virtual BOOL InitInstance();
	//}}AFX_VIRTUAL

// Implementation
	//{{AFX_MSG(CDoProApp)
	afx_msg void OnAppAbout();
	afx_msg void OnViewTop();
	afx_msg void OnUpdateViewTop(CCmdUI* pCmdUI);
	afx_msg void OnAppAuto();
	afx_msg void OnUpdateAppAuto(CCmdUI* pCmdUI);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DOPRO_H__13C8E0C2_C04A_4077_85DC_B38EFCC696FA__INCLUDED_)
