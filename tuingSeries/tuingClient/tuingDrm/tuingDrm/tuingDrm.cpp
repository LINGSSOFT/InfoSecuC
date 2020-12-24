#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS

// tuingDrm.cpp : Defines the entry point for the application.
//

#include "framework.h"
#include "stdafx.h"
#include <Iphlpapi.h>
#include "tuingDrm.h"
#include "Rijndael.h"
#include "XOR256Stream.h"
#include <shlwapi.h>
#include <memory>
#include <string.h>
#include <stdio.h>

#pragma comment(lib,"Iphlpapi.lib")
#pragma comment(lib,"shlwapi.lib")

#define MAX_LOADSTRING 100
#define MALLOC(x) HeapAlloc(GetProcessHeap(), 0, (x)) 
#define FREE(x) HeapFree(GetProcessHeap(), 0, (x))
CHAR m_szMAC[MAX_PATH];
CHAR m_szIP[MAX_PATH];

#define HEADER_SIZE 22
#define KEY_SIZE	8
using namespace std;

enum { ENCRYPT = 0, DECRYPT = 1 };
enum { EFILE = 0, STRING = 1 };
enum { AES = 0, BLOWFISH = 1, TEA = 2, XOR256_BLOCK = 3, XOR256_STREAM = 4 };

int m_iAction = 0;


auto_ptr<CRijndael> m_apAES;
auto_ptr<CXOR256Stream> m_apXOR256Stream;

void AESParams(char* roStrKeyData, BOOL& rbHex, int& riKeyLength, int& riBlockSize, int& riMode, int& riPadding)
{

	strcpy(roStrKeyData, "BNA");
	//Key Length
	riKeyLength = 16;
	//Block Size
	riBlockSize = 16;
	//Mode
	riMode = IMethod::ECB;
	//Padding
	riPadding = IMethod::ZEROES;

}
void XOR256StreamParams(char* roStrKeyData, BOOL& rbHex, int& riIRounds)
{
	strcpy(roStrKeyData, "BNA");
	rbHex = FALSE;
	riIRounds = 4;
}

