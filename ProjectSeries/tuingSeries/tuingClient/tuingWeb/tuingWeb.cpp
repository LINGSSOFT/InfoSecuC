#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
// tuingWeb.cpp : Defines the entry point for the application.
//

#include "framework.h"
#include "tuingWeb.h"
#include "psapi.h"  /// PSAPI header for computer name and login user name
#include "Tlhelp32.h"

#pragma comment(lib,"psapi.lib")

#define MAX_LOADSTRING  100
#define BUFSIZE	        32
#define INDEX	        100		// maximum items
#define TIME_GAP        9000	// 9 seconds
#define MAX_LENGTH      250
#define DMAX_LEN        512

///////////////////////////////////////////////////////////////////////
//
// Output 구조체(최신 정보 갱신)
//
typedef struct  UrlLog							// URL Log를 담는 구조체
{
    DWORD dwPID[INDEX];
    TCHAR szUrl[INDEX][MAX_LENGTH + 1];
    TCHAR szTime[INDEX][BUFSIZE + 1];
    TCHAR szTitle[INDEX][MAX_LENGTH + 1];
    DWORD dwType[INDEX];
    DWORD dwDialog[INDEX];
    DWORD dwIni[INDEX];
} *PUrlLog;
static UrlLog urllog;		// Url 리스트 구조체 생성

////////////////////////////////////////////////////////////////////////
//
// Input 전역 변수 (서버에서 다운받은 ini파일을 담는 변수들)
//
static TCHAR szInstallPath[MAX_LENGTH + 1];			// 인스톨 Path
static TCHAR szGetAppPath[MAX_LENGTH + 1];			// GetAppLog Path       GetAppLog.ini
static TCHAR szUrlListPath[MAX_LENGTH + 1];			// Input Url List Path	UrlList.ini
static TCHAR szUrlLogPath[MAX_LENGTH + 1];			// Output Url List Path	UrlList.ini

static char arrUrlList[MAX_LENGTH][BUFSIZE + 1];	// UrlList Name ini값을 읽어 List를 배열로 저장
static unsigned int arrUrlListType[MAX_LENGTH + 1];	// UrlList Type ini값을 읽어 List를 배열로 저장
static unsigned int nUrlListCount;					// UrlList 갯수 저장

static int nCountUrl;								// Url 저장을 위한 카운트 (Temp)

//static int nLogging;								// GetAppLog.ini 값을 읽어 저장 0 no log, 1 app write log, 2 url write log, 3 app,url write log
static int nLoggap;									// GetAppLog.ini 값을 읽어 저장  ini파일에 write하는 주기
static const char szProgName[] = "tuingWeb";		// 중복 실행 방지

// 쓰래드
static DWORD WINAPI URL_CheckThread(void* lpVoid);	// Url 쓰래드
DWORD WINAPI  UrlInputThreadProc(void* dummy);		// Url warning 쓰래드
DWORD WINAPI  UrlInputThreadProc1(void* dummy);		// Url Block 쓰래드

// 함수
void PrintChildrenWindows(HWND hwnd);
BOOL CheckUrl(DWORD dwPid, char*);					// 구조체에 URL  중복 저장 방지
void FilterUrl();	//경고 차단
void RemoveUnusedIE();
LONG GetRegKey(LPBYTE lbData, int type);									// Regstry 값 가져오기 Value로 추가 가능
BOOL SafeTerminateProcess(char* pszImage, DWORD dwPID);						// Process Kill
BOOL CompareIni(char* szType, int nType);									// INI 파일 비교한다.
BOOL WriteIni(int nCount, char* szType);									// INI 파일 데이터 집어넣기
BOOL isCompleteDomain(char* str);
BOOL RemoveW3(char* str);

// Global Variables:
HINSTANCE hInst;                                // current instance
WCHAR szTitle[MAX_LOADSTRING];                  // The title bar text
WCHAR szWindowClass[MAX_LOADSTRING];            // the main window class name

