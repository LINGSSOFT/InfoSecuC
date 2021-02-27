// DoProView.cpp : implementation of the CDoProView class
//

#include "stdafx.h"
#include "DoPro.h"

#include "DoProDoc.h"
#include "DoProView.h"

#include "DialogDetail.h"

#include "process.h"
#include "./Lib/CpuUsage.h"
#include "./Lib/util.h"
#include "tlhelp32.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#include "./Lib/FileVersionInfo.h"
#pragma comment(lib,"version.lib")

#include "psapi.h"  /// PSAPI header for computer name and login user name
#pragma comment(lib,"psapi.lib")

#include "winsvc.h"
#pragma comment(lib,"advapi32.lib")

BOOL GetFileDateAsString( LPFILETIME pFt, char * pszDate, unsigned cbIn);
BOOL GetFileTimeAsString( LPFILETIME pFt, char * pszTime, unsigned cbIn);

#define PUSHGRAPH_SYSTEM_LINE	0		// Graph First Line
#define PUSHGRAPH_PROCESS_LINE	1		// Graph Second Line After Later Accept
#define MAXLOGCOUNT				100		// LogList Max Count
#define INTERVAL_TIME			1000	// Thread Time

/////////////////////////////////////////////////////////////////////////////
// CDoProView

IMPLEMENT_DYNCREATE(CDoProView, CFormView)

BEGIN_MESSAGE_MAP(CDoProView, CFormView)
	//{{AFX_MSG_MAP(CDoProView)
	ON_WM_SIZE()
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB_MAIN, OnSelchangeTabMain)
	ON_COMMAND(ID_APP_LOGVIEW, OnAppLogview)
	ON_COMMAND(ID_APP_LOGDEL, OnAppLogdel)
	ON_NOTIFY(NM_DBLCLK, IDC_LIST_PROCESS, OnDblclkListProcess)
	ON_NOTIFY(NM_DBLCLK, IDC_LIST_RUN, OnDblclkListRun)
	ON_NOTIFY(NM_RCLICK, IDC_LIST_PROCESS, OnRclickListProcess)
	ON_COMMAND(ID_CONTEXT_KILL, OnContextKill)
	ON_COMMAND(ID_CONTEXT_DETAIL, OnContextDetail)
	ON_COMMAND(ID_VIEW_REFRESH, OnViewRefresh)
	ON_WM_PAINT()
	ON_NOTIFY(TVN_SELCHANGED, IDC_TREE_REGIST, OnSelchangedTreeRegist)
	ON_NOTIFY(NM_CLICK, IDC_TREE_REGIST, OnClickTreeRegist)
	ON_NOTIFY(NM_DBLCLK, IDC_LIST_INSTALL, OnDblclkListInstall)
	//}}AFX_MSG_MAP
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CFormView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, CFormView::OnFilePrintPreview)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDoProView construction/destruction

CDoProView::CDoProView()
	: CFormView(CDoProView::IDD)
{
	//{{AFX_DATA_INIT(CDoProView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here
	m_nTabNum = 0;
}

CDoProView::~CDoProView()
{
	delete m_pImageList;
	if(hThread[3]!=NULL)
	{
		TerminateThread(hThread[3], 0);
		CloseHandle(hThread[3]);
	}
	if(hThread[2]!=NULL)
	{
		TerminateThread(hThread[2], 0);
		CloseHandle(hThread[2]);
	}
	if(hThread[1]!=NULL)
	{
		TerminateThread(hThread[1], 0);
		CloseHandle(hThread[1]);
	}
	if(hThread[0]!=NULL)
	{
		TerminateThread(hThread[0], 0);
		CloseHandle(hThread[0]);
	}
}

void CDoProView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDoProView)
	DDX_Control(pDX, IDC_TREE_REGIST, m_TreeRegist);
	DDX_Control(pDX, IDC_TAB_MAIN, m_TabMain);
	DDX_Control(pDX, IDC_LIST_RUN, m_ListRun);
	DDX_Control(pDX, IDC_LIST_PROCESS, m_ListProcess);
	DDX_Control(pDX, IDC_LIST_LOG, m_ListLog);
	DDX_Control(pDX, IDC_LIST_INSTALL, m_ListInstall);
	DDX_Control(pDX, IDC_LIST_SERVICE, m_ListService);
	DDX_Control(pDX, IDC_LIST_REGIST, m_ListRegist);
	//}}AFX_DATA_MAP
}

BOOL CDoProView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

