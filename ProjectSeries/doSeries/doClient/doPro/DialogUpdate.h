#if !defined(AFX_DIALOGUPDATE_H__2C2BB727_A0B8_468A_8E41_F31E427B8FF6__INCLUDED_)
#define AFX_DIALOGUPDATE_H__2C2BB727_A0B8_468A_8E41_F31E427B8FF6__INCLUDED_

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
	virtual BOOL OnInitDialog();
	afx_msg void OnButtonUpdate();
	afx_msg void OnButtonUpdatedetail();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DIALOGUPDATE_H__2C2BB727_A0B8_468A_8E41_F31E427B8FF6__INCLUDED_)
