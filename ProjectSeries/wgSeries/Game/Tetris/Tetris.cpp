// Tetris.cpp : Defines the entry point for the application.
//
#include "stdafx.h"
#include "Tetris.h"

int APIENTRY _tWinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPTSTR    lpCmdLine,
                     int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

 	// TODO: Place code here.
	_tcscat_s(szProgName, _T("Tetris"));
	CreateMutex(NULL, TRUE, szProgName);
	if( GetLastError() == ERROR_ALREADY_EXISTS)
		return 0;

	MSG msg;
	HACCEL hAccelTable;

	// Initialize global strings
	LoadString(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadString(hInstance, IDC_TETRIS, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// Perform application initialization:
	if (!InitInstance (hInstance, nCmdShow))
	{
		return FALSE;
	}

	if(bAdsence)	// 처음 유료 사용자를 위한 부분(사용 안할듯하나 일단 놔둠
		rtWindow = {0, 0, STAGE_WIDTH_SINGLE, STAGE_HEIGHT_SINGLE + STAGE_SFIGHT_AREA + ADSENSE_HEIGHT_SINGLE};
	else
		rtWindow = { 0, 0, STAGE_WIDTH_SINGLE, STAGE_HEIGHT_SINGLE };

	AdjustWindowRectEx(&rtWindow, GetWindowLong(hMainWnd, GWL_STYLE), TRUE, GetWindowLong(hMainWnd, GWL_EXSTYLE));
	SetWindowPos(hMainWnd, NULL, 0, 0, rtWindow.right - rtWindow.left, rtWindow.bottom - rtWindow.top, SWP_NOZORDER | SWP_NOMOVE);

	hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_TETRIS));

	TCHAR szSkinFile[MAX_PATH];
	GetModuleFileName(GetModuleHandle(NULL), szSkinFile, MAX_PATH);
	_tcsrchr(szSkinFile, '\\')[0] = 0;
	_tcsrchr(szSkinFile, '\\')[0] = 0;
	_tcscat_s(szSkinFile, _T("\\DoTetris\\skin\\StageSingle.xml"));

	CSkinFileReader reader;
	if(!reader.LoadSkin(szSkinFile, _T("default"), hMainWnd))
	{
		DestroyWindow(hMainWnd);
		return 1;
	}

	stage.StartGame(reader.GetSkinWindow());

//	hAccelTable = LoadAccelerators(hInstance, (LPCTSTR)IDC_NETTETRISSER);

	// Main message loop:
	while (GetMessage(&msg, NULL, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int) msg.wParam;
}

//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
//  COMMENTS:
//
//    This function and its usage are only necessary if you want this code
//    to be compatible with Win32 systems prior to the 'RegisterClassEx'
//    function that was added to Windows 95. It is important to call this function
//    so that the application will get 'well formed' small icons associated
//    with it.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEX wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style			= CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc	= (WNDPROC)WndProc;
	wcex.cbClsExtra		= 0;
	wcex.cbWndExtra		= 0;
	wcex.hInstance		= hInstance;
	wcex.hIcon			= LoadIcon(hInstance, MAKEINTRESOURCE(IDI_TETRIS));
	wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
	wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW+1);
//	wcex.lpszMenuName	= MAKEINTRESOURCE(IDC_TETRIS);
	wcex.lpszClassName	= szWindowClass;
	wcex.hIconSm		= LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassEx(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
//	HWND hWnd;

	hInst = hInstance; // Store instance handle in our global variable

	hMainWnd = CreateWindowEx(WS_EX_DLGMODALFRAME, szWindowClass, szTitle,
				WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU ,
				CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, NULL, NULL, hInstance, NULL);

	if (!hMainWnd)
	{
		return FALSE;
	}

	ShowWindow(hMainWnd, nCmdShow);
	UpdateWindow(hMainWnd);

	// Add icon on system tray during initialzation
	hIcon = LoadIcon(hInstance, (LPCTSTR) MAKEINTRESOURCE(IDI_TETRIS/*IconResourceArray[nIndexOfIcon]*/));

	IconData.cbSize = sizeof(NOTIFYICONDATA);
	IconData.hIcon  = hIcon;
	IconData.hWnd   = hMainWnd;
	_stprintf_s(IconData.szTip, _T("Tetris"));

	IconData.uCallbackMessage = MYMSG_NOTIFYICON;
	IconData.uFlags = NIF_MESSAGE | NIF_ICON | NIF_TIP;
	
	Shell_NotifyIcon(NIM_ADD, &IconData);
	SendMessage(hMainWnd, WM_SETICON, NULL, (long) hIcon);

	return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE:  Processes messages for the main window.
//
//  WM_COMMAND	- process the application menu
//  WM_PAINT	- Paint the main window
//  WM_DESTROY	- post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	stage.RelayMessage(hWnd, message, wParam, lParam);

	int wmId, wmEvent;
	PAINTSTRUCT ps;
	HDC hdc;

	switch (message)
	{
		case WM_COMMAND:
			wmId    = LOWORD(wParam);
			wmEvent = HIWORD(wParam);
			// Parse the menu selections:
			switch (wmId)
			{
				case IDM_ABOUT:
					DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
					break;
				case IDM_EXIT:
					DestroyWindow(hWnd);
					break;
				case ID_TRAYICON_SHOWWINDOW:
					ShowWindow(hWnd, SW_SHOW);
					break;
				default:
					return DefWindowProc(hWnd, message, wParam, lParam);
			}
			break;
		case WM_PAINT:
			hdc = BeginPaint(hWnd, &ps);
			// TODO: Add any drawing code here...
			EndPaint(hWnd, &ps);
			break;
		case MYMSG_NOTIFYICON:
			// System Tray Icon notification will be executed..
			OnTrayNotification(hInst, hWnd, wParam, lParam);
			break;
		case WM_CLOSE:
			stage.FrameClose();
			ShowWindow(hWnd, SW_HIDE);
			break;
		case WM_QUIT:
		case WM_DESTROY:
			KillTimer(hWnd, 1);
			Shell_NotifyIcon(NIM_DELETE, &IconData);
			DestroyIcon(hIcon);
			PostQuitMessage(0);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

/*	Function Name   : OnTrayNotification
	Description		: CALLBACK routine which will be get executed when a notification is identified on the
					  system tray. 
	Function Called	: LoadMenu	-	Load the menu into the application
*/
LONG OnTrayNotification(HINSTANCE hInstance, HWND hWnd, WPARAM wParam, LPARAM lParam)
{
	HMENU hMenu;

	switch(lParam)
	{
		case WM_RBUTTONDOWN:
			/** Load and verify the menu**/
			if(hMenu = LoadMenu(hInstance, (LPCTSTR) MAKEINTRESOURCE(IDC_TETRIS)))
			{
				hMenu = GetSubMenu(hMenu, 0);
				if(hMenu != NULL)
				{
					POINT pt;
					GetCursorPos(&pt);
					SetForegroundWindow(hWnd);
					TrackPopupMenu(hMenu, TPM_LEFTALIGN | TPM_RIGHTBUTTON, pt.x, pt.y, 0, hWnd, NULL);
				}
			}
			break ;
		case WM_LBUTTONDBLCLK:
			ShowWindow(hWnd, SW_SHOW);
			break;
		default:
			return FALSE;
	}

	return 0;
}

// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}
