// MainFrm.cpp : implementation of the CMainFrame class
//

#include "stdafx.h"
#include "DoPro.h"

#include "MainFrm.h"

#include "Splash.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CMainFrame

IMPLEMENT_DYNCREATE(CMainFrame, CFrameWnd)

BEGIN_MESSAGE_MAP(CMainFrame, CFrameWnd)
	//{{AFX_MSG_MAP(CMainFrame)
	ON_WM_CREATE()
	ON_WM_CLOSE()
	ON_COMMAND(ID_TRAY_SHOW, OnTrayShow)
	ON_WM_SYSCOMMAND()
	//}}AFX_MSG_MAP
	ON_MESSAGE(WM_ICON_NOTIFY, OnTrayNotification)	// Add header After Function Add
END_MESSAGE_MAP()

static UINT indicators[] =
{
	ID_SEPARATOR,           // status line indicator
	ID_INDICATOR_CAPS,
	ID_INDICATOR_NUM,
	ID_INDICATOR_SCRL,
};

/////////////////////////////////////////////////////////////////////////////
// CMainFrame construction/destruction

CMainFrame::CMainFrame()
{
	// TODO: add member initialization code here
	
}

CMainFrame::~CMainFrame()
{
	m_TrayIcon.RemoveIcon();	// Delete Tray Icon
}

int CMainFrame::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CFrameWnd::OnCreate(lpCreateStruct) == -1)
		return -1;
	
/*	if (!m_wndToolBar.CreateEx(this, TBSTYLE_FLAT, WS_CHILD | WS_VISIBLE | CBRS_TOP
		| CBRS_GRIPPER | CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC) ||
		!m_wndToolBar.LoadToolBar(IDR_MAINFRAME))
	{
		TRACE0("Failed to create toolbar\n");
		return -1;      // fail to create
	}
*/
	if (!m_wndStatusBar.Create(this) ||
		!m_wndStatusBar.SetIndicators(indicators,
		  sizeof(indicators)/sizeof(UINT)))
	{
		TRACE0("Failed to create status bar\n");
		return -1;      // fail to create
	}

	// TODO: Delete these three lines if you don't want the toolbar to
	//  be dockable
//	m_wndToolBar.EnableDocking(CBRS_ALIGN_ANY);
//	EnableDocking(CBRS_ALIGN_ANY);
//	DockControlBar(&m_wndToolBar);

	//////////////////////////////////////////////////////////////////
	// Create the tray icon
    if (!m_TrayIcon.Create(this, WM_ICON_NOTIFY, _T("DoPro TaskManager"), NULL, IDR_TRAY_MENU))
		return -1;
	m_TrayIcon.SetIcon(IDR_MAINFRAME);

	// CG: The following line was added by the Splash Screen component.
	CSplashWnd::ShowSplashScreen(this);
	return 0;
}

BOOL CMainFrame::PreCreateWindow(CREATESTRUCT& cs)
{
	if( !CFrameWnd::PreCreateWindow(cs) )
		return FALSE;
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return TRUE;
}

/////////////////////////////////////////////////////////////////////////////
// CMainFrame diagnostics

#ifdef _DEBUG
void CMainFrame::AssertValid() const
{
	CFrameWnd::AssertValid();
}

void CMainFrame::Dump(CDumpContext& dc) const
{
	CFrameWnd::Dump(dc);
}

#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CMainFrame message handlers

/////////////////////////////////////////////////////////////////////////////
// Helpers for saving/restoring window state

static char szSection[]   = "Settings";
static char szWindowPos[] = "WindowPos";
static char szFormat[] = "%u,%u,%d,%d,%d,%d,%d,%d,%d,%d";

