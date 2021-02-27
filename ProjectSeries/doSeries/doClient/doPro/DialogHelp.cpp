// DialogHelp.cpp : implementation file
//

#include "stdafx.h"
#include "DoPro.h"
#include "DialogHelp.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDialogHelp dialog


CDialogHelp::CDialogHelp(CWnd* pParent /*=NULL*/)
	: CDialog(CDialogHelp::IDD, pParent)
{
	//{{AFX_DATA_INIT(CDialogHelp)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CDialogHelp::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDialogHelp)
	DDX_Control(pDX, IDC_EDIT_HELP, m_edHelp);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CDialogHelp, CDialog)
	//{{AFX_MSG_MAP(CDialogHelp)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDialogHelp message handlers

BOOL CDialogHelp::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// TODO: Add extra initialization here
	CString strLS;
	strLS.LoadString(IDS_STRING61447);
	m_edHelp.SetWindowText(strLS);

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}
