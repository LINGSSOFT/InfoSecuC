// DialogError.cpp : implementation file
//

#include "stdafx.h"
#include "DoPro.h"
#include "DialogError.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDialogError dialog


CDialogError::CDialogError(CWnd* pParent /*=NULL*/)
	: CDialog(CDialogError::IDD, pParent)
{
	//{{AFX_DATA_INIT(CDialogError)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CDialogError::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDialogError)
	DDX_Control(pDX, IDC_LIST_ERROR, m_ListError);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CDialogError, CDialog)
	//{{AFX_MSG_MAP(CDialogError)
	ON_BN_CLICKED(IDC_BUTTON_SEND, OnButtonSend)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDialogError message handlers

BOOL CDialogError::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// TODO: Add extra initialization here
	CString strLS;
	m_ListError.SetExtendedStyle(m_ListError.GetExtendedStyle() | LVS_EX_FULLROWSELECT | LVS_EX_FLATSB);	// LVS_EX_GRIDLINES |

	strLS.LoadString(IDS_STRING61462);
	m_ListError.SetHeadings(strLS);
	m_ListError.LoadColumnInfo();
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CDialogError::OnButtonSend() 
{
	// TODO: Add your control notification handler code here
	AfxMessageBox("준비중입니다");
}