void CDoProView::OnInitialUpdate()
{
	CFormView::OnInitialUpdate();
	GetParentFrame()->RecalcLayout();
	ResizeParentToFit();

	// Todo User Coding..
	CString strLS;

	// TabCtrl Setting
	m_pImageList = new CImageList;
	m_pImageList->Create(16, 16, ILC_COLOR16 | ILC_MASK, 6, 6);
	m_pImageList->Add(AfxGetApp()->LoadIcon(IDI_TAB1));
	m_pImageList->Add(AfxGetApp()->LoadIcon(IDI_TAB2));
	m_pImageList->Add(AfxGetApp()->LoadIcon(IDI_TAB3));
	m_pImageList->Add(AfxGetApp()->LoadIcon(IDI_TAB4));
	m_pImageList->Add(AfxGetApp()->LoadIcon(IDI_TAB5));
	m_pImageList->Add(AfxGetApp()->LoadIcon(IDI_TAB6));
	m_pImageList->SetBkColor( GetSysColor(COLOR_MENU) );
	m_TabMain.SetImageList(m_pImageList);

	CString tabSrting1, tabSrting2, tabSrting3, tabSrting4, tabSrting5, tabSrting6;
	tabSrting1.LoadString(IDS_STRING61464);
	tabSrting2.LoadString(IDS_STRING61465);
	tabSrting3.LoadString(IDS_STRING61466);
	tabSrting4.LoadString(IDS_STRING61467);
	tabSrting5.LoadString(IDS_STRING61468);
	tabSrting6.LoadString(IDS_STRING61469);

	TC_ITEM item;
	char *tabname[6] = {LPSTR(LPCTSTR(tabSrting1)), LPSTR(LPCTSTR(tabSrting2)), LPSTR(LPCTSTR(tabSrting3)),
						LPSTR(LPCTSTR(tabSrting4)), LPSTR(LPCTSTR(tabSrting5)), LPSTR(LPCTSTR(tabSrting6))};
	for(int i = 0; i < 6; i++)
	{
		item.mask=TCIF_TEXT|TCIF_IMAGE;
		item.pszText=tabname[i];
		item.iImage=i;
		m_TabMain.InsertItem(i, &item);
	}

	// 1. Process ListCtrl Setting
	m_ListRun.SetExtendedStyle(m_ListRun.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
	m_ImgListRun.Create( GetSystemMetrics(SM_CXSMICON),
		          GetSystemMetrics(SM_CYSMICON),
				  ILC_COLOR24|ILC_MASK, 50, 50);
	m_ImgListRun.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ListRun.SetImageList(&m_ImgListRun, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61448);
	m_ListRun.SetHeadings(strLS);
	m_ListRun.LoadColumnInfo();

	// 2. Process ListCtrl Setting
	m_ListProcess.SetExtendedStyle(m_ListProcess.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
	m_ImgList.Create( GetSystemMetrics(SM_CXSMICON),
		          GetSystemMetrics(SM_CYSMICON),
				  ILC_COLOR24|ILC_MASK, 50, 50);
	m_ImgList.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ListProcess.SetImageList(&m_ImgList, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61449);
	m_ListProcess.SetHeadings(strLS);
	LV_COLUMN lvcolumn;
	lvcolumn.mask = LVCF_FMT;
	lvcolumn.fmt = LVCFMT_RIGHT;
	m_ListProcess.SetColumn(1, &lvcolumn);
	m_ListProcess.SetColumn(3, &lvcolumn);
	m_ListProcess.SetColumn(4, &lvcolumn);
	m_ListProcess.LoadColumnInfo();

	// 3. Log ListCtrl Setting
	m_ListLog.SetExtendedStyle(m_ListLog.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |

	strLS.LoadString(IDS_STRING61450);
	m_ListLog.SetHeadings(strLS);
	m_ListLog.LoadColumnInfo();

	// 4. Service ListCtrl Setting
	m_ListInstall.SetExtendedStyle(m_ListInstall.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
	m_ImgListInstall.Create( GetSystemMetrics(SM_CXSMICON),
		          GetSystemMetrics(SM_CYSMICON),
				  ILC_COLOR24|ILC_MASK, 50, 50);
	m_ImgListInstall.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ImgListInstall.Add(AfxGetApp()->LoadIcon(IDI_INSTALL));
	m_ListInstall.SetImageList(&m_ImgListInstall, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61459);
	m_ListInstall.SetHeadings(strLS);
	m_ListInstall.LoadColumnInfo();

	// 5. Service ListCtrl Setting
	m_ListService.SetExtendedStyle(m_ListService.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
	m_ImgListService.Create( GetSystemMetrics(SM_CXSMICON),
		          GetSystemMetrics(SM_CYSMICON),
				  ILC_COLOR24|ILC_MASK, 50, 50);
	m_ImgListService.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ImgListService.Add(AfxGetApp()->LoadIcon(IDI_SERVICE));
	m_ListService.SetImageList(&m_ImgListService, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61458);
	m_ListService.SetHeadings(strLS);
	m_ListService.LoadColumnInfo();

	// 6. Service ListCtrl & Tree Setting
	m_ListRegist.SetExtendedStyle(m_ListRegist.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
	m_ImgListRegist.Create( GetSystemMetrics(SM_CXSMICON),
		          GetSystemMetrics(SM_CYSMICON),
				  ILC_COLOR24|ILC_MASK, 50, 50);
	m_ImgListRegist.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ListRegist.SetImageList(&m_ImgListRegist, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61460);
	m_ListRegist.SetHeadings(strLS);
	m_ListRegist.LoadColumnInfo();

	m_ImgTreeRegist.Create( GetSystemMetrics(SM_CXSMICON), 
							GetSystemMetrics(SM_CYSMICON), 
							ILC_COLOR24|ILC_MASK, 50, 50);
	m_ImgTreeRegist.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ImgTreeRegist.Add(AfxGetApp()->LoadIcon(IDI_REGCOM));
	m_ImgTreeRegist.Add(AfxGetApp()->LoadIcon(IDI_REGOPEN));
	
	m_TreeRegist.SetImageList( &m_ImgTreeRegist, TVSIL_NORMAL );
	/*
	// 2D GRAPH CPU Setting
	m_PushCpu.CreateFromStatic(IDC_STATICCPU, this);
	m_PushCpu.ModifyStyle(0, WS_THICKFRAME);
	m_PushCpu.AddLine( PUSHGRAPH_SYSTEM_LINE,  RGB(0, 255, 0));
	m_PushCpu.AddLine( PUSHGRAPH_PROCESS_LINE, RGB(255, 255, 0));
	m_PushCpu.ShowLabels(TRUE);
	m_PushCpu.ShowGrid(TRUE);
	m_PushCpu.ShowAsBar(0, false);
	m_PushCpu.ShowAsBar(1, false);
	m_PushCpu.SetInterval(5);
	m_PushCpu.SetGridSize(14);
	m_PushCpu.SetLabelForMax("100%");
	m_PushCpu.SetLabelForMin("0%");
	m_PushCpu.SetPeekRange(0, 100);
	m_PushCpu.SetBGColor(RGB(63,66,60));
	m_PushCpu.SetGridColor(RGB(165,165,165));
	m_PushCpu.SetTextColor(RGB(0,255,0));
	m_PushCpu.SetCaption("CPU");
	m_PushCpu.Push(50,  PUSHGRAPH_SYSTEM_LINE);
	m_PushCpu.Update();

	// 2D GRAPH Memory Setting
	_MEMORYSTATUS lpBuffer;
	::GlobalMemoryStatus(&lpBuffer);
	int MAXRANGE = lpBuffer.dwTotalPhys/1024;

	m_PushMemory.CreateFromStatic(IDC_STATICMEMORY, this);
	m_PushMemory.ModifyStyle(0, WS_THICKFRAME);
	m_PushMemory.AddLine( PUSHGRAPH_SYSTEM_LINE,  RGB(0, 255, 0));
	m_PushMemory.AddLine( PUSHGRAPH_PROCESS_LINE, RGB(255, 255, 0));
	m_PushMemory.ShowLabels(TRUE);
	m_PushMemory.ShowGrid(TRUE);
	m_PushMemory.ShowAsBar(0, false);
	m_PushMemory.ShowAsBar(1, false);
	m_PushMemory.SetInterval(5);
	m_PushMemory.SetGridSize(14);
	m_PushMemory.SetLabelForMax("100%");
	m_PushMemory.SetLabelForMin("0%");
	m_PushMemory.SetPeekRange(0, MAXRANGE);			// Total Memory Check
	m_PushMemory.SetBGColor(RGB(63,66,60));
	m_PushMemory.SetGridColor(RGB(165,165,165));
	m_PushMemory.SetTextColor(RGB(0,255,0));
	m_PushMemory.SetCaption("Memory");
	m_PushMemory.RedrawWindow();
	*/
	//IsCurrentUserLocalAdministrator();

	HANDLE hToken;	// Get System Process
	if(!::OpenProcessToken(GetCurrentProcess(),  TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &hToken))
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

	SetWindow();		// Resize
	RunThreadProc();	// Application, Process, List
	LoadInstall();		// Install
	LoadService();		// Services
	bStart = false;
	LoadTreeReg(&m_TreeRegist);
	bStart = true;
	EnableSystemPriv();
}

/////////////////////////////////////////////////////////////////////////////
// CDoProView printing

BOOL CDoProView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// default preparation
	return DoPreparePrinting(pInfo);
}

void CDoProView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add extra initialization before printing
}

void CDoProView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add cleanup after printing
}

void CDoProView::OnPrint(CDC* pDC, CPrintInfo* /*pInfo*/)
{
	// TODO: add customized printing code here
}

/////////////////////////////////////////////////////////////////////////////
// CDoProView diagnostics

#ifdef _DEBUG
void CDoProView::AssertValid() const
{
	CFormView::AssertValid();
}

void CDoProView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CDoProDoc* CDoProView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CDoProDoc)));
	return (CDoProDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CDoProView message handlers
void CDoProView::OnPaint()
{
	CPaintDC dc(this); // device context for painting

	// TODO: Add your message handler code here
//	m_PushCpu.Invalidate(TRUE);
//	m_PushMemory.Invalidate(TRUE);
	m_ListRun.Invalidate(TRUE);
	m_ListProcess.Invalidate(TRUE);
	m_ListLog.Invalidate(TRUE);
	m_ListInstall.Invalidate(TRUE);
	m_ListService.Invalidate(TRUE);
	m_ListRegist.Invalidate(TRUE);
	m_TreeRegist.Invalidate(TRUE);
	// Do not call CFormView::OnPaint() for painting messages
}

void CDoProView::OnSize(UINT nType, int cx, int cy)
{
	CFormView::OnSize(nType, cx, cy);

	// TODO: Add your message handler code here
	if(m_TabMain.m_hWnd != NULL)		// Tab Control
	{
		CRect rc(0, 0, cx, cy);
		m_TabMain.MoveWindow(&rc);
	}
/*
	if(m_PushCpu.m_hWnd != NULL)		// Cpu Control
	{
		CRect rc(0, 23, cx/2-1, 101);
		m_PushCpu.MoveWindow(&rc);
	}

	if(m_PushMemory.m_hWnd != NULL)		// Memory Control
	{
		CRect rc(cx/2, 23, cx, 101);
		m_PushMemory.MoveWindow(&rc);
	}
*/
	if(m_ListRun.m_hWnd != NULL)		// Run List Control
	{
		CRect rc(0, 23, cx, cy);
		m_ListRun.MoveWindow(&rc);
		m_ListRun.Resize(rc.Width(), rc.Height(), 0);
	}

	if(m_ListProcess.m_hWnd != NULL)	// Process List Control
	{
		CRect rc(0, 101, cx, cy);
		m_ListProcess.MoveWindow(&rc);
		m_ListProcess.Resize(rc.Width(), rc.Height(), 1);
	}

	if(m_ListLog.m_hWnd != NULL)		// Log List Control
	{
		CRect rc(0, 23, cx, cy);
		m_ListLog.MoveWindow(&rc);
		m_ListLog.Resize(rc.Width(), rc.Height(), 2);
	}

	if(m_ListInstall.m_hWnd != NULL)	// Install List Control
	{
		CRect rc(0, 23, cx, cy);
		m_ListInstall.MoveWindow(&rc);
		m_ListInstall.Resize(rc.Width(), rc.Height(), 3);
	}

	if(m_ListService.m_hWnd != NULL)	// Service List Control
	{
		CRect rc(0, 23, cx, cy);
		m_ListService.MoveWindow(&rc);
		m_ListService.Resize(rc.Width(), rc.Height(), 4);
	}

	if(m_TreeRegist.m_hWnd != NULL)		// Registry Tree Control
	{
		CRect rc(0, 23, 200, cy);
		m_TreeRegist.MoveWindow(&rc);
	}

	if(m_ListRegist.m_hWnd != NULL)		// Registry List Control
	{
		CRect rc(200, 23, cx, cy);
		m_ListRegist.MoveWindow(&rc);
		m_ListRegist.Resize(rc.Width(), rc.Height(), 5);
	}
}

void CDoProView::OnSelchangeTabMain(NMHDR* pNMHDR, LRESULT* pResult)
{
	// TODO: Add your control notification handler code here
	m_nTabNum = m_TabMain.GetCurSel();
	SetWindow();
	*pResult = 0;
}

void CDoProView::SetWindow()
{
	switch(m_nTabNum)
	{
		case 0:
			GetDlgItem(IDC_LIST_RUN)->ShowWindow(SW_SHOW);			// Run List

			GetDlgItem(IDC_STATICCPU)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_STATICMEMORY)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_PROCESS)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_LOG)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_INSTALL)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SERVICE)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_REGIST)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_REGIST)->ShowWindow(SW_HIDE);
			m_ListRun.Invalidate(TRUE);
			break;
		case 1:
			GetDlgItem(IDC_STATICCPU)->ShowWindow(SW_SHOW);			// CPU Graph
			GetDlgItem(IDC_STATICMEMORY)->ShowWindow(SW_SHOW);		// Memory Graph
			GetDlgItem(IDC_LIST_PROCESS)->ShowWindow(SW_SHOW);		// Process List

			GetDlgItem(IDC_LIST_RUN)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_LOG)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_INSTALL)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SERVICE)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_REGIST)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_REGIST)->ShowWindow(SW_HIDE);
			m_ListProcess.Invalidate(TRUE);
			break;
		case 2:
			GetDlgItem(IDC_LIST_LOG)->ShowWindow(SW_SHOW);			// Log List

			GetDlgItem(IDC_LIST_RUN)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_STATICCPU)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_STATICMEMORY)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_PROCESS)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_INSTALL)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SERVICE)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_REGIST)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_REGIST)->ShowWindow(SW_HIDE);
			m_ListLog.Invalidate(TRUE);
			break;
		case 3:
			GetDlgItem(IDC_LIST_INSTALL)->ShowWindow(SW_SHOW);		// Install List

			GetDlgItem(IDC_LIST_RUN)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_STATICCPU)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_STATICMEMORY)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_PROCESS)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_LOG)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SERVICE)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_REGIST)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_REGIST)->ShowWindow(SW_HIDE);
			m_ListInstall.Invalidate(TRUE);
			break;
		case 4:
			GetDlgItem(IDC_LIST_SERVICE)->ShowWindow(SW_SHOW);		// Service List

			GetDlgItem(IDC_LIST_RUN)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_STATICCPU)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_STATICMEMORY)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_PROCESS)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_LOG)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_INSTALL)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_REGIST)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_REGIST)->ShowWindow(SW_HIDE);
			m_ListService.Invalidate(TRUE);
			break;
		case 5:
			GetDlgItem(IDC_LIST_REGIST)->ShowWindow(SW_SHOW);		// Registry List
			GetDlgItem(IDC_TREE_REGIST)->ShowWindow(SW_SHOW);		// Registry Tree

			GetDlgItem(IDC_LIST_RUN)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_STATICCPU)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_STATICMEMORY)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_PROCESS)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_LOG)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_INSTALL)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SERVICE)->ShowWindow(SW_HIDE);
			m_TreeRegist.Invalidate(TRUE);
			m_ListRegist.Invalidate(TRUE);
			break;
	}
}

