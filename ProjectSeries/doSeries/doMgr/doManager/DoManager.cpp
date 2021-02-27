// DoManager.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "DoManager.h"

#include "MainFrm.h"
#include "DoManagerDoc.h"
#include "DoManagerView.h"

#include "Splash.h"
#include "./Lib/Util.h"

#define INFO_BUFFER_SIZE 32767

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDoManagerApp

BEGIN_MESSAGE_MAP(CDoManagerApp, CWinApp)
	//{{AFX_MSG_MAP(CDoManagerApp)
	ON_COMMAND(ID_APP_ABOUT, OnAppAbout)
	ON_COMMAND(ID_VIEW_TOP, OnViewTop)
	ON_UPDATE_COMMAND_UI(ID_VIEW_TOP, OnUpdateViewTop)
	ON_COMMAND(ID_APP_AUTO, OnAppAuto)
	ON_UPDATE_COMMAND_UI(ID_APP_AUTO, OnUpdateAppAuto)
	//}}AFX_MSG_MAP
	// Standard file based document commands
	ON_COMMAND(ID_FILE_NEW, CWinApp::OnFileNew)
	ON_COMMAND(ID_FILE_OPEN, CWinApp::OnFileOpen)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDoManagerApp construction

CDoManagerApp::CDoManagerApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CDoManagerApp object

CDoManagerApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CDoManagerApp initialization

BOOL CDoManagerApp::InitInstance()
{
	// CG: The following block was added by the Splash Screen component.
	{
		CCommandLineInfo cmdInfo;
		ParseCommandLine(cmdInfo);
		CSplashWnd::EnableSplashScreen(cmdInfo.m_bShowSplash);
	}

	// OS Version Check
	int nOS = getOSInfo();
	if(nOS != 0)
	{
		CString strLS;
		strLS.LoadString(IDS_STRING61204);
		AfxMessageBox(strLS);
		return FALSE;
	}

	//prevent duplication for your application on windows system 
	static const char szProgName[] = "DoManager";

	CreateMutex(NULL, TRUE, szProgName);
	if( GetLastError() == ERROR_ALREADY_EXISTS) 
		return FALSE;

	AfxEnableControlContainer();

	// Standard initialization
	// If you are not using these features and wish to reduce the size
	//  of your final executable, you should remove from the following
	//  the specific initialization routines you do not need.

#ifdef _AFXDLL
	Enable3dControls();			// Call this when using MFC in a shared DLL
#else
	Enable3dControlsStatic();	// Call this when linking to MFC statically
#endif

	// Change the registry key under which our settings are stored.
	// TODO: You should modify this string to be something appropriate
	// such as the name of your company or organization.
	SetRegistryKey(_T("Local AppWizard-Generated Applications"));

	LoadStdProfileSettings();  // Load standard INI file options (including MRU)

	// Register the application's document templates.  Document templates
	//  serve as the connection between documents, frame windows and views.

	CSingleDocTemplate* pDocTemplate;
	pDocTemplate = new CSingleDocTemplate(
		IDR_MAINFRAME,
		RUNTIME_CLASS(CDoManagerDoc),
		RUNTIME_CLASS(CMainFrame),       // main SDI frame window
		RUNTIME_CLASS(CDoManagerView));
	AddDocTemplate(pDocTemplate);

	// Parse command line for standard shell commands, DDE, file open
	CCommandLineInfo cmdInfo;
	ParseCommandLine(cmdInfo);

	// Dispatch commands specified on the command line
	if (!ProcessShellCommand(cmdInfo))
		return FALSE;

	// The one and only window has been initialized, so show and update it.
	m_pMainWnd->ShowWindow(SW_SHOW);
	m_pMainWnd->UpdateWindow();
//	SetWindowPos(m_pMainWnd->GetSafeHwnd(), HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE|SWP_NOSIZE);

	m_bCheck = 0;	// Always Top Menu
	m_bAuto = 0;	// Auto Start

	LPBYTE lbData = 0;
	long datasize=MAX_PATH;
	HKEY hkey;
	LONG retval = RegOpenKeyEx(HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"), 0, KEY_QUERY_VALUE, &hkey);

    if (retval == ERROR_SUCCESS)
	{
		LONG ret = RegQueryValueEx(hkey, "DoManager", 0, 0, (LPBYTE )lbData, (LPDWORD)&datasize);
		if(ret == ERROR_SUCCESS)
			m_bAuto = 1;	// Auto Start RegKey OK
	}
	RegCloseKey(hkey);

	return TRUE;
}


/////////////////////////////////////////////////////////////////////////////
// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	//{{AFX_DATA(CAboutDlg)
	enum { IDD = IDD_ABOUTBOX };
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	//{{AFX_MSG(CAboutDlg)
		// No message handlers
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	//}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	//{{AFX_MSG_MAP(CAboutDlg)
		// No message handlers
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

// App command to run the dialog
void CDoManagerApp::OnAppAbout()
{
	CAboutDlg aboutDlg;
	aboutDlg.DoModal();
}

/////////////////////////////////////////////////////////////////////////////
// CDoManagerApp message handlers


#include "Splash.h"
BOOL CDoManagerApp::PreTranslateMessage(MSG* pMsg)
{
	// CG: The following lines were added by the Splash Screen component.
	if (CSplashWnd::PreTranslateAppMessage(pMsg))
		return TRUE;

	return CWinApp::PreTranslateMessage(pMsg);
}

void CDoManagerApp::OnViewTop() 
{
	// TODO: Add your command handler code here
	m_bCheck = 1 - m_bCheck;
	if(m_bCheck == 0)
		SetWindowPos(m_pMainWnd->GetSafeHwnd(), HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE|SWP_NOSIZE);
	else
		SetWindowPos(m_pMainWnd->GetSafeHwnd(), HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE|SWP_NOSIZE);	
}

void CDoManagerApp::OnUpdateViewTop(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	pCmdUI->SetCheck(m_bCheck);
}

void CDoManagerApp::OnAppAuto() 
{
	// TODO: Add your command handler code here
	m_bAuto = 1 - m_bAuto;
	if(m_bAuto == 0)
	{
		HKEY hkey;
		LONG retval = RegOpenKeyEx(HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"), 0, KEY_WRITE, &hkey);
		if (retval == ERROR_SUCCESS) 
			::RegDeleteValue(hkey, "DoManager");
		RegCloseKey(hkey);
	}
	else
	{
		CString strLS;
		strLS.LoadString(IDS_STRING61205);
		AfxMessageBox(strLS);

		char szTemp[MAX_PATH];
		memset(szTemp, NULL, sizeof(szTemp));
		DWORD dwResultLen =  GetModuleFileName(NULL, szTemp, MAX_PATH);

		HKEY hkey;
		LONG retval = RegOpenKeyEx(HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"), 0, KEY_WRITE, &hkey);

		if (retval == ERROR_SUCCESS) 
			RegSetValueEx(hkey, "DoManager", 0, REG_SZ, (LPBYTE)szTemp, strlen(szTemp));
		RegCloseKey(hkey);
	}
}

void CDoManagerApp::OnUpdateAppAuto(CCmdUI* pCmdUI) 
{
	// TODO: Add your command update UI handler code here
	pCmdUI->SetCheck(m_bAuto);
}
