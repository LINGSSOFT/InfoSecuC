// DialogDetail.cpp : implementation file
//

#include "stdafx.h"
#include "DoPro.h"
#include "DialogDetail.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#include "psapi.h"  /// PSAPI header for computer name and login user name
#pragma comment(lib,"psapi.lib")

#include "./Lib/FileVersionInfo.h"
#pragma comment(lib,"version.lib")


/////////////////////////////////////////////////////////////////////////////
// CDialogDetail dialog


CDialogDetail::CDialogDetail(CWnd* pParent /*=NULL*/)
	: CDialog(CDialogDetail::IDD, pParent)
{
	//{{AFX_DATA_INIT(CDialogDetail)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CDialogDetail::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDialogDetail)
	DDX_Control(pDX, IDC_STATICIMAGE, m_ImageIcon);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CDialogDetail, CDialog)
	//{{AFX_MSG_MAP(CDialogDetail)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDialogDetail message handlers

BOOL CDialogDetail::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// TODO: Add extra initialization here
	// Get Process Handle
	HANDLE hProcess = OpenProcess(PROCESS_QUERY_INFORMATION|PROCESS_VM_READ,FALSE, m_PID);
	if(!hProcess)
		return FALSE;

	// Get FullPath
	HMODULE hMod;
	DWORD cbNeeded;	
	char szFullpath[MAX_PATH];	
	char szPid[32];
	if(EnumProcessModules( hProcess, &hMod, sizeof(hMod), &cbNeeded))
	{
		GetModuleFileNameEx(hProcess, hMod, szFullpath,sizeof(szFullpath));
	}
	CloseHandle(hProcess);

	CFileVersionInfo fvi;
	// Retrieve version information for this module
	if (fvi.Create(szFullpath))
	{
		// Get File Icon
		SHFILEINFO shinfo;
		SHGetFileInfo( szFullpath, NULL, 
					   &shinfo, 
					   sizeof(shinfo), 
					   SHGFI_DISPLAYNAME | SHGFI_ICON);

		// Put Image
		m_ImageIcon.SetIcon(shinfo.hIcon);
		SetDlgItemText(IDC_PIMAGENAME2, fvi.GetProductName());

		// Insert Data
		wsprintf(szPid, "%u", m_PID);
		SetDlgItemText(IDC_PIMAGENAME, fvi.GetOriginalFileName());
		SetDlgItemText(IDC_PVERSION, fvi.GetProductVersion());
		SetDlgItemText(IDC_PFULLPATH, szFullpath);
		SetDlgItemText(IDC_PDESCRIPT, fvi.GetFileDescription());
		SetDlgItemText(IDC_PCOMPANY,fvi.GetCompanyName());
	}
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}
