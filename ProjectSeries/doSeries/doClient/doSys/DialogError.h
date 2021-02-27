#if !defined(AFX_DIALOGERROR_H__512A9389_F8D6_4AD7_8A55_ADEE582246BD__INCLUDED_)
#define AFX_DIALOGERROR_H__512A9389_F8D6_4AD7_8A55_ADEE582246BD__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// DialogError.h : header file
//

#include "./Lib/DoListCtrl.h"

/////////////////////////////////////////////////////////////////////////////
// CDialogError dialog

class CDialogError : public CDialog
{
// Construction
public:
	CDialogError(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CDialogError)
	enum { IDD = IDD_DIALOG_ERROR };
	CDoListCtrl	m_ListError;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDialogError)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CDialogError)
	virtual BOOL OnInitDialog();
	afx_msg void OnButtonSend();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DIALOGERROR_H__512A9389_F8D6_4AD7_8A55_ADEE582246BD__INCLUDED_)
