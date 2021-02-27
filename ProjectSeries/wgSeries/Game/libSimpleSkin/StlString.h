#pragma once

#include <tchar.h>
#include <string>
#pragma warning( disable: 4786 )

#ifdef _UNICODE
typedef std::wstring		STRING;
#else
typedef std::string			STRING;
#endif


void MakeUpper(STRING & str);
void MakeLower(STRING & str);