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
#define APPCLASS		7		// 집중관리 프로그램 갯수
#define BUFSIZE			32
#define INDEX			100		// maximum items
#define TIME_GAP		9000	// 9 seconds
#define APPCOUNT		19		// Applist의 최대 지정할수 있는 갯수 19개
#define MAX_LENGTH		250
#define DMAX_LEN		512

///////////////////////////////////////////////////////////////////////
//
// Output 구조체(최신 정보 갱신)
//
typedef struct AppLog							// 애플리케이션 Log를 담는 구조체
{
    DWORD dwPID[INDEX];							// PID
    TCHAR szName[INDEX][BUFSIZE + 1];			// Image명
    TCHAR szTime[INDEX][BUFSIZE + 1];			// 시작한 시간
    TCHAR szTitle[INDEX][MAX_LENGTH + 1];		// 캡션명
    DWORD dwType[INDEX];						// 감시/경고/차단 타입
    DWORD dwDialog[INDEX];						// 다이어로그 유/무
    DWORD dwIni[INDEX];							// INI 유/무

} *pAppLog;

static AppLog applog;		// Url 리스트 구조체 생성

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
// Input 전역 변수 (서버에서 다운받은 ini파일을 담는 변수들)
//
static TCHAR szGetAppPath[MAX_LENGTH + 1];			// GetAppLog Path       GetAppLog.ini
static TCHAR szAppListPath[MAX_LENGTH + 1];			// Input APP List Path	AppList.ini
static TCHAR szAppLogPath[MAX_LENGTH + 1];			// Output APP List Path	AppList.ini

static char arrAppList[APPCOUNT][BUFSIZE + 1];		// AppList Name ini값을 읽어 List를 배열로 저장
static unsigned int arrAppListType[APPCOUNT + 1];	// AppList Type ini값을 읽어 List를 배열로 저장
static unsigned int nAppListCount;					// AppList 갯수 저장

static int nCountApp;								// App 저장을 위한 카운트 (Temp)

static int nLoggap;									// NeoApp.ini 값을 읽어 저장  ini파일에 write하는 주기
static const char szProgName[] = "tuingApp";		// 중복 실행 방지

// 쓰래드
static DWORD WINAPI APP_CheckThread(void* lpVoid);	// App 쓰래드
DWORD WINAPI  InputThreadProc(void* dummy);		// APP warning 쓰래드
DWORD WINAPI  InputThreadProc1(void* dummy);		// APP Block 쓰래드

