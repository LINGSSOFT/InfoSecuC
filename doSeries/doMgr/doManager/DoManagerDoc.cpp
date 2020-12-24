// DoManagerDoc.cpp : implementation of the CDoManagerDoc class
//

#include "stdafx.h"
#include "DoManager.h"

#include "DoManagerDoc.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDoManagerDoc

IMPLEMENT_DYNCREATE(CDoManagerDoc, CDocument)

BEGIN_MESSAGE_MAP(CDoManagerDoc, CDocument)
	//{{AFX_MSG_MAP(CDoManagerDoc)
	ON_COMMAND(ID_PWR_LOGOFF, OnPwrLogoff)
	ON_COMMAND(ID_PWR_REBOOT, OnPwrReboot)
	ON_COMMAND(ID_PWR_OFF, OnPwrOff)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDoManagerDoc construction/destruction

CDoManagerDoc::CDoManagerDoc()
{
	// TODO: add one-time construction code here

}

CDoManagerDoc::~CDoManagerDoc()
{
}

BOOL CDoManagerDoc::OnNewDocument()
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
// CDoManagerDoc serialization

void CDoManagerDoc::Serialize(CArchive& ar)
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
// CDoManagerDoc diagnostics

#ifdef _DEBUG
void CDoManagerDoc::AssertValid() const
{
	CDocument::AssertValid();
}

void CDoManagerDoc::Dump(CDumpContext& dc) const
{
	CDocument::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// CDoManagerDoc commands

void CDoManagerDoc::OnPwrLogoff() 
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

void CDoManagerDoc::OnPwrReboot() 
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

void CDoManagerDoc::OnPwrOff() 
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
