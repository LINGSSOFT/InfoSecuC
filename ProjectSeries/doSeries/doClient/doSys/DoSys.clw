; CLW file contains information for the MFC ClassWizard

[General Info]
Version=1
LastClass=CDoSysView
LastTemplate=CDialog
NewFileInclude1=#include "stdafx.h"
NewFileInclude2=#include "DoSys.h"
LastPage=0

ClassCount=9
Class1=CDoSysApp
Class2=CDoSysDoc
Class3=CDoSysView
Class4=CMainFrame

ResourceCount=7
Resource1=IDD_DIALOG_ERROR
Resource2=IDR_TRAY_MENU
Class5=CAboutDlg
Class6=CSplashWnd
Resource3=IDD_ABOUTBOX
Resource4=IDR_MAINFRAME
Resource5=IDD_DIALOG_UPDATE
Resource6=IDD_DOSYS_FORM
Class7=CDialogUpdate
Class8=CDialogHelp
Class9=CDialogError
Resource7=IDD_DIALOG_HELP

[CLS:CDoSysApp]
Type=0
HeaderFile=DoSys.h
ImplementationFile=DoSys.cpp
Filter=N
BaseClass=CWinApp
VirtualFilter=AC
LastObject=ID_VIEW_TOP

[CLS:CDoSysDoc]
Type=0
HeaderFile=DoSysDoc.h
ImplementationFile=DoSysDoc.cpp
Filter=N
LastObject=CDoSysDoc
BaseClass=CDocument
VirtualFilter=DC

[CLS:CDoSysView]
Type=0
HeaderFile=DoSysView.h
ImplementationFile=DoSysView.cpp
Filter=D
BaseClass=CFormView
VirtualFilter=VWC
LastObject=IDC_LIST_INSTALL


[CLS:CMainFrame]
Type=0
HeaderFile=MainFrm.h
ImplementationFile=MainFrm.cpp
Filter=T
BaseClass=CFrameWnd
VirtualFilter=fWC
LastObject=CMainFrame




[CLS:CAboutDlg]
Type=0
HeaderFile=DoSys.cpp
ImplementationFile=DoSys.cpp
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
Command8=ID_HELP_MANUAL
Command9=ID_HELP_ERROR
Command10=ID_HELP_UPDATE
Command11=ID_APP_ABOUT
CommandCount=11

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

[DLG:IDD_DOSYS_FORM]
Type=1
Class=CDoSysView
ControlCount=11
Control1=IDC_TAB_MAIN,SysTabControl32,1350565888
Control2=IDC_TREE_SYSTEM,SysTreeView32,1350631475
Control3=IDC_LIST_SYSTEM,SysListView32,1350631437
Control4=IDC_TREE_NETWORK,SysTreeView32,1350631475
Control5=IDC_LIST_NETWORK,SysListView32,1350631437
Control6=IDC_TREE_PACKET,SysTreeView32,1350631475
Control7=IDC_LIST_PACKET,SysListView32,1350631437
Control8=IDC_LIST_INSTALL,SysListView32,1350631453
Control9=IDC_LIST_SERVICE,SysListView32,1350631437
Control10=IDC_TREE_REGIST,SysTreeView32,1350631475
Control11=IDC_LIST_REGIST,SysListView32,1350631437

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

[DLG:IDD_DIALOG_ERROR]
Type=1
Class=CDialogError
ControlCount=3
Control1=IDOK,button,1342242817
Control2=IDC_BUTTON_SEND,button,1342242816
Control3=IDC_LIST_ERROR,SysListView32,1350631437

[DLG:IDD_DIALOG_HELP]
Type=1
Class=CDialogHelp
ControlCount=2
Control1=IDOK,button,1342242817
Control2=IDC_EDIT_HELP,edit,1352734724

[DLG:IDD_DIALOG_UPDATE]
Type=1
Class=CDialogUpdate
ControlCount=7
Control1=IDOK,button,1342242817
Control2=IDC_BUTTON_UPDATEDETAIL,button,1342242816
Control3=IDC_BUTTON_UPDATE,button,1342242816
Control4=IDC_STATIC,static,1342308352
Control5=IDC_STATIC,static,1342308352
Control6=IDC_EDIT_CURRENT,edit,1350633600
Control7=IDC_EDIT_NEW,edit,1350633600

[CLS:CDialogUpdate]
Type=0
HeaderFile=DialogUpdate.h
ImplementationFile=DialogUpdate.cpp
BaseClass=CDialog
Filter=D
LastObject=CDialogUpdate
VirtualFilter=dWC

[CLS:CDialogHelp]
Type=0
HeaderFile=DialogHelp.h
ImplementationFile=DialogHelp.cpp
BaseClass=CDialog
Filter=D
LastObject=CDialogHelp
VirtualFilter=dWC

[CLS:CDialogError]
Type=0
HeaderFile=DialogError.h
ImplementationFile=DialogError.cpp
BaseClass=CDialog
Filter=D
LastObject=IDC_BUTTON_SEND
VirtualFilter=dWC

