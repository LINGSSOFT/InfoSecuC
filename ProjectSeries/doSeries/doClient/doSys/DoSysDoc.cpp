// DoSysDoc.cpp : implementation of the CDoSysDoc class
//

#include "stdafx.h"
#include "DoSys.h"

#include "DoSysDoc.h"
#include "DialogHelp.h"
#include "DialogError.h"
#include "DialogUpdate.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDoSysDoc

IMPLEMENT_DYNCREATE(CDoSysDoc, CDocument)

BEGIN_MESSAGE_MAP(CDoSysDoc, CDocument)
	//{{AFX_MSG_MAP(CDoSysDoc)
	ON_COMMAND(ID_PWR_LOGOFF, OnPwrLogoff)
	ON_COMMAND(ID_PWR_REBOOT, OnPwrReboot)
	ON_COMMAND(ID_PWR_OFF, OnPwrOff)
	ON_COMMAND(ID_HELP_MANUAL, OnHelpManual)
	ON_COMMAND(ID_HELP_UPDATE, OnHelpUpdate)
	ON_COMMAND(ID_HELP_ERROR, OnHelpError)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDoSysDoc construction/destruction

CDoSysDoc::CDoSysDoc()
{
	// TODO: add one-time construction code here

}

CDoSysDoc::~CDoSysDoc()
{
}

BOOL CDoSysDoc::OnNewDocument()
{
	if (!CDocument::OnNewDocument())
		return FALSE;

	// TODO: add reinitialization code here
	// (SDI documents will reuse this document)
	CString str;
	str.LoadString(IDS_CAPTION);
	SetTitle(str);

	return TRUE;
}



/////////////////////////////////////////////////////////////////////////////
// CDoSysDoc serialization

void CDoSysDoc::Serialize(CArchive& ar)
{
	if (ar.IsStoring())
	{
		// TODO: add storing code here
	}
	else
	{
		// TODO: add loading code here
	}
}

/////////////////////////////////////////////////////////////////////////////
// CDoSysDoc diagnostics

#ifdef _DEBUG
void CDoSysDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CDoSysDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CDoSysDoc commands

void CDoSysDoc::OnPwrLogoff() 
{
	// TODO: Add your command handler code here
	HANDLE hToken;
	TOKEN_PRIVILEGES tkp;
	if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY,  &hToken ) )
	{
		OutputDebugString("Process Failed : Open ProcessToken");
		return;
	}
	LookupPrivilegeValue(NULL, SE_SHUTDOWN_NAME, &tkp.Privileges[0].Luid );
	tkp.PrivilegeCount = 1;
	tkp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
	AdjustTokenPrivileges( hToken, FALSE, &tkp, 0, (PTOKEN_PRIVILEGES)NULL, 0);
	if (GetLastError() != ERROR_SUCCESS)
	{
		OutputDebugString("Process Failed : AdjustTokenPrivileges");
		return;
	}
	if (!ExitWindowsEx(EWX_LOGOFF | EWX_FORCE, 0 ))
	{
		OutputDebugString("Process Failed : ExitWindowsEx");
		return;
	}
	CloseHandle(hToken);
}

void CDoSysDoc::OnPwrReboot() 
{
	// TODO: Add your command handler code here
	HANDLE hToken;
	TOKEN_PRIVILEGES tkp;
	if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY,  &hToken ) )
	{
		OutputDebugString("Process Failed : Open ProcessToken");
		return;
	}
	LookupPrivilegeValue(NULL, SE_SHUTDOWN_NAME, &tkp.Privileges[0].Luid );
	tkp.PrivilegeCount = 1;
	tkp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
	AdjustTokenPrivileges( hToken, FALSE, &tkp, 0, (PTOKEN_PRIVILEGES)NULL, 0);
	if (GetLastError() != ERROR_SUCCESS)
	{
		OutputDebugString("Process Failed : AdjustTokenPrivileges");
		return;
	}
	if (!ExitWindowsEx(EWX_REBOOT | EWX_FORCE, 0 ))
	{
		OutputDebugString("Process Failed : ExitWindowsEx");
		return;
	}
	CloseHandle(hToken);
}

void CDoSysDoc::OnPwrOff() 
{
	// TODO: Add your command handler code here
	HANDLE hToken;
	TOKEN_PRIVILEGES tkp;
	if (!OpenProcessToken(GetCurrentProcess(), TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY,  &hToken ) )
	{
		OutputDebugString("Process Failed : Open ProcessToken");
		return;
	}
	LookupPrivilegeValue(NULL, SE_SHUTDOWN_NAME, &tkp.Privileges[0].Luid );
	tkp.PrivilegeCount = 1;
	tkp.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
	AdjustTokenPrivileges( hToken, FALSE, &tkp, 0, (PTOKEN_PRIVILEGES)NULL, 0);
	if (GetLastError() != ERROR_SUCCESS)
	{
		OutputDebugString("Process Failed : AdjustTokenPrivileges");
		return;
	}
	if (!ExitWindowsEx(EWX_POWEROFF | EWX_FORCE, 0 ))
	{
		OutputDebugString("Process Failed : ExitWindowsEx");
		return;
	}
	CloseHandle(hToken);
}

void CDoSysDoc::OnHelpManual() 
{
	// TODO: Add your command handler code here
	CDialogHelp dlg;
	dlg.DoModal();
}

void CDoSysDoc::OnHelpError() 
{
	// TODO: Add your command handler code here
	CDialogError dlg;
	dlg.DoModal();
}

void CDoSysDoc::OnHelpUpdate() 
{
	// TODO: Add your command handler code here
	CDialogUpdate dlg;
	dlg.DoModal();
}
