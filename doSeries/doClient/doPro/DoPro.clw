; CLW file contains information for the MFC ClassWizard

[General Info]
Version=1
LastClass=CDoProApp
LastTemplate=CDialog
NewFileInclude1=#include "stdafx.h"
NewFileInclude2=#include "DoPro.h"
LastPage=0

ClassCount=10
Class1=CDoProApp
Class2=CDoProDoc
Class3=CDoProView
Class4=CMainFrame

ResourceCount=9
Resource1=IDD_DIALOG_HELP
Resource2=IDR_TRAY_MENU
Class5=CAboutDlg
Class6=CSplashWnd
Resource3=IDD_DIALOG_DETAIL
Resource4=IDR_MAINFRAME
Class7=CDialogDetail
Resource5=IDD_ABOUTBOX
Resource6=IDD_DOPRO_FORM
Class8=CDialogHelp
Resource7=IDD_DIALOG_ERROR
Class9=CDialogError
Resource8=IDR_CONTEXT_PRO
Class10=CDialogUpdate
Resource9=IDD_DIALOG_UPDATE

[CLS:CDoProApp]
Type=0
HeaderFile=DoPro.h
ImplementationFile=DoPro.cpp
Filter=N
BaseClass=CWinApp
VirtualFilter=AC
LastObject=CDoProApp

[CLS:CDoProDoc]
Type=0
HeaderFile=DoProDoc.h
ImplementationFile=DoProDoc.cpp
Filter=N
BaseClass=CDocument
VirtualFilter=DC
LastObject=ID_HELP_ERROR

[CLS:CDoProView]
Type=0
HeaderFile=DoProView.h
ImplementationFile=DoProView.cpp
Filter=D
BaseClass=CFormView
VirtualFilter=VWC
LastObject=CDoProView


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
HeaderFile=DoPro.cpp
ImplementationFile=DoPro.cpp
Filter=D
LastObject=CAboutDlg

[DLG:IDD_ABOUTBOX]
Type=1
Class=CAboutDlg
ControlCount=6
Control1=IDOK,button,1342373889
Control2=IDC_STATIC,static,1342177283
Control3=IDC_STATIC,static,1342308480
Control4=IDC_STATIC,static,1342308352
Control5=IDC_STATIC,static,1342308352
Control6=IDC_STATIC,static,1342308352

[MNU:IDR_MAINFRAME]
Type=1
Class=CMainFrame
Command1=ID_APP_LOGVIEW
Command2=ID_APP_LOGDEL
Command3=ID_APP_AUTO
Command4=ID_APP_EXIT
Command5=ID_VIEW_STATUS_BAR
Command6=ID_VIEW_TOP
Command7=ID_VIEW_REPLACE
Command8=ID_VIEW_REFRESH
Command9=ID_PWR_LOGOFF
Command10=ID_PWR_REBOOT
Command11=ID_PWR_OFF
Command12=ID_HELP_MANUAL
Command13=ID_HELP_ERROR
Command14=ID_HELP_UPDATE
Command15=ID_APP_ABOUT
CommandCount=15

[ACL:IDR_MAINFRAME]
Type=1
Class=CMainFrame
Command1=ID_FILE_NEW
Command2=ID_FILE_OPEN
Command3=ID_FILE_SAVE
Command4=ID_FILE_PRINT
Command5=ID_EDIT_UNDO
Command6=ID_EDIT_CUT
Command7=ID_EDIT_COPY
Command8=ID_EDIT_PASTE
Command9=ID_EDIT_UNDO
Command10=ID_EDIT_CUT
Command11=ID_EDIT_COPY
Command12=ID_EDIT_PASTE
Command13=ID_NEXT_PANE
Command14=ID_PREV_PANE
CommandCount=14

[DLG:IDD_DOPRO_FORM]
Type=1
Class=CDoProView
ControlCount=10
Control1=IDC_TAB_MAIN,SysTabControl32,1350565888
Control2=IDC_STATICCPU,static,1073741839
Control3=IDC_STATICMEMORY,static,1073741839
Control4=IDC_LIST_PROCESS,SysListView32,1350631453
Control5=IDC_LIST_LOG,SysListView32,1350631437
Control6=IDC_LIST_RUN,SysListView32,1350631453
Control7=IDC_LIST_SERVICE,SysListView32,1350631453
Control8=IDC_LIST_INSTALL,SysListView32,1350631453
Control9=IDC_LIST_REGIST,SysListView32,1350631453
Control10=IDC_TREE_REGIST,SysTreeView32,1350631475

