// DoSysDoc.h : interface of the CDoSysDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_DOSYSDOC_H__FCECF31A_26FF_4CAA_94AA_646718BCAFFB__INCLUDED_)
#define AFX_DOSYSDOC_H__FCECF31A_26FF_4CAA_94AA_646718BCAFFB__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CDoSysDoc : public CDocument
{
protected: // create from serialization only
	CDoSysDoc();
	DECLARE_DYNCREATE(CDoSysDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDoSysDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CDoSysDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CDoSysDoc)
	afx_msg void OnPwrLogoff();
	afx_msg void OnPwrReboot();
	afx_msg void OnPwrOff();
	afx_msg void OnHelpManual();
	afx_msg void OnHelpUpdate();
	afx_msg void OnHelpError();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DOSYSDOC_H__FCECF31A_26FF_4CAA_94AA_646718BCAFFB__INCLUDED_)