// Forward declarations of functions included in this code module:
ATOM                MyRegisterClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
	// TODO: Place code here.
	CreateMutex(NULL, TRUE, szProgName);			// tuingWeb라는 Mutex객체를 생성한다.
	if (GetLastError() == ERROR_ALREADY_EXISTS)		// tuingWeb라는 Mutex Object가 이미 생성되있다면 실행을 시키지 않는다.
	{
		return 0;
	}

	GetTempPath(MAX_LENGTH + 1, szInstallPath);
	GetTempPath(MAX_LENGTH + 1, szGetAppPath);
	GetTempPath(MAX_LENGTH + 1, szUrlListPath);
	GetTempPath(MAX_LENGTH + 1, szUrlLogPath);

	strcat(szGetAppPath, "NeoWeb.ini");
	strcat(szUrlListPath, "UrlList.ini");
	strcat(szUrlLogPath, "Url_Log.ini");

	OutputDebugString("Hello");


	HANDLE hThread2;
	DWORD dwThreadID2;

	hThread2 = CreateThread(NULL, 0, URL_CheckThread, NULL, 0, &dwThreadID2);
	if (!hThread2)
	{
		OutputDebugString("fails to create the thread2!");
		return 0;
	}

	WaitForSingleObject(hThread2, INFINITE);
	CloseHandle(hThread2);

	return 0;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//
DWORD m_dwProcessID = 0;
DWORD GetProcessID(char* image)
{
	HANDLE hSnapshot;
	PROCESSENTRY32 pe32;
	ZeroMemory(&pe32, sizeof(pe32));
	DWORD temp = 0;

	hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, NULL);
	pe32.dwSize = sizeof(PROCESSENTRY32);
	if (Process32First(hSnapshot, &pe32))
	{
		do
		{
			if (strcmpi(pe32.szExeFile, image) == 0)
			{
				temp = pe32.th32ProcessID;
				m_dwProcessID = pe32.th32ProcessID;
				PrintChildrenWindows(NULL);
			}
		} while (Process32Next(hSnapshot, &pe32));
	}
	CloseHandle(hSnapshot);
	return temp;
}

DWORD CheckProcessID(DWORD dwProcessid)
{
	HANDLE hSnapshot;
	PROCESSENTRY32 pe32;
	ZeroMemory(&pe32, sizeof(pe32));
	DWORD temp = 0;

	hSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, NULL);
	pe32.dwSize = sizeof(PROCESSENTRY32);
	if (Process32First(hSnapshot, &pe32))
	{
		do
		{
			if (pe32.th32ProcessID == dwProcessid)
			{
				temp = pe32.th32ProcessID;
				break;
			}
		} while (Process32Next(hSnapshot, &pe32));
	}
	CloseHandle(hSnapshot);
	return temp;
}

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// 실행중인 브라우져 URL을 Check하는 Thread (URL_CheckThread)
//
DWORD WINAPI URL_CheckThread(void* lpVoid)
{
	int nWrite = 0;			// 감시 시간 계산

	//Initialize the AppLog structure
	ZeroMemory(&urllog, sizeof(urllog));		// 구조체 처음 초기화
	nCountUrl = 0;							// 구조체에 데이터를 넣기 위한 카운트


	while (1)	// 무한루프
	{
		// Initialize GetAppLog Configure
//		nLogging = GetPrivateProfileInt("GETAPPLOG", "LOGGING", 3, szGetAppPath);	// ini가 없으면 Default 3
		nLoggap = GetPrivateProfileInt("NEOWEB", "LOGGAP", 60000, szGetAppPath);	// ini가 없으면 Default 60000

		// Initialize arrUrlList, arrUrlListType and Setting
		ZeroMemory(&arrUrlList, sizeof(arrUrlList));
		ZeroMemory(&arrUrlListType, sizeof(arrUrlListType));
		nUrlListCount = GetPrivateProfileInt("URLLIST", "COUNT", 0, szUrlListPath);

		for (unsigned int j = 0; j < nUrlListCount; j++)
		{
			CHAR szBuf[BUFSIZE];
			CHAR szTemp[BUFSIZE];

			memset(szBuf, NULL, sizeof(szBuf));
			memset(szTemp, NULL, sizeof(szTemp));

			wsprintf(szTemp, "URLNAME%d", j + 1);
			GetPrivateProfileString("URLLIST", szTemp, "", szBuf, sizeof(szBuf), szUrlListPath);
			strcpy(arrUrlList[j], szBuf);

			memset(szTemp, NULL, sizeof(szTemp));
			wsprintf(szTemp, "URLTYPE%d", j + 1);

			arrUrlListType[j] = GetPrivateProfileInt("URLLIST", szTemp, 0, szUrlListPath);

		}
		GetProcessID("iexplore.exe");

		// 감시가 ini에 Write되는 시점 = 감시 설정이 되고 nLoggap(write gap)/TIME_GAP(loop gap)이 참일 때
		// 기본 Loop는 TimeGap Write되는 시점은 서버에 접속주기 시점을 계산 한 값
		if (/*(nLogging == 2 || nLogging == 3) &&*/ (nLoggap / TIME_GAP) < nWrite)
		{
			CompareIni("URLLOG", 0);
			nWrite = 0;
		}
		else
			nWrite++;

		// 경고 , 차단
//		if(nLogging == 2 || nLogging == 3)
//		{
		FilterUrl();
		//		}

		RemoveUnusedIE();

		Sleep(TIME_GAP);
	}
	ExitThread(0);
}