BOOL CMainFrame::ReadWindowPlacement(WINDOWPLACEMENT *pwp)
{
	CString 		strBuffer;
	int				nRead ;
	
	strBuffer = AfxGetApp()->GetProfileString(szSection, szWindowPos);
	if ( strBuffer.IsEmpty() )  return FALSE;

	nRead = sscanf(strBuffer, szFormat,
				&pwp->flags, &pwp->showCmd,
				&pwp->ptMinPosition.x, &pwp->ptMinPosition.y,
				&pwp->ptMaxPosition.x, &pwp->ptMaxPosition.y,
				&pwp->rcNormalPosition.left,  &pwp->rcNormalPosition.top,
				&pwp->rcNormalPosition.right, &pwp->rcNormalPosition.bottom);
	if ( nRead != 10 )  return FALSE;
	pwp->length = sizeof(WINDOWPLACEMENT);

	return TRUE;
}

void CMainFrame::WriteWindowPlacement(WINDOWPLACEMENT *pwp)
{
	char		szBuffer[sizeof("-32767")*8 + sizeof("65535")*2];
	int			max = 1;
	CString		s;

	sprintf(szBuffer, szFormat,
			pwp->flags, pwp->showCmd,
			pwp->ptMinPosition.x, pwp->ptMinPosition.y,
			pwp->ptMaxPosition.x, pwp->ptMaxPosition.y,
			pwp->rcNormalPosition.left, pwp->rcNormalPosition.top,
			pwp->rcNormalPosition.right, pwp->rcNormalPosition.bottom);
 	AfxGetApp()->WriteProfileString(szSection, szWindowPos, szBuffer);
}
/////////////////////////////////////////////////////////////////////////////


void CMainFrame::OnClose() 
{
	// TODO: Add your message handler code here and/or call default
	//	Save window state and position
	WINDOWPLACEMENT	wp;
	// before it is destroyed, save the position of the window
	wp.length = sizeof wp;
	if ( GetWindowPlacement(&wp) )
	{
		if ( IsIconic() )
			// never restore to Iconic state
			wp.showCmd = SW_SHOW ;
		if ((wp.flags & WPF_RESTORETOMAXIMIZED) != 0)
			// if maximized and maybe iconic restore maximized state
			wp.showCmd = SW_SHOWMAXIMIZED ;
			// and write it to the .INI file
		WriteWindowPlacement(&wp);
	}
	CFrameWnd::OnClose();
}

void CMainFrame::ActivateFrame(int nCmdShow) 
{
	// TODO: Add your specialized code here and/or call the base class
	// nCmdShow is the normal show mode this frame should be in
	// translate default nCmdShow (-1)
	if (nCmdShow == -1)
	{
		if (!IsWindowVisible())
			nCmdShow = SW_SHOWNORMAL;
		else if (IsIconic())
			nCmdShow = SW_RESTORE;
	}

	// bring to top before showing
	BringToTop(nCmdShow);
	if (nCmdShow != -1)
	{
		// show the window as specified
		WINDOWPLACEMENT wp;

		//  Read and set the previously saved window position and state 
		if ( !ReadWindowPlacement(&wp) )
			ShowWindow(nCmdShow);
		else
		{
			if ( nCmdShow != SW_SHOWNORMAL )  
			wp.showCmd = nCmdShow;
			SetWindowPlacement(&wp);
		}
		// and finally, bring to top after showing
		BringToTop(nCmdShow);
	}
	//CFrameWnd::ActivateFrame(nCmdShow);
}

void CMainFrame::OnTrayShow() 
{
	// TODO: Add your command handler code here
	ShowWindow(SW_SHOW);
}

LRESULT CMainFrame::OnTrayNotification(WPARAM wParam, LPARAM lParam)
{
	// Delegate all the work back to the default implementation in CTrayIcon.
	return m_TrayIcon.OnTrayNotification(wParam, lParam);
}

void CMainFrame::OnSysCommand(UINT nID, LPARAM lParam) 
{
	// TODO: Add your message handler code here and/or call default
	if(nID == SC_MINIMIZE)
	{
		ShowWindow(SW_HIDE);
		return;
	}

	CFrameWnd::OnSysCommand(nID, lParam);
}
