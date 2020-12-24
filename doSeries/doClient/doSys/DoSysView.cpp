// DoSysView.cpp : implementation of the CDoSysView class
//

#include "stdafx.h"
#include "DoSys.h"

#include "DoSysDoc.h"
#include "DoSysView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#include "winsvc.h"
#pragma comment(lib,"advapi32.lib")

/////////////////////////////////////////////////////////////////////////////
// CDoSysView

IMPLEMENT_DYNCREATE(CDoSysView, CFormView)

BEGIN_MESSAGE_MAP(CDoSysView, CFormView)
	//{{AFX_MSG_MAP(CDoSysView)
	ON_WM_PAINT()
	ON_WM_SIZE()
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB_MAIN, OnSelchangeTabMain)
	ON_NOTIFY(NM_DBLCLK, IDC_LIST_INSTALL, OnDblclkListInstall)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDoSysView construction/destruction

CDoSysView::CDoSysView()
	: CFormView(CDoSysView::IDD)
{
	//{{AFX_DATA_INIT(CDoSysView)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// TODO: add construction code here

}

CDoSysView::~CDoSysView()
{
}

void CDoSysView::DoDataExchange(CDataExchange* pDX)
{
	CFormView::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDoSysView)
	DDX_Control(pDX, IDC_TREE_SYSTEM, m_TreeSystem);
	DDX_Control(pDX, IDC_TREE_REGIST, m_TreeRegist);
	DDX_Control(pDX, IDC_TREE_PACKET, m_TreePacket);
	DDX_Control(pDX, IDC_TREE_NETWORK, m_TreeNetwork);
	DDX_Control(pDX, IDC_LIST_SYSTEM, m_ListSystem);
	DDX_Control(pDX, IDC_LIST_SERVICE, m_ListService);
	DDX_Control(pDX, IDC_LIST_REGIST, m_ListRegist);
	DDX_Control(pDX, IDC_LIST_PACKET, m_ListPacket);
	DDX_Control(pDX, IDC_LIST_NETWORK, m_ListNetwork);
	DDX_Control(pDX, IDC_LIST_INSTALL, m_ListInstall);
	DDX_Control(pDX, IDC_TAB_MAIN, m_TabMain);
	//}}AFX_DATA_MAP
}

BOOL CDoSysView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CFormView::PreCreateWindow(cs);
}