void PrintChildrenWindows(HWND hwnd)
{
	SYSTEMTIME st;
	TCHAR szTitle[MAX_LENGTH];			// URL를 담을 변수
	TCHAR szUrl[MAX_LENGTH];			// Caption을 담을 변수
	CHAR buf[MAX_LENGTH];
	char lpszClassName[MAX_PATH];
	DWORD dwPid = 0;

	hwnd = ::GetTopWindow(hwnd);

	if (!hwnd)
		return;

	while (hwnd)
	{

		ZeroMemory(lpszClassName, MAX_PATH);
		::GetClassName(hwnd, lpszClassName, MAX_PATH);
		GetWindowThreadProcessId(hwnd, &dwPid);

		if (m_dwProcessID == dwPid && strcmp(lpszClassName, "IEFrame") == 0)
		{
			::SendMessage(hwnd, WM_GETTEXT, (WPARAM)MAX_LENGTH, (LPARAM)szTitle);
			strcpy(urllog.szTitle[nCountUrl], szTitle);

		}
		if (m_dwProcessID == dwPid && strcmp(lpszClassName, "Edit") == 0)
		{
			::SendMessage(hwnd, WM_GETTEXT, (WPARAM)MAX_LENGTH, (LPARAM)szUrl);
			if (!isCompleteDomain(szUrl))
				break;

			if (!CheckUrl(m_dwProcessID, szUrl))
			{
				// 문자열 길이 검사할 것 
				if (lstrlen(szUrl) > MAX_LENGTH) {
					//					lstrcpyn(urllog.szUrl[nCountUrl], szUrl,MAX_LENGTH-1);
					strncpy(urllog.szUrl[nCountUrl], szUrl, MAX_LENGTH);
				}
				else
					lstrcpy(urllog.szUrl[nCountUrl], szUrl);


				GetLocalTime(&st);
				wsprintf(buf, "%d-%d-%d %d:%d:%d", st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond);
				strcpy(urllog.szTime[nCountUrl], buf);

				urllog.dwPID[nCountUrl] = m_dwProcessID;
				urllog.dwDialog[nCountUrl] = 0;
				urllog.dwIni[nCountUrl] = 0;
				urllog.dwType[nCountUrl] = 0;

				// 경고, 차단 구분 있는 URL의 경우에는 해당 값을 넣는다 
				_strlwr(szUrl);
				for (unsigned int i = 0; i < nUrlListCount; i++)
				{
					ZeroMemory(buf, sizeof(buf));
					lstrcpy(buf, arrUrlList[i]);

					RemoveW3(buf);

					if (strstr(szUrl, buf/*arrUrlList[i]*/) != NULL)
					{
						urllog.dwType[nCountUrl] = arrUrlListType[i];
					}
				}

				nCountUrl++;
				if (nCountUrl >= 100) {

					for (int jj = 0; jj < nCountUrl; jj++) {

						urllog.dwPID[jj] = urllog.dwPID[jj + 1];
						strcpy(urllog.szUrl[jj], urllog.szUrl[jj + 1]);
						strcpy(urllog.szTitle[jj], urllog.szTitle[jj + 1]);
						strcpy(urllog.szTime[jj], urllog.szTime[jj + 1]);
						urllog.dwType[jj] = urllog.dwType[jj + 1];
						urllog.dwIni[jj] = urllog.dwIni[jj + 1];
						urllog.dwDialog[jj] = urllog.dwDialog[jj + 1];
					}
					nCountUrl = 99;
				}

				OutputDebugString(szUrl);
			}
		}
		PrintChildrenWindows(hwnd);
		hwnd = ::GetNextWindow(hwnd, GW_HWNDNEXT);
	} // end of while loop
}

