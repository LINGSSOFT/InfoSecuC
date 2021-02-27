# Microsoft Developer Studio Project File - Name="DoPro" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Application" 0x0101

CFG=DoPro - Win32 Debug
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "DoPro.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "DoPro.mak" CFG="DoPro - Win32 Debug"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "DoPro - Win32 Release" (based on "Win32 (x86) Application")
!MESSAGE "DoPro - Win32 Debug" (based on "Win32 (x86) Application")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
MTL=midl.exe
RSC=rc.exe

!IF  "$(CFG)" == "DoPro - Win32 Release"

# PROP BASE Use_MFC 6
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "Release"
# PROP BASE Intermediate_Dir "Release"
# PROP BASE Target_Dir ""
# PROP Use_MFC 6
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "Release"
# PROP Intermediate_Dir "Release"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /MD /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_AFXDLL" /Yu"stdafx.h" /FD /c
# ADD CPP /nologo /MD /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_WINDOWS" /D "_AFXDLL" /D "_MBCS" /Yu"stdafx.h" /FD /c
# ADD BASE MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "NDEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x412 /d "NDEBUG" /d "_AFXDLL"
# ADD RSC /l 0x412 /d "NDEBUG" /d "_AFXDLL"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 /nologo /subsystem:windows /machine:I386
# ADD LINK32 /nologo /subsystem:windows /machine:I386

!ELSEIF  "$(CFG)" == "DoPro - Win32 Debug"

# PROP BASE Use_MFC 6
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir "Debug"
# PROP BASE Intermediate_Dir "Debug"
# PROP BASE Target_Dir ""
# PROP Use_MFC 6
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "Debug"
# PROP Intermediate_Dir "Debug"
# PROP Ignore_Export_Lib 0
# PROP Target_Dir ""
# ADD BASE CPP /nologo /MDd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_AFXDLL" /Yu"stdafx.h" /FD /GZ /c
# ADD CPP /nologo /MDd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_WINDOWS" /D "_AFXDLL" /D "_MBCS" /FR /Yu"stdafx.h" /FD /GZ /c
# ADD BASE MTL /nologo /D "_DEBUG" /mktyplib203 /win32
# ADD MTL /nologo /D "_DEBUG" /mktyplib203 /win32
# ADD BASE RSC /l 0x412 /d "_DEBUG" /d "_AFXDLL"
# ADD RSC /l 0x412 /d "_DEBUG" /d "_AFXDLL"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LINK32=link.exe
# ADD BASE LINK32 /nologo /subsystem:windows /debug /machine:I386 /pdbtype:sept
# ADD LINK32 /nologo /subsystem:windows /debug /machine:I386 /pdbtype:sept

!ENDIF 

# Begin Target

# Name "DoPro - Win32 Release"
# Name "DoPro - Win32 Debug"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=.\Lib\2DPushGraph.cpp
# End Source File
# Begin Source File

SOURCE=.\Lib\CpuUsage.cpp
# End Source File
# Begin Source File

SOURCE=.\DialogDetail.cpp
# End Source File
# Begin Source File

SOURCE=.\DialogError.cpp
# End Source File
# Begin Source File

SOURCE=.\DialogHelp.cpp
# End Source File
# Begin Source File

SOURCE=.\DialogUpdate.cpp
# End Source File
# Begin Source File

SOURCE=.\Lib\DoListCtrl.cpp
# End Source File
# Begin Source File

SOURCE=.\DoPro.cpp
# End Source File
# Begin Source File

SOURCE=.\DoPro.rc
# End Source File
# Begin Source File

SOURCE=.\DoProDoc.cpp
# End Source File
# Begin Source File

SOURCE=.\DoProView.cpp
# End Source File
# Begin Source File

SOURCE=.\Lib\FileVersionInfo.cpp
# End Source File
# Begin Source File

SOURCE=.\MainFrm.cpp
# End Source File
# Begin Source File

SOURCE=.\Splash.cpp
# End Source File
# Begin Source File

SOURCE=.\StdAfx.cpp
# ADD CPP /Yc"stdafx.h"
# End Source File
# Begin Source File

SOURCE=.\Lib\TrayIcon.cpp
# End Source File
# Begin Source File

SOURCE=.\Lib\Util.cpp
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=.\Lib\2DPushGraph.h
# End Source File
# Begin Source File

SOURCE=.\Lib\CpuUsage.h
# End Source File
# Begin Source File

SOURCE=.\DialogDetail.h
# End Source File
# Begin Source File

SOURCE=.\DialogError.h
# End Source File
# Begin Source File

SOURCE=.\DialogHelp.h
# End Source File
# Begin Source File

SOURCE=.\DialogUpdate.h
# End Source File
# Begin Source File

SOURCE=.\Lib\DoListCtrl.h
# End Source File
# Begin Source File

SOURCE=.\DoPro.h
# End Source File
# Begin Source File

SOURCE=.\DoProDoc.h
# End Source File
# Begin Source File

SOURCE=.\DoProView.h
# End Source File
# Begin Source File

SOURCE=.\Lib\FileVersionInfo.h
# End Source File
# Begin Source File

SOURCE=.\MainFrm.h
# End Source File
# Begin Source File

SOURCE=.\Lib\PerfCounters.h
# End Source File
# Begin Source File

SOURCE=.\Resource.h
# End Source File
# Begin Source File

SOURCE=.\Splash.h
# End Source File
# Begin Source File

SOURCE=.\StdAfx.h
# End Source File
# Begin Source File

SOURCE=.\Lib\TrayIcon.h
# End Source File
# Begin Source File

SOURCE=.\Lib\Util.h
# End Source File
# End Group
# Begin Group "Resource Files"

# PROP Default_Filter "ico;cur;bmp;dlg;rc2;rct;bin;rgs;gif;jpg;jpeg;jpe"
# Begin Source File

SOURCE=.\res\DoPro.ico
# End Source File
# Begin Source File

SOURCE=.\res\DoPro.rc2
# End Source File
# Begin Source File

SOURCE=.\res\DoProDoc.ico
# End Source File
# Begin Source File

SOURCE=.\res\Regcomputer.ico
# End Source File
# Begin Source File

SOURCE=.\res\Regexplorer.ico
# End Source File
# Begin Source File

SOURCE=.\res\service.ico
# End Source File
# Begin Source File

SOURCE=.\res\Splsh16.bmp
# End Source File
# Begin Source File

SOURCE=.\Splsh16.bmp
# End Source File
# Begin Source File

SOURCE=.\res\Tab1.ico
# End Source File
# Begin Source File

SOURCE=.\res\Tab2.ico
# End Source File
# Begin Source File

SOURCE=.\res\Tab3.ico
# End Source File
# Begin Source File

SOURCE=.\res\Tab4.ico
# End Source File
# Begin Source File

SOURCE=.\res\Tab5.ico
# End Source File
# Begin Source File

SOURCE=.\res\Tab6.ico
# End Source File
# Begin Source File

SOURCE=.\res\Toolbar.bmp
# End Source File
# Begin Source File

SOURCE=.\res\wupdate.ico
# End Source File
# Begin Source File

SOURCE=.\res\xptheme.bin
# End Source File
# End Group
# Begin Source File

SOURCE=.\ReadMe.txt
# End Source File
# End Target
# End Project
# Section DoPro : {72ADFD78-2C39-11D0-9903-00A0C91BC942}
# 	1:10:IDB_SPLASH:102
# 	2:21:SplashScreenInsertKey:4.0
# End Section
