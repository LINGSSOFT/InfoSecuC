#define _CRT_SECURE_NO_WARNINGS

// InfoSecuMFCPortDlg.cpp : implementation file
//

#include "pch.h"
#include "framework.h"
#include "InfoSecuMFCPort.h"
#include "InfoSecuMFCPortDlg.h"
#include "afxdialogex.h"
#include "sqlite3.h"

#pragma comment(lib, "sqlite3.lib")

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// SQLite는 UTF8을 사용하기 때문에 코드 변환이 필요합니다.
// 출처 - http://dolphin.ivyro.net/file/algorithm/SQLite/tutoria03.html
int AnsiToUTF8(char* szSrc, char* strDest, int destSize)
{
	WCHAR 	szUnicode[255];
	char 	szUTF8code[255];

	int nUnicodeSize = MultiByteToWideChar(CP_ACP, 0, szSrc, (int)strlen(szSrc), szUnicode, sizeof(szUnicode));
	int nUTF8codeSize = WideCharToMultiByte(CP_UTF8, 0, szUnicode, nUnicodeSize, szUTF8code, sizeof(szUTF8code), NULL, NULL);
	assert(destSize > nUTF8codeSize);
	memcpy(strDest, szUTF8code, nUTF8codeSize);
	strDest[nUTF8codeSize] = 0;
	return nUTF8codeSize;
}

int UTF8ToAnsi(char* szSrc, char* strDest, int destSize)
{
	WCHAR 	szUnicode[255];
	char 	szAnsi[255];

	int nSize = MultiByteToWideChar(CP_UTF8, 0, szSrc, -1, 0, 0);
	int nUnicodeSize = MultiByteToWideChar(CP_UTF8, 0, szSrc, -1, szUnicode, nSize);
	int nAnsiSize = WideCharToMultiByte(CP_ACP, 0, szUnicode, nUnicodeSize, szAnsi, sizeof(szAnsi), NULL, NULL);
	assert(destSize > nAnsiSize);
	memcpy(strDest, szAnsi, nAnsiSize);
	strDest[nAnsiSize] = 0;
	return nAnsiSize;
}



// CAboutDlg dialog used for App About
class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_ABOUTBOX };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

// Implementation
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(IDD_ABOUTBOX)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CInfoSecuMFCPortDlg dialog
CInfoSecuMFCPortDlg::CInfoSecuMFCPortDlg(CWnd* pParent /*=nullptr*/)
	: CDialogEx(IDD_INFOSECUMFCPORT_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CInfoSecuMFCPortDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_VIEW, m_list);
	DDX_Control(pDX, IDC_EDIT_NAME, m_name);
	DDX_Control(pDX, IDC_EDIT_TEL, m_tel);
	DDX_Control(pDX, IDOK, m_add);
	DDX_Control(pDX, IDCANCEL, m_remove);
}

BEGIN_MESSAGE_MAP(CInfoSecuMFCPortDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDOK, &CInfoSecuMFCPortDlg::OnBnClickedOk)
	ON_BN_CLICKED(IDCANCEL, &CInfoSecuMFCPortDlg::OnBnClickedCancel)
END_MESSAGE_MAP()


// CInfoSecuMFCPortDlg message handlers

BOOL CInfoSecuMFCPortDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// Add "About..." menu item to system menu.

	// IDM_ABOUTBOX must be in the system command range.
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != nullptr)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	m_list.SetExtendedStyle(LVS_EX_FULLROWSELECT | LVS_EX_GRIDLINES);

	// 리스트 컨트롤에 컬럼 이름 입력
	m_list.InsertColumn(0, L"이름");
	m_list.SetColumnWidth(0, 120);
	m_list.InsertColumn(1, L"전화번호");
	m_list.SetColumnWidth(1, 240);

	// 데이터베이스 파일 생성 및 열기
	sqlite3* db;
	sqlite3_stmt* stmt;
	char* errmsg = NULL;

	int rc = sqlite3_open("test.db", &db);

	if (rc != SQLITE_OK)
	{
		printf("Failed to open DB\n");
		sqlite3_close(db);
		exit(1);
	}


	//SQL 테이블 생성
	char* sql;
	sql = "CREATE TABLE IF NOT EXISTS DB("
		"ID INTEGER PRIMARY        KEY     AUTOINCREMENT,"
		"NAME          TEXT     NOT NULL,"
		"TEL           TEXT     NOT NULL);";

	rc = sqlite3_exec(db, sql, NULL, NULL, &errmsg);

	if (rc != SQLITE_OK)
	{
		printf("create table");
		sqlite3_free(errmsg);
		sqlite3_close(db);
		exit(1);
	}




	// 테이블을 읽어와 리스트 컨트롤에 보여주기
	sqlite3_prepare_v2(db, "select * from db", -1, &stmt, NULL);

	while (sqlite3_step(stmt) != SQLITE_DONE)
	{
		//int i;
		int num_cols = sqlite3_column_count(stmt);


		char szAnsi[300];
		UTF8ToAnsi((char*)sqlite3_column_text(stmt, 1), szAnsi, 300);
		CString name(szAnsi);

		UTF8ToAnsi((char*)sqlite3_column_text(stmt, 2), szAnsi, 300);
		CString tel(szAnsi);

		int nItem = m_list.InsertItem(0, name);
		m_list.SetItemText(nItem, 1, tel);

	}

	sqlite3_finalize(stmt);

	sqlite3_close(db);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CInfoSecuMFCPortDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CInfoSecuMFCPortDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CInfoSecuMFCPortDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CInfoSecuMFCPortDlg::OnBnClickedOk()
{
	// TODO: Add your control notification handler code here
	// 에디터 박스에 입력된 데이터를 리스트컨트롤에 입력합니다. 
	CString name;
	m_name.GetWindowText(name);

	CString tel;
	m_tel.GetWindowText(tel);


	int nItem = m_list.InsertItem(0, name);
	m_list.SetItemText(nItem, 1, tel);


	m_name.SetWindowTextW(L"");
	m_tel.SetWindowTextW(L"");



	sqlite3* db;
	int rc = sqlite3_open("test.db", &db);

	if (rc != SQLITE_OK)
	{
		printf("Failed to open DB\n");
		sqlite3_close(db);
		exit(1);
	}



	char* s_name;

	int sLen = WideCharToMultiByte(CP_ACP, 0, name, -1, NULL, 0, NULL, NULL);
	s_name = new char[sLen + 1];
	WideCharToMultiByte(CP_ACP, 0, name, -1, s_name, sLen, NULL, NULL);

	char szName[100];
	AnsiToUTF8(s_name, szName, 100);

	delete[]s_name;




	char* s_tel;

	sLen = WideCharToMultiByte(CP_ACP, 0, tel, -1, NULL, 0, NULL, NULL);
	s_tel = new char[sLen + 1];
	WideCharToMultiByte(CP_ACP, 0, tel, -1, s_tel, sLen, NULL, NULL);

	char szTel[100];
	AnsiToUTF8(s_tel, szTel, 100);

	delete[]s_tel;



	char* errmsg = NULL;
	char sql[255] = { 0 };
	sprintf(sql, "insert into db(name, tel) values('%s','%s');", szName, szTel);

	if (SQLITE_OK != sqlite3_exec(db, sql, NULL, NULL, &errmsg))
	{
		printf("insert");
	}

	sqlite3_close(db);


	//CDialogEx::OnOK();
}


void CInfoSecuMFCPortDlg::OnBnClickedCancel()
{
	// TODO: Add your control notification handler code here
	// 리스트 컨트롤에서 선택한 아이템을 제거합니다.
	int row = m_list.GetSelectionMark();
	CString name = m_list.GetItemText(row, 0);


	char* s_name;

	int sLen = WideCharToMultiByte(CP_ACP, 0, name, -1, NULL, 0, NULL, NULL);
	s_name = new char[sLen + 1];
	WideCharToMultiByte(CP_ACP, 0, name, -1, s_name, sLen, NULL, NULL);

	char szName[100];
	AnsiToUTF8(s_name, szName, 100);

	delete[]s_name;



	sqlite3* db;
	int rc = sqlite3_open("test.db", &db);

	if (rc != SQLITE_OK)
	{
		printf("Failed to open DB\n");
		sqlite3_close(db);
		exit(1);
	}


	char* errmsg = NULL;
	char sql[255] = { 0 };
	sprintf(sql, "delete from db where name = '%s';", szName);

	if (SQLITE_OK != sqlite3_exec(db, sql, NULL, NULL, &errmsg))
	{
		printf("delete");
	}



	sqlite3_close(db);


	m_list.DeleteItem(row);

	//CDialogEx::OnCancel();
}