void CDoProView::RunThreadProc()
{
	DWORD dwThreadID1;
	DWORD dwThreadID2;
	DWORD dwThreadID3;
	DWORD dwThreadID4;
	/*
	hThread[0] = (HANDLE)_beginthreadex(NULL, 0, GraphStartCpu, this, 0, (unsigned *)&dwThreadID1);
	if(hThread[0]==NULL)	// CPU Graph Start
		OutputDebugString("The _beginthreadex CPU function failed");

	hThread[1] = (HANDLE)_beginthreadex(NULL, 0, GraphStartMemory, this, 0, (unsigned *)&dwThreadID2);
	if(hThread[1]==NULL)	// MEMORY Graph Start
		OutputDebugString("The _beginthreadex Memory function failed");
	*/
	hThread[2] = (HANDLE)_beginthreadex(NULL, 0, ListRefresh, this, 0, (unsigned *)&dwThreadID3);
	if(hThread[2]==NULL)	// List Refresh Start
 		OutputDebugString("The _beginthreadex ProcessList function failed");

	hThread[3] = (HANDLE)_beginthreadex(NULL, 0, ListRun, this, 0, (unsigned *)&dwThreadID4);
	if(hThread[3]==NULL)	// List Run Start
 		OutputDebugString("The _beginthreadex RunList function failed");
}

UINT WINAPI CDoProView::GraphStartCpu(void *arg)
{
	char szPer[MAX_PATH];
	CDoProView *pDlg = (CDoProView *)arg;
	static CCpuUsage syscpu;
	int nSystem_CPU_Usage;

	while(TRUE)
	{
		Sleep(INTERVAL_TIME);
		nSystem_CPU_Usage  = syscpu.GetCpuUsage();
		if(nSystem_CPU_Usage > 100)
			nSystem_CPU_Usage = 100;
		memset(szPer, NULL, sizeof(szPer));
		wsprintf(szPer, "%d %%", nSystem_CPU_Usage);
		pDlg->m_PushCpu.SetPercent(szPer);
		pDlg->m_PushCpu.Push(nSystem_CPU_Usage,  PUSHGRAPH_SYSTEM_LINE);

		pDlg->m_PushCpu.Update();
	}
	ExitThread(0);
}

UINT WINAPI CDoProView::GraphStartMemory(void *arg)
{
	CDoProView *pDlg = (CDoProView *)arg;
	int nSystem_Memory_Usage;
	char szPer[MAX_PATH];
	DWORDLONG dlTotal, dlAvail;
	MEMORYSTATUSEX lpBuffer;
	lpBuffer.dwLength = sizeof(lpBuffer);

	while(TRUE)
	{
		Sleep(INTERVAL_TIME);
		GlobalMemoryStatusEx (&lpBuffer);		// PF Avail
		dlTotal = lpBuffer.ullTotalPageFile/1024/1024;
		dlAvail = lpBuffer.ullAvailPageFile/1024/1024;
		memset(szPer, NULL, sizeof(szPer));
		wsprintf(szPer, "%d M", dlTotal-dlAvail);
		pDlg->m_PushMemory.SetPercent(szPer);

		nSystem_Memory_Usage  = pDlg->LoadMemoryDetailsForProcess();
		pDlg->m_PushMemory.Push(nSystem_Memory_Usage,  PUSHGRAPH_SYSTEM_LINE);

		pDlg->m_PushMemory.Update();
	}
	ExitThread(0);
}

int CDoProView::LoadMemoryDetailsForProcess()	// Total Memory
{
	int t, sum;
	t = 0;
	sum = 0;

	DWORD aProcesses[1024], cbNeeded;
	if(!EnumProcesses(aProcesses, sizeof(aProcesses), &cbNeeded))	// Get All Process
		return t;

	DWORD cProcesses = cbNeeded / sizeof(DWORD);					// Get Total Process

	PROCESS_MEMORY_COUNTERS pmcProcMemCounter;
	BOOL b;
	for(int i = 0; i < (int)cProcesses; i++ )
	{
		HANDLE hProcess = OpenProcess(MAXIMUM_ALLOWED, FALSE, aProcesses[i]);	// PROCESS_QUERY_INFORMATION|PROCESS_VM_READ
		if(hProcess)
		{
			b = ::GetProcessMemoryInfo( hProcess, &pmcProcMemCounter, sizeof( pmcProcMemCounter ));
			if(b)
			{
				sum = pmcProcMemCounter.WorkingSetSize/1024;
				t += sum;
			}
		}
		else
		{
			CloseHandle(hProcess);
			continue;
		}
		CloseHandle(hProcess);
	}
	return t;
}

int CDoProView::LoadMemoryDetailsForProcess(HANDLE Process)	// One Process Memory
{
	int t = 0;
	PROCESS_MEMORY_COUNTERS pmcProcMemCounter;				// Process details
	BOOL b = ::GetProcessMemoryInfo( Process, &pmcProcMemCounter, sizeof( pmcProcMemCounter ));
	if(b)
	{
		t = pmcProcMemCounter.WorkingSetSize/1024;
	}// End if
	return t;
}

