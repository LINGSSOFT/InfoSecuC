// DoManagerDoc.h : interface of the CDoManagerDoc class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_DOMANAGERDOC_H__CD337621_9811_4C6B_82B0_4A9864996216__INCLUDED_)
#define AFX_DOMANAGERDOC_H__CD337621_9811_4C6B_82B0_4A9864996216__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


class CDoManagerDoc : public CDocument
{
protected: // create from serialization only
	CDoManagerDoc();
	DECLARE_DYNCREATE(CDoManagerDoc)

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDoManagerDoc)
	public:
	virtual BOOL OnNewDocument();
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CDoManagerDoc();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CDoManagerDoc)
	afx_msg void OnPwrLogoff();
	afx_msg void OnPwrReboot();
	afx_msg void OnPwrOff();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DOMANAGERDOC_H__CD337621_9811_4C6B_82B0_4A9864996216__INCLUDED_)