void CDoSysView::OnInitialUpdate()
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
	tabSrting1.LoadString(IDS_STRING61205);
	tabSrting2.LoadString(IDS_STRING61206);
	tabSrting3.LoadString(IDS_STRING61207);
	tabSrting4.LoadString(IDS_STRING61208);
	tabSrting5.LoadString(IDS_STRING61209);
	tabSrting6.LoadString(IDS_STRING61210);

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

	// 1. System ListCtrl & Tree Setting
	m_ListSystem.SetExtendedStyle(m_ListSystem.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
	m_ImgListSystem.Create( GetSystemMetrics(SM_CXSMICON),
		          GetSystemMetrics(SM_CYSMICON),
				  ILC_COLOR24|ILC_MASK, 50, 50);
	m_ImgListSystem.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ListSystem.SetImageList(&m_ImgListSystem, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61211);
	m_ListSystem.SetHeadings(strLS);
	m_ListSystem.LoadColumnInfo();

	// 2. Network ListCtrl & Tree Setting
	m_ListNetwork.SetExtendedStyle(m_ListNetwork.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
//	m_ImgListSystem.Create( GetSystemMetrics(SM_CXSMICON),
//		          GetSystemMetrics(SM_CYSMICON),
//				  ILC_COLOR24|ILC_MASK, 50, 50);
//	m_ImgListSystem.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ListNetwork.SetImageList(&m_ImgListSystem, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61211);
	m_ListNetwork.SetHeadings(strLS);
	m_ListNetwork.LoadColumnInfo();

	// 3. Packet ListCtrl & Tree Setting
	m_ListPacket.SetExtendedStyle(m_ListPacket.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |

	strLS.LoadString(IDS_STRING61211);
	m_ListPacket.SetHeadings(strLS);
	m_ListPacket.LoadColumnInfo();

	// 4. Install ListCtrl Setting
	m_ListInstall.SetExtendedStyle(m_ListInstall.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
//	m_ImgListSystem.Create( GetSystemMetrics(SM_CXSMICON),
//		          GetSystemMetrics(SM_CYSMICON),
//				  ILC_COLOR24|ILC_MASK, 50, 50);
//	m_ImgListSystem.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ListInstall.SetImageList(&m_ImgListSystem, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61221);
	m_ListInstall.SetHeadings(strLS);
	m_ListInstall.LoadColumnInfo();

	// 5. Service ListCtrl Setting
	m_ListService.SetExtendedStyle(m_ListService.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
//	m_ImgListSystem.Create( GetSystemMetrics(SM_CXSMICON),
//		          GetSystemMetrics(SM_CYSMICON),
//				  ILC_COLOR24|ILC_MASK, 50, 50);
//	m_ImgListSystem.SetBkColor( GetSysColor(COLOR_WINDOW) );
//	m_ImgListSystem.Add(AfxGetApp()->LoadIcon(IDI_SERVICE));
	m_ListService.SetImageList(&m_ImgListSystem, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61222);
	m_ListService.SetHeadings(strLS);
	m_ListService.LoadColumnInfo();

	// 6. Regist ListCtrl & Tree Setting
	m_ListRegist.SetExtendedStyle(m_ListRegist.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |
//	m_ImgListSystem.Create( GetSystemMetrics(SM_CXSMICON),
//		          GetSystemMetrics(SM_CYSMICON),
//				  ILC_COLOR24|ILC_MASK, 50, 50);
//	m_ImgListSystem.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ListRegist.SetImageList(&m_ImgListSystem, LVSIL_SMALL );

	strLS.LoadString(IDS_STRING61223);
	m_ListRegist.SetHeadings(strLS);
	m_ListRegist.LoadColumnInfo();

/*	m_ImgTreeRegist.Create( GetSystemMetrics(SM_CXSMICON), 
							GetSystemMetrics(SM_CYSMICON), 
							ILC_COLOR24|ILC_MASK, 50, 50);
	m_ImgTreeRegist.SetBkColor( GetSysColor(COLOR_WINDOW) );
	m_ImgTreeRegist.Add(AfxGetApp()->LoadIcon(IDI_REGCOM));
	m_ImgTreeRegist.Add(AfxGetApp()->LoadIcon(IDI_REGOPEN));
*/	
	m_TreeRegist.SetImageList( &m_ImgListSystem, TVSIL_NORMAL );

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

	SetWindow();					// Resize
	LoadInstall();					// Install
	LoadService();					// Services
//	LoadTreeReg(&m_TreeRegist);		// Regist
}

/////////////////////////////////////////////////////////////////////////////
// CDoSysView diagnostics

#ifdef _DEBUG
void CDoSysView::AssertValid() const
{
	CFormView::AssertValid();
}

void CDoSysView::Dump(CDumpContext& dc) const
{
	CFormView::Dump(dc);
}

CDoSysDoc* CDoSysView::GetDocument() // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CDoSysDoc)));
	return (CDoSysDoc*)m_pDocument;
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CDoSysView message handlers

void CDoSysView::OnPaint() 
{
	CPaintDC dc(this); // device context for painting
	
	// TODO: Add your message handler code here
	m_ListSystem.Invalidate(TRUE);
	m_ListNetwork.Invalidate(TRUE);
	m_ListPacket.Invalidate(TRUE);
	m_ListInstall.Invalidate(TRUE);
	m_ListService.Invalidate(TRUE);
	m_ListRegist.Invalidate(TRUE);
	m_TreeSystem.Invalidate(TRUE);
	m_TreeNetwork.Invalidate(TRUE);
	m_TreePacket.Invalidate(TRUE);
	m_TreeRegist.Invalidate(TRUE);
	// Do not call CFormView::OnPaint() for painting messages
}

void CDoSysView::OnSize(UINT nType, int cx, int cy) 
{
	CFormView::OnSize(nType, cx, cy);
	
	// TODO: Add your message handler code here
	if(m_TabMain.m_hWnd != NULL)		// Tab Control
	{
		CRect rc(0, 0, cx, cy);
		m_TabMain.MoveWindow(&rc);
	}

	if(m_TreeSystem.m_hWnd != NULL)		// System Tree Control
	{
		CRect rc(0, 23, 200, cy);
		m_TreeSystem.MoveWindow(&rc);
	}

	if(m_ListSystem.m_hWnd != NULL)		// System List Control
	{
		CRect rc(200, 23, cx, cy);
		m_ListSystem.MoveWindow(&rc);
		m_ListSystem.Resize(rc.Width(), rc.Height(), 0);
	}

	if(m_TreeNetwork.m_hWnd != NULL)	// Network Tree Control
	{
		CRect rc(0, 23, 200, cy);
		m_TreeNetwork.MoveWindow(&rc);
	}

	if(m_ListNetwork.m_hWnd != NULL)	// Network List Control
	{
		CRect rc(200, 23, cx, cy);
		m_ListNetwork.MoveWindow(&rc);
		m_ListNetwork.Resize(rc.Width(), rc.Height(), 1);
	}

	if(m_TreePacket.m_hWnd != NULL)		// Packet Tree Control
	{
		CRect rc(0, 23, 200, cy);
		m_TreePacket.MoveWindow(&rc);
	}

	if(m_ListPacket.m_hWnd != NULL)		// Packet List Control
	{
		CRect rc(200, 23, cx, cy);
		m_ListPacket.MoveWindow(&rc);
		m_ListPacket.Resize(rc.Width(), rc.Height(), 2);
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

void CDoSysView::OnSelchangeTabMain(NMHDR* pNMHDR, LRESULT* pResult) 
{
	// TODO: Add your control notification handler code here
	m_nTabNum = m_TabMain.GetCurSel();
	SetWindow();
	*pResult = 0;
}

void CDoSysView::SetWindow()
{
	switch(m_nTabNum)
	{
		case 0:
			GetDlgItem(IDC_TREE_SYSTEM)->ShowWindow(SW_SHOW);		// System Tree
			GetDlgItem(IDC_LIST_SYSTEM)->ShowWindow(SW_SHOW);		// System List

			GetDlgItem(IDC_TREE_NETWORK)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_NETWORK)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_PACKET)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_PACKET)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_INSTALL)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SERVICE)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_REGIST)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_REGIST)->ShowWindow(SW_HIDE);
			m_TreeSystem.Invalidate(TRUE);
			m_ListSystem.Invalidate(TRUE);
			break;
		case 1:
			GetDlgItem(IDC_TREE_NETWORK)->ShowWindow(SW_SHOW);		// Network Tree
			GetDlgItem(IDC_LIST_NETWORK)->ShowWindow(SW_SHOW);		// Network List

			GetDlgItem(IDC_TREE_SYSTEM)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SYSTEM)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_PACKET)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_PACKET)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_INSTALL)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SERVICE)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_REGIST)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_REGIST)->ShowWindow(SW_HIDE);
			m_TreeNetwork.Invalidate(TRUE);
			m_ListNetwork.Invalidate(TRUE);
			break;
		case 2:
			GetDlgItem(IDC_TREE_PACKET)->ShowWindow(SW_SHOW);		// Packet Tree
			GetDlgItem(IDC_LIST_PACKET)->ShowWindow(SW_SHOW);		// Packet List

			GetDlgItem(IDC_TREE_SYSTEM)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SYSTEM)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_NETWORK)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_NETWORK)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_INSTALL)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SERVICE)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_REGIST)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_REGIST)->ShowWindow(SW_HIDE);
			m_TreePacket.Invalidate(TRUE);
			m_ListPacket.Invalidate(TRUE);
			break;
		case 3:
			GetDlgItem(IDC_LIST_INSTALL)->ShowWindow(SW_SHOW);		// Install List

			GetDlgItem(IDC_TREE_SYSTEM)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SYSTEM)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_NETWORK)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_NETWORK)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_PACKET)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_PACKET)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SERVICE)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_REGIST)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_REGIST)->ShowWindow(SW_HIDE);
			m_ListInstall.Invalidate(TRUE);
			break;
		case 4:
			GetDlgItem(IDC_LIST_SERVICE)->ShowWindow(SW_SHOW);		// Service List

			GetDlgItem(IDC_TREE_SYSTEM)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SYSTEM)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_NETWORK)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_NETWORK)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_PACKET)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_PACKET)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_INSTALL)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_REGIST)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_REGIST)->ShowWindow(SW_HIDE);
			m_ListService.Invalidate(TRUE);
			break;
		case 5:
			GetDlgItem(IDC_TREE_REGIST)->ShowWindow(SW_SHOW);		// Regist Tree
			GetDlgItem(IDC_LIST_REGIST)->ShowWindow(SW_SHOW);		// Regist List

			GetDlgItem(IDC_TREE_SYSTEM)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SYSTEM)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_NETWORK)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_NETWORK)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_TREE_PACKET)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_PACKET)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_INSTALL)->ShowWindow(SW_HIDE);
			GetDlgItem(IDC_LIST_SERVICE)->ShowWindow(SW_HIDE);
			m_TreeRegist.Invalidate(TRUE);
			m_ListRegist.Invalidate(TRUE);
			break;
	}
}

void CDoSysView::LoadInstall()
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
					BYTE buff1[MAX_PATH];
					BYTE buff2[MAX_PATH];
					BYTE buff3[MAX_PATH];
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
							InstallDate.LoadString(IDS_STRING61216);
						if(UninstallString == "")
							UninstallString.LoadString(IDS_STRING61217);
						else
							UninstallString.LoadString(IDS_STRING61218);

						m_ListInstall.AddItem(0, DisPlayName, InstallDate, UninstallString);
					}
					RegCloseKey(hQureKey);
				}
			}
		}	// End of For
	}
	else	// No Find
	{
		CString strTemp;
		strTemp.LoadString(IDS_STRING61219);
		m_ListInstall.AddItem(NULL, strTemp, "", "");
	}
	RegCloseKey( key );
}

void CDoSysView::LoadService()
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
			strStatus.LoadString(IDS_STRING61220);
		m_ListService.AddItem(0, pInfo[i].lpDisplayName, "", pInfo[i].lpServiceName, strStatus, "");
	}
	free(pBuf);
	CloseServiceHandle(hSCM);
}

void CDoSysView::OnDblclkListInstall(NMHDR* pNMHDR, LRESULT* pResult) 
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