UINT WINAPI CDoProView::ListRun(void *arg)
{
	CDoProView *pDlg = (CDoProView *)arg;
	CWnd * pWnd;
	CString str;
	int n = 0;
	DWORD dwProcessId = 0;
	DWORD dwthisId = 0;
	int iItem;

	while(TRUE)
	{
		Sleep(INTERVAL_TIME);
		pWnd = GetDesktopWindow()->GetTopWindow(); // Get TopWindow
		GetWindowThreadProcessId(pDlg->GetSafeHwnd(), &dwthisId);
		while(pWnd)
		{
			if(pWnd->IsWindowVisible()
				&& pWnd->GetWindowTextLength()
				&& !pWnd->GetOwner()
				&& !(GetWindowLong(pWnd->GetSafeHwnd(), GWL_EXSTYLE) & WS_EX_TOOLWINDOW))
			{
				GetWindowThreadProcessId(pWnd->GetSafeHwnd(), &dwProcessId);	// Get ProcessID
				if(dwProcessId != dwthisId)
				{
					pWnd->GetWindowText(str);	// Title Caption

					BOOL bFind = false;
					n = pDlg->m_ListRun.GetItemCount();
					for (int iIndex =0; iIndex < n; iIndex++ )
					{
						if( dwProcessId == pDlg->m_ListRun.GetItemData( iIndex ) &&
							pDlg->m_ListRun.GetItemText(iIndex, 0) == str )
							bFind = true;
					} // END OF FOR LOOP

					if ( bFind == false)					// add an item to the list control
					{
						HANDLE hProcess = OpenProcess(MAXIMUM_ALLOWED, FALSE, dwProcessId);	// PROCESS_QUERY_INFORMATION|PROCESS_VM_READ
						if(!hProcess)
							continue;

						HMODULE hMod;
						DWORD cbNeeded;
						char szFullpath[MAX_PATH];
						if(EnumProcessModules( hProcess, &hMod, sizeof(hMod), &cbNeeded))
							GetModuleFileNameEx(hProcess, hMod, szFullpath,sizeof(szFullpath));
						else
						{
							CloseHandle(hProcess);
							continue;
						}

						CFileVersionInfo fvi;	// Retrieve version information for this module
						if(fvi.Create(szFullpath))
						{
							SHFILEINFO shinfo;

							SHGetFileInfo( szFullpath, NULL,
										   &shinfo,
										   sizeof(shinfo),
										   SHGFI_DISPLAYNAME | SHGFI_ICON | SHGFI_SMALLICON);	// | SHGFI_OPENICON

							int icon    =  pDlg->m_ImgListRun.Add(shinfo.hIcon);

							CString strLB;
							strLB.LoadString(IDS_STRING61457);

							iItem = pDlg->m_ListRun.AddItem(icon, str, strLB);
							pDlg->m_ListRun.SetItemData( iItem, /*(DWORD)_ttoi(str)*/dwProcessId);
						}
						CloseHandle(hProcess);
					}
				}
			}
			pWnd = pWnd->GetNextWindow();					// Get (CWnd *) Next Window Pointer
		}	// End of if

		n = pDlg->m_ListRun.GetItemCount();					// Delete List
		for (int iIndex =0; iIndex < n; iIndex++ )
		{
			BOOL bChk = false;
			pWnd = GetDesktopWindow()->GetTopWindow(); // Get TopWindow
			GetWindowThreadProcessId(pDlg->GetSafeHwnd(), &dwthisId);
			while(pWnd)
			{
				if(pWnd->IsWindowVisible() && pWnd->GetWindowTextLength() && !pWnd->GetOwner()
					&& !(GetWindowLong(pWnd->GetSafeHwnd(), GWL_EXSTYLE) & WS_EX_TOOLWINDOW))
				{
					GetWindowThreadProcessId(pWnd->GetSafeHwnd(), &dwProcessId);	// Get ProcessID
					if(dwProcessId != dwthisId)
					{
						pWnd->GetWindowText(str);	// Title Caption

						if( dwProcessId == pDlg->m_ListRun.GetItemData( iIndex ) &&
							pDlg->m_ListRun.GetItemText(iIndex, 0) == str )
							bChk = TRUE;
					}
				}
				pWnd = pWnd->GetNextWindow();					// Get (CWnd *) Next Window Pointer
			}	// End of if
			if(bChk == false)
			{
				pDlg->m_ListRun.DeleteItem(iIndex);
				break;
			}
		} // END OF FOR LOOP
		DeleteObject(pWnd);

	}	// End of While

	ExitThread(0);
}

UINT WINAPI CDoProView::ListRefresh(void *arg)
{
	CDoProView *pDlg = (CDoProView *)arg;
	DWORD aProcesses[1024], cbNeeded;

	while(TRUE)
	{
		Sleep(INTERVAL_TIME);
		memset(aProcesses, NULL, sizeof(aProcesses));
		if(!EnumProcesses(aProcesses, sizeof(aProcesses), &cbNeeded))	// Get All PID
			continue;

		DWORD cProcesses = cbNeeded / sizeof(DWORD);					// Get PID Count
		for(int i = 0; i < (int)cProcesses; i++ )
		{
			BOOL bFind = false;
			int n = pDlg->m_ListProcess.GetItemCount();
			for (int iIndex =0; iIndex < n; iIndex++ )
			{
				CCpuUsage* cpu = (CCpuUsage *)pDlg->m_ListProcess.GetItemData( iIndex );
				if( aProcesses[i] == cpu->m_pid )
					bFind = true;
			} // END OF FOR LOOP

			if ( bFind == false && aProcesses[i] != 0)					// add an item to the list control
			{
				HANDLE hToken;
				LUID luidDebug;
				TOKEN_PRIVILEGES tp;
				if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &hToken))
					printf("OpenProcessToken error");

				if (!LookupPrivilegeValue(NULL, SE_DEBUG_NAME, &luidDebug))
					CloseHandle(hToken);

				tp.PrivilegeCount = 1;
				tp.Privileges[0].Luid = luidDebug;
				tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
				if (!AdjustTokenPrivileges(hToken, FALSE, &tp, sizeof(tp), NULL, NULL))
				{
					printf("Adjust error: %d", GetLastError());
					CloseHandle(hToken);
				}
				CloseHandle(hToken);


				FILETIME ftCreate, ftExit, ftKernel, ftUser;
				char szItem[128];

				char szFileDate[32] = { 0 };
				char szFileTime[32] = { 0 };

				HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS/*MAXIMUM_ALLOWED*/, FALSE, aProcesses[i]);	// /*|PROCESS_QUERY_INFORMATION|PROCESS_VM_READ*/
				if(!hProcess)
				{
					DWORD bbdp = GetLastError();
					CString sTemp;
					sTemp.Format("%d, Error = %d\n", aProcesses[i], bbdp);
					OutputDebugString(sTemp);
					CloseHandle(hProcess);
					continue;
				}

				if(GetProcessTimes(hProcess, &ftCreate, &ftExit, &ftKernel, &ftUser))
				{
					// Format the user and kernel times, and add to the process node
					if(!ftCreate.dwHighDateTime&&!ftCreate.dwLowDateTime)
					{
						strcpy(szFileDate,"");
						strcpy(szFileTime,"");
					}
					else
					{
						GetFileDateAsString(&ftCreate, szFileDate, sizeof(szFileDate));
						GetFileTimeAsString(&ftCreate, szFileTime, sizeof(szFileTime));
					}
					wsprintf(szItem, "%s %s", szFileDate, szFileTime);
				}

				HMODULE hMod;
				DWORD cbNeeded;
				char szBaseName[MAX_PATH];
				char szFullpath[MAX_PATH];
				char szPid[32];
				if(EnumProcessModules( hProcess, &hMod, sizeof(hMod), &cbNeeded))
				{
					GetModuleBaseName( hProcess, hMod, szBaseName,sizeof(szBaseName));
					GetModuleFileNameEx(hProcess, hMod, szFullpath,sizeof(szFullpath));
				}
				else
				{
					CloseHandle(hProcess);
					continue;
				}

				CString memoryuse;							// Get Memory
				PROCESS_MEMORY_COUNTERS pmcProcMemCounter;
				BOOL b = ::GetProcessMemoryInfo( hProcess, &pmcProcMemCounter, sizeof( pmcProcMemCounter ));
				if(b)
					memoryuse.Format("%d", pmcProcMemCounter.WorkingSetSize/1024);	// update memory string

				CloseHandle(hProcess);

				CFileVersionInfo fvi;	// Retrieve version information for this module
				if(fvi.Create(szFullpath))
				{
					SHFILEINFO shinfo;

					SHGetFileInfo( szFullpath, NULL,
								   &shinfo,
								   sizeof(shinfo),
								   SHGFI_DISPLAYNAME | SHGFI_ICON | SHGFI_SMALLICON);	// | SHGFI_OPENICON

					int icon    =  pDlg->m_ImgList.Add(shinfo.hIcon);

					wsprintf(szPid, "%u", aProcesses[i]);	// Add ListCtrl

					int iItem;
					if(aProcesses[i] != 4)
					{
						char ctmp[MAX_PATH];		// 000,000 View
						memset(ctmp, 0, sizeof(ctmp));
						NUMBERFMT nFmt = { 0, 0, 3, ".", ",", 1 };
						GetNumberFormat(NULL, NULL, memoryuse.GetBuffer(MAX_PATH), &nFmt, ctmp, sizeof(ctmp));
						memoryuse.Format("%s KB",ctmp);

						iItem = pDlg->m_ListProcess.AddItem(icon, szBaseName, szPid, szItem, "00",
							memoryuse, fvi.GetProductName(), "");
						CCpuUsage * cpu =new CCpuUsage();
						cpu->m_pid = aProcesses[i];
						pDlg->m_ListProcess.SetItemData( iItem, (DWORD)cpu);	/*aProcesses[i]*/
					}
				}
			}
		}	// end of for

		EnumWindows(enumwndfn, (LPARAM)pDlg);	// update the title column text

		// RealTime CPU, Memory Update and Remove ListCtrl
		for (int iIndex =0; iIndex < pDlg->m_ListProcess.GetItemCount(); iIndex++ )
		{
			CCpuUsage* cpu = (CCpuUsage *)pDlg->m_ListProcess.GetItemData( iIndex );
			DWORD sPid = cpu->m_pid;
			HANDLE hProcess = OpenProcess(PROCESS_QUERY_INFORMATION|PROCESS_VM_READ, FALSE, sPid);	// MAXIMUM_ALLOWED
			if(hProcess)
			{
				PROCESS_MEMORY_COUNTERS pmcProcMemCounter;
				BOOL b = ::GetProcessMemoryInfo( hProcess, &pmcProcMemCounter, sizeof( pmcProcMemCounter ));
				if(b)
				{
					CString cpuuse, memoryuse;							// CPU, Memory Check
					int nProcess_CPU_Usage = cpu->GetCpuUsage(sPid);

					// update cpu column string
					cpuuse.Format("%02d", nProcess_CPU_Usage);
					if( cpuuse.Compare(pDlg->m_ListProcess.GetItemText(iIndex,3)) != 0)
						pDlg->m_ListProcess.SetItemText(iIndex, 3, cpuuse);

					// update memory string
					int nChkSize = (INT)pmcProcMemCounter.WorkingSetSize/1024;	// Special Casse
					if(nChkSize < 100)
						pDlg->DeleteList(sPid, iIndex);

					memoryuse.Format("%d", pmcProcMemCounter.WorkingSetSize/1024);
					if( memoryuse.Compare(pDlg->m_ListProcess.GetItemText(iIndex,4)) != 0)
					{
						char ctmp[MAX_PATH];		// 000,000 View
						memset(ctmp, 0, sizeof(ctmp));
						NUMBERFMT nFmt = { 0, 0, 3, ".", ",", 1 };
						GetNumberFormat(NULL, NULL, memoryuse.GetBuffer(MAX_PATH), &nFmt, ctmp, sizeof(ctmp));
						memoryuse.Format("%s KB",ctmp);

						pDlg->m_ListProcess.SetItemText(iIndex, 4, memoryuse);
					}
				}
			}
			else // remove the process from list control and write to the log file
				pDlg->DeleteList(sPid, iIndex);
			CloseHandle(hProcess);
		}
	}
	ExitThread(0);
}

