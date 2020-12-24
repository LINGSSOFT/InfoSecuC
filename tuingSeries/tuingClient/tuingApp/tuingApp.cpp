#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
// tuingApp.cpp : Defines the entry point for the application.
//

#include "framework.h"
#include "tuingApp.h"
#include "psapi.h"  /// PSAPI header for computer name and login user name
#include "Tlhelp32.h"

#pragma comment(lib,"psapi.lib")

#define MAX_LOADSTRING	100
#define APPCLASS		7		// ���߰��� ���α׷� ����
#define BUFSIZE			32
#define INDEX			100		// maximum items
#define TIME_GAP		9000	// 9 seconds
#define APPCOUNT		19		// Applist�� �ִ� �����Ҽ� �ִ� ���� 19��
#define MAX_LENGTH		250
#define DMAX_LEN		512

///////////////////////////////////////////////////////////////////////
//
// Output ����ü(�ֽ� ���� ����)
//
typedef struct AppLog							// ���ø����̼� Log�� ��� ����ü
{
    DWORD dwPID[INDEX];							// PID
    TCHAR szName[INDEX][BUFSIZE + 1];			// Image��
    TCHAR szTime[INDEX][BUFSIZE + 1];			// ������ �ð�
    TCHAR szTitle[INDEX][MAX_LENGTH + 1];		// ĸ�Ǹ�
    DWORD dwType[INDEX];						// ����/���/���� Ÿ��
    DWORD dwDialog[INDEX];						// ���̾�α� ��/��
    DWORD dwIni[INDEX];							// INI ��/��

} *pAppLog;

static AppLog applog;		// Url ����Ʈ ����ü ����

// HwpApp : 6.0			HWP 2004
// XLMAIN				Excel 2003
// OpusApp				Word 2003
// PP11FrameClass		PowerPoint 2003
// AcrobatSDIWindow		Acrobat
// Taro					Taro
static char szClass[APPCLASS][MAX_LENGTH] =
{
    "HwpApp : 6.0", "XLMAIN", "OpusApp", "PP11FrameClass", "AcrobatSDIWindow", "PP12FrameClass", "js:TARO19"
};

////////////////////////////////////////////////////////////////////////
//
// Input ���� ���� (�������� �ٿ���� ini������ ��� ������)
//
static TCHAR szGetAppPath[MAX_LENGTH + 1];			// GetAppLog Path       GetAppLog.ini
static TCHAR szAppListPath[MAX_LENGTH + 1];			// Input APP List Path	AppList.ini
static TCHAR szAppLogPath[MAX_LENGTH + 1];			// Output APP List Path	AppList.ini

static char arrAppList[APPCOUNT][BUFSIZE + 1];		// AppList Name ini���� �о� List�� �迭�� ����
static unsigned int arrAppListType[APPCOUNT + 1];	// AppList Type ini���� �о� List�� �迭�� ����
static unsigned int nAppListCount;					// AppList ���� ����

static int nCountApp;								// App ������ ���� ī��Ʈ (Temp)

static int nLoggap;									// NeoApp.ini ���� �о� ����  ini���Ͽ� write�ϴ� �ֱ�
static const char szProgName[] = "tuingApp";		// �ߺ� ���� ����

// ������
static DWORD WINAPI APP_CheckThread(void* lpVoid);	// App ������
DWORD WINAPI  InputThreadProc(void* dummy);		// APP warning ������
DWORD WINAPI  InputThreadProc1(void* dummy);		// APP Block ������

// �Լ�
BOOL CALLBACK EnumWindowProc(HWND hwnd, LPARAM lParam);
BOOL GetFileDateAsString(LPFILETIME pFt, char* pszDate, unsigned cbIn);	// FileDate�� ���ڿ��� ��ȯ�ϴ� �Լ�
BOOL GetFileTimeAsString(LPFILETIME pFt, char* pszTime, unsigned cbIn);	// FileTime�� ���ڿ��� ��ȯ�ϴ� �Լ�
BOOL CheckApp(char* pszText, int nCount, DWORD dwPid);						// AppList Name�� ����ü �̹��� �� Check
void FilterApp();															// ����ü�� ��� �� ���, ���� ����
void RemoveUnusedApp(DWORD aProcesses[], DWORD cProcesses);
LONG GetRegKey(LPBYTE lbData, int type);									// Regstry �� �������� Value�� �߰� ����
BOOL SafeTerminateProcess(char* pszImage, DWORD dwPID);						// Process Kill
BOOL CompareIni(char* szType, int nType);									// INI ���� ���Ѵ�.
BOOL WriteIni(int nCount, char* szType);									// INI ���� ������ ����ֱ�

// Global Variables:
HINSTANCE hInst;                                // current instance
WCHAR szTitle[MAX_LOADSTRING];                  // The title bar text
WCHAR szWindowClass[MAX_LOADSTRING];            // the main window class name

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  Main Function
//
int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
	// TODO: Place code here.
	CreateMutex(NULL, TRUE, szProgName);			// tuingAPP��� Mutex��ü�� �����Ѵ�.
	if (GetLastError() == ERROR_ALREADY_EXISTS)		// tuingAPP��� Mutex Object�� �̹� �������ִٸ� ������ ��Ű�� �ʴ´�.
	{
		return 0;
	}

	GetTempPath(MAX_LENGTH + 1, szGetAppPath);
	GetTempPath(MAX_LENGTH + 1, szAppListPath);
	GetTempPath(MAX_LENGTH + 1, szAppLogPath);

	strcat(szGetAppPath, "NeoApp.ini");
	strcat(szAppListPath, "AppList.ini");
	strcat(szAppLogPath, "App_Log.ini");

	OutputDebugString(szGetAppPath);
	HANDLE hThread1;
	DWORD dwThreadID1;

	hThread1 = CreateThread(NULL, 0, APP_CheckThread, NULL, 0, &dwThreadID1);
	if (!hThread1)
	{
		OutputDebugString("fails to create the thread!");
		return 0;
	}

	WaitForSingleObject(hThread1, INFINITE);
	CloseHandle(hThread1);

    return 0;
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// �������� �۾������ Check�ϴ� Thread (APP_CheckThread)
//
DWORD WINAPI APP_CheckThread(void* lpVoid)
{
	FILETIME ftCreate, ftExit, ftKernel, ftUser;
	char szItem[128];

	char szFileDate[32] = { 0 };
	char szFileTime[32] = { 0 };

	HANDLE hProcess;
	DWORD aProcesses[1024];
	HMODULE hMod;
	DWORD cbNeeded;
	char szBaseName[MAX_LENGTH];

	int nWrite = 0;			// ���� �ð� ���

	//Initialize the AppLog structure
	ZeroMemory(&applog, sizeof(applog));		// ����ü ó�� �ʱ�ȭ
	nCountApp = 0;							// ����ü�� �����͸� �ֱ� ���� ī��Ʈ

	while (1)	// ���� ����
	{
		// Initialize GetAppLog Configure
		nLoggap = GetPrivateProfileInt("NEOAPP", "LOGGAP", 60000, szGetAppPath);	// ini�� ������ Default 60000

		// Initialize arrAppList, arrAppListType and Setting
		ZeroMemory(&applog, sizeof(applog));		// ����ü ó�� �ʱ�ȭ
		ZeroMemory(&arrAppList, sizeof(arrAppList));
		ZeroMemory(&arrAppListType, sizeof(arrAppListType));
		nAppListCount = GetPrivateProfileInt("APPLIST", "COUNT", 0, szAppListPath);
		if (nAppListCount > 19)		// APPList�� ������ 19���� ���� �ʵ��� �����Ѵ�.
		{
			nAppListCount = 19;
		}
		for (unsigned int j = 0; j < nAppListCount; j++)
		{
			CHAR szBuf[BUFSIZE];	// ���� ���� �ӽú���
			CHAR szTemp[BUFSIZE];	// KeyNmae ���� �ӽú���

			memset(szBuf, NULL, sizeof(szBuf));
			memset(szTemp, NULL, sizeof(szTemp));

			wsprintf(szTemp, "APPNAME%d", j + 1);
			GetPrivateProfileString("APPLIST", szTemp, "", szBuf, sizeof(szBuf), szAppListPath);
			strcpy(arrAppList[j], szBuf);

			memset(szTemp, NULL, sizeof(szTemp));
			wsprintf(szTemp, "APPTYPE%d", j + 1);

			arrAppListType[j] = GetPrivateProfileInt("APPLIST", szTemp, 0, szAppListPath);
		}
		if (nAppListCount == 0)
		{
			// ���� ��� application �� ������ ���μ��� �˻縦 ���� �ʴ´� 
			Sleep(TIME_GAP);
			continue;
		}

		//�������� ��� ���μ����� ����� �����´�
		if (!EnumProcesses(aProcesses, sizeof(aProcesses), &cbNeeded))
			continue;

		// ��� ��ƾ�Ǿ����� ����Ѵ�. 
		DWORD cProcesses = cbNeeded / sizeof(DWORD);
		for (int i = 0; i < (int)cProcesses; i++)
		{
			//���μ��� ���̵� �̿��Ͽ� �ڵ��� ���Ѵ�
			hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, aProcesses[i]);
			if (!hProcess)
			{
				continue;
			}
			// ���μ��� �ڵ��� �̿��Ͽ� �̹��� �̸��� ���´�
			if (EnumProcessModules(hProcess, &hMod, sizeof(hMod), &cbNeeded))
			{
				GetModuleBaseName(hProcess, hMod, szBaseName, sizeof(szBaseName));

				//���μ����� ���۽ð��� �����´�
				if (GetProcessTimes(hProcess, &ftCreate, &ftExit, &ftKernel, &ftUser))
				{
					// Format the user and kernel times, and add to the process node
					if (!ftCreate.dwHighDateTime && !ftCreate.dwLowDateTime)
					{
						memset(szFileDate, 0, sizeof(szFileDate));
						memset(szFileTime, 0, sizeof(szFileTime));

					}
					else
					{
						GetFileDateAsString(&ftCreate, szFileDate, sizeof(szFileDate));
						GetFileTimeAsString(&ftCreate, szFileTime, sizeof(szFileTime));
					}
					wsprintf(szItem, "%s %s", szFileDate, szFileTime);		// szItem ���۽ð�
				}

				// AppList.ini�� ��ϰ� ���� PID�� ���͸� �� ����ü�� �����Ѵ�
				if (CheckApp(szBaseName, nCountApp, aProcesses[i]))				// ����Ʈ�� �̹��� ���� ������ ����ü�� ����, Type ����
				{
					applog.dwPID[nCountApp] = aProcesses[i];
					strcpy(applog.szName[nCountApp], szBaseName);
					if (strlen(applog.szTime[nCountApp]) == 0)
					{
						strcpy(applog.szTime[nCountApp], szItem);
					}
					nCountApp++;
				}
			}
			else
			{
				CloseHandle(hProcess);
				continue;
			}
			CloseHandle(hProcess);
		} // end of for for loop

		::EnumWindows(EnumWindowProc, (LPARAM)0);	//���� �϶� Caption���� ����

		// ������� �ʴ� app �׸� ���� 
		RemoveUnusedApp(aProcesses, cProcesses);

		// ��� ���� �޽���
		FilterApp();

		//���μ����� ������ ������ױ� ������ 
		RemoveUnusedApp(aProcesses, cProcesses);

		// �⺻ Loop�� TimeGap, Write�Ǵ� ������ ������ �����ֱ� ������ ��� �� ��
		// nLoggap(write gap)/TIME_GAP(loop gap)�� ���� ��
		if ((nLoggap / TIME_GAP) < nWrite)
		{
			CompareIni("APPLOG", 0);
			nWrite = 0;
		}
		else
			nWrite++;

		Sleep(TIME_GAP);
	}

	ExitThread(0);
}

