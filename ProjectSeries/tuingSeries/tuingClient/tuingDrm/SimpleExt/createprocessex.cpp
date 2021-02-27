/*  CreateProcessEx.cpp  */

// Written by Boherna ( www.gemiscorp.com )
// Copyright (C) 2004. All rights reserved.
//
// You are allowed to include the source code in any product (commercial, shareware, freeware, etc)
// You are allowed to modify the source code in any way you want except you cannot modify the original 
// copyright details at the top of each module.
//
// This file is provided "as is" with no expressed or implied warranty.
// The author accepts no liability if it causes any damage whatsoever.

#include "stdafx.h"
#include "CreateProcessEx.h"

DWORD CreateProcessEx ( LPCSTR lpAppPath, LPCSTR lpCmdLine, BOOL bAppInCmdLine, BOOL bCompletePath,
					    BOOL bWaitForProcess, BOOL bMinimizeOnWait, HWND hMainWnd ) {

	STARTUPINFO startupInfo;
	PROCESS_INFORMATION	processInformation;	

	char szAppPath	[ _MAX_PATH  ];
	char szCmdLine	[ _MAX_PATH  ];
//	char drive	    [ _MAX_DRIVE ];
//	char dir	    [ _MAX_DIR   ];
//	char name       [ _MAX_FNAME ];
//	char ext        [ _MAX_EXT ];

	szAppPath[ 0 ] = '\0';
	szCmdLine[ 0 ] = '\0';
    
    ZeroMemory( &startupInfo, sizeof( STARTUPINFO ));

    startupInfo.cb = sizeof( STARTUPINFO );

	ZeroMemory( &processInformation, sizeof( PROCESS_INFORMATION ));

	if ( bCompletePath ) {

//		GetModuleFileName( GetModuleHandle( NULL ), szAppPath, _MAX_PATH );
//
//		_splitpath( szAppPath, drive, dir, NULL, NULL );
//        _splitpath( lpAppPath, NULL, NULL, name, ext );
//
//        _makepath ( szAppPath, drive, dir, name, strlen( ext ) ? ext : ".exe" );
	}
	else lstrcpy( szAppPath, lpAppPath );


	if ( bAppInCmdLine ) {

		lstrcpy( szCmdLine, "\"" );
		lstrcat( szCmdLine, szAppPath );
		lstrcat( szCmdLine, "\"" );
	}

	if ( lpCmdLine ) {

		lstrcat( szCmdLine, " " );
		lstrcat( szCmdLine, lpCmdLine );

	}

	DWORD dwExitCode = -1;
	       
    if ( CreateProcess(	bAppInCmdLine ? NULL: szAppPath,	// lpszImageName
						szCmdLine,							// lpszCommandLine
						0,									// lpsaProcess
						0,									// lpsaThread
						FALSE,								// fInheritHandles
						DETACHED_PROCESS,					// fdwCreate
						0,									// lpvEnvironment
						0,									// lpszCurDir
						&startupInfo,						// lpsiStartupInfo
						&processInformation					// lppiProcInfo
					)) {

		if ( bWaitForProcess ) {

			if ( bMinimizeOnWait )
					
				if ( IsWindow( hMainWnd )) ShowWindow( hMainWnd, SW_MINIMIZE );
				#ifdef __AFX_H__
				else AfxGetMainWnd()->ShowWindow( SW_MINIMIZE );
				#endif

			WaitForSingleObject( processInformation.hProcess, INFINITE );

			if ( bMinimizeOnWait )

				if ( IsWindow( hMainWnd )) ShowWindow( hMainWnd, SW_RESTORE );
				#ifdef __AFX_H__
				else AfxGetMainWnd()->ShowWindow( SW_RESTORE );
				#endif

			GetExitCodeProcess( processInformation.hProcess, &dwExitCode );
		}
		else {

			CloseHandle( processInformation.hThread );
			CloseHandle( processInformation.hProcess );

			dwExitCode = 0;
		}
	}

	return dwExitCode;
}