[TB:IDR_MAINFRAME]
Type=1
Class=?
Command1=ID_FILE_NEW
Command2=ID_FILE_OPEN
Command3=ID_FILE_SAVE
Command4=ID_EDIT_CUT
Command5=ID_EDIT_COPY
Command6=ID_EDIT_PASTE
Command7=ID_FILE_PRINT
Command8=ID_APP_ABOUT
CommandCount=8

[CLS:CSplashWnd]
Type=0
HeaderFile=Splash.h
ImplementationFile=Splash.cpp
BaseClass=CWnd
LastObject=CSplashWnd

[MNU:IDR_CONTEXT_PRO]
Type=1
Class=?
Command1=ID_CONTEXT_KILL
Command2=ID_CONTEXT_DETAIL
CommandCount=2

[DLG:IDD_DIALOG_DETAIL]
Type=1
Class=CDialogDetail
ControlCount=13
Control1=IDCANCEL,button,1342242816
Control2=IDC_STATIC,button,1342177287
Control3=IDC_STATICIMAGE,static,1476395011
Control4=IDC_PIMAGENAME,edit,1342244992
Control5=IDC_PIMAGENAME2,edit,1342244992
Control6=IDC_STATIC,static,1342308352
Control7=IDC_STATIC,static,1342308352
Control8=IDC_STATIC,static,1342308352
Control9=IDC_STATIC,static,1342308352
Control10=IDC_PVERSION,edit,1342244992
Control11=IDC_PFULLPATH,edit,1342244868
Control12=IDC_PDESCRIPT,edit,1342244992
Control13=IDC_PCOMPANY,edit,1342244868

[CLS:CDialogDetail]
Type=0
HeaderFile=DialogDetail.h
ImplementationFile=DialogDetail.cpp
BaseClass=CDialog
Filter=D
VirtualFilter=dWC
LastObject=ID_APP_LOGDEL

[MNU:IDR_TRAY_MENU]
Type=1
Class=?
Command1=ID_TRAY_SHOW
Command2=ID_APP_EXIT
Command3=ID_APP_ABOUT
CommandCount=3

[DLG:IDD_DIALOG_HELP]
Type=1
Class=CDialogHelp
ControlCount=2
Control1=IDOK,button,1342242817
Control2=IDC_EDIT_HELP,edit,1352734724

[CLS:CDialogHelp]
Type=0
HeaderFile=DialogHelp.h
ImplementationFile=DialogHelp.cpp
BaseClass=CDialog
Filter=D
VirtualFilter=dWC
LastObject=IDC_EDIT_HELP

[DLG:IDD_DIALOG_ERROR]
Type=1
Class=CDialogError
ControlCount=3
Control1=IDOK,button,1342242817
Control2=IDC_LIST_ERROR,SysListView32,1350631437
Control3=IDC_BUTTON_SEND,button,1342242816

[CLS:CDialogError]
Type=0
HeaderFile=DialogError.h
ImplementationFile=DialogError.cpp
BaseClass=CDialog
Filter=D
VirtualFilter=dWC
LastObject=IDC_BUTTON_SEND

[DLG:IDD_DIALOG_UPDATE]
Type=1
Class=CDialogUpdate
ControlCount=7
Control1=IDOK,button,1342242817
Control2=IDC_STATIC,static,1342308352
Control3=IDC_STATIC,static,1342308352
Control4=IDC_EDIT_CURRENT,edit,1350633600
Control5=IDC_EDIT_NEW,edit,1350633600
Control6=IDC_BUTTON_UPDATE,button,1342242816
Control7=IDC_BUTTON_UPDATEDETAIL,button,1342242816

[CLS:CDialogUpdate]
Type=0
HeaderFile=DialogUpdate.h
ImplementationFile=DialogUpdate.cpp
BaseClass=CDialog
Filter=D
VirtualFilter=dWC
LastObject=IDC_BUTTON_UPDATEDETAIL

