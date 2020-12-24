// DoManagerView.cpp : implementation of the CDoManagerView class
//

#include "stdafx.h"
#include "DoManager.h"

#include "DoManagerDoc.h"
#include "DoManagerView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDoManagerView

IMPLEMENT_DYNCREATE(CDoManagerView, CFormView)

BEGIN_MESSAGE_MAP(CDoManagerView, CFormView)
	//{{AFX_MSG_MAP(CDoManagerView)
	ON_WM_PAINT()
	ON_WM_SIZE()
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB_MAIN, OnSelchangeTabMain)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDoManagerView construction/destruction

CDoManagerView::CDoManagerView()
	: CFormView(CDoManagerView::IDD)
{
	//{{AFX_DATA_INIT(CDoManagerView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here
	m_nTabNum = 0;
}

CDoManagerView::~CDoManagerView()
{
}

void CDoManagerView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDoManagerView)
	DDX_Control(pDX, IDC_LIST_WEB, m_ListWeb);
	DDX_Control(pDX, IDC_LIST_KEY, m_ListKey);
	DDX_Control(pDX, IDC_LIST_FILE, m_ListFile);
	DDX_Control(pDX, IDC_TAB_MAIN, m_TabMain);
	//}}AFX_DATA_MAP
}

BOOL CDoManagerView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

void CDoManagerView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	GetParentFrame()->RecalcLayout();
	ResizeParentToFit();

	// Todo User Coding..
	CString strLS;

	// TabCtrl Setting
	m_pImageList = new CImageList;
	m_pImageList->Create(16, 16, ILC_COLOR16 | ILC_MASK, 3, 3);
