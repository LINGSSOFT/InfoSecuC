#include "stdafx.h"
#include "StlString.h"
#include <mbstring.h>

void MakeUpper(STRING & str)
{
	LPTSTR pszBuffer = (LPTSTR)str.c_str();

#ifdef _UNICODE
	_wcsupr(pszBuffer);
#else
	_strupr(pszBuffer);
#endif
}

void MakeLower(STRING & str)
{
	LPTSTR pszBuffer = (LPTSTR)str.c_str();

#ifdef _UNICODE
	_wcslwr(pszBuffer);
#else
	_strlwr(pszBuffer);
#endif
}