void CDoProView::DeleteList(DWORD sPid, INT iIndex)
{
	COleDateTime st;
	st.ParseDateTime(m_ListProcess.GetItemText(iIndex,2));

	COleDateTime t = COleDateTime::GetCurrentTime();
	COleDateTimeSpan rt = t - st;

	WriteLog(0, "%s, %s, %s, %s, %s, %s", m_ListProcess.GetItemText(iIndex,0),
			m_ListProcess.GetItemText(iIndex,1),
			m_ListProcess.GetItemText(iIndex,2), rt.Format("%H:%M:%S"),
			m_ListProcess.GetItemText(iIndex,5), m_ListProcess.GetItemText(iIndex,6));

	int cnt = m_ListLog.GetItemCount();		// Log List Update
	if(cnt == MAXLOGCOUNT)							// Log List Limit
	{
		cnt = MAXLOGCOUNT - 1;
		m_ListLog.DeleteItem(cnt);
	}

	cnt = m_ListLog.AddItem(0, m_ListProcess.GetItemText(iIndex,0),
							   m_ListProcess.GetItemText(iIndex,1),
							   m_ListProcess.GetItemText(iIndex,2),
							   rt.Format("%H:%M:%S"),
							   m_ListProcess.GetItemText(iIndex,5),
							   m_ListProcess.GetItemText(iIndex,6));
	m_ListLog.SetItemData( cnt, (DWORD)sPid );

	CCpuUsage* cpu = (CCpuUsage *)m_ListProcess.GetItemData( iIndex );
	delete cpu;
	m_ListProcess.DeleteItem(iIndex);
}

BOOL CALLBACK CDoProView::enumwndfn(HWND hWnd, LPARAM lParam)
{
	HWND hWND;
	hWND = ::GetTopWindow(hWnd);
	if (!hWND)
		return TRUE;

	CDoProView * me = (CDoProView *)lParam;
    return me->enumwndfn(CWnd::FromHandle(hWnd));
}

BOOL CDoProView::enumwndfn(CWnd *wnd)
{
	CString lpszWindowTitle,lpszClassName;
	DWORD processID=0;

	GetWindowThreadProcessId(wnd->GetSafeHwnd(),&processID);	// Get Window Handle To PID
	if(wnd == NULL)
	{
		AfxMessageBox("Worning Exit Program");
		PostQuitMessage(WM_QUIT);
	}
	wnd->GetWindowText(lpszWindowTitle);
	wnd->IsWindowVisible();
	wnd->GetWindowTextLength();

	for (int iIndex =0; iIndex < m_ListProcess.GetItemCount(); iIndex++ )
	{
		CCpuUsage* cpu = (CCpuUsage *) m_ListProcess.GetItemData( iIndex );
		if( processID == cpu->m_pid && wnd->GetWindowTextLength()
				&& !(GetWindowLong(wnd->GetSafeHwnd(), GWL_EXSTYLE) & WS_EX_TOOLWINDOW))
		{
			CString strOldCaption;
			strOldCaption = m_ListProcess.GetItemText(iIndex, 6);

			if(strOldCaption.CompareNoCase(lpszWindowTitle) != 0 && lpszWindowTitle != "")
			{
				m_ListProcess.SetItemText( iIndex, 6, lpszWindowTitle );
			}
		}
	} // END OF FOR LOOP

	return TRUE;
}

// function to convert FILETIME to meaningful string
BOOL GetFileTimeAsString( LPFILETIME pFt, char * pszTime, unsigned cbIn)
{
	FILETIME ftLocal;
	SYSTEMTIME st;

	if(!FileTimeToLocalFileTime( pFt, &ftLocal))
		return FALSE;
 
	if(!FileTimeToSystemTime( &ftLocal, &st))
		return FALSE;
 
	char szTemp[12];
 
	wsprintf(szTemp, "%02u:%02u:%02u", st.wHour, st.wMinute, st.wSecond);
	lstrcpyn(pszTime, szTemp, cbIn);
	return TRUE;
}

// function to convert FILETIME to meaningful string
BOOL GetFileDateAsString( LPFILETIME pFt, char * pszDate, unsigned cbIn)
{
	FILETIME ftLocal;
	SYSTEMTIME st;

	if(!FileTimeToLocalFileTime(pFt, &ftLocal))
		return FALSE;
 
	if(!FileTimeToSystemTime(&ftLocal, &st))
		return FALSE;
 
	char szTemp[12];
	wsprintf(szTemp, "%04u-%02u-%02u", st.wYear, st.wMonth, st.wDay);
	lstrcpyn(pszDate, szTemp, cbIn);
	return TRUE;
}

void CDoProView::OnAppLogview()
{
	// TODO: Add your command handler code here
	ViewLog(0);
}

void CDoProView::OnAppLogdel()
{
	// TODO: Add your command handler code here
	RemoveLog(0);
	m_ListLog.DeleteAllItems();
}

