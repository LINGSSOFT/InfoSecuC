// DoProDoc.h : interface of the CDoProDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_DOPRODOC_H__42A3D67C_05EC_4758_B955_60B2BE96D20D__INCLUDED_)
#define AFX_DOPRODOC_H__42A3D67C_05EC_4758_B955_60B2BE96D20D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CDoProDoc : public CDocument
{
protected: // create from serialization only
	CDoProDoc();
	DECLARE_DYNCREATE(CDoProDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDoProDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CDoProDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CDoProDoc)
	afx_msg void OnPwrLogoff();
	afx_msg void OnPwrOff();
	afx_msg void OnPwrReboot();
	afx_msg void OnHelpManual();
	afx_msg void OnHelpError();
	afx_msg void OnHelpUpdate();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DOPRODOC_H__42A3D67C_05EC_4758_B955_60B2BE96D20D__INCLUDED_)