// 함수
BOOL CALLBACK EnumWindowProc(HWND hwnd, LPARAM lParam);
BOOL GetFileDateAsString(LPFILETIME pFt, char* pszDate, unsigned cbIn);	// FileDate를 문자열로 변환하는 함수
BOOL GetFileTimeAsString(LPFILETIME pFt, char* pszTime, unsigned cbIn);	// FileTime를 문자열로 변환하는 함수
BOOL CheckApp(char* pszText, int nCount, DWORD dwPid);						// AppList Name과 구조체 이미지 명 Check
void FilterApp();															// 구조체에 목록 중 경고, 차단 설정
void RemoveUnusedApp(DWORD aProcesses[], DWORD cProcesses);
LONG GetRegKey(LPBYTE lbData, int type);									// Regstry 값 가져오기 Value로 추가 가능
BOOL SafeTerminateProcess(char* pszImage, DWORD dwPID);						// Process Kill
BOOL CompareIni(char* szType, int nType);									// INI 파일 비교한다.
BOOL WriteIni(int nCount, char* szType);									// INI 파일 데이터 집어넣기

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
	CreateMutex(NULL, TRUE, szProgName);			// tuingAPP라는 Mutex객체를 생성한다.
	if (GetLastError() == ERROR_ALREADY_EXISTS)		// tuingAPP라는 Mutex Object가 이미 생성되있다면 실행을 시키지 않는다.
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
// 실행중인 작업목록을 Check하는 Thread (APP_CheckThread)
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

	int nWrite = 0;			// 감시 시간 계산

	//Initialize the AppLog structure
	ZeroMemory(&applog, sizeof(applog));		// 구조체 처음 초기화
	nCountApp = 0;							// 구조체에 데이터를 넣기 위한 카운트

	while (1)	// 무한 루프
	{
		// Initialize GetAppLog Configure
		nLoggap = GetPrivateProfileInt("NEOAPP", "LOGGAP", 60000, szGetAppPath);	// ini가 없으면 Default 60000

		// Initialize arrAppList, arrAppListType and Setting
		ZeroMemory(&applog, sizeof(applog));		// 구조체 처음 초기화
		ZeroMemory(&arrAppList, sizeof(arrAppList));
		ZeroMemory(&arrAppListType, sizeof(arrAppListType));
		nAppListCount = GetPrivateProfileInt("APPLIST", "COUNT", 0, szAppListPath);
		if (nAppListCount > 19)		// APPList에 갯수는 19개가 넘지 않도록 설정한다.
		{
			nAppListCount = 19;
		}
		for (unsigned int j = 0; j < nAppListCount; j++)
		{
			CHAR szBuf[BUFSIZE];	// 내용 담을 임시변수
			CHAR szTemp[BUFSIZE];	// KeyNmae 담을 임시변수

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
			// 감시 대상 application 이 없으면 프로세스 검사를 하지 않는다 
			Sleep(TIME_GAP);
			continue;
		}

		//실행중인 모든 프로세스의 목록을 가져온다
		if (!EnumProcesses(aProcesses, sizeof(aProcesses), &cbNeeded))
			continue;

		// 몇개가 리틴되었는지 계산한다. 
		DWORD cProcesses = cbNeeded / sizeof(DWORD);
		for (int i = 0; i < (int)cProcesses; i++)
		{
			//프로세스 아이디를 이용하여 핸들을 구한다
			hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, aProcesses[i]);
			if (!hProcess)
			{
				continue;
			}
			// 프로세스 핸들을 이용하여 이미지 이름을 얻어온다
			if (EnumProcessModules(hProcess, &hMod, sizeof(hMod), &cbNeeded))
			{
				GetModuleBaseName(hProcess, hMod, szBaseName, sizeof(szBaseName));

				//프로세스의 시작시간을 가져온다
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
					wsprintf(szItem, "%s %s", szFileDate, szFileTime);		// szItem 시작시간
				}

				// AppList.ini의 목록과 현재 PID를 필터링 후 구조체를 갱신한다
				if (CheckApp(szBaseName, nCountApp, aProcesses[i]))				// 리스트와 이미지 명이 같으면 구조체에 저장, Type 저장
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

		::EnumWindows(EnumWindowProc, (LPARAM)0);	//윈도 일람 Caption명을 저장

		// 사용하지 않는 app 항목 정리 
		RemoveUnusedApp(aProcesses, cProcesses);

		// 경고 차단 메시지
		FilterApp();

		//프로세스를 강제로 종료시켰기 때문에 
		RemoveUnusedApp(aProcesses, cProcesses);

		// 기본 Loop는 TimeGap, Write되는 시점은 서버에 접속주기 시점을 계산 한 값
		// nLoggap(write gap)/TIME_GAP(loop gap)이 참일 때
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
		::GetWindowThreadProcessId(hwnd, &pid);	// 최상위 PID

		//구조체안에서 pid를 찾아서 데이터를 갱신한다.
		for (int i = 0; i < nCountApp; i++)
		{
			if (applog.dwPID[i] == pid)
			{
				// Office, PDF, HWP 집중 점검
				for (int j = 0; j < APPCLASS; j++)
				{
					memset(szTitle, 0, sizeof(szTitle));
					if (strcmp(lpszClassName, szClass[j]) == 0)
					{
						::SendMessage(hwnd, WM_GETTEXT, (WPARAM)MAX_PATH, (LPARAM)szTitle);
						HWND foHwnd = GetForegroundWindow();
						// 변화 되었을 때 시간과 캡션을 가져온다.
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
						// 처음 빈값일 경우
//						if( strlen(applog.szTitle[i]) == 0 && strlen(szTitle) != 0 )
//						{
//							OutputDebugString(szTitle);
//							strcpy(applog.szTitle[i], szTitle);
//						}
						return true;
					}
					/*					else
										{
											// PID는 같은 경우 집중관리 외 프로그램 모두 여기에 걸림.
											::SendMessage(hwnd, WM_GETTEXT, (WPARAM)MAX_LENGTH, (LPARAM)szTitle);
											// 집중관리 외 프로그램 캡션은 applog에 타이틀이 없고 캡션이 비어있지 않으면 무조건 집어 넣음
											// 차후 분류를 하여 타이틀을 가져오도록 수정
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
		if (strcmp(arrAppList[i], pszText) == 0)					// 감시대상 리스트에 있으면 
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
				//이미 구조체에 목록이 있으면 
				return false;
			}
		}
		OutputDebugString(pszText);
		//없으면 등록
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
	int nCount = nCountApp; // total applog 아이템 수를 복사  

	for (int i = 0; i < nCount; i++)
	{
		BOOL bExist = FALSE;

		for (int k = 0; k < (int)cProcesses; k++)
		{
			if (applog.dwPID[i] == aProcesses[k])
			{
				bExist = TRUE; // 프로세스 리스트에 app 아이템이 있다
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

	// applog 아이템의 갯수를 출인다 
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
			if (applog.dwIni[i] == 0)		// INI에 썼냐 안썼냐 체크
			{
				//OutputDebugString("경고쓰기");
				WriteIni(i, "APPLOG");
				applog.dwIni[i] = 1;
			}

			if (applog.dwDialog[i] == 0)		// 다이어로그가 떠있냐 안떠있냐 체크
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
					// 다이얼로그 쓰레드가 종료되는 것을 기다리지 않는다 	
					//WaitForSingleObject(hThread, INFINITE);
					//CloseHandle(hThread);
				}
			}
		}
		else if (applog.dwType[i] == 2)							// Terminate Dialog
		{
			SafeTerminateProcess(applog.szName[i], applog.dwPID[i]);	// Process Kill
			if (applog.dwIni[i] == 0)			// INI에 썼냐 안썼냐 체크
			{
				//OutputDebugString("차단쓰기");
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
					// 다이얼로그 쓰레드가 종료되는 것을 기다리지 않는다 	
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
// 실행중인 프로세스 Kill
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
				// APP 구조체에서 ini에 안쓴것만 그리고 감시인것만 
				//OutputDebugString("감시쓰기");
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