/*	m_pImageList->Add(AfxGetApp()->LoadIcon(IDI_TAB1));
	m_pImageList->Add(AfxGetApp()->LoadIcon(IDI_TAB2));
	m_pImageList->Add(AfxGetApp()->LoadIcon(IDI_TAB3));
*/	m_TabMain.SetImageList(m_pImageList);

	CString tabSrting1, tabSrting2, tabSrting3, tabSrting4;
	tabSrting1.LoadString(IDS_STRING61207);
	tabSrting2.LoadString(IDS_STRING61208);
	tabSrting3.LoadString(IDS_STRING61209);
	tabSrting4.LoadString(IDS_STRING61210);

	TC_ITEM item;
	char *tabname[4] = {LPSTR(LPCTSTR(tabSrting1)), LPSTR(LPCTSTR(tabSrting2)), LPSTR(LPCTSTR(tabSrting3)), LPSTR(LPCTSTR(tabSrting4))};
	for(int i = 0; i < 4; i++)
	{
		item.mask=TCIF_TEXT|TCIF_IMAGE;
		item.pszText=tabname[i];
		item.iImage=i;
		m_TabMain.InsertItem(i, &item);
	}

	// Web ListCtrl Setting
	m_ListWeb.SetExtendedStyle(m_ListWeb.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
	m_ImgWeb.Create( GetSystemMetrics(SM_CXSMICON),
		          GetSystemMetrics(SM_CYSMICON),
				  ILC_COLOR24|ILC_MASK, 50, 50);
	m_ImgWeb.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ListWeb.SetImageList(&m_ImgWeb, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61211);
	m_ListWeb.SetHeadings(strLS);
	m_ListWeb.LoadColumnInfo();
	
	// File ListCtrl Setting
	m_ListFile.SetExtendedStyle(m_ListFile.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
	m_ImgFile.Create( GetSystemMetrics(SM_CXSMICON),
		          GetSystemMetrics(SM_CYSMICON),
				  ILC_COLOR24|ILC_MASK, 50, 50);
	m_ImgFile.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ListFile.SetImageList(&m_ImgFile, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61212);
	m_ListFile.SetHeadings(strLS);
	m_ListFile.LoadColumnInfo();

	// Key ListCtrl Setting
	m_ListKey.SetExtendedStyle(m_ListKey.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
	m_ImgKey.Create( GetSystemMetrics(SM_CXSMICON),
		          GetSystemMetrics(SM_CYSMICON),
				  ILC_COLOR24|ILC_MASK, 50, 50);
	m_ImgKey.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ListKey.SetImageList(&m_ImgKey, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61213);
	m_ListKey.SetHeadings(strLS);
	m_ListKey.LoadColumnInfo();

	HANDLE hToken;	// Get System Process
	if(!::OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &hToken))
		OutputDebugString("Token Error!");

	TOKEN_PRIVILEGES tp;
	tp.PrivilegeCount = 1;

	if(LookupPrivilegeValue(0, SE_DEBUG_NAME, &tp.Privileges[0].Luid))
	{
		DWORD dwSize = sizeof(TOKEN_PRIVILEGES);
		tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
		if(!AdjustTokenPrivileges(hToken, FALSE, &tp, 0, (PTOKEN_PRIVILEGES)NULL, 0))
			OutputDebugString("AdjustTokenPrivileges Error\n");
	}
	CloseHandle(hToken);

	SetWindow();	// Resize
}

/////////////////////////////////////////////////////////////////////////////
// CDoManagerView diagnostics

#ifdef _DEBUG
void CDoManagerView::AssertValid() const
{
	CFormView::AssertValid();
}

void CDoManagerView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CDoManagerDoc* CDoManagerView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CDoManagerDoc)));
	return (CDoManagerDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CDoManagerView message handlers
void CDoManagerView::OnPaint() 
{
	CPaintDC dc(this); // device context for painting
	
	// TODO: Add your message handler code here
	m_ListWeb.Invalidate(TRUE);
	m_ListFile.Invalidate(TRUE);
	m_ListKey.Invalidate(TRUE);
	// Do not call CFormView::OnPaint() for painting messages
}

void CDoManagerView::OnSize(UINT nType, int cx, int cy) 
{
	CFormView::OnSize(nType, cx, cy);
	
	// TODO: Add your message handler code here
	if(m_TabMain.m_hWnd != NULL)		// Tab Control
	{
		CRect rc(0, 0, cx, cy);
		m_TabMain.MoveWindow(&rc);
	}

	if(m_ListWeb.m_hWnd != NULL)		// Web ListControl
	{
		CRect rc(0, 21, cx, cy);
		m_ListWeb.MoveWindow(&rc);
	}

	if(m_ListFile.m_hWnd != NULL)		// File ListControl
	{
		CRect rc(0, 21, cx, cy);
		m_ListFile.MoveWindow(&rc);
	}

	if(m_ListKey.m_hWnd != NULL)		// Key ListControl
	{
		CRect rc(0, 21, cx, cy);
		m_ListKey.MoveWindow(&rc);
	}
}

void CDoManagerView::OnSelchangeTabMain(NMHDR* pNMHDR, LRESULT* pResult) 
{
	// TODO: Add your control notification handler code here
	m_nTabNum = m_TabMain.GetCurSel();
	SetWindow();
	*pResult = 0;
}

void CDoManagerView::SetWindow()
{
	switch(m_nTabNum)
	{
		case 0:
			GetDlgItem(IDC_LIST_WEB)->ShowWindow(SW_SHOW);			// WEB List

			GetDlgItem(IDC_LIST_FILE)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_KEY)->ShowWindow(SW_HIDE);
			break;
		case 1:
			GetDlgItem(IDC_LIST_FILE)->ShowWindow(SW_SHOW);			// FILE List

			GetDlgItem(IDC_LIST_WEB)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_KEY)->ShowWindow(SW_HIDE);
			break;
		case 2:
			GetDlgItem(IDC_LIST_KEY)->ShowWindow(SW_SHOW);			// KEY List

			GetDlgItem(IDC_LIST_WEB)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_FILE)->ShowWindow(SW_HIDE);
			break;
	}
}
