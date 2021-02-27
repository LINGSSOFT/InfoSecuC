#if !defined(AFX_DIALOGUPDATE_H__BEC1BA17_EFF4_4329_BB0A_4E6A5F3C2053__INCLUDED_)
#define AFX_DIALOGUPDATE_H__BEC1BA17_EFF4_4329_BB0A_4E6A5F3C2053__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// DialogUpdate.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CDialogUpdate dialog

class CDialogUpdate : public CDialog
{
// Construction
public:
	CDialogUpdate(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CDialogUpdate)
	enum { IDD = IDD_DIALOG_UPDATE };
	CEdit	m_edNew;
	CEdit	m_edCurr;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDialogUpdate)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CDialogUpdate)
	afx_msg void OnButtonUpdate();
	afx_msg void OnButtonUpdatedetail();
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DIALOGUPDATE_H__BEC1BA17_EFF4_4329_BB0A_4E6A5F3C2053__INCLUDED_)
