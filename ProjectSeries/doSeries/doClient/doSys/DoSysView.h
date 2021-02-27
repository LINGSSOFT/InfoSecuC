// DoSysView.h : interface of the CDoSysView class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_DOSYSVIEW_H__18B4E51B_AA50_4CF9_9C1B_A10B3D843A49__INCLUDED_)
#define AFX_DOSYSVIEW_H__18B4E51B_AA50_4CF9_9C1B_A10B3D843A49__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "./Lib/DoListCtrl.h"

class CDoSysView : public CFormView
{
protected: // create from serialization only
	CDoSysView();
	DECLARE_DYNCREATE(CDoSysView)

public:
	//{{AFX_DATA(CDoSysView)
	enum { IDD = IDD_DOSYS_FORM };
	CTreeCtrl	m_TreeSystem;
	CTreeCtrl	m_TreeRegist;
	CTreeCtrl	m_TreePacket;
	CTreeCtrl	m_TreeNetwork;
	CDoListCtrl	m_ListSystem;
	CDoListCtrl	m_ListService;
	CDoListCtrl	m_ListRegist;
	CDoListCtrl	m_ListPacket;
	CDoListCtrl	m_ListNetwork;
	CDoListCtrl	m_ListInstall;
	CTabCtrl	m_TabMain;
	//}}AFX_DATA

// Attributes
public:
	CDoSysDoc* GetDocument();
	int m_nTabNum;					// Current Tab Number
	CImageList *m_pImageList;		// Tab ImageList
	CImageList m_ImgListSystem;		// System ListImage
	CImageList m_ImgTreeSystem;		// System TreeImage

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDoSysView)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual void OnInitialUpdate(); // called first time after construct
	//}}AFX_VIRTUAL

// Implementation
public:
	void LoadService();
	void LoadInstall();
	void SetWindow();
	virtual ~CDoSysView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CDoSysView)
	afx_msg void OnPaint();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnSelchangeTabMain(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnDblclkListInstall(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in DoSysView.cpp
inline CDoSysDoc* CDoSysView::GetDocument()
   { return (CDoSysDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DOSYSVIEW_H__18B4E51B_AA50_4CF9_9C1B_A10B3D843A49__INCLUDED_)