// if the domain is http://aaa.bbb.bbb/, return true 
BOOL isCompleteDomain(char* str)
{
	char* header = "http://";
	char szbuf[DMAX_LEN];
	char* pdest;
	int len = 0;
	memset(szbuf, 0x00, sizeof(szbuf));

	//	lstrcpy(szbuf,str);
	lstrcpyn(szbuf, str, MAX_LENGTH);

	if (strncmp(szbuf, header, lstrlen(header)) == 0)
	{
		pdest = strstr(szbuf, header);
		len = lstrlen(header);
		pdest = strchr(pdest + len, '/');
		if (pdest)
		{
			return true;
		}
	}
	return false;
}

// if the domain is http://aaa.bbb.bbb/, return true 
BOOL ExtractDomain(char* str)
{
	char* header = "http://";
	char szbuf[MAX_PATH];
	char sztmp[MAX_PATH];
	char* pdest;
	memset(szbuf, 0x00, sizeof(szbuf));
	memset(sztmp, 0x00, sizeof(sztmp));

	//	lstrcpy(szbuf,str);
	lstrcpyn(szbuf, str, MAX_LENGTH);

	pdest = strstr(szbuf, header);

	if (pdest)
	{
		strcpy(sztmp, pdest + 7);

		pdest = strchr(sztmp, '/');
		if (pdest)
		{
			int len = pdest - sztmp;
			lstrcpyn(szbuf, sztmp, len + 1);
			return true;
		}
	}
	return false;
}

// remove w3 from www.aaa.com in INI 
BOOL RemoveW3(char* str)
{
	char* header = "www.";
	char szbuf[MAX_PATH];
	char* pdest;
	memset(szbuf, 0x00, sizeof(szbuf));

	//	lstrcpy(szbuf,str);
	lstrcpyn(szbuf, str, MAX_LENGTH);

	pdest = strstr(szbuf, header);
	if (pdest)
	{
		lstrcpy(str, pdest + 4);
		return true;
	}
	return false;
}

void RemoveUnusedIE()
{
	//	char buf[DMAX_LEN];
	int nRemove = 0;
	int nCount = nCountUrl; // total urllog 아이템 수를 복사  
	BOOL bExist = FALSE;
	for (int i = 0; i < nCount; i++)
	{
		if (CheckProcessID(urllog.dwPID[i]))
		{
			bExist = TRUE; // the process is running 
		}

		// if is not running , remove an item in urllog list
		if (!bExist)
		{
			nRemove++;
			if (i < nCount - 1)
			{
				urllog.dwPID[i] = urllog.dwPID[i + 1];
				strcpy(urllog.szUrl[i], urllog.szUrl[i + 1]);
				strcpy(urllog.szTitle[i], urllog.szTitle[i + 1]);
				strcpy(urllog.szTime[i], urllog.szTime[i + 1]);
				urllog.dwType[i] = urllog.dwType[i + 1];
				urllog.dwIni[i] = urllog.dwIni[i + 1];
				urllog.dwDialog[i] = urllog.dwDialog[i + 1];

				//wsprintf(buf,"Remove urllog[%d][%s]", i, urllog.szUrl[i]);
				//OutputDebugString(buf);
			}
		}
	}

	// urllog 아이템의 갯수를 줄인다 
	int nVal = nCountUrl - nRemove;
	if (nVal > 0)
		nCountUrl -= nRemove;
	else
		nCountUrl = 0;
}

