# Microsoft Developer Studio Project File - Name="libSimpleSkin" - Package Owner=<4>
# Microsoft Developer Studio Generated Build File, Format Version 6.00
# ** DO NOT EDIT **

# TARGTYPE "Win32 (x86) Static Library" 0x0104

CFG=libSimpleSkin - Win32 Unicode Debug Static
!MESSAGE This is not a valid makefile. To build this project using NMAKE,
!MESSAGE use the Export Makefile command and run
!MESSAGE 
!MESSAGE NMAKE /f "libSimpleSkin.mak".
!MESSAGE 
!MESSAGE You can specify a configuration when running NMAKE
!MESSAGE by defining the macro CFG on the command line. For example:
!MESSAGE 
!MESSAGE NMAKE /f "libSimpleSkin.mak" CFG="libSimpleSkin - Win32 Unicode Debug Static"
!MESSAGE 
!MESSAGE Possible choices for configuration are:
!MESSAGE 
!MESSAGE "libSimpleSkin - Win32 Unicode Debug Static" (based on "Win32 (x86) Static Library")
!MESSAGE "libSimpleSkin - Win32 Unicode Release Static" (based on "Win32 (x86) Static Library")
!MESSAGE 

# Begin Project
# PROP AllowPerConfigDependencies 0
# PROP Scc_ProjName ""
# PROP Scc_LocalPath ""
CPP=cl.exe
RSC=rc.exe

!IF  "$(CFG)" == "libSimpleSkin - Win32 Unicode Debug Static"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 1
# PROP BASE Output_Dir "libSimpleSkin___Win32_Unicode_Debug_Static"
# PROP BASE Intermediate_Dir "libSimpleSkin___Win32_Unicode_Debug_Static"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 1
# PROP Output_Dir "libSimpleSkin___Win32_Unicode_Debug_Static"
# PROP Intermediate_Dir "libSimpleSkin___Win32_Unicode_Debug_Static"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /MDd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_MBCS" /D "_LIB" /Yu"stdafx.h" /FD /GZ /c
# ADD CPP /nologo /MTd /W3 /Gm /GX /ZI /Od /D "WIN32" /D "_DEBUG" /D "_UNICODE" /D "_LIB" /D "UNICODE" /Yu"stdafx.h" /FD /GZ /c
# ADD BASE RSC /l 0x412 /d "_DEBUG"
# ADD RSC /l 0x412 /d "_DEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo /out:"..\build\libSimpleSkinWDS.lib"

!ELSEIF  "$(CFG)" == "libSimpleSkin - Win32 Unicode Release Static"

# PROP BASE Use_MFC 0
# PROP BASE Use_Debug_Libraries 0
# PROP BASE Output_Dir "libSimpleSkin___Win32_Unicode_Release_Static"
# PROP BASE Intermediate_Dir "libSimpleSkin___Win32_Unicode_Release_Static"
# PROP BASE Target_Dir ""
# PROP Use_MFC 0
# PROP Use_Debug_Libraries 0
# PROP Output_Dir "libSimpleSkin___Win32_Unicode_Release_Static"
# PROP Intermediate_Dir "libSimpleSkin___Win32_Unicode_Release_Static"
# PROP Target_Dir ""
# ADD BASE CPP /nologo /MD /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_MBCS" /D "_LIB" /Yu"stdafx.h" /FD /c
# ADD CPP /nologo /MT /W3 /GX /O2 /D "WIN32" /D "NDEBUG" /D "_UNICODE" /D "_LIB" /D "UNICODE" /Yu"stdafx.h" /FD /c
# ADD BASE RSC /l 0x412 /d "NDEBUG"
# ADD RSC /l 0x412 /d "NDEBUG"
BSC32=bscmake.exe
# ADD BASE BSC32 /nologo
# ADD BSC32 /nologo
LIB32=link.exe -lib
# ADD BASE LIB32 /nologo
# ADD LIB32 /nologo /out:"..\DoTetris\libSimpleSkinWS.lib"

!ENDIF 

# Begin Target

# Name "libSimpleSkin - Win32 Unicode Debug Static"
# Name "libSimpleSkin - Win32 Unicode Release Static"
# Begin Group "Source Files"

# PROP Default_Filter "cpp;c;cxx;rc;def;r;odl;idl;hpj;bat"
# Begin Source File

SOURCE=.\ResourceManager.cpp
# End Source File
# Begin Source File

SOURCE=.\SkinControl.cpp
# End Source File
# Begin Source File

SOURCE=.\SkinDraggableControl.cpp
# End Source File
# Begin Source File

SOURCE=.\SkinFileReader.cpp
# End Source File
# Begin Source File

SOURCE=.\SkinFont.cpp
# End Source File
# Begin Source File

SOURCE=.\SkinImageControl.cpp
# End Source File
# Begin Source File

SOURCE=.\SkinImageMap.cpp
# End Source File
# Begin Source File

SOURCE=.\SkinRectangleControl.cpp
# End Source File
# Begin Source File

SOURCE=.\SkinResource.cpp
# End Source File
# Begin Source File

SOURCE=.\SkinStaticControl.cpp
# End Source File
# Begin Source File

SOURCE=.\SkinWindow.cpp
# End Source File
# Begin Source File

SOURCE=.\stdafx.cpp
# ADD CPP /Yc"stdafx.h"
# End Source File
# Begin Source File

SOURCE=.\StlString.cpp
# End Source File
# End Group
# Begin Group "Header Files"

# PROP Default_Filter "h;hpp;hxx;hm;inl"
# Begin Source File

SOURCE=.\ResourceManager.h
# End Source File
# Begin Source File

SOURCE=.\Singleton.h
# End Source File
# Begin Source File

SOURCE=.\SkinControl.h
# End Source File
# Begin Source File

SOURCE=.\SkinDraggableControl.h
# End Source File
# Begin Source File

SOURCE=.\SkinFileReader.h
# End Source File
# Begin Source File

SOURCE=.\SkinFont.h
# End Source File
# Begin Source File

SOURCE=.\SkinImageControl.h
# End Source File
# Begin Source File

SOURCE=.\SkinImageMap.h
# End Source File
# Begin Source File

SOURCE=.\SkinRectangleControl.h
# End Source File
# Begin Source File

SOURCE=.\SkinResource.h
# End Source File
# Begin Source File

SOURCE=.\SkinStaticControl.h
# End Source File
# Begin Source File

SOURCE=.\SkinWindow.h
# End Source File
# Begin Source File

SOURCE=.\StdAfx.h
# End Source File
# Begin Source File

SOURCE=.\StlString.h
# End Source File
# Begin Source File

SOURCE=.\targetver.h
# End Source File
# End Group
# Begin Source File

SOURCE=.\Readme.txt
# End Source File
# End Target
# End Project
