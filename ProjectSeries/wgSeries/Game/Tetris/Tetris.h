#pragma once

#include "winbase.h"
#include "resource.h"
#include "Stage.h"

//#define		NUM_ICON_FOR_ANIMATION	1
#define		MAX_LOADSTRING			100
#define		STAGE_WIDTH_SINGLE		468		// Single	468 * 500
#define		STAGE_HEIGHT_SINGLE		500	
#define		STAGE_WIDTH_MULTI		728		// Multi	728 * 500
#define		STAGE_HEIGHT_MULTI		500
#define		ADSENCE_WIDTH_SINGLE	468		// Google Ads 468 * 60
#define		ADSENSE_HEIGHT_SINGLE	60
#define		ADSENCE_WIDTH_MULTI		728		// Google Ads 728 * 90
#define		ADSENSE_HEIGHT_MULTI	90
#define		MYMSG_NOTIFYICON		(WM_APP + 100)

// Global Variables:
HINSTANCE		hInst;								// current instance
TCHAR			szTitle[MAX_LOADSTRING];			// The title bar text
TCHAR			szWindowClass[MAX_LOADSTRING];		// the main window class name
HWND			hMainWnd = NULL;					// the main window handle name
TCHAR			szProgName[MAX_PATH];				// Mutex Name
HICON			hIcon;								// TrayIcon
NOTIFYICONDATA	IconData;							// Notify Icon

// Foward declarations of functions included in this code module:
ATOM				MyRegisterClass(HINSTANCE hInstance);
BOOL				InitInstance(HINSTANCE, int);
LRESULT CALLBACK	WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK	About(HWND, UINT, WPARAM, LPARAM);
LONG				OnTrayNotification(HINSTANCE hInstance, HWND hWnd, WPARAM wParam, LPARAM lParam);
Stage				stage;