BOOL CheckUrl(DWORD dwpid, char* szUrl)
{
	char buf[MAX_PATH];
	int nCount = nCountUrl;
	memset(buf, 0x00, sizeof(buf));
	//	lstrcpy(buf, szUrl);
	lstrcpyn(buf, szUrl, MAX_LENGTH);

	for (int j = 0; j < nCount; j++)
	{
		ExtractDomain(buf);
		if (urllog.dwPID[j] == dwpid && strstr(urllog.szUrl[j], buf) != NULL)	// 구조체에 PID목록이 같은게 있다면 저장 안함 szUrl
		{
			return true;
		}
	}
	return false;
}

// urllog 구조체를 검사하여 경고, 차단 메시지를 표시한다
void FilterUrl()
{
	for (int i = 0; i < nCountUrl; i++)
	{
		if (urllog.dwType[i] == 1)			// warning Dialog
		{
			if (urllog.dwIni[i] == 0)		// INI에 썼냐 안썼냐 체크
			{
				WriteIni(i, "URLLOG");
				urllog.dwIni[i] = 1;
			}

			// 중복 메시지 창 방지 
			char szLoadTemp[MAX_LENGTH];
			memset(szLoadTemp, NULL, sizeof(szLoadTemp));
			LoadString(NULL, IDS_STRING3, szLoadTemp, sizeof(szLoadTemp));

			if (FindWindow(NULL, szLoadTemp)) //"사용 제한 안내" 
			{
				OutputDebugString("the previous warning window is on the screen.");
				urllog.dwDialog[i] = 1;
			}
			else if (urllog.dwDialog[i] == 0)		// 다이어로그가 떠있냐 안떠있냐 체크
			{
				HANDLE hThread;
				DWORD dwThreadID;

				// create a new thread to allow user input
				hThread = CreateThread(NULL, 0, UrlInputThreadProc, (LPVOID)i, 0, &dwThreadID);
				if (!hThread)
				{
					OutputDebugString("Failed to create dialog thread");
				}
				else
				{
					urllog.dwDialog[i] = 1;
					// 다이얼로그 쓰레드가 종료되는 것을 기다리지 않는다 	
					//WaitForSingleObject(hThread, INFINITE);
					//CloseHandle(hThread);
				}
			}
		}
		else if (urllog.dwType[i] == 2)							// Terminate Dialog
		{
			SafeTerminateProcess("iexplore.exe", urllog.dwPID[i]);	// Process Kill
			if (urllog.dwIni[i] == 0)			// INI에 썼냐 안썼냐 체크
			{
				WriteIni(i, "URLLOG");
				urllog.dwIni[i] = 1;
			}
			char szLoadTemp[MAX_LENGTH];
			memset(szLoadTemp, NULL, sizeof(szLoadTemp));
			LoadString(NULL, IDS_STRING4, szLoadTemp, sizeof(szLoadTemp));

			// 중복 메시지 창 방지 
			if (FindWindow(NULL, szLoadTemp))  //"사용 차단 안내" 
			{
				OutputDebugString("the previous warning window is on the screen.");
				urllog.dwDialog[i] = 1;
			}
			else if (urllog.dwDialog[i] == 0)
			{
				HANDLE hThread;		// Terminate Thread
				DWORD dwThreadID;	// Terminate ThreadID

				// create a new thread to allow user input
				hThread = CreateThread(NULL, 0, UrlInputThreadProc1, (LPVOID)i, 0, &dwThreadID);
				if (!hThread)
				{
					OutputDebugString("Failed to create dialog thread");
				}
				else
				{
					urllog.dwDialog[i] = 1;
					// 다이얼로그 쓰레드가 종료되는 것을 기다리지 않는다 	
					//WaitForSingleObject(hThread, INFINITE);
					//CloseHandle(hThread);
				}
			}
		}
	}// end of for loop
}


