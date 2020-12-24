#include "StdAfx.h"
#include <windows.h>
#include <stdio.h>
#include <stdarg.h>

#include "util.h"

#define LOGFILENAME TEXT("DoProUseLog.log") 
#define ERRORLOGFILENAME TEXT("DoProErrorLog.log") 

int GetFilePath(char *fname, char *path)
{
	*path =0;

	char *p =fname+strlen(fname);
	while(p > fname)
	{
		if(*p =='\\' || *p =='//')
		{
			strncpy(path,  fname, p-fname);
			path[p-fname] =0;
			return 0;
		}
		p--;
	}

	return -1;
}
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
void OpenUserProgram(char *szCommand)
{
//	TCHAR key[MAX_PATH + MAX_PATH];    

	// First try ShellExecute()
	HINSTANCE result = ShellExecute(NULL, _T("open"), szCommand, NULL,NULL, SW_SHOW);

	// If it failed, get the .htm regkey and lookup the program
	if ((UINT)result <= HINSTANCE_ERROR)
	{        
/*		if (GetRegKey(HKEY_CLASSES_ROOT, _T(".htm"), key) == ERROR_SUCCESS)
		{
			lstrcat(key, _T("\\shell\\open\\command"));

			if (GetRegKey(HKEY_CLASSES_ROOT,key,key) == ERROR_SUCCESS)
			{
				TCHAR *pos;
				pos = _tcsstr(key, _T("\"%1\""));
				if (pos == NULL)						// No quotes found
				{
					pos = strstr(key, _T("%1"));		// Check for %1, without quotes
					if (pos == NULL)					// No parameter at all...
						pos = key+lstrlen(key)-1;
					else
						*pos = 0x00;//"\0";				// Remove the parameter
				}
				else
				*pos = 0x00;//"\0";						// Remove the parameter

				lstrcat(pos, _T(" "));
				lstrcat(pos, szCommand);
				result = (HINSTANCE) WinExec(key,SW_SHOW);
			}
		}
*/	}
}

void ViewLog(int nLog)
{
	char path[MAX_PATH];
	GetTempPath(MAX_PATH, path);

	if(nLog == 0)
		strcat(path,LOGFILENAME);
	else if(nLog == 1)
		strcat(path,ERRORLOGFILENAME);

	OpenUserProgram(path);
}
void RemoveLog(int nLog)
{
	char path[MAX_PATH];
	GetTempPath(MAX_PATH, path);

	if(nLog == 0)
		strcat(path,LOGFILENAME);
	else if(nLog == 1)
		strcat(path,ERRORLOGFILENAME);

	DeleteFile(path);
}
void WriteLog(int nLog, char *fmt,...)
{
	va_list args;
	char modname[200];

	char path[MAX_PATH];
	char temp[5000];
	HANDLE hFile;

	GetModuleFileName(NULL, modname, sizeof(modname));

	GetTempPath(MAX_PATH, path);

	if(nLog == 0)
		strcat(path,LOGFILENAME);
	else if(nLog == 1)
		strcat(path,ERRORLOGFILENAME);
	
	if((hFile =CreateFile(path, GENERIC_WRITE, 0, NULL, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL)) <0)
	{
		return;
	}
	
	int pos = SetFilePointer (hFile, 0, NULL, FILE_END);
	
	if (pos != -1) // Test for failure
	{ 
		DWORD dw;
		//wsprintf(temp, "%s:", modname);
		//WriteFile(hFile, temp, strlen(temp), &dw, NULL);
		
		va_start(args,fmt);
		vsprintf(temp, fmt, args);
		va_end(args);
		
		WriteFile(hFile, temp, strlen(temp), &dw, NULL);
		
		wsprintf(temp, "\r\n");
		WriteFile(hFile, temp, strlen(temp), &dw, NULL);
	}

	CloseHandle(hFile);
}

void WriteLog2(char *fname, char *fmt, ...)
{
	va_list args;
	char modname[200];

	char temp[5000];
	HANDLE hFile;

	GetModuleFileName(NULL, modname, sizeof(modname));

	if((hFile =CreateFile(fname, GENERIC_WRITE, 0, NULL, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL)) <0)
	{
		return;
	}
	
	_llseek((HFILE)hFile, 0, SEEK_END);

	wsprintf(temp, "%s:", modname);
	DWORD dw;
	WriteFile(hFile, temp, strlen(temp), &dw, NULL);
	
	va_start(args,fmt);
	vsprintf(temp, fmt, args);
	va_end(args);

	WriteFile(hFile, temp, strlen(temp), &dw, NULL);

	wsprintf(temp, "\r\n");
	WriteFile(hFile, temp, strlen(temp), &dw, NULL);

	_lclose((HFILE)hFile);
}

char * GetErrString(char *msg, DWORD msg_size, DWORD err)
{
	*msg =0;

	if(FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM |FORMAT_MESSAGE_IGNORE_INSERTS,
		NULL, err, 0, msg, msg_size, NULL))
		return NULL;

	return msg;
}

int getOSInfo()
{
	int nOS;
	OSVERSIONINFOEX osvi;
	osvi.dwOSVersionInfoSize = sizeof(OSVERSIONINFOEX);
	GetVersionEx((OSVERSIONINFO *)&osvi);

	int osversion = osvi.dwMajorVersion*10+osvi.dwMinorVersion;	// get major version and minor version of OS for deciding OS

	switch (osversion)
	{
		case 61:
			// Windows 7, Windows Server 2008 R2
			nOS = 5;
			break;

		case 60:
			// Windows Vista, Windows Server 2008 Datacenter Edition, Windows Server 2008 Enterprise Edition, Windows Server 2008
			nOS = 4;
			break;

		case 52:
			// Windows Storage Server 2003, Windows Server 2003 Datacenter Edition, Windows Server 2003 Enterprise Edition, Windows Server 2003, Windows Storage Server 2003 R2, Windows Server 2003 R2, Windows XP Professional 64bit Edition
			nOS = 3;
			break;

		case 51:
			// Windows XP Home Edition, Windows XP Professional Edition, Windows XP Media Center Edition, Windows XP Starter Edition, Windows XP Tablet PC Edition, Windows XP Embedded Edition, Windows XP
			nOS = 2;
			break;

		case 50:
			// Windows 2000 Professional Edition, Windows 2000 Datacenter Edition, Windows 2000 Advanced Server Edition, Windows 2000
			nOS = 1;
			break;

		case 49:
		case 41:
		case 40:
			// Windows Me, Windows 98, Windows 95
			nOS = 0;
			break;

		default:
			nOS = 0;
			break;
	}
	return nOS;
}
