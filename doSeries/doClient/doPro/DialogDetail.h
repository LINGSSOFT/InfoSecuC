#if !defined(AFX_DIALOGDETAIL_H__5FFFD136_ED88_48A3_B686_EC61D00440DE__INCLUDED_)
#define AFX_DIALOGDETAIL_H__5FFFD136_ED88_48A3_B686_EC61D00440DE__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// DialogDetail.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CDialogDetail dialog

class CDialogDetail : public CDialog
{
// Construction
public:
	CDialogDetail(CWnd* pParent = NULL);   // standard constructor
	DWORD m_PID;

// Dialog Data
	//{{AFX_DATA(CDialogDetail)
	enum { IDD = IDD_DIALOG_DETAIL };
	CStatic	m_ImageIcon;
	//}}AFX_DATA


// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDialogDetail)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:

	// Generated message map functions
	//{{AFX_MSG(CDialogDetail)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DIALOGDETAIL_H__5FFFD136_ED88_48A3_B686_EC61D00440DE__INCLUDED_)