// thread for dialog of warning
DWORD WINAPI  UrlInputThreadProc(void* dummy)
{
	// create the dialog window
	HWND hWnd = ::CreateDialog(NULL, MAKEINTRESOURCE(IDD_WARNDIALOG), NULL, NULL);
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


		SetDlgItemText(hWnd, IDC_WARN_STATIC1, urllog.szUrl[(int)dummy]);

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
				urllog.dwDialog[(int)dummy] = 0;
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

// thread for dialog of blocking
DWORD WINAPI  UrlInputThreadProc1(void* dummy)
{
	// create the dialog window
	HWND hWnd = ::CreateDialog(NULL, MAKEINTRESOURCE(IDD_BLOCKDIALOG), NULL, NULL);
	if (hWnd != NULL)
	{
		// show dialog
		::SetWindowPos(hWnd, HWND_TOPMOST, 300, 300, 350, 300, SWP_NOSIZE);

		HICON hIcon = LoadIcon((HINSTANCE)GetModuleHandle(NULL), MAKEINTRESOURCE(IDI_ICON_BLOCK));
		if (hIcon) {

			HWND hwnd2 = GetDlgItem(hWnd, IDC_ICONSTATIC);
			SendMessage(hwnd2, STM_SETICON, (WPARAM)hIcon, 0);
		}
		else
			OutputDebugString("loadicon fails");

		SetDlgItemText(hWnd, IDC_BLOCK_STATIC1, urllog.szUrl[(int)dummy]);

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


// 실행중인 프로세스 Kill
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
	if ((DWORD)hndl == -1) {

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
				if (hHandle) {

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
		return true;
	}
	else if (strcmp(szType, "URLLOG") == 0)
	{
		for (int i = 0; i < nCountUrl; i++)
		{
			if (urllog.dwIni[i] == 0 && urllog.dwType[i] == 0)
			{
				// Url 구조체에서 ini에 안쓴것만 그리고 감시인것만 
				WriteIni(i, szType);
				urllog.dwIni[i] = 1;
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
		return true;
	}
	else if (strcmp(szType, "URLLOG") == 0)
	{
		int nRegCount = GetPrivateProfileInt(szType, "COUNT", 0, szUrlLogPath);
		CHAR szTemp[BUFSIZE];		// ini APP/URL Section
		char buf[10];				// App/Url Type
		memset(buf, NULL, sizeof(buf));

		wsprintf(buf, "%d", nRegCount + 1);
		WritePrivateProfileString("URLLOG", "COUNT", buf, szUrlLogPath);

		memset(szTemp, NULL, sizeof(szTemp));
		wsprintf(szTemp, "URLNAME%d", nRegCount + 1);
		WritePrivateProfileString("URLLOG", szTemp, urllog.szUrl[nCount], szUrlLogPath);

		memset(szTemp, NULL, sizeof(szTemp));
		wsprintf(szTemp, "URLTITLE%d", nRegCount + 1);
		WritePrivateProfileString("URLLOG", szTemp, urllog.szTitle[nCount], szUrlLogPath);

		memset(szTemp, NULL, sizeof(szTemp));
		wsprintf(szTemp, "URLTIME%d", nRegCount + 1);
		WritePrivateProfileString("URLLOG", szTemp, urllog.szTime[nCount], szUrlLogPath);

		memset(szTemp, NULL, sizeof(szTemp));
		wsprintf(szTemp, "URLTYPE%d", nRegCount + 1);

		memset(buf, NULL, sizeof(buf));
		wsprintf(buf, "%d", urllog.dwType[nCount]);
		WritePrivateProfileString("URLLOG", szTemp, buf, szUrlLogPath);
		OutputDebugString("Success Write URLLOG Ini\n");
		return true;
	}
	return false;
}
