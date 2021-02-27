// MyOverlayIcon.cpp : Implementation of 

#include "stdafx.h"
#include "MyOverlayIcon.h"

#define HEADER_SIZE 22
#define KEY_SIZE	8

LPSTR UnicodeToAnsi(LPCWSTR s)
{
if (s==NULL) return NULL;
int cw=lstrlenW(s);
if (cw==0) {CHAR *psz=new CHAR[1];*psz='\0';return psz;}
int cc=WideCharToMultiByte(CP_ACP,0,s,cw,NULL,0,NULL,NULL);
if (cc==0) return NULL;
CHAR *psz=new CHAR[cc+1];
cc=WideCharToMultiByte(CP_ACP,0,s,cw,psz,cc,NULL,NULL);
if (cc==0) {delete[] psz;return NULL;}
psz[cc]='\0';
return psz;
}


// CMyOverlayIcon

// IShellIconOverlayIdentifier Method Implementation 
// IShellIconOverlayIdentifier::GetOverlayInfo
// returns The Overlay Icon Location to the system
STDMETHODIMP CMyOverlayIcon::GetOverlayInfo(
  LPWSTR pwszIconFile,
  int cchMax,int* pIndex,
  DWORD* pdwFlags)
{
  OutputDebugString("GetOverlayInfo");	
  GetModuleFileNameW(_AtlBaseModule.GetModuleInstance(), pwszIconFile, cchMax);

  *pIndex = 0;
  *pdwFlags = ISIOI_ICONFILE | ISIOI_ICONINDEX;

//OutputDebugString(UnicodeToAnsi(pwszIconFile));	
  
  return S_OK;
}

// IShellIconOverlayIdentifier Method Implementation 

// returns the priority of this overlay 0 being the highest. 
// this overlay is always selected do to its high priority 
STDMETHODIMP CMyOverlayIcon::GetPriority(int* pPriority)
{
  // highest priority
  *pPriority=0;
  return S_OK;
}

// IShellIconOverlayIdentifier Method Implementation
// IShellIconOverlayIdentifier::IsMemberOf
// Returns Whether the object should have this overlay or not 
STDMETHODIMP CMyOverlayIcon::IsMemberOf(LPCWSTR pwszPath, DWORD dwAttrib)
{
	wchar_t *s = _wcsdup(pwszPath);
	HRESULT r = S_FALSE;

	//_wcslwr(s);

	DWORD     dwFileSizeLo, dwFileSizeHi;
	DWORDLONG qwSize;
	HANDLE    hFile;

	hFile = CreateFile ( UnicodeToAnsi(s), GENERIC_READ, FILE_SHARE_READ, NULL,
						OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL );

	if ( INVALID_HANDLE_VALUE == hFile )
	{
		free(s);
		return S_FALSE;    
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
				r = S_OK;
				OutputDebugString(UnicodeToAnsi(s));
			}
		}
	}
	CloseHandle ( hFile );
	free(s);
	return r;
}