BOOL CALLBACK EnumWindowProc(HWND hwnd, LPARAM lParam)
{
	char szTitle[MAX_LENGTH];
	char lpszClassName[MAX_PATH];
	DWORD pid;

	// get window pointer of the parent window
	HWND pHwnd = ::GetParent(hwnd);

	if (pHwnd == NULL)
	{
		ZeroMemory(lpszClassName, MAX_PATH);
		::GetClassName(hwnd, lpszClassName, MAX_PATH);
		::GetWindowThreadProcessId(hwnd, &pid);	// �ֻ��� PID

		//����ü�ȿ��� pid�� ã�Ƽ� �����͸� �����Ѵ�.
		for (int i = 0; i < nCountApp; i++)
		{
			if (applog.dwPID[i] == pid)
			{
				// Office, PDF, HWP ���� ����
				for (int j = 0; j < APPCLASS; j++)
				{
					memset(szTitle, 0, sizeof(szTitle));
					if (strcmp(lpszClassName, szClass[j]) == 0)
					{
						::SendMessage(hwnd, WM_GETTEXT, (WPARAM)MAX_PATH, (LPARAM)szTitle);
						HWND foHwnd = GetForegroundWindow();
						// ��ȭ �Ǿ��� �� �ð��� ĸ���� �����´�.
						if (strcmp(applog.szTitle[i], szTitle) != 0 && strlen(szTitle) != 0 && hwnd == foHwnd)
						{
							strcpy(applog.szTitle[i], szTitle);
							applog.dwDialog[i] = 0;
							applog.dwIni[i] = 0;

							char buf[BUFSIZE];
							memset(buf, NULL, sizeof(buf));
							SYSTEMTIME st;
							GetLocalTime(&st);
							wsprintf(buf, "%d-%d-%d %d:%d:%d", st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond);
							strcpy(applog.szTime[i], buf);
							OutputDebugString(buf);
						}
						// ó�� ���� ���
//						if( strlen(applog.szTitle[i]) == 0 && strlen(szTitle) != 0 )
//						{
//							OutputDebugString(szTitle);
//							strcpy(applog.szTitle[i], szTitle);
//						}
						return true;
					}
					/*					else
										{
											// PID�� ���� ��� ���߰��� �� ���α׷� ��� ���⿡ �ɸ�.
											::SendMessage(hwnd, WM_GETTEXT, (WPARAM)MAX_LENGTH, (LPARAM)szTitle);
											// ���߰��� �� ���α׷� ĸ���� applog�� Ÿ��Ʋ�� ���� ĸ���� ������� ������ ������ ���� ����
											// ���� �з��� �Ͽ� Ÿ��Ʋ�� ���������� ����
											if( strlen(applog.szTitle[i]) == 0 && strlen(szTitle) != 0 )
											{
												OutputDebugString(szTitle);
												strcpy(applog.szTitle[i], szTitle);
											}
										}
					*/
				}
			}
		}	// End of FOR
	}
	return true;
}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// running process
//
BOOL CheckApp(char* pszText, int nCount, DWORD dwPid)
{
	int idx = 0;
	bool bFind = false;

	_strlwr(pszText);
	for (unsigned int i = 0; i < nAppListCount; i++)
	{
		_strlwr(arrAppList[i]);

		if (strlen(arrAppList[i]) == 0)
		{
			continue;
		}
		if (strcmp(arrAppList[i], pszText) == 0)					// ���ô�� ����Ʈ�� ������ 
		{
			idx = i;
			bFind = true;
			break;
		}
	}

	if (bFind)
	{
		for (int j = 0; j < nCount; j++)
		{
			_strlwr(applog.szName[j]);
			if (strcmp(applog.szName[j], pszText) == 0 && applog.dwPID[j] == dwPid)
			{
				//�̹� ����ü�� ����� ������ 
				return false;
			}
		}
		OutputDebugString(pszText);
		//������ ���
		applog.dwType[nCount] = arrAppListType[idx];
		applog.dwDialog[nCount] = 0;
		applog.dwIni[nCount] = 0;

		return true;
	}
	return false;
}

