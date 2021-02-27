#pragma once

// The following macros define the minimum required platform.  The minimum required platform
// is the earliest version of Windows, Internet Explorer etc. that has the necessary features to run 
// your application.  The macros work by enabling all features available on platform versions up to and 
// including the version specified.

// Modify the following defines if you have to target a platform prior to the ones specified below.
// Refer to MSDN for the latest info on corresponding values for different platforms.
#ifndef _WIN32_WINNT            // Specifies that the minimum required platform is Windows Vista.
#define _WIN32_WINNT 0x0600     // Change this to the appropriate value to target other versions of Windows.
#endif

#if defined (_MSC_VER) && (_MSC_VER <= 1200)

#pragma warning( disable: 4786 )

#define _tcscpy_s(dst, size, src)			_tcscpy(dst, src)
#define _tcstok_s(token, deli, context)		_tcstok(token, deli)
#define wcscpy_s(dst, size, src)			wcscpy(dst, src)
#define wcscat_s(dst, size, src)			wcscat(dst, src)
#define swprintf_s(str, size, fmt, param1)	swprintf(str, fmt, param1)
#define _stprintf_s(str, size, fmt, param1, param2, param3)		_stprintf(str, fmt, param1, param2, param3)

#endif