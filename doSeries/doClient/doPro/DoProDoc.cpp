// DoProDoc.cpp : implementation of the CDoProDoc class
//

#include "stdafx.h"
#include "DoPro.h"

#include "DoProDoc.h"
#include "DialogHelp.h"
#include "DialogError.h"
#include "DialogUpdate.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDoProDoc

IMPLEMENT_DYNCREATE(CDoProDoc, CDocument)

BEGIN_MESSAGE_MAP(CDoProDoc, CDocument)
	//{{AFX_MSG_MAP(CDoProDoc)
	ON_COMMAND(ID_PWR_LOGOFF, OnPwrLogoff)
	ON_COMMAND(ID_PWR_OFF, OnPwrOff)
	ON_COMMAND(ID_PWR_REBOOT, OnPwrReboot)
	ON_COMMAND(ID_HELP_MANUAL, OnHelpManual)
	ON_COMMAND(ID_HELP_ERROR, OnHelpError)
	ON_COMMAND(ID_HELP_UPDATE, OnHelpUpdate)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDoProDoc construction/destruction

CDoProDoc::CDoProDoc()
{
	// TODO: add one-time construction code here

}

CDoProDoc::~CDoProDoc()
{
}

BOOL CDoProDoc::OnNewDocument()
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
// CDoProDoc serialization

void CDoProDoc::Serialize(CArchive& ar)
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
// CDoProDoc diagnostics

#ifdef _DEBUG
void CDoProDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CDoProDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CDoProDoc commands

void CDoProDoc::OnPwrLogoff() 
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

void CDoProDoc::OnPwrReboot() 
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

void CDoProDoc::OnPwrOff() 
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

void CDoProDoc::OnHelpManual()
{
	// TODO: Add your command handler code here
	CDialogHelp dlg;
	dlg.DoModal();
}

void CDoProDoc::OnHelpError()
{
	// TODO: Add your command handler code here
	CDialogError dlg;
	dlg.DoModal();
}

void CDoProDoc::OnHelpUpdate()
{
	// TODO: Add your command handler code here
	CDialogUpdate dlg;
	dlg.DoModal();
}
