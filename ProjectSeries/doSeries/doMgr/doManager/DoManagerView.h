// DoManagerView.h : interface of the CDoManagerView class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_DOMANAGERVIEW_H__04818BB8_F6EE_4EF9_9269_679528D6027F__INCLUDED_)
#define AFX_DOMANAGERVIEW_H__04818BB8_F6EE_4EF9_9269_679528D6027F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "./Lib/DoListCtrl.h"

class CDoManagerView : public CFormView
{
protected: // create from serialization only
	CDoManagerView();
	DECLARE_DYNCREATE(CDoManagerView)

public:
	//{{AFX_DATA(CDoManagerView)
	enum { IDD = IDD_DOMANAGER_FORM };
	CDoListCtrl	m_ListKey;
	CDoListCtrl	m_ListWeb;
	CDoListCtrl	m_ListFile;
	CTabCtrl	m_TabMain;
	//}}AFX_DATA

// Attributes
public:
	CDoManagerDoc* GetDocument();
	int m_nTabNum;				// Current Tab Number
	CImageList *m_pImageList;	// Tab ImageList
	CImageList m_ImgKey;		// Key ListImage
	CImageList m_ImgWeb;		// Web ListImage
	CImageList m_ImgFile;		// File ListImage

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDoManagerView)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual void OnInitialUpdate(); // called first time after construct
	//}}AFX_VIRTUAL

// Implementation
public:
	void SetWindow();
	virtual ~CDoManagerView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CDoManagerView)
	afx_msg void OnPaint();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnSelchangeTabMain(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in DoManagerView.cpp
inline CDoManagerDoc* CDoManagerView::GetDocument()
   { return (CDoManagerDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DOMANAGERVIEW_H__04818BB8_F6EE_4EF9_9269_679528D6027F__INCLUDED_)
