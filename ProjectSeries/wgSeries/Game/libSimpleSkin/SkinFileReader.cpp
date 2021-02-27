#include "StdAfx.h"
#include "SkinFileReader.h"

static int _nRefCount = 0;

CSkinFileReader::CSkinFileReader(void)
{
	m_pSkinWindow = NULL;
	_nRefCount++;
	if(_nRefCount == 1)
		CoInitialize(0);
}

CSkinFileReader::~CSkinFileReader(void)
{
	ReleaseSkin();

	_nRefCount--;
	if(_nRefCount == 0)
		CoUninitialize();
}

CSkinWindow * CSkinFileReader::LoadSkin(LPCTSTR pszFileName, LPCTSTR pszSkinMode, HWND hTargetWindow)
{
	if(m_pSkinWindow != NULL)
		return NULL;

	m_szBasePath = pszFileName;
	int nIndex = m_szBasePath.rfind('\\');
	if(nIndex == -1)
		return NULL;
	m_szBasePath = m_szBasePath.substr(0, nIndex);
	m_szCurrentSkinFile = pszFileName;
	m_szCurrentMode = pszSkinMode;

	XMLNS::IXMLDOMDocumentPtr spDoc;

	try
	{
		do
		{
			if(spDoc.CreateInstance(XMLNS::CLSID_DOMDocument) != S_OK)
				break;

			_bstr_t bsFileName = pszFileName;
			spDoc->put_preserveWhiteSpace(VARIANT_TRUE);
			if(spDoc->load(bsFileName) == VARIANT_FALSE)
				break;

			wchar_t szQuery[1024];
			_bstr_t bsSkinMode = pszSkinMode;
			
			swprintf_s(szQuery, 1024, L"mode[@name='%s']", (LPCWSTR)bsSkinMode);
			XMLNS::IXMLDOMNodePtr spNode = spDoc->documentElement->selectSingleNode(szQuery);
			if(spNode == NULL)
				break;

			m_pSkinWindow = ApplySkinMode(spNode, hTargetWindow);
		}while(FALSE);
	} catch(_com_error &)
	{
	}

	return m_pSkinWindow;
}

CSkinWindow * CSkinFileReader::GetSkinWindow()
{
	return m_pSkinWindow;
}

void CSkinFileReader::ReleaseSkin()
{
	if(m_pSkinWindow == NULL)
		return;

	delete m_pSkinWindow;
	m_pSkinWindow = NULL;
	m_szBasePath = _T("");
	m_szCurrentSkinFile = _T("");
	m_szCurrentMode = _T("");
}

CSkinWindow * CSkinFileReader::ApplySkinMode(XMLNS::IXMLDOMNodePtr spModeNode, HWND hTargetWindow)
{
	XMLNS::IXMLDOMNodePtr spWindowNode = spModeNode->selectSingleNode(L"window");
	if(spWindowNode == NULL)
		return NULL;

	CSkinWindow * pWindow = (CSkinWindow *)CreateSkinControl(NULL, spWindowNode);
	if(pWindow == NULL)
		return NULL;

	pWindow->Attach(hTargetWindow, FALSE);
	return pWindow;
}

CSkinControl * CSkinFileReader::CreateSkinControl(CSkinControl * pParent, XMLNS::IXMLDOMNodePtr spNode)
{
	_bstr_t bsNodeName = spNode->GetnodeName();

	CSkinControl * pRet = NULL;
	LPCSTR pszNodeName = bsNodeName;
	if(strcmp(pszNodeName, "control") == 0)
		pRet = new CSkinControl;
	else if(strcmp(pszNodeName, "draggable") == 0)
		pRet = new CSkinDraggableControl;
	else if(strcmp(pszNodeName, "rectangle") == 0)
		pRet = new CSkinRectangleControl;
	else if(strcmp(pszNodeName, "image") == 0)
		pRet = new CSkinImageControl;
	else if(strcmp(pszNodeName, "static") == 0)
		pRet = new CSkinStaticControl;
	else if(strcmp(pszNodeName, "window") == 0)
		pRet = new CSkinWindow;

	if(pRet == NULL)
		return NULL;

	pRet->SetSkinFileReader(this);
	if(!pRet->ApplyXML(spNode))
	{
		delete pRet;
		return NULL;
	}
	pRet->SetAutoDelete(TRUE);

	XMLNS::IXMLDOMNodePtr spChild = spNode->firstChild;
	while(spChild)
	{
		if(spChild->nodeType == XMLNS::NODE_ELEMENT)
		{
			if(!CreateSkinControl(pRet, spChild))
				return FALSE;
		}
		spChild = spChild->nextSibling;
	}

	if(pParent)
		pParent->AddChildControl(pRet);

	return pRet;
}

CSkinWindow * CSkinFileReader::SetCurrentMode(LPCTSTR pszMode)
{
	if(m_pSkinWindow == NULL)
		return FALSE;
	
	STRING szBasePath = m_szBasePath;
	STRING szSkinFile = m_szCurrentSkinFile;
	HWND hTargetWnd = m_pSkinWindow->GetHWND();

	SendMessage(hTargetWnd, WM_SETREDRAW, FALSE, 0);
	ReleaseSkin();
	CSkinWindow * pRet = LoadSkin(szSkinFile.c_str(), pszMode, hTargetWnd);
	SendMessage(hTargetWnd, WM_SETREDRAW, TRUE, 0);
	InvalidateRect(hTargetWnd, NULL, FALSE);
	return pRet;
}

LPCTSTR CSkinFileReader::GetCurrentMode()
{
	return m_szCurrentMode.c_str();
}

LPCTSTR CSkinFileReader::GetBasePath()
{
	return m_szBasePath.c_str();
}

void CSkinFileReader::GetFullFilePath(LPCTSTR pszFileName, STRING * pszResult)
{
	BOOL bRelative = (_tcschr(pszFileName, ':') == NULL);
	if(bRelative)
		*pszResult = m_szBasePath + _T("\\") + pszFileName;
	else
		*pszResult = pszFileName;
}