void CDoProView::OnViewRefresh()
{
	// TODO: Add your command handler code here
	m_ListRun.DeleteAllItems();
	m_ListProcess.DeleteAllItems();
	m_ListInstall.DeleteAllItems();
	m_ListService.DeleteAllItems();
	LoadService();
	LoadInstall();
}

void CDoProView::OnDblclkListRun(NMHDR* pNMHDR, LRESULT* pResult)
{
	// TODO: Add your control notification handler code here
	CWnd* pWnd = CWnd::GetTopWindow()->GetActiveWindow();		// Minimized Program
	::ShowWindow(pWnd->m_hWnd, SW_MINIMIZE);

	NMLISTVIEW* pNMLV = (NMLISTVIEW* )pNMHDR;					// Maximize Program
	CWnd* pWnd1 = CWnd::FindWindow(NULL, m_ListRun.GetItemText(pNMLV->iItem, 0));
	::ShowWindow(pWnd1->m_hWnd, SW_MAXIMIZE);

	*pResult = 0;
}

void CDoProView::OnRclickListProcess(NMHDR* pNMHDR, LRESULT* pResult)
{
	// TODO: Add your control notification handler code here
	CPoint point;
	GetCursorPos(&point);
	CMenu menu;
	menu.LoadMenu(IDR_CONTEXT_PRO);
	CMenu * pMenu = menu.GetSubMenu(0);
	pMenu->TrackPopupMenu(TPM_LEFTALIGN|TPM_RIGHTBUTTON,point.x,point.y,AfxGetMainWnd());

	*pResult = 0;
}

void CDoProView::OnDblclkListProcess(NMHDR* pNMHDR, LRESULT* pResult)
{
	// TODO: Add your control notification handler code here
	NMLISTVIEW* pList = (NMLISTVIEW* )pNMHDR;

	CDialogDetail dlg;
	CCpuUsage* cpu = (CCpuUsage *)m_ListProcess.GetItemData(pList->iItem);
	dlg.m_PID = cpu->m_pid;
	if(dlg.DoModal() == IDCANCEL)
		OutputDebugString("DetailDialog Exit");
	*pResult = 0;
}

void CDoProView::OnContextDetail()	// Process List Context Menu
{
	// TODO: Add your command handler code here
    int nCount = m_ListProcess.GetItemCount();
	for(int i=0; i < nCount; i++)
	{
		if(m_ListProcess.GetItemState(i, LVNI_SELECTED))
		{
			CDialogDetail dlg;
			CCpuUsage* cpu = (CCpuUsage *)m_ListProcess.GetItemData(i);
			dlg.m_PID = cpu->m_pid;
			if(dlg.DoModal() == IDCANCEL)
				OutputDebugString("DetailDialog Exit");
			break;
		}
	}
}

void CDoProView::OnContextKill()	// Process List Context Menu
{
	// TODO: Add your command handler code here
	CString strLS;
    int nCount = m_ListProcess.GetItemCount();
	for(int i=0; i < nCount; i++)
	{
		if(m_ListProcess.GetItemState(i, LVNI_SELECTED))
		{
			CString str = m_ListProcess.GetItemText(i, 0);
			str.MakeLower();
			if( str == "dopro.exe" | str == "dosys.exe" | str == "domanager.exe" | str == "doclean.exe" )
			{
				strLS.LoadString(IDS_STRING61451);
				AfxMessageBox(strLS);
				break;
			}
			else
			{
				strLS.LoadString(IDS_STRING61452);	// Warnning Message
				str.Format("%s %s", m_ListProcess.GetItemText(i, 0), strLS);
				strLS.LoadString(IDS_STRING61453);
				int nMe = MessageBox(str, strLS, MB_YESNO|MB_ICONEXCLAMATION);	/*MB_ICONQUESTION*/
				if(nMe == IDYES)
				{
					CCpuUsage* cpu = (CCpuUsage *)m_ListProcess.GetItemData(i);
					DWORD processGet = cpu->m_pid;
					HANDLE hProcess = OpenProcess(PROCESS_QUERY_INFORMATION|PROCESS_VM_READ, FALSE, processGet);
					BOOL b = SafeTerminateProcess(hProcess, (UINT)-1);		// Process Kill
					if(!b)
					{
						CString strLS1;
						strLS.LoadString(IDS_STRING61454);	// Not Kill
						strLS1.LoadString(IDS_STRING61455);
						MessageBox(strLS, strLS1, MB_OK|MB_ICONINFORMATION);
					}
					CloseHandle(hProcess);
				}
			}
			break;
		}
	}	// End of For
	m_ListProcess.Invalidate();
}

BOOL CDoProView::SafeTerminateProcess(HANDLE hProcess, UINT uExitCode)
{
	DWORD dwTID, dwCode, dwErr = 0;
	HANDLE hProcessDup = INVALID_HANDLE_VALUE;
	HANDLE hRT = NULL;
	HINSTANCE hKernel = GetModuleHandle("Kernel32");

	BOOL bSuccess = FALSE;
	BOOL bDup = DuplicateHandle(GetCurrentProcess(),
									hProcess,
									GetCurrentProcess(),
									&hProcessDup,
									PROCESS_ALL_ACCESS,
									FALSE,
									0);
	if ( GetExitCodeProcess((bDup) ? hProcessDup : hProcess, &dwCode) && (dwCode == STILL_ACTIVE) )
	{
		FARPROC pfnExitProc;
		pfnExitProc = GetProcAddress(hKernel, "ExitProcess");
		hRT = CreateRemoteThread((bDup) ? hProcessDup : hProcess,
									NULL,
									0,
									(LPTHREAD_START_ROUTINE)pfnExitProc,
									(PVOID)uExitCode, 0, &dwTID);
		if ( hRT == NULL ) dwErr = GetLastError();
	}
	else
		dwErr = ERROR_PROCESS_ABORTED;

	if ( hRT )
	{
		WaitForSingleObject((bDup) ? hProcessDup : hProcess, INFINITE);
		CloseHandle(hRT);
		bSuccess = TRUE;
	}
	if( bDup )
		CloseHandle(hProcessDup);

	if( !bSuccess )
		SetLastError(dwErr);

	return bSuccess;
}

void CDoProView::LoadService()
{
	SC_HANDLE hSCM    = NULL;
	PUCHAR  pBuf    = NULL;
	ULONG  dwBufSize   = 0x00;
	ULONG  dwBufNeed   = 0x00;
	ULONG  dwNumberOfService = 0x00;
	LPENUM_SERVICE_STATUS_PROCESS pInfo = NULL;

	hSCM = OpenSCManager(NULL, NULL, /*SC_MANAGER_LOCK*/SC_MANAGER_ENUMERATE_SERVICE | SC_MANAGER_CONNECT );
	if (hSCM == NULL)
		OutputDebugString("OpenSCManager fail \n");

	EnumServicesStatusEx( hSCM, SC_ENUM_PROCESS_INFO, SERVICE_WIN32, SERVICE_STATE_ALL, NULL, dwBufSize, &dwBufNeed, &dwNumberOfService, NULL, NULL);
	if(dwBufNeed < 0x01)
		OutputDebugString("EnumServicesStatusEx fail ?? \n");

	dwBufSize = dwBufNeed + 0x10;
	pBuf  = (PUCHAR) malloc(dwBufSize);
	EnumServicesStatusEx( hSCM, SC_ENUM_PROCESS_INFO, SERVICE_WIN32, SERVICE_STATE_ALL, pBuf, dwBufSize, &dwBufNeed, &dwNumberOfService, NULL, NULL);
	pInfo = (LPENUM_SERVICE_STATUS_PROCESS)pBuf;
	for(ULONG i=0; i < dwNumberOfService; i++)
	{
		CString strStatus;
		if(pInfo[i].ServiceStatusProcess.dwProcessId)
			strStatus = "���۵�";
		m_ListService.AddItem(0, pInfo[i].lpDisplayName, "", pInfo[i].lpServiceName, strStatus, "");
	}
	free(pBuf);
	CloseServiceHandle(hSCM);
}

