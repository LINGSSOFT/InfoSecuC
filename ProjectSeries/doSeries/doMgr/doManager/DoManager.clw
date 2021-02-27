; CLW file contains information for the MFC ClassWizard

[General Info]
Version=1
LastClass=CDoManagerView
LastTemplate=CDialog
NewFileInclude1=#include "stdafx.h"
NewFileInclude2=#include "DoManager.h"
LastPage=0

ClassCount=6
Class1=CDoManagerApp
Class2=CDoManagerDoc
Class3=CDoManagerView
Class4=CMainFrame

ResourceCount=8
Resource1=IDR_TRAY_MENU
Resource2=IDD_DOMANAGER_FORM
Resource7=IDD_ABOUTBOX
Class5=CAboutDlg
Class6=CSplashWnd
Resource8=IDR_MAINFRAME

[CLS:CDoManagerApp]
Type=0
HeaderFile=DoManager.h
ImplementationFile=DoManager.cpp
Filter=N
BaseClass=CWinApp
VirtualFilter=AC
LastObject=ID_APP_AUTO

[CLS:CDoManagerDoc]
Type=0
HeaderFile=DoManagerDoc.h
ImplementationFile=DoManagerDoc.cpp
Filter=N
BaseClass=CDocument
VirtualFilter=DC
LastObject=ID_PWR_OFF

[CLS:CDoManagerView]
Type=0
HeaderFile=DoManagerView.h
ImplementationFile=DoManagerView.cpp
Filter=D
LastObject=IDC_TAB_MAIN
BaseClass=CFormView
VirtualFilter=VWC


[CLS:CMainFrame]
Type=0
HeaderFile=MainFrm.h
ImplementationFile=MainFrm.cpp
Filter=T
BaseClass=CFrameWnd
VirtualFilter=fWC
LastObject=ID_TRAY_SHOW




[CLS:CAboutDlg]
Type=0
HeaderFile=DoManager.cpp
ImplementationFile=DoManager.cpp
Filter=D

[DLG:IDD_ABOUTBOX]
Type=1
Class=CAboutDlg
ControlCount=6
Control1=IDC_STATIC,static,1342177283
Control2=IDC_STATIC,static,1342308480
Control3=IDC_STATIC,static,1342308352
Control4=IDOK,button,1342373889
Control5=IDC_STATIC,static,1342308352
Control6=IDC_STATIC,static,1342308352

[MNU:IDR_MAINFRAME]
Type=1
Class=CMainFrame
Command1=ID_APP_AUTO
Command2=ID_APP_EXIT
Command3=ID_VIEW_STATUS_BAR
Command4=ID_VIEW_TOP
Command5=ID_PWR_LOGOFF
Command6=ID_PWR_REBOOT
Command7=ID_PWR_OFF
Command8=ID_APP_ABOUT
CommandCount=8

[ACL:IDR_MAINFRAME]
Type=1
Class=CMainFrame
Command1=ID_FILE_NEW
Command2=ID_FILE_OPEN
Command3=ID_FILE_SAVE
Command4=ID_EDIT_UNDO
Command5=ID_EDIT_CUT
Command6=ID_EDIT_COPY
Command7=ID_EDIT_PASTE
Command8=ID_EDIT_UNDO
Command9=ID_EDIT_CUT
Command10=ID_EDIT_COPY
Command11=ID_EDIT_PASTE
Command12=ID_NEXT_PANE
Command13=ID_PREV_PANE
CommandCount=13

[DLG:IDD_DOMANAGER_FORM]
Type=1
Class=CDoManagerView
ControlCount=4
Control1=IDC_TAB_MAIN,SysTabControl32,1342177280
Control2=IDC_LIST_WEB,SysListView32,1350631437
Control3=IDC_LIST_FILE,SysListView32,1350631437
Control4=IDC_LIST_KEY,SysListView32,1350631437

[CLS:CSplashWnd]
Type=0
HeaderFile=Splash.h
ImplementationFile=Splash.cpp
BaseClass=CWnd

[MNU:IDR_TRAY_MENU]
Type=1
Class=?
Command1=ID_TRAY_SHOW
Command2=ID_APP_EXIT
Command3=ID_APP_ABOUT
CommandCount=3

[TB:IDR_MAINFRAME]
Type=1
Class=?
Command1=ID_APP_ABOUT
Command2=ID_APP_ABOUT
Command3=ID_APP_ABOUT
Command4=ID_APP_ABOUT
Command5=ID_APP_ABOUT
CommandCount=5

