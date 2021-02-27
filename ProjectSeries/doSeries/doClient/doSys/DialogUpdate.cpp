// DialogUpdate.cpp : implementation file
//

#include "stdafx.h"
#include "DoSys.h"
#include "DialogUpdate.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDialogUpdate dialog


CDialogUpdate::CDialogUpdate(CWnd* pParent /*=NULL*/)
	: CDialog(CDialogUpdate::IDD, pParent)
{
	//{{AFX_DATA_INIT(CDialogUpdate)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
}


void CDialogUpdate::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDialogUpdate)
	DDX_Control(pDX, IDC_EDIT_NEW, m_edNew);
	DDX_Control(pDX, IDC_EDIT_CURRENT, m_edCurr);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CDialogUpdate, CDialog)
	//{{AFX_MSG_MAP(CDialogUpdate)
	ON_BN_CLICKED(IDC_BUTTON_UPDATE, OnButtonUpdate)
	ON_BN_CLICKED(IDC_BUTTON_UPDATEDETAIL, OnButtonUpdatedetail)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDialogUpdate message handlers

BOOL CDialogUpdate::OnInitDialog() 
{
	CDialog::OnInitDialog();
	
	// TODO: Add extra initialization here
	m_edCurr.SetWindowText("1.0.0.1");
	m_edNew.SetWindowText("1.0.0.1");
	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CDialogUpdate::OnButtonUpdate() 
{
	// TODO: Add your control notification handler code here
	AfxMessageBox("준비중입니다.");
}

void CDialogUpdate::OnButtonUpdatedetail() 
{
	// TODO: Add your control notification handler code here
	AfxMessageBox("준비중입니다.");
}