IMethod* GetMethod(int iMethod)
{
	IMethod* poMethod = nullptr;
	char  szStrKeyData[MAX_PATH];
	BOOL bHex;
	switch (iMethod)
	{
	case AES:
	{
		int iMode, iPadding;
		int iKeyLength, iBlockSize;
		AESParams(szStrKeyData, bHex, iKeyLength, iBlockSize, iMode, iPadding);
		if (NULL == m_apAES.get())
			//Create First Time
			m_apAES = auto_ptr<CRijndael>(new CRijndael());
		char acKey[32];
		strncpy(acKey, LPCTSTR(szStrKeyData), 32);

		m_apAES->Initialize(acKey, 32, CRijndael::sm_chain0, iKeyLength, iBlockSize,
			iMode, iPadding);
		poMethod = m_apAES.get();
		break;
	}
	case XOR256_STREAM:
	{
		int iIRounds;
		XOR256StreamParams(szStrKeyData, bHex, iIRounds);
		if (NULL == m_apXOR256Stream.get())
			//Create First Time
			m_apXOR256Stream = auto_ptr<CXOR256Stream>(new CXOR256Stream());
		char acKey[256];
		strncpy(acKey, LPCTSTR(szStrKeyData), 256);
		m_apXOR256Stream->Initialize(acKey, 256, iIRounds);
		poMethod = m_apXOR256Stream.get();
		break;
	}
	default:
		break;
	}

	return poMethod;
}
// check the header of the file, compare hexadecimal value
// the first 12 bytes is  owner who this file encrypts
// the second 1 byte is authority ; 0: user, 1, 2, 3, 4, 5: CEO
// the third 1 byte is a reserved byte
BOOL IsEncryptFile(LPCSTR lpFileName)
{
	DWORD     dwFileSizeLo, dwFileSizeHi;
	DWORDLONG qwSize;
	HANDLE    hFile;

	hFile = CreateFile(lpFileName, GENERIC_READ, FILE_SHARE_READ, NULL,
		OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);

	if (INVALID_HANDLE_VALUE == hFile)
	{
		OutputDebugString("the encrypt file is invalid!");
		return FALSE;
	}

	dwFileSizeLo = GetFileSize(hFile, &dwFileSizeHi);
	qwSize = ((DWORDLONG)dwFileSizeHi) << 32 | dwFileSizeLo;
	if (qwSize > 8)
	{
		DWORD dwBytesRead = 0;
		BYTE ReadBuffer[HEADER_SIZE + 1] = { 0 };
		BYTE Header[KEY_SIZE + 1] = { 0x01, 0xAB, 0x0C, 0xC4, 0x64, 0x2E, 0xB1, 0xCB, 0x00 };
		if (ReadFile(hFile, ReadBuffer, HEADER_SIZE, &dwBytesRead, NULL))
		{

			// Criteria
			if (memcmp(ReadBuffer + 14, Header, KEY_SIZE) == 0)
			{
				OutputDebugString("read ok");
			}
			else
			{
				OutputDebugString("the encrypt file is invalid!");
				CloseHandle(hFile);
				return FALSE;
			}

		}
	}
	else
	{
		OutputDebugString("the encrypt file is invalid!");
		CloseHandle(hFile);
		return FALSE;
	}
	CloseHandle(hFile);
	return TRUE;
}
//
//
BOOL CompareDoc(char* szExt)
{
	char caplist[8][6] = { ".hwp", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx" };

	_strlwr(szExt);

	for (int i = 0; i < 7; i++)
	{
		if (strcmp(szExt, caplist[i]) == 0)
		{
			return TRUE;
		}
	}
	return FALSE;
}


void GetMacAddress()
{

	TCHAR szBuf[MAX_PATH];
	// Global variables
	ULONG		ulOutBufLen;
	DWORD		dwRetVal;

	/* variables used for GetAdapterInfo */
	IP_ADAPTER_INFO* pAdapterInfo;
	IP_ADAPTER_INFO* pAdapter;

	//GetAdaptersInfo

	pAdapterInfo = (IP_ADAPTER_INFO*)MALLOC(sizeof(IP_ADAPTER_INFO));
	if (pAdapterInfo == NULL) {
		OutputDebugString("Error allocating memory needed to call GetAdapterInfo");
		return;
	}
	ulOutBufLen = sizeof(IP_ADAPTER_INFO);

	if (GetAdaptersInfo(pAdapterInfo, &ulOutBufLen) == ERROR_BUFFER_OVERFLOW) {
		FREE(pAdapterInfo);
		pAdapterInfo = (IP_ADAPTER_INFO*)MALLOC(ulOutBufLen);
		if (pAdapterInfo == NULL) {
			OutputDebugString("Error allocating memory needed to call GetAdapterInfo");
			return;
		}
	}

	if ((dwRetVal = GetAdaptersInfo(pAdapterInfo, &ulOutBufLen)) != NO_ERROR) {
		FREE(pAdapterInfo);
		OutputDebugString("GetAdaptersInfo call failed");
		return;
	}
	pAdapter = pAdapterInfo;

	while (pAdapter) {

		//pAdapter->AdapterName);
		//pAdapter->Description);

		unsigned char* MACData = pAdapter->Address;
		wsprintf(szBuf, "%02X%02X%02X%02X%02X%02X",
			MACData[0], MACData[1], MACData[2], MACData[3], MACData[4], MACData[5]);
		memset(m_szMAC, 0x00, sizeof(m_szMAC));
		lstrcpy(m_szMAC, szBuf);
		memset(m_szIP, 0x00, sizeof(m_szIP));
		lstrcpy(m_szIP, pAdapter->IpAddressList.IpAddress.String);

		//pAdapter->IpAddressList.IpAddress.String);
		//pAdapter->IpAddressList.IpMask.String);
		//pAdapter->GatewayList.IpAddress.String);

		break;  // only first adapter

		pAdapter = pAdapter->Next;
	}

	FREE(pAdapterInfo);
}
void CopyToTemp(char* szInputFile1)
{

	char* lpFname;
	lpFname = PathFindFileName(szInputFile1);

	char szPath[MAX_PATH];
	GetTempPath(MAX_PATH, szPath);
	strcat(szPath, lpFname);

	CopyFile(szInputFile1, szPath, FALSE);
}

void ActionFile(LPSTR cmdline)
{
	OutputDebugString(cmdline);
	char szInputFile1[MAX_PATH];
	char szInputFile3[MAX_PATH];

	char szOutputFile1[MAX_PATH];
	char szOutputFile3[MAX_PATH];

	if (cmdline[0] == 'e')
	{
		m_iAction = ENCRYPT;
	}
	else if (cmdline[0] == 'd')
	{
		m_iAction = DECRYPT;
	}
	else
	{
		OutputDebugString(cmdline);
		OutputDebugString("neodrm command error!");
		return;
	}
	//Input File
	if (ENCRYPT == m_iAction)
	{
		strcpy(szInputFile1, cmdline + 3);

		if (!PathFileExists(szInputFile1)) {
			OutputDebugString(szInputFile1);
			OutputDebugString("the file does not exist!");
			return;
		}
		DWORD dwFile = GetFileAttributes(szInputFile1);
		if (dwFile & FILE_ATTRIBUTE_DIRECTORY) {
			OutputDebugString("the file is directory!");
			return;
		}

		char* szExt;
		szExt = PathFindExtension((LPCTSTR)szInputFile1);

		if (!CompareDoc(szExt)) {
			OutputDebugString("the file is not supported type!");
			return;

		}
	}
	else if (DECRYPT == m_iAction)
	{
		strcpy(szInputFile3, cmdline + 3);
		if (!IsEncryptFile(szInputFile3))
			return;

	}
	else
		return;

	//Output File	
	if (ENCRYPT == m_iAction)
	{

		LPSTR lp1 = PathFindFileName(szInputFile1);
		GetTempPath(MAX_PATH, szOutputFile1);
		strcat(szOutputFile1, lp1);

		//strcpy(szOutputFile1,szInputFile1);
		//strcat(szOutputFile1,".drm"); // add temp extension

	}
	else if (DECRYPT == m_iAction)
	{

		LPSTR lp3 = PathFindFileName(szInputFile3);
		GetTempPath(MAX_PATH, szOutputFile3);
		strcat(szOutputFile3, lp3);

		//strcpy(szOutputFile3,szInputFile3);
		//strcat(szOutputFile3,".drm"); // add temp extension
		//PathRemoveExtension(szOutputFile3); // strip extension .drm

	}
	else
		return;

	try
	{
		IMethod* poMethod = GetMethod(4); // 0 : AES, 4 : XOR256STREAM

		if (ENCRYPT == m_iAction)
		{
			// 0-11 : MAC, 12: nLevel, 13: reserved
			GetMacAddress();
			int p = cmdline[1] - 0x30;
			m_szMAC[12] = (char)p;
			m_szMAC[13] = '0';

			poMethod->SetAuthority(m_szMAC, 14);

			poMethod->EncryptFile(LPCTSTR(szInputFile1), LPCTSTR(szOutputFile1));
		}
		else //DECRYPT == m_iAction
		{
			poMethod->DecryptFile(LPCTSTR(szInputFile3), LPCTSTR(szOutputFile3));
		}
	}
	catch (exception const& roException)
	{
		OutputDebugString(roException.what());
		return;
	}
	if (ENCRYPT == m_iAction)
	{
		OutputDebugString("ENCRYPT");
		OutputDebugString(szOutputFile1);
		OutputDebugString(szInputFile1);

		// copy original to the backup folder
		//CopyToTemp(szInputFile1);

		if (!CopyFile(szOutputFile1, szInputFile1, FALSE))
		{
			wsprintf(szInputFile1, "CopyFile Error Code [%d]", GetLastError());
		}


	}
	else if (DECRYPT == m_iAction)
	{
		OutputDebugString("DECRYPT");
		OutputDebugString(szOutputFile3);
		OutputDebugString(szInputFile3);

		if (!CopyFile(szOutputFile3, szInputFile3, FALSE))
		{
			wsprintf(szInputFile3, "CopyFile Error Code [%d]", GetLastError());
		}

	}

}

// Forward declarations of functions included in this code module:
int APIENTRY WinMain(HINSTANCE hInstance,
                     HINSTANCE hPrevInstance,
                     LPSTR     lpCmdLine,
                     int       nCmdShow)
{
    // TODO: Place code here.
	ActionFile(lpCmdLine);

	return 0;
}
