// SimpleShlExt.cpp : Implementation of CSimpleShlExt

#include "stdafx.h"

#include "resource.h"
#include <shlwapi.h>

#include <Iphlpapi.h>
#pragma comment(lib,"Iphlpapi.lib")

//#include "winsock2.h"
#include "SimpleExt.h"
#include "SimpleShlExt.h"
#include "CreateProcessEx.h"
#include "DefineStructure.h"
/////////////////////////////////////////////////////////////////////////////
#define HEADER_SIZE 22
#define KEY_SIZE	8

CHAR m_szMAC[MAX_PATH];
CHAR m_szIP[MAX_PATH];
#define MALLOC(x) HeapAlloc(GetProcessHeap(), 0, (x)) 
#define FREE(x) HeapFree(GetProcessHeap(), 0, (x))

int	m_nLevel = 0;
int m_nDocLevel = 0;
int m_nSDocLevel = 0;
CHAR m_strLv[MAX_PATH];

void GetMacAddress()
{

	TCHAR szBuf[MAX_PATH];
	// Global variables
	ULONG		ulOutBufLen;
	DWORD		dwRetVal;
	
	/* variables used for GetAdapterInfo */
	IP_ADAPTER_INFO		*pAdapterInfo;
	IP_ADAPTER_INFO		*pAdapter;

	//GetAdaptersInfo
 	pAdapterInfo = (IP_ADAPTER_INFO *) MALLOC( sizeof(IP_ADAPTER_INFO) );
	if (pAdapterInfo == NULL) {
		OutputDebugString("Error allocating memory needed to call GetAdapterInfo");
		return ;
	}
 	ulOutBufLen = sizeof(IP_ADAPTER_INFO);

	 if (GetAdaptersInfo( pAdapterInfo, &ulOutBufLen) == ERROR_BUFFER_OVERFLOW) {
		  FREE (pAdapterInfo);
		  pAdapterInfo = (IP_ADAPTER_INFO *) MALLOC (ulOutBufLen);
			if (pAdapterInfo == NULL) {
				OutputDebugString("Error allocating memory needed to call GetAdapterInfo");
				return  ;
			}
	 }

	 if ((dwRetVal = GetAdaptersInfo( pAdapterInfo, &ulOutBufLen)) != NO_ERROR) {
			FREE (pAdapterInfo);
			OutputDebugString("GetAdaptersInfo call failed"); 
			return  ;
	 }
	 pAdapter = pAdapterInfo;

	 while (pAdapter) {

		//pAdapter->AdapterName);
		//pAdapter->Description);
		
		unsigned char *MACData = pAdapter->Address;
		wsprintf(szBuf,"%02X%02X%02X%02X%02X%02X", 
		MACData[0], MACData[1], MACData[2], MACData[3], MACData[4], MACData[5]);
		lstrcpy(m_szMAC, szBuf);
		lstrcpy(m_szIP, pAdapter->IpAddressList.IpAddress.String);

		//pAdapter->IpAddressList.IpAddress.String);
		//pAdapter->IpAddressList.IpMask.String);
		//pAdapter->GatewayList.IpAddress.String);
		
		break;  // only first adapter

  		pAdapter = pAdapter->Next;
 	}
	
	FREE (pAdapterInfo);
}
// 암호화된 파일 인지 헤더부분 확인
BOOL IsEncryptFile(LPCSTR lpFileName )
{
	DWORD     dwFileSizeLo, dwFileSizeHi;
	DWORDLONG qwSize;
	HANDLE    hFile;

	hFile = CreateFile ( lpFileName, GENERIC_READ, FILE_SHARE_READ, NULL,
					   OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL );

	if ( INVALID_HANDLE_VALUE == hFile )
	{
		OutputDebugString("the encrypt file is invalid!");
		return FALSE;
	}

	dwFileSizeLo = GetFileSize ( hFile, &dwFileSizeHi );
	qwSize = ((DWORDLONG) dwFileSizeHi)<<32 | dwFileSizeLo;
	if(qwSize > 8)
	{
		DWORD dwBytesRead = 0;
		BYTE ReadBuffer[HEADER_SIZE+1] = {0};
		BYTE Header[KEY_SIZE+1] = {0x01, 0xAB, 0x0C, 0xC4, 0x64, 0x2E, 0xB1, 0xCB, 0x00};
		if( ReadFile(hFile, ReadBuffer, HEADER_SIZE, &dwBytesRead, NULL))
		{
			// Criteria
			if(memcmp( ReadBuffer + 14, Header, KEY_SIZE ) == 0)
			{
				OutputDebugString("read ok");
				m_nDocLevel = (int)ReadBuffer[12];
			}
			else
			{
				OutputDebugString("the encrypt file is invalid!");
				CloseHandle ( hFile );
				return FALSE;
			}
		}
	}
	else
	{
		OutputDebugString("the encrypt file is invalid!");
		CloseHandle ( hFile );
		return FALSE;
	}
	CloseHandle ( hFile );
	return TRUE;
}
/////////////////////////////////////////////////////////////////////////////
LONG GetRegKey(HKEY key, LPCTSTR subkey, LPTSTR retdata)
{
    HKEY hkey;
    LONG retval = RegOpenKeyEx(key, subkey, 0, KEY_QUERY_VALUE, &hkey);

    if (retval == ERROR_SUCCESS) {
        long datasize = MAX_PATH;
        TCHAR data[MAX_PATH];
        RegQueryValue(hkey, NULL, data, &datasize);
        lstrcpy(retdata,data);
        RegCloseKey(hkey);
    }

    return retval;
}
void OpenFileType(LPCSTR lpFileName)
{
	
	TCHAR key[MAX_PATH + MAX_PATH];    
	TCHAR extension[MAX_PATH];    
    
	// First try ShellExecute()
    HINSTANCE result = ShellExecute(NULL, _T("open"), lpFileName, NULL,NULL, SW_SHOW);
	char * lpstr;

	lpstr = PathFindExtension(lpFileName);
	lstrcpy(extension, lpstr);

    // If it failed, get the .htm regkey and lookup the program
    if ((UINT)result <= HINSTANCE_ERROR) {        
        
        if (GetRegKey(HKEY_CLASSES_ROOT, extension, key) == ERROR_SUCCESS) {
            lstrcat(key, _T("\\shell\\open\\command"));

            if (GetRegKey(HKEY_CLASSES_ROOT,key,key) == ERROR_SUCCESS) {
                TCHAR *pos;
                pos = StrStr(key, _T("\"%1\""));
                if (pos == NULL) {                     // No quotes found
                    pos = StrStr(key, _T("%1"));       // Check for %1, without quotes
                    if (pos == NULL)                   // No parameter at all...
                        pos = key+lstrlen(key)-1;
                    else
                        *pos = 0x00;//"\0";                   // Remove the parameter
                }
                else
                    *pos = 0x00;//"\0";                       // Remove the parameter

                lstrcat(pos, _T(" "));
                lstrcat(pos, lpFileName);
                result = (HINSTANCE) WinExec(key,SW_SHOW);
				if ((UINT)result > 31) {
					return;
				}
            }
        }
    }
}
// get data from registry
LONG GetRegKey(LPSTR retdata, int type)
{  
	long datasize;
    HKEY hkey;
	DWORD dwtype=REG_SZ;
	TCHAR szData[MAX_PATH];
	LONG retval;

    retval = RegOpenKeyEx(HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\BNASolutions\\NeoSystem"), 0, KEY_QUERY_VALUE, &hkey);

    if (retval == ERROR_SUCCESS) {
			
		ZeroMemory(szData,sizeof(szData)); 

		if( type == 2 )
			retval = RegQueryValueEx(hkey, "ServerPort", 0, (LPDWORD)&dwtype, (LPBYTE)szData, (LPDWORD)&datasize);
		else if( type == 3 )
			retval = RegQueryValueEx(hkey, "WaterMark", 0, (LPDWORD)&dwtype, (LPBYTE)szData, (LPDWORD)&datasize);      
		else if( type == 4 )
			retval = RegQueryValueEx(hkey, "Angle", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
		else if( type == 5 )
			retval = RegQueryValueEx(hkey, "FontName", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
		else if( type == 6 )
			retval = RegQueryValueEx(hkey, "FontSize", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
		else if( type == 7 )
			retval = RegQueryValueEx(hkey, "FontColor", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
		else if( type == 8)
			retval = RegQueryValueEx(hkey, "installpath", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
		else if( type == 10)
			retval = RegQueryValueEx(hkey, "DBUID", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
		else if( type == 11)
			retval = RegQueryValueEx(hkey, "DBPWD", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
		
		if(retval != ERROR_SUCCESS)
		{
			if( type == 2 )
				retval = RegQueryValueEx(hkey, "ServerPort", 0, (LPDWORD)&dwtype, (LPBYTE)szData, (LPDWORD)&datasize);
			else if( type == 3 )
				retval = RegQueryValueEx(hkey, "WaterMark", 0, (LPDWORD)&dwtype, (LPBYTE)szData, (LPDWORD)&datasize);      
			else if( type == 4 )
				retval = RegQueryValueEx(hkey, "Angle", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
			else if( type == 5 )
				retval = RegQueryValueEx(hkey, "FontName", 0, (LPDWORD)&dwtype, (LPBYTE)szData, (LPDWORD)&datasize); 
			else if( type == 6 )
				retval = RegQueryValueEx(hkey, "FontSize", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
			else if( type == 7 )
				retval = RegQueryValueEx(hkey, "FontColor", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
			else if( type == 8)
				retval = RegQueryValueEx(hkey, "installpath", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
			else if( type == 10)
				retval = RegQueryValueEx(hkey, "DBUID", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
			else if( type == 11)
				retval = RegQueryValueEx(hkey, "DBPWD", 0, 0, (LPBYTE)szData, (LPDWORD)&datasize); 
		}
		
		lstrcpy(retdata, (LPCSTR)szData);
		
        RegCloseKey(hkey);
    }

    return datasize;
}
LONG GetRegData(LPBYTE retdata, int type)
{
	long datasize;
	HKEY hkey;
    LONG retval = RegOpenKeyEx(HKEY_LOCAL_MACHINE, TEXT("SOFTWARE\\BNASolutions\\NeoSystem"), 0, KEY_QUERY_VALUE, &hkey);

    if (retval == ERROR_SUCCESS) {
		
		DWORD dwtype=REG_DWORD;

		if( type == 1)
			retval = RegQueryValueEx(hkey, "ServerIP", 0, 0, (LPBYTE )retdata, (LPDWORD)&datasize);
		else if( type == 9)
			retval = RegQueryValueEx(hkey, "DBIP", 0, 0, (LPBYTE )retdata, (LPDWORD)&datasize);
		else if( type == 12)
			retval = RegQueryValueEx(hkey, "RetryInterval", 0, 0, (LPBYTE )retdata, (LPDWORD)&datasize);
		else if( type == 16)
			retval = RegQueryValueEx(hkey, "DBTYPE", 0, 0, (LPBYTE )retdata, (LPDWORD)&datasize);
		
		if(retval != ERROR_SUCCESS)
		{
			if( type == 1)
			retval = RegQueryValueEx(hkey, "ServerIP", 0, 0, (LPBYTE )retdata, (LPDWORD)&datasize);
			else if( type == 9)
				retval = RegQueryValueEx(hkey, "DBIP", 0, 0, (LPBYTE )retdata, (LPDWORD)&datasize);
			else if( type == 12)
				retval = RegQueryValueEx(hkey, "RetryInterval", 0, 0, (LPBYTE )retdata, (LPDWORD)&datasize);
			else if( type == 16)
				retval = RegQueryValueEx(hkey, "DBTYPE", 0, 0, (LPBYTE )retdata, (LPDWORD)&datasize);

		}
		RegCloseKey(hkey);
	}
	return datasize;
}

// 인증 서버 연결 테스트
BOOL connectServer()
{
	
WSADATA wsaData;
SOCKET sock;
SOCKADDR_IN servAddr;
DWORD dwipaddress=0;
char szIP[32];
char szPort[32];
USHORT uPort=0;
TCHAR buf[MAX_PATH];
	
	// get server port
	ZeroMemory(szPort,sizeof(szPort));
	GetRegKey( szPort  ,2);
	uPort = atoi(szPort);
		
	if(WSAStartup(MAKEWORD(2, 2), &wsaData) != 0) /* Load Winsock 2.2 DLL */
	{
	  OutputDebugString("WSAStartup() error!");
	  return FALSE;
	 }
	
	sock=socket(PF_INET, SOCK_STREAM, 0);
	if(sock == INVALID_SOCKET)
	{
		OutputDebugString("socket() error");
		return FALSE;
	}

	ZeroMemory(&servAddr, sizeof(servAddr));
	servAddr.sin_family=AF_INET;

	// get server ipaddress
	GetRegData( (LPBYTE)&dwipaddress,1);
	
	wsprintf(szIP,"%d.%d.%d.%d", HIBYTE(HIWORD(dwipaddress)),LOBYTE(HIWORD(dwipaddress)),
		HIBYTE(LOWORD(dwipaddress)),LOBYTE(LOWORD(dwipaddress)));
	
	servAddr.sin_addr.s_addr=inet_addr(szIP);
	servAddr.sin_port=htons(uPort);
	
	BOOL bRet=TRUE;
	if(connect(sock, (SOCKADDR*)&servAddr, sizeof(servAddr))==SOCKET_ERROR)
	{
		bRet = FALSE;
	}
	else
	{
		// send user information and receive user's level
		TQuestionData tqd;
		TCommData tcd;
		GetMacAddress();
		ZeroMemory(&tqd, sizeof(tqd));
		ZeroMemory(&tcd, sizeof(tcd));
		strcpy(tqd.szCommand,"RQTLVL");
		strcpy(tqd.szMAC,m_szMAC);
		send(sock, (LPSTR)&tqd,sizeof(tqd), 0);
		OutputDebugString(tqd.szCommand);
		OutputDebugString(tqd.szMAC);
		//memset(buf, NULL, sizeof(buf));
		int len = recv(sock, (LPSTR)&tcd, sizeof(tcd), 0);
		if( len == SOCKET_ERROR ) 
		{
			wsprintf(buf,"User Level receive error[%d]",WSAGetLastError());
			OutputDebugString(buf);		
		}
		else
		{
			// the first byte is the level value
			m_nLevel = (int)tcd.szCommand[0];
			wsprintf(buf,"User Level is [%d]", m_nLevel);
			OutputDebugString(buf);	
			memset(m_strLv, NULL, sizeof(m_strLv));
			switch(m_nLevel)
			{
				case 0:
					wsprintf(m_strLv, "%s", "사원");
					break;
				case 1:
					wsprintf(m_strLv, "%s", "주임");
					break;
				case 2:
					wsprintf(m_strLv, "%s", "대리");
					break;
				case 3:
					wsprintf(m_strLv, "%s", "과장");
					break;
				case 4:
					wsprintf(m_strLv, "%s", "부장");
					break;
				case 5:
					wsprintf(m_strLv, "%s", "실장");
					break;
				case 6:
					wsprintf(m_strLv, "%s", "이사");
					break;
				case 7:
					wsprintf(m_strLv, "%s", "사장");
					break;
				case 8:
					wsprintf(m_strLv, "%s", "회장");
					break;
				default:
					wsprintf(m_strLv, "%s", "사원");
					break;
			}
			switch(m_nLevel)
			{
				case 0:
					m_nSDocLevel = 0;
					break;
				case 1:
				case 2:
					m_nSDocLevel = 1;
					break;
				case 3:
				case 4:
				case 5:
					m_nSDocLevel = 2;
					break;
				case 6:
				case 7:
				case 8:
					m_nSDocLevel = 3;
					break;
				default:
					m_nSDocLevel = 0;
					break;
			}
		}
	}
	
	closesocket(sock);
	WSACleanup();
	return bRet;

}

void WriteLog(LPCSTR lpFileName, UINT nType)
{

	TCHAR szPath[MAX_PATH];
	TCHAR buf[MAX_PATH];
	TCHAR szValue[MAX_PATH];
	TCHAR szTime[MAX_PATH];

	//로그 생성, INI 파일로 파일 정보를 쓴다 
	GetTempPath(MAX_PATH, szPath);
	strcat(szPath, "CryptLog.ini");

	
	int nCount = GetPrivateProfileInt("FILELOG", "COUNT", 0, szPath);
	wsprintf(buf,"%d", nCount+1);
	WritePrivateProfileString("FILELOG", "COUNT", buf, szPath); // 갯수


	wsprintf(buf,"FILENAME%d", nCount+1);
	WritePrivateProfileString("FILELOG", buf, lpFileName, szPath); // 파일 명
	
	wsprintf(buf,"FILETYPE%d", nCount+1);
	wsprintf(szValue,"%d", m_nSDocLevel/*m_nLevel*//*nType*/);
	WritePrivateProfileString("FILELOG", buf, szValue, szPath); // 권한 0-8 //(구분 1: 암호 해제, 2:내용보기 )
	SYSTEMTIME st; 
	GetLocalTime(&st);
	wsprintf(buf,"FILETIME%d", nCount+1);
	wsprintf(szTime, "%d-%d-%d %d:%d:%d", st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond);
	WritePrivateProfileString("FILELOG", buf, szTime, szPath); // 로그 시각
	
}
// CSimpleShlExt
CSimpleShlExt::CSimpleShlExt()
{
	m_hRegBmp = LoadBitmap ( _Module.GetModuleInstance(),
						   MAKEINTRESOURCE(IDB_REGISTERBMP) );

	m_hUnregBmp = LoadBitmap ( _Module.GetModuleInstance(),
							 MAKEINTRESOURCE(IDB_UNREGISTERBMP) );

	m_hViewBmp = LoadBitmap ( _Module.GetModuleInstance(),
							 MAKEINTRESOURCE(IDB_VIEWBMP) );
	s_drm = 0;
}
CSimpleShlExt::~CSimpleShlExt()
{
	if ( NULL != m_hRegBmp )
		DeleteObject ( m_hRegBmp );

	if ( NULL != m_hUnregBmp )
		DeleteObject ( m_hUnregBmp );

	if ( NULL != m_hViewBmp )
		DeleteObject ( m_hViewBmp );
}

STDMETHODIMP CSimpleShlExt::Initialize (
    LPCITEMIDLIST pidlFolder, LPDATAOBJECT pDataObj, HKEY hProgID )
{
	FORMATETC fmt = { CF_HDROP, NULL, DVASPECT_CONTENT, -1, TYMED_HGLOBAL };
	STGMEDIUM stg = { TYMED_HGLOBAL };
	HDROP     hDrop;

	// Look for CF_HDROP data in the data object.
	if ( FAILED( pDataObj->GetData ( &fmt, &stg ) ))
	{
		// Nope! Return an "invalid argument" error back to Explorer.
		return E_INVALIDARG;
	}

    // Get a pointer to the actual data.
	hDrop = (HDROP) GlobalLock ( stg.hGlobal );

	// Make sure it worked.
	if ( NULL == hDrop )
		return E_INVALIDARG;

	// Sanity check - make sure there is at least one filename.
	UINT uNumFiles = DragQueryFile ( hDrop, 0xFFFFFFFF, NULL, 0 );
	HRESULT hr = S_OK;

	if ( 0 == uNumFiles )
	{
		GlobalUnlock ( stg.hGlobal );
		ReleaseStgMedium ( &stg );
		return E_INVALIDARG;
	}

	// Get the name of the first file and store it in our member variable m_szFile.
	if ( 0 == DragQueryFile ( hDrop, 0, m_szFile, MAX_PATH ) )
		hr = E_INVALIDARG;

	s_drm = 0;
	if(IsEncryptFile(m_szFile))
	{
		s_drm = 1;
	}
//	 확장자로 암호문서를 구별하는 경우
//		char *lpExt;
//		lpExt = PathFindExtension(m_szFile);
//		
//		if (StrCmpNI(lpExt,".drm",4) == 0) {
//			s_drm = 1;
//		}

	GlobalUnlock ( stg.hGlobal );
	ReleaseStgMedium ( &stg );

	return hr;
}

STDMETHODIMP CSimpleShlExt::QueryContextMenu (
    HMENU hmenu, UINT uMenuIndex, UINT uidFirstCmd,
    UINT uidLastCmd, UINT uFlags )
{
    // If the flags include CMF_DEFAULTONLY then we shouldn't do anything.
    if ( uFlags & CMF_DEFAULTONLY )
        return MAKE_HRESULT ( SEVERITY_SUCCESS, FACILITY_NULL, 0 );

	UINT items = 1;
	if (s_drm == 0)
	{
		InsertMenu ( hmenu, uMenuIndex, MF_BYPOSITION, uidFirstCmd, _T("DRM으로 보안문서 만들기") );

		// Set the bitmap for the register item.
		if ( NULL != m_hRegBmp )
			SetMenuItemBitmaps ( hmenu, uMenuIndex, MF_BYPOSITION, m_hRegBmp, NULL );
	}
	
	if (s_drm == 1)
	{
		InsertMenu ( hmenu, uMenuIndex, MF_BYPOSITION, uidFirstCmd, _T("DRM으로 보안문서 해제하기") );

		// Set the bitmap for the unregister item.
		if ( NULL != m_hUnregBmp )
			SetMenuItemBitmaps ( hmenu, uMenuIndex, MF_BYPOSITION, m_hUnregBmp, NULL );

		uMenuIndex++;
		uidFirstCmd++;
		// 내용보기 차후 지원
/*		InsertMenu ( hmenu, uMenuIndex, MF_BYPOSITION, uidFirstCmd, _T("DRM으로 내용보기 ") );
		
		// Set the bitmap for the unregister item.
		if ( NULL != m_hUnregBmp )
			SetMenuItemBitmaps ( hmenu, uMenuIndex, MF_BYPOSITION, m_hViewBmp, NULL );
		items = 2;
*/	}
	//at the end, we tell Explorer how many items we added.
    return MAKE_HRESULT ( SEVERITY_SUCCESS, FACILITY_NULL, items );
}

STDMETHODIMP CSimpleShlExt::GetCommandString (
    UINT idCmd, UINT uFlags, UINT* pwReserved, LPSTR pszName, UINT cchMax )
{
USES_CONVERSION;
LPCTSTR szText;

    // Check idCmd, it must be 0 since we have only one menu item.
//    if ( 0 != idCmd )
//        return E_INVALIDARG;


    // If Explorer is asking for a help string, copy our string into the
    // supplied buffer.
	if ( uFlags & GCS_HELPTEXT )
    {
		switch ( idCmd )
		{
			case 0:
				if (s_drm == 0)
				{
					szText = _T("NEO-DRM으로 보안문서를 만듭니다.");
				}
				else
				{
					szText = _T("NEO-DRM으로 보안문서를 해제합니다.");
				}
			break;
// 내용 보기 차후 지원
//			case 1:
//				szText = _T("NEO-DRM으로 암호문서의 내용을 조회합니다.");
//			break;

			default:
				return E_INVALIDARG;
			break;
		}
    

        if ( uFlags & GCS_UNICODE )
		{
			// We need to cast pszName to a Unicode string, and then use the
			// Unicode string copy API.
			lstrcpynW ( (LPWSTR) pszName, T2CW(szText), cchMax );
		}
        else
		{
			// Use the ANSI string copy API to return the help string.
			lstrcpynA ( pszName, T2CA(szText), cchMax );
		}
        return S_OK;
    }

    return E_INVALIDARG;
}

STDMETHODIMP CSimpleShlExt::InvokeCommand ( LPCMINVOKECOMMANDINFO pCmdInfo )
{
    // If lpVerb really points to a string, ignore this function call and bail out.
    if ( 0 != HIWORD( pCmdInfo->lpVerb ) )
        return E_INVALIDARG;

    // Get the command index - the only valid one is 0.
    switch ( LOWORD( pCmdInfo->lpVerb) )
	{
        case 0:
		{
            TCHAR szPath[MAX_PATH];
			TCHAR szCmdline[MAX_PATH];
			TCHAR szLevel[MAX_PATH];
			ZeroMemory(szPath,MAX_PATH);
			ZeroMemory(szCmdline,MAX_PATH);
			ZeroMemory(szLevel,MAX_PATH);
			
            // call "neodrm.exe e m_szFile"
			GetRegKey(szPath,8);
			StrNCat(szPath,"NeoDrm.exe",11);
			if (s_drm == 0) // 암호 만들기 명령 
			{
//				if(!connectServer())
//				{
//					MessageBox(NULL, "사내 인증서버와 \n연결된 상태에서만 가능합니다.\n", "NEO-DRM으로 암호화 문서 만들기", MB_OK);
//					return S_OK;
//				}
//				else
//				{
					wsprintf(szLevel, "e%d ", m_nSDocLevel);
					StrNCat(szCmdline, szLevel, 4); // include null
//				}
			}
			else // 암호 풀기 명령 
			{
				//wsprintf(szLevel, "d%d ", m_nLevel);
				StrNCat(szCmdline, "d  ", 4); // include null7
//				if ( !connectServer())
//				{
					// 인증서버 연결이 안되면 , 하드락 프로그램을 호출한다.
//					GetRegKey(szPath,8);
//					StrNCat(szPath,"NeoDrmHL.exe",13);
//				}
//				else
//				{
					if(m_nSDocLevel >= m_nDocLevel)
					{
						OutputDebugString("Level ok");
					}
					else
					{
						char szTemp[MAX_PATH];
						memset(szTemp, NULL, sizeof(MAX_PATH));
						// 문서 등급을 메시지로 표시하기 위한 변환작업
						int nLvType = 0;
						if(m_nSDocLevel == 0){ nLvType = 4; }
						else if(m_nSDocLevel == 1){ nLvType = 3; }
						else if(m_nSDocLevel == 2){ nLvType = 2; }
						else if(m_nSDocLevel == 3){ nLvType = 1; }
						else{ nLvType = 0; }
						wsprintf(szTemp, "이 문서의 등급은 %d등급 입니다.\n현재 %s님 권한으로는\n조회할 수 없습니다.", nLvType, m_strLv);
						MessageBox(NULL, szTemp, "NEO-DRM으로 암호화 문서 해제하기", MB_OK);
						return S_OK;
					}
//				}
			}
			StrNCat(szCmdline,m_szFile, strlen(m_szFile)+1);
			CreateProcessEx ( szPath, szCmdline, true, false, false, false, NULL );
			if (s_drm == 1) //복호화 로그를  생성한다.
				WriteLog(m_szFile, 1);

            return S_OK;
		}
        break;
		// 내용 보기 차후 지원
/*		case 1:
		{
			//임시 디렉터리로 복사하여 파일을 해제한 다음, 프로그램을 연결한다.
			TCHAR szPath[MAX_PATH];
			TCHAR szTempPath[MAX_PATH];
			TCHAR szCmdline[MAX_PATH];
			char *lpstr;
			ZeroMemory(szPath,MAX_PATH);
			ZeroMemory(szTempPath,MAX_PATH);
			ZeroMemory(szCmdline,MAX_PATH);

			GetTempPath(MAX_PATH,szTempPath);
			lpstr =  PathFindFileName( m_szFile );
			lstrcat(szTempPath,lpstr);
			CopyFile(m_szFile, szTempPath,FALSE); //임시 디렉터리로 복사
			
			// call "neodrm.exe e m_szFile"
			GetRegKey(szPath,8);
			if ( !connectServer())
			{
				// 인증서버 연결이 안되면 , 하드락 프로그램을 호출한다. 
				StrNCat(szPath,"NeoDrmHL.exe",13);
			}
			else
			{
				StrNCat(szPath,"NeoDrm.exe",11);
			}
			
			// 암호 풀기 명령
			if(m_nLevel >= m_nDocLevel)
			{
				lstrcat(szCmdline,"d "); // include null
				lstrcat(szCmdline, szTempPath);
				
				BOOL bWait=TRUE;
				CreateProcessEx ( szPath, szCmdline, true, false, bWait, false, NULL );
				if(!IsEncryptFile(szTempPath))
				{
					OpenFileType(szTempPath);
					WriteLog(m_szFile, 2); // 내용보기 로그
				}

			}
			else
			{
				MessageBox(NULL, "조회권한이 없습니다.\n", "Neo-Drm", MB_OK);
			}
            return S_OK;
		}
        break;
*/
        default:
            return E_INVALIDARG;
        break;
	}
}
