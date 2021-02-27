#pragma once

#include <tchar.h>
#include "ResourceManager.h"

#import "msxml.dll" named_guids
#define XMLNS		MSXML
//#import "msxml3.dll" named_guids
//#define XMLNS		MSXML2


#define ANK_LEFT			1
#define ANK_TOP				2
#define ANK_RIGHT			4
#define ANK_BOTTOM			8


#define SET_CLASS_NAME(name, baseclass) \
	virtual LPCTSTR GetClassName() \
	{ \
		return name; \
	} \
	virtual BOOL IsKindOf(LPCTSTR pszClassName) \
	{ \
		if(_tcscmp(name, pszClassName) == 0) \
			return TRUE; \
		return baseclass::IsKindOf(pszClassName); \
	}



class CSkinWindow;
class CSkinControl;
class CSkinFileReader;

class CSkinObject
{
public:
	virtual LPCTSTR GetClassName(){return _T("CSkinObject");}
	virtual BOOL IsKindOf(LPCTSTR pszClassName){return _tcscmp(pszClassName, _T("CSkinObject")) == 0;}
};

class ISkinControlEvents
{
public:
	virtual LRESULT OnSkinControlMessage(CSkinControl * pControl, UINT uMsg, WPARAM wParam, LPARAM lParam) = 0;
};

class CSkinControl : public CSkinObject
{
public:
	CSkinControl(void);
	virtual ~CSkinControl(void);

	SET_CLASS_NAME(_T("CSkinControl"), CSkinObject)

	void SetName(LPCTSTR pszName);
	LPCTSTR GetName();

	void SetAutoDelete(BOOL bAutoDelete);
	BOOL GetAutoDelete();

	int GetChildCount();
	void AddChildControl(CSkinControl * pControl);
	void InsertChildControl(CSkinControl * pPrevControl, CSkinControl * pControl);
	void RemoveChildControl(CSkinControl * pControl);
	CSkinControl * GetFirstChild();
	CSkinControl * GetLastChild();
	CSkinControl * GetNextSibling();
	CSkinControl * GetPrevSibling();
	CSkinControl * GetParent();
	CSkinWindow * GetParentWindow();
	CSkinControl * GetNextControl(CSkinControl * pControl);
	CSkinControl * GetControlFromPoint(POINT pt, BOOL bEnableOnly);
	CSkinControl * FindControl(LPCTSTR pszName, CSkinControl * pAfter = NULL);

	void SetControlRect(const RECT * prt);
	void GetControlRect(RECT * prt);
	void GetAbsoluteRect(RECT * prt);
	void ApplyAnchors(int dx, int dy);
	int GetAnchors();
	void SetAnchors(int nAnchors);
	BOOL IsWindowControl();

	CSkinFileReader * GetSkinFileReader();
	void SetSkinFileReader(CSkinFileReader * pReader);

	void * GetData();
	void SetData(void * pData);

	void SetEvents(ISkinControlEvents * pEvents);
	void Invalidate();

	virtual void DrawControl(HDC hDC);
	virtual LRESULT NotifyMessage(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL & bHandled);
	virtual void OnMouseEnter(){}
	virtual void OnMouseLeave(){}
	virtual void OnParentWindowAttached(CSkinWindow * pWindow){}

	BOOL GetEnabled();
	void SetEnabled(BOOL bEnabled);
	BOOL GetVisible();
	void SetVisible(BOOL bVisible);

	void SetToolTip(LPCTSTR pszToolTip);
	LPCTSTR GetToolTip();

	virtual BOOL ApplyXML(XMLNS::IXMLDOMNodePtr spNode);

	static BOOL StringToAnchors(LPCTSTR pszAnchors, int * pValue);
	static BOOL StringToBOOL(LPCTSTR pszBool, BOOL * pValue);
	static BOOL StringToInt(LPCTSTR pszInt, int * pValue);
	static BOOL StringToColor(LPCTSTR pszColor, COLORREF * pValue);
	static BOOL StringToFontStyle(LPCTSTR pszStyle, DWORD * pValue);
	static BOOL StringToDTFlag(LPCTSTR pszFlag, DWORD * pValue);
	static BOOL StringToRect(LPCTSTR pszRect, RECT * pValue);
	static int AnchorsToDirection(int nDirection);

protected:

	void RemoveAllChildren();

	RECT m_rtControl;
	int m_nAnchors;
	void * m_pData;
	TCHAR * m_pszName;
	
	int m_nChildCount;
	CSkinControl * m_pParent;
	CSkinControl * m_pFirstChild;
	CSkinControl * m_pLastChild;
	CSkinControl * m_pNextSibling;
	CSkinControl * m_pPrevSibling;
	BOOL m_bAutoDelete;
	BOOL m_bEnabled;
	ISkinControlEvents * m_pEvents;
	TCHAR * m_pszToolTip;
	CSkinFileReader * m_pSkinFileReader;
	BOOL m_bVisible;
};