void CDoProView::LoadInstall()
{
	HKEY key;
	LONG lResult;
	lResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", 0, KEY_ALL_ACCESS, &key);

	if (lResult == ERROR_SUCCESS)	// Find Registry OK
	{
		char lpSubKey[MAX_PATH];

		DWORD i;
		LONG Result;
		DWORD Size;
		FILETIME FileTime;

		CString strString;
		HKEY hQureKey;
		DWORD dwSize = MAX_PATH;
		Result = ERROR_SUCCESS;
		for ( i=0; Result==ERROR_SUCCESS; i++ )	// Find SubKey
		{
			Size=MAX_PATH;
			Result=RegEnumKeyEx( key, i ,lpSubKey, &Size, NULL, NULL, NULL, &FileTime );

			if ( Result==ERROR_SUCCESS )
			{
				strString.Format( "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\%s", lpSubKey );
				int err = RegOpenKeyEx(HKEY_LOCAL_MACHINE, strString, 0, KEY_QUERY_VALUE, &hQureKey);

				if ( err == ERROR_SUCCESS )		// Find Values
				{
					BYTE buff1[12];
					BYTE buff2[12];
					BYTE buff3[16];
					DWORD len1, type1, len2, type2, len3, type3;
					int nChk1 = RegQueryValueEx(hQureKey, "DisplayName", NULL, &type1, buff1, &len1);
					int nChk2 = RegQueryValueEx(hQureKey, "InstallDate", NULL, &type2, buff2, &len2);
					int nChk3 = RegQueryValueEx(hQureKey, "UninstallString", NULL, &type3, buff3, &len3);

					if(ERROR_SUCCESS == nChk1)
					{
						CString DisPlayName, InstallDate, UninstallString;
						DisPlayName = buff1;
						InstallDate = buff2;
						UninstallString = buff3;
						if(InstallDate.GetLength() > 8)
							InstallDate = "��������";
						if(UninstallString == "")
							UninstallString = "���źҰ�";
						else
							UninstallString = "���Ű���";

						m_ListInstall.AddItem(0, DisPlayName, InstallDate, UninstallString);
					}
					RegCloseKey(hQureKey);
				}
			}
		}	// End of For
	}
	else	// No Find
		m_ListInstall.AddItem(NULL, "��ġ�� ���α׷��� �����ϴ�.", "", "");

	RegCloseKey( key );
}

void CDoProView::OnDblclkListInstall(NMHDR* pNMHDR, LRESULT* pResult) 
{
	// TODO: Add your control notification handler code here
	NMLISTVIEW* pNMLV = (NMLISTVIEW* )pNMHDR;

	HKEY key;
	LONG lResult;
	lResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", 0, KEY_ALL_ACCESS, &key);

	if (lResult == ERROR_SUCCESS)	// Find Registry OK
	{
		char lpSubKey[MAX_PATH];

		DWORD i;
		LONG Result;
		DWORD Size;
		FILETIME FileTime;

		CString strString;
		HKEY hQureKey;
		DWORD dwSize = MAX_PATH;
		Result = ERROR_SUCCESS;
		for ( i=0; Result==ERROR_SUCCESS; i++ )	// Find SubKey
		{
			Size=MAX_PATH;
			Result=RegEnumKeyEx( key, i ,lpSubKey, &Size, NULL, NULL, NULL, &FileTime );

			if ( Result==ERROR_SUCCESS )
			{
				strString.Format( "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\%s", lpSubKey );
				int err = RegOpenKeyEx(HKEY_LOCAL_MACHINE, strString, 0, KEY_QUERY_VALUE, &hQureKey);

				if ( err == ERROR_SUCCESS )		// Find Values
				{
					BYTE buff1[MAX_PATH];
					BYTE buff2[MAX_PATH];
					DWORD len1, type1, len2, type2;
					int nChk1 = RegQueryValueEx(hQureKey, "DisplayName", NULL, &type1, buff1, &len1);
					int nChk2 = RegQueryValueEx(hQureKey, "UninstallString", NULL, &type2, buff2, &len2);

					if(ERROR_SUCCESS == nChk1)
					{
						CString DisPlayName = buff1;
						if(DisPlayName == m_ListInstall.GetItemText(pNMLV->iItem, 0))
						{
							CString str;
							str.Format("%s", buff2);
							//AfxMessageBox(str);
							HINSTANCE hInst = ShellExecute(NULL, "open", str, NULL, NULL, SW_SHOWNORMAL);
							if(!hInst)
								OutputDebugString("Fail To Uninstall");
						}
					}
					RegCloseKey(hQureKey);
				}
			}
		}	// End of For
	}
	RegCloseKey( key );

	*pResult = 0;
}

void CDoProView::LoadTreeReg(CTreeCtrl *m_pTreeCtrl)
{
	m_pTreeCtrl->DeleteAllItems();

	CString strLS;
	strLS.LoadString(IDS_STRING61461);
	HTREEITEM hRoot = m_TreeRegist.InsertItem( strLS, 0, 0 );	// Root
	m_pTreeCtrl->SetItemData( hRoot, (DWORD)"ROOT" );

	
	HTREEITEM hClass = m_pTreeCtrl->InsertItem( "HKEY_CLASSES_ROOT", 1, 1, hRoot );
	HTREEITEM hCurrent = m_pTreeCtrl->InsertItem( "HKEY_CURRENT_USER", 1, 1, hRoot );
	HTREEITEM hLocal = m_pTreeCtrl->InsertItem( "HKEY_LOCAL_MACHINE", 1, 1, hRoot );
	HTREEITEM hUsers = m_pTreeCtrl->InsertItem( "HKEY_USERS", 1, 1, hRoot );
	HTREEITEM hConfig = m_pTreeCtrl->InsertItem( "HKEY_CURRENT_CONFIG", 1, 1, hRoot );
	
	LoadTreeChild(m_pTreeCtrl, hClass, 0, "");
	LoadTreeChild(m_pTreeCtrl, hCurrent, 1, "");
	LoadTreeChild(m_pTreeCtrl, hLocal, 2, "");
	LoadTreeChild(m_pTreeCtrl, hUsers, 3, "");
	LoadTreeChild(m_pTreeCtrl, hConfig, 4, "");

	m_pTreeCtrl->Expand( hRoot, TVE_EXPAND );
	m_pTreeCtrl->SetFocus();
}

void CDoProView::LoadTreeChild(CTreeCtrl *m_pTreeCtrl, HTREEITEM hParent, int nType, CString strPath)
{
	BOOL bFind = false;
	HKEY key, rootkey;
	LONG lResult;
	switch(nType)
	{
		case 0:
			rootkey = HKEY_CLASSES_ROOT;
			break;
		case 1:
			rootkey = HKEY_CURRENT_USER;
			break;
		case 2:
			rootkey = HKEY_LOCAL_MACHINE;
			break;
		case 3:
			rootkey = HKEY_USERS;
			break;
		case 4:
			rootkey = HKEY_CURRENT_CONFIG;
			break;
	}
	
	lResult = RegOpenKeyEx(rootkey, strPath, 0, KEY_ALL_ACCESS | KEY_ENUMERATE_SUB_KEYS | KEY_NOTIFY, &key);
	if (lResult == ERROR_SUCCESS)	// Find Registry OK
	{
		char lpSubKey[MAX_PATH];

		DWORD i;
		LONG Result;
		DWORD Size;
		FILETIME FileTime;

		CString strString;
		HKEY hQureKey;
		DWORD dwSize = MAX_PATH;
		Result = ERROR_SUCCESS;
		for ( i=0; Result==ERROR_SUCCESS; i++ )	// Find SubKey
		{
			Size=MAX_PATH;
			Result=RegEnumKeyEx( key, i ,lpSubKey, &Size, NULL, NULL, NULL, &FileTime );
			if ( Result==ERROR_SUCCESS )
			{
				HTREEITEM hSub = m_pTreeCtrl->InsertItem(lpSubKey, 1, 1, hParent);
				if(strString != "")
					strString = strPath + "\\" + lpSubKey;
				else
					strString = lpSubKey;

				int err = RegOpenKeyEx(rootkey, strString, 0, KEY_ALL_ACCESS | KEY_ENUMERATE_SUB_KEYS | KEY_NOTIFY, &hQureKey);
				if(err == ERROR_SUCCESS)
//					LoadTreeChild(m_pTreeCtrl, hSub, nType, strString);

				RegCloseKey(hQureKey);
			}
		}	// End of For
	}

	RegCloseKey( key );

}

void CDoProView::OnSelchangedTreeRegist(NMHDR* pNMHDR, LRESULT* pResult) 
{
	NM_TREEVIEW* pNMTreeView = (NM_TREEVIEW*)pNMHDR;
	// TODO: Add your control notification handler code here
	
	*pResult = 0;
}

void CDoProView::OnClickTreeRegist(NMHDR* pNMHDR, LRESULT* pResult) 
{
	// TODO: Add your control notification handler code here
	
	*pResult = 0;
}




