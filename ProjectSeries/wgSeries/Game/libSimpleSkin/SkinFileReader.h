#pragma once

#include "SkinWindow.h"
#include "SkinControl.h"
#include "SkinDraggableControl.h"
#include "SkinRectangleControl.h"
#include "SkinImageControl.h"
#include "SkinStaticControl.h"

class CSkinFileReader
{
public:
	CSkinFileReader(void);
	virtual ~CSkinFileReader(void);

	CSkinWindow * LoadSkin(LPCTSTR pszFileName, LPCTSTR pszSkinMode, HWND hTargetWindow);
	void ReleaseSkin();
	CSkinWindow * GetSkinWindow();
	CSkinWindow * SetCurrentMode(LPCTSTR pszMode);
	LPCTSTR GetCurrentMode();
	LPCTSTR GetBasePath();
	void GetFullFilePath(LPCTSTR pszFileName, STRING * pszResult);

protected:
	CSkinWindow * ApplySkinMode(XMLNS::IXMLDOMNodePtr spModeNode, HWND hTargetWindow);
	CSkinControl * CreateSkinControl(CSkinControl * pParent, XMLNS::IXMLDOMNodePtr spNode);

	CSkinWindow * m_pSkinWindow;
	STRING m_szBasePath;
	STRING m_szCurrentMode;
	STRING m_szCurrentSkinFile;
};
