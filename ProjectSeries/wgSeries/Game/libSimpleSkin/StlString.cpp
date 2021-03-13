#include "stdafx.h"
#include "StlString.h"
#include <mbstring.h>

void MakeUpper(STRING & str)
{
	LPTSTR pszBuffer = (LPTSTR)str.c_str();

#ifdef _UNICODE
	_wcsupr_s(pszBuffer, lstrlen(pszBuffer) + 1);
#else
	_strupr_s(pszBuffer, lstrlen(pszBuffer) + 1);
#endif
}

void MakeLower(STRING & str)
{
	LPTSTR pszBuffer = (LPTSTR)str.c_str();

#ifdef _UNICODE
	_wcslwr_s(pszBuffer, lstrlen(pszBuffer) + 1);
#else
	_strlwr_s(pszBuffer, lstrlen(pszBuffer) + 1);
#endif
}