HRESULT CDoProView::CheckIfIsUserAdmin(BOOL* pIsAdmin)
{
	int b;
	HANDLE hProcess = NULL;
	HANDLE hProcessToken = NULL;
	HANDLE hLinkedToken = NULL;
	BOOL fIsAdmin = FALSE;
	DWORD dwLength = 0;
	OSVERSIONINFO osver = { sizeof(OSVERSIONINFO) };
	HRESULT hr = S_OK;

	*pIsAdmin = FALSE;

	hProcess = GetCurrentProcess();
	if (!OpenProcessToken(hProcess, TOKEN_QUERY, &hProcessToken))
	{
		hr = HRESULT_FROM_WIN32(GetLastError());
		goto Exit;
	}

	char AdminSID[SECURITY_MAX_SID_SIZE];
	dwLength = sizeof(AdminSID);
	if (!CreateWellKnownSid(WinBuiltinAdministratorsSid, NULL, &AdminSID, &dwLength))
	{
		hr = HRESULT_FROM_WIN32(GetLastError());
		goto Exit;
	}

	if (!CheckTokenMembership(NULL, &AdminSID, &fIsAdmin))
	{
		hr = HRESULT_FROM_WIN32(GetLastError());
		goto Exit;
	}

	if (fIsAdmin)
	{
		*pIsAdmin = TRUE;
		goto Exit;
	}

	if (!GetVersionEx(&osver))
	{
		hr = HRESULT_FROM_WIN32(GetLastError());
		goto Exit;
	}

	if (osver.dwMajorVersion < 6)
	{
		goto Exit;
	}

	// Code to handle admin SID filtering in Vista and above
	if (!GetTokenInformation(hProcessToken, TokenLinkedToken,
		(VOID*)&hLinkedToken, sizeof(HANDLE), &dwLength))
	{
		b = GetLastError();
		if (b == ERROR_NO_SUCH_LOGON_SESSION || b == ERROR_PRIVILEGE_NOT_HELD)
		{
			goto Exit;
		}

		hr = HRESULT_FROM_WIN32(b);
		goto Exit;
	}

	if (!CheckTokenMembership(hLinkedToken, &AdminSID, &fIsAdmin))
	{
		hr = HRESULT_FROM_WIN32(GetLastError());
		goto Exit;
	}

	if (fIsAdmin)
	{
		*pIsAdmin = TRUE;
	}

Exit:
	if (hProcess)
	{
		CloseHandle(hProcess);
	}

	if (hProcessToken)
	{
		CloseHandle(hProcessToken);
	}

	if (hLinkedToken)
	{
		CloseHandle(hLinkedToken);
	}

	return hr;
}

//-------------------------------------------------------------------------
// This function checks the token of the calling thread to see if the caller
// belongs to the Administrators group.
//
// Return Value:
//   TRUE if the caller is an administrator on the local machine.
//   Otherwise, FALSE.
// --------------------------------------------------------------------------
// Based on code from KB #Q118626, at http://support.microsoft.com/kb/118626
BOOL CDoProView::IsCurrentUserLocalAdministrator()
{
	BOOL   fReturn = FALSE;
	DWORD  dwStatus = 0;
	DWORD  dwAccessMask = 0;
	DWORD  dwAccessDesired = 0;
	DWORD  dwACLSize = 0;
	DWORD  dwStructureSize = sizeof(PRIVILEGE_SET);
	PACL   pACL = NULL;
	PSID   psidAdmin = NULL;

	HANDLE hToken = NULL;
	HANDLE hImpersonationToken = NULL;

	PRIVILEGE_SET   ps = { 0 };
	GENERIC_MAPPING GenericMapping = { 0 };

	PSECURITY_DESCRIPTOR     psdAdmin = NULL;
	SID_IDENTIFIER_AUTHORITY SystemSidAuthority = SECURITY_NT_AUTHORITY;

	// Determine if the current thread is running as a user that is a member 
	// of the local admins group.  To do this, create a security descriptor 
	// that has a DACL which has an ACE that allows only local administrators 
	// access.  Then, call AccessCheck with the current thread's token and
	// the security descriptor.  It will say whether the user could access an
	// object if it had that security descriptor.  Note: you do not need to
	// actually create the object.  Just checking access against the
	// security descriptor alone will be sufficient.

	const DWORD ACCESS_READ = 1;
	const DWORD ACCESS_WRITE = 2;

	__try
	{
		// AccessCheck() requires an impersonation token.  We first get a 
		// primary token and then create a duplicate impersonation token.
		// The impersonation token is not actually assigned to the thread, but
		// is used in the call to AccessCheck.  Thus, this function itself never
		// impersonates, but does use the identity of the thread.  If the thread
		// was impersonating already, this function uses that impersonation 
		// context.
		if (!OpenThreadToken(GetCurrentThread(), TOKEN_DUPLICATE | TOKEN_QUERY, TRUE, &hToken))
		{
			if (GetLastError() != ERROR_NO_TOKEN)
				__leave;

			if (!OpenProcessToken(GetCurrentProcess(),
				TOKEN_DUPLICATE | TOKEN_QUERY, &hToken))
				__leave;
		}

		if (!DuplicateToken(hToken, SecurityImpersonation, &hImpersonationToken))
			__leave;

		// Create the binary representation of the well-known SID that
		// represents the local administrators group.  Then create the 
		// security descriptor and DACL with an ACE that allows only local
		// admins access.  After that, perform the access check.  This will
		// determine whether the current user is a local admin.
		if (!AllocateAndInitializeSid(&SystemSidAuthority, 2,
			SECURITY_BUILTIN_DOMAIN_RID, DOMAIN_ALIAS_RID_ADMINS,
			0, 0, 0, 0, 0, 0, &psidAdmin))
			__leave;

		psdAdmin = LocalAlloc(LPTR, SECURITY_DESCRIPTOR_MIN_LENGTH);
		if (psdAdmin == NULL)
			__leave;

		if (!InitializeSecurityDescriptor(psdAdmin, SECURITY_DESCRIPTOR_REVISION))
			__leave;

		// Compute size needed for the ACL.
		dwACLSize = sizeof(ACL) + sizeof(ACCESS_ALLOWED_ACE) +
			GetLengthSid(psidAdmin) - sizeof(DWORD);

		pACL = (PACL)LocalAlloc(LPTR, dwACLSize);
		if (pACL == NULL)
			__leave;

		if (!InitializeAcl(pACL, dwACLSize, ACL_REVISION2))
			__leave;

		dwAccessMask = ACCESS_READ | ACCESS_WRITE;

		if (!AddAccessAllowedAce(pACL, ACL_REVISION2, dwAccessMask, psidAdmin))
			__leave;

		if (!SetSecurityDescriptorDacl(psdAdmin, TRUE, pACL, FALSE))
			__leave;

		// AccessCheck validates a security descriptor somewhat; set the group
		// and owner so that enough of the security descriptor is filled out to
		// make AccessCheck happy.

		SetSecurityDescriptorGroup(psdAdmin, psidAdmin, FALSE);
		SetSecurityDescriptorOwner(psdAdmin, psidAdmin, FALSE);

		if (!IsValidSecurityDescriptor(psdAdmin))
			__leave;

		dwAccessDesired = ACCESS_READ;

		// Initialize GenericMapping structure even though you
		// do not use generic rights.
		GenericMapping.GenericRead = ACCESS_READ;
		GenericMapping.GenericWrite = ACCESS_WRITE;
		GenericMapping.GenericExecute = 0;
		GenericMapping.GenericAll = ACCESS_READ | ACCESS_WRITE;

		if (!AccessCheck(psdAdmin, hImpersonationToken, dwAccessDesired,
			&GenericMapping, &ps, &dwStructureSize, &dwStatus,
			&fReturn))
		{
			fReturn = FALSE;
			__leave;
		}
	}
	__finally
	{
		// Clean up.
		if (pACL)
			LocalFree(pACL);
		if (psdAdmin)
			LocalFree(psdAdmin);
		if (psidAdmin)
			FreeSid(psidAdmin);
		if (hImpersonationToken)
			CloseHandle(hImpersonationToken);
		if (hToken)
			CloseHandle(hToken);
	}

	return fReturn;
}

void CDoProView::EnableSystemPriv(void)
{
	HANDLE hToken;
	LUID luidDebug;
	TOKEN_PRIVILEGES tp;
	if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, &hToken))
		return;

	if (!LookupPrivilegeValue(NULL, SE_DEBUG_NAME, &luidDebug))
	{	// �����?����..............
		CloseHandle(hToken);
		return;
	}

	tp.PrivilegeCount = 1;
	tp.Privileges[0].Luid = luidDebug;
	tp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
	if (!AdjustTokenPrivileges(hToken, FALSE, &tp, sizeof(tp), NULL, NULL))
	{
		printf("Adjust error: %d", GetLastError());
		CloseHandle(hToken);
	}
	CloseHandle(hToken);
}