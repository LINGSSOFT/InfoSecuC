// DoProView.h : interface of the CDoProView class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_DOPROVIEW_H__03357107_DC1D_4936_B5AD_8581626DCA5F__INCLUDED_)
#define AFX_DOPROVIEW_H__03357107_DC1D_4936_B5AD_8581626DCA5F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
#define MAX_THREAD			4		// Limit Thread

#include "./Lib/DoListCtrl.h"
#include "./Lib/2DPushGraph.h"

class CDoProView : public CFormView
{
protected: // create from serialization only
	CDoProView();
	DECLARE_DYNCREATE(CDoProView)

public:
	//{{AFX_DATA(CDoProView)
	enum { IDD = IDD_DOPRO_FORM };
	CTreeCtrl	m_TreeRegist;
	CTabCtrl	m_TabMain;
	CDoListCtrl	m_ListRun;
	CDoListCtrl	m_ListProcess;
	CDoListCtrl	m_ListLog;
	CDoListCtrl	m_ListInstall;
	CDoListCtrl	m_ListService;
	CDoListCtrl	m_ListRegist;
	//}}AFX_DATA

// Attributes
public:
	CDoProDoc* GetDocument();
	int m_nTabNum;					// Current Tab Number
	CImageList *m_pImageList;		// Tab ImageList
	CImageList m_ImgList;			// Process ListImage
	CImageList m_ImgListRun;		// Process ListImage
	CImageList m_ImgListInstall;	// Install ListImage
	CImageList m_ImgListService;	// Process ListImage
	CImageList m_ImgListRegist;		// Regist ListImage
	CImageList m_ImgTreeRegist;		// Regist TreeImage
	HANDLE hThread[MAX_THREAD];		// Thread
	BOOL bStart;					// Tree Change Chk

private:
	C2DPushGraph m_PushCpu,m_PushMemory;		// 2DPushGraph Cpu, Memory

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDoProView)
	public:
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual void OnInitialUpdate(); // called first time after construct
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnPrint(CDC* pDC, CPrintInfo* pInfo);
	//}}AFX_VIRTUAL

// Implementation
public:
	void LoadTreeChild(CTreeCtrl *m_pTreeCtrl, HTREEITEM hParent, int nType, CString strPath);
	void LoadTreeReg(CTreeCtrl *m_pTreeCtrl);
	void LoadInstall();
	void LoadService();
	void DeleteList(DWORD sPid, INT iIndex);
	BOOL SafeTerminateProcess(HANDLE hProcess, UINT uExitCode);
	void RunThreadProc();
	int LoadMemoryDetailsForProcess(HANDLE Process);
	int LoadMemoryDetailsForProcess();
	// Test function
	HRESULT CheckIfIsUserAdmin(BOOL* pIsAdmin);
	BOOL IsCurrentUserLocalAdministrator();
	void EnableSystemPriv(void);

	static UINT WINAPI ListRun(void *arg);
	static UINT WINAPI GraphStartCpu(void *arg);
	static UINT WINAPI GraphStartMemory(void *arg);
	static UINT WINAPI ListRefresh(void *arg);

	void SetWindow();

	static BOOL CALLBACK enumwndfn(HWND hWnd, LPARAM lParam);
	static BOOL CALLBACK EnumWindowsProc(HWND hwnd, LPARAM lParam);
//	static int CALLBACK SortFunc(LPARAM lParam1, LPARAM lParam2, LPARAM lParamSort);
	BOOL enumwndfn(CWnd * wnd);

	virtual ~CDoProView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	//{{AFX_MSG(CDoProView)
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnSelchangeTabMain(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnAppLogview();
	afx_msg void OnAppLogdel();
	afx_msg void OnDblclkListProcess(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnDblclkListRun(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnRclickListProcess(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnContextKill();
	afx_msg void OnContextDetail();
	afx_msg void OnViewRefresh();
	afx_msg void OnPaint();
	afx_msg void OnSelchangedTreeRegist(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnClickTreeRegist(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnDblclkListInstall(NMHDR* pNMHDR, LRESULT* pResult);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in DoProView.cpp
inline CDoProDoc* CDoProView::GetDocument()
   { return (CDoProDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DOPROVIEW_H__03357107_DC1D_4936_B5AD_8581626DCA5F__INCLUDED_)
