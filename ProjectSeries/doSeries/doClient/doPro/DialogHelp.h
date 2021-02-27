#if !defined(AFX_DIALOGHELP_H__BA7FB81E_499C_4B6A_BA75_95AA8CDCB4D2__INCLUDED_)
#define AFX_DIALOGHELP_H__BA7FB81E_499C_4B6A_BA75_95AA8CDCB4D2__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// DialogHelp.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CDialogHelp dialog

class CDialogHelp : public CDialog
{
// Construction
public:
	CDialogHelp(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CDialogHelp)
	enum { IDD = IDD_DIALOG_HELP };
	CEdit	m_edHelp;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDialogHelp)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CDialogHelp)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DIALOGHELP_H__BA7FB81E_499C_4B6A_BA75_95AA8CDCB4D2__INCLUDED_)
