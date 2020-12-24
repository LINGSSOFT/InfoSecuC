/*  CreateProcessEx.h  */

// Written by Boherna ( www.gemiscorp.com )
// Copyright (C) 2004. All rights reserved.
//
// You are allowed to include the source code in any product (commercial, shareware, freeware, etc)
// You are allowed to modify the source code in any way you want except you cannot modify the original 
// copyright details at the top of each module.
//
// This file is provided "as is" with no expressed or implied warranty.
// The author accepts no liability if it causes any damage whatsoever.

#ifndef __CREATEPROCESSEX_H
#define __CREATEPROCESSEX_H

/*
	The CreateProcessEx function creates a new process and its primary thread. 
	The new process runs the specified executable file in the security context of the calling process.

  Parameters:

	lpAppName -> Pointer to a null-terminated string that specifies the application to execute. 
				 The string can specify the full path and file name of the application to execute or it can 
				 specify a partial name.

				 If lpAppName is NULL the application name must be the first white space-delimited token
				 in the lpCmdLine string.

	lpCmdLine -> Pointer to a null-terminated string that specifies the command line to execute. 
				 This parameter can be NULL.

	bAppInCmdLine -> Indicates if the lpAppName string must be used as the first white space-delimited token
					 in the lpCmdLine string.

	bCompletePath -> Indicates if the lpAppName string specifies a partial name that must be completed using
					 the path of the calling process.
					 
	bWaitForProcess -> Indicates if the calling process must wait until child process exits.

	bMinimizeOnWait -> Indicates if the main window will be minimized while the child process runs and 
					   automatically restored when it exits. 

	hMainWnd -> Handle of the main window to be minimized. If hMainWnd is NULL, then AfxGetMainWnd() is used.
*/

DWORD CreateProcessEx ( LPCSTR lpAppName, LPCSTR lpCmdLine, BOOL bAppInCmdLine = true, BOOL bCompletePath = false,
					    BOOL bWaitForProcess = false, BOOL bMinimizeOnWait = false, HWND hMainWnd = NULL );

#endif // __CREATEPROCESSEX_H