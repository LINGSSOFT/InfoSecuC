#pragma once

#include "winbase.h"
#include "resource.h"
#include "SceneMgr.h"
#include "Stage.h"

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