void RemoveUnusedApp(DWORD aProcesses[], DWORD cProcesses)
{
	char buf[DMAX_LEN];
	int nRemove = 0;
	int nCount = nCountApp; // total applog ������ ���� ����  

	for (int i = 0; i < nCount; i++)
	{
		BOOL bExist = FALSE;

		for (int k = 0; k < (int)cProcesses; k++)
		{
			if (applog.dwPID[i] == aProcesses[k])
			{
				bExist = TRUE; // ���μ��� ����Ʈ�� app �������� �ִ�
			}
		}
		// remove an item in applog list
		if (!bExist)
		{
			nRemove++;
			if (i < nCount - 1)
			{
				applog.dwPID[i] = applog.dwPID[i + 1];
				strcpy(applog.szName[i], applog.szName[i + 1]);
				strcpy(applog.szTitle[i], applog.szTitle[i + 1]);
				strcpy(applog.szTime[i], applog.szTime[i + 1]);
				applog.dwType[i] = applog.dwType[i + 1];
				applog.dwIni[i] = applog.dwIni[i + 1];
				applog.dwDialog[i] = applog.dwDialog[i + 1];

				wsprintf(buf, "Remove applog[%d][%s]", i, applog.szName[i]);
				OutputDebugString(buf);
			}
		}
	}

	// applog �������� ������ ���δ� 
	int nVal = nCountApp - nRemove;
	if (nVal > 0)
		nCountApp -= nRemove;
	else
		nCountApp = 0;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// function to arrAppList Filtering
//
void  FilterApp()
{
	int nCount = nCountApp;

	for (int i = 0; i < nCount; i++)
	{
		if (applog.dwType[i] == 1)			// warning Dialog
		{
			if (applog.dwIni[i] == 0)		// INI�� ��� �Ƚ�� üũ
			{
				//OutputDebugString("�����");
				WriteIni(i, "APPLOG");
				applog.dwIni[i] = 1;
			}

			if (applog.dwDialog[i] == 0)		// ���̾�αװ� ���ֳ� �ȶ��ֳ� üũ
			{
				HANDLE hThread;
				DWORD dwThreadID;

				// create a new thread to allow user input
				hThread = CreateThread(NULL, 0, InputThreadProc, (LPVOID)i, 0, &dwThreadID);
				if (!hThread)
				{
					OutputDebugString("Failed to create dialog thread");
				}
				else
				{
					applog.dwDialog[i] = 1;
					// ���̾�α� �����尡 ����Ǵ� ���� ��ٸ��� �ʴ´� 	
					//WaitForSingleObject(hThread, INFINITE);
					//CloseHandle(hThread);
				}
			}
		}
		else if (applog.dwType[i] == 2)							// Terminate Dialog
		{
			SafeTerminateProcess(applog.szName[i], applog.dwPID[i]);	// Process Kill
			if (applog.dwIni[i] == 0)			// INI�� ��� �Ƚ�� üũ
			{
				//OutputDebugString("���ܾ���");
				WriteIni(i, "APPLOG");
				applog.dwIni[i] = 1;
			}
			if (applog.dwDialog[i] == 0)
			{
				HANDLE hThread;		// Terminate Thread
				DWORD dwThreadID;	// Terminate ThreadID

				// create a new thread to allow user input
				hThread = CreateThread(NULL, 0, InputThreadProc1, (LPVOID)i, 0, &dwThreadID);
				if (!hThread)
				{
					OutputDebugString("Failed to create dialog thread");
				}
				else
				{
					applog.dwDialog[i] = 1;
					// ���̾�α� �����尡 ����Ǵ� ���� ��ٸ��� �ʴ´� 	
					//WaitForSingleObject(hThread, INFINITE);
					//CloseHandle(hThread);
				}
			}
		}
	}
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// thread for dialog of warning
//
DWORD WINAPI  InputThreadProc(void* dummy)
{
	// create the dialog window
	HWND hWnd = ::CreateDialog(NULL, MAKEINTRESOURCE(IDD_WARNDIALOG), ::GetDesktopWindow(), NULL);
	if (hWnd != NULL)
	{
		// show dialog
		::SetWindowPos(hWnd, HWND_TOPMOST, 300, 300, 350, 300, SWP_NOSIZE);

		HICON hIcon = LoadIcon((HINSTANCE)GetModuleHandle(NULL), MAKEINTRESOURCE(IDI_ICON_WARN));
		if (hIcon)
		{
			HWND hwnd2 = GetDlgItem(hWnd, IDC_ICONSTATIC);
			SendMessage(hwnd2, STM_SETICON, (WPARAM)hIcon, 0);
		}
		else
			OutputDebugString("loadicon fails");

		SetDlgItemText(hWnd, IDC_WARN_STATIC1, applog.szName[(int)dummy]);

		char szLoadTemp[MAX_LENGTH];
		memset(szLoadTemp, NULL, sizeof(szLoadTemp));
		LoadString(NULL, IDS_STRING2, szLoadTemp, sizeof(szLoadTemp));
		SetDlgItemText(hWnd, IDC_WARN_STATIC2, szLoadTemp);

		::ShowWindow(hWnd, SW_SHOW);
	}
	else
	{
		OutputDebugString("Failed to create dialog");
		ExitThread(1);
		return 1;
	}
	// message loop to process user input
	MSG msg;
	while (1)
	{
		if (::PeekMessage(&msg, hWnd, 0, 0, PM_REMOVE))
		{
			if ((msg.message == WM_COMMAND && msg.wParam == IDCANCEL) ||
				(msg.message == WM_LBUTTONDOWN && msg.wParam == IDOK))
			{
				applog.dwDialog[(int)dummy] = 0;
				// destroy the dialog and get out of the message loop
				::DestroyWindow(hWnd);
				break;
			}

			// process message
			::TranslateMessage(&msg);
			::DispatchMessage(&msg);
		}
		else
		{
			// if there is no message to process, 
			// then sleep for a while to avoid burning
			// too much CPU cycles
			::Sleep(100);
		}
	}
	ExitThread(1);
	return 1;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// thread for dialog of blocking
//
DWORD WINAPI  InputThreadProc1(void* dummy)
{
	// create the dialog window
	HWND hWnd = ::CreateDialog(NULL, MAKEINTRESOURCE(IDD_BLOCKDIALOG), NULL, NULL);
	if (hWnd != NULL)
	{
		// show dialog
		::SetWindowPos(hWnd, HWND_TOPMOST, 300, 300, 350, 300, SWP_NOSIZE);

		HICON hIcon = LoadIcon((HINSTANCE)GetModuleHandle(NULL), MAKEINTRESOURCE(IDI_ICON_BLOCK));
		if (hIcon)
		{
			HWND hwnd2 = GetDlgItem(hWnd, IDC_ICONSTATIC);
			SendMessage(hwnd2, STM_SETICON, (WPARAM)hIcon, 0);
		}
		else
			OutputDebugString("loadicon fails");

		SetDlgItemText(hWnd, IDC_BLOCK_STATIC1, applog.szName[(int)dummy]);

		char szLoadTemp[MAX_LENGTH];
		memset(szLoadTemp, NULL, sizeof(szLoadTemp));
		LoadString(NULL, IDS_STRING2, szLoadTemp, sizeof(szLoadTemp));
		SetDlgItemText(hWnd, IDC_BLOCK_STATIC2, szLoadTemp);

		::ShowWindow(hWnd, SW_SHOW);
	}
	else
	{
		OutputDebugString("Failed to create dialog");
		ExitThread(1);
		return 1;
	}
	// message loop to process user input
	MSG msg;
	while (1)
	{
		if (::PeekMessage(&msg, hWnd, 0, 0, PM_REMOVE))
		{
			if ((msg.message == WM_COMMAND && msg.wParam == IDCANCEL) ||
				(msg.message == WM_LBUTTONDOWN && msg.wParam == IDOK))
			{
				//applog.dwDialog[(int)dummy] = 0;
				// destroy the dialog and get out of the message loop
				::DestroyWindow(hWnd);
				break;
			}

			// process message
			::TranslateMessage(&msg);
			::DispatchMessage(&msg);
		}
		else
		{
			// if there is no message to process, 
			// then sleep for a while to avoid burning
			// too much CPU cycles
			::Sleep(100);
		}
	}
	ExitThread(1);
	return 1;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Common Function
//
LONG GetRegKey(LPBYTE lbData, int type)
{
	long datasize = MAX_LENGTH;
	HKEY hkey;
	LONG retval = RegOpenKeyEx(HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\BNASolutions\\NeoSystem"), 0, KEY_QUERY_VALUE, &hkey);

	if (retval == ERROR_SUCCESS)
	{
		//LPBYTE lbData = retdata;
		if (type == 1)
			RegQueryValueEx(hkey, "installpath", 0, 0, (LPBYTE)lbData, (LPDWORD)&datasize);

		RegCloseKey(hkey);
	}
	return datasize;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// function to convert FILETIME to meaningful string
//
BOOL GetFileDateAsString(LPFILETIME pFt, char* pszDate, unsigned cbIn)
{
	FILETIME ftLocal;
	SYSTEMTIME st;

	if (!FileTimeToLocalFileTime(pFt, &ftLocal))
		return FALSE;

	if (!FileTimeToSystemTime(&ftLocal, &st))
		return FALSE;

	char szTemp[12];

	wsprintf(szTemp, "%04u-%02u-%02u", st.wYear, st.wMonth, st.wDay);
	lstrcpyn(pszDate, szTemp, cbIn);
	return TRUE;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// function to convert FILETIME to meaningful string
//
BOOL GetFileTimeAsString(LPFILETIME pFt, char* pszTime, unsigned cbIn)
{
	FILETIME ftLocal;
	SYSTEMTIME st;

	if (!FileTimeToLocalFileTime(pFt, &ftLocal))
		return FALSE;

	if (!FileTimeToSystemTime(&ftLocal, &st))
		return FALSE;

	char szTemp[12];

	wsprintf(szTemp, "%02u:%02u:%02u", st.wHour, st.wMinute, st.wSecond);
	lstrcpyn(pszTime, szTemp, cbIn);
	return TRUE;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// �������� ���μ��� Kill
//
BOOL SafeTerminateProcess(char* pszImage, DWORD dwPID/*HANDLE hProcess, UINT uExitCode*/)
{
	///////////////////////////////////////////////////////////////
	// Finding Application which we have specified once that 
	// application finds it will give you message Application found
	// then first of all it opens that process and getting all rights 
	// of that application.Once we have all rights then we can kill 
	// that application with Terminate Process.
	///////////////////////////////////////////////////////////////
	HANDLE hndl = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
	if ((DWORD)hndl == -1)
	{
		//DWORD dwsma = GetLastError();
		return FALSE;
	}

	DWORD dwExitCode = 0;

	PROCESSENTRY32  procEntry = { 0 };
	procEntry.dwSize = sizeof(PROCESSENTRY32);
	Process32First(hndl, &procEntry);
	do
	{
		if (!_strnicmp(procEntry.szExeFile, pszImage, strlen(pszImage)))
			//if(!strcmp(procEntry.szExeFile, pszImage))
		{
			if (procEntry.th32ProcessID == dwPID)
			{

				HANDLE hHandle;
				hHandle = ::OpenProcess(PROCESS_ALL_ACCESS, 0, procEntry.th32ProcessID);
				if (hHandle)
				{
					::GetExitCodeProcess(hHandle, &dwExitCode);
					::TerminateProcess(hHandle, dwExitCode);
					CloseHandle(hHandle);
				}
				break;
			}
		}

	} while (Process32Next(hndl, &procEntry));

	///////////////////////////////////////////////////////////////	
	//		HANDLE hHandle;
	//		hHandle = ::OpenProcess(PROCESS_ALL_ACCESS,0,procEntry.th32ProcessID);
	//		
	//		::GetExitCodeProcess(hHandle,&dwExitCode);
	//		::TerminateProcess(hHandle,dwExitCode);
	CloseHandle(hndl);
	return TRUE;
}

BOOL CompareIni(char* szType, int nType)
{
	if (strcmp(szType, "APPLOG") == 0)
	{
		for (int i = 0; i < nCountApp; i++)
		{
			if (strlen(applog.szTitle[i]) == 0)
			{
				continue;
			}
			if (applog.dwIni[i] == 0 && applog.dwType[i] == 0)
			{
				// APP ����ü���� ini�� �Ⱦ��͸� �׸��� �����ΰ͸� 
				//OutputDebugString("���þ���");
				WriteIni(i, szType);
				applog.dwIni[i] = 1;
			}
		}
		return true;
	}
	OutputDebugString("unknown ini type");
	return false;
}

BOOL WriteIni(int nCount, char* szType)
{
	if (strcmp(szType, "APPLOG") == 0)
	{
		OutputDebugString(szAppLogPath);
		int nRegCount = GetPrivateProfileInt(szType, "COUNT", 0, szAppLogPath);
		CHAR szTemp[BUFSIZE];		// ini APP/URL Section
		char buf[10];				// App/Url Type
		memset(buf, NULL, sizeof(buf));

		wsprintf(buf, "%d", nRegCount + 1);
		WritePrivateProfileString("APPLOG", "COUNT", buf, szAppLogPath);

		memset(szTemp, NULL, sizeof(szTemp));
		wsprintf(szTemp, "APPNAME%d", nRegCount + 1);
		WritePrivateProfileString("APPLOG", szTemp, applog.szName[nCount], szAppLogPath);

		if (strlen(applog.szTitle[nCount]) == 0)
		{
			wsprintf(applog.szTitle[nCount], "Wanning Program!!", applog.szTitle[nCount]);
		}
		OutputDebugString(applog.szTitle[nCount]);
		memset(szTemp, NULL, sizeof(szTemp));
		wsprintf(szTemp, "APPTITLE%d", nRegCount + 1);
		WritePrivateProfileString("APPLOG", szTemp, applog.szTitle[nCount], szAppLogPath);

		memset(szTemp, NULL, sizeof(szTemp));
		wsprintf(szTemp, "APPTIME%d", nRegCount + 1);
		WritePrivateProfileString("APPLOG", szTemp, applog.szTime[nCount], szAppLogPath);

		memset(szTemp, NULL, sizeof(szTemp));
		wsprintf(szTemp, "APPTYPE%d", nRegCount + 1);

		memset(buf, NULL, sizeof(buf));
		wsprintf(buf, "%d", applog.dwType[nCount]);
		WritePrivateProfileString("APPLOG", szTemp, buf, szAppLogPath);

		OutputDebugString("Success Write APPLOG Ini\n");
		return true;
	}
	return false;
}
