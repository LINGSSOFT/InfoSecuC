// MyOverlayIcon.h : Declaration of the CMyOverlayIcon

#pragma once
#include "resource.h"       // main symbols
#include "OverlayIcon.h"

#include <comdef.h>
#include <shlobj.h>

// CMyOverlayIcon

class ATL_NO_VTABLE CMyOverlayIcon : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CMyOverlayIcon, &CLSID_MyOverlayIcon>,
	public IDispatchImpl<IMyOverlayIcon, &IID_IMyOverlayIcon, &LIBID_OverlayIconLib, /*wMajor =*/ 1, /*wMinor =*/ 0>,
	public IShellIconOverlayIdentifier
{
public:
	CMyOverlayIcon()
	{
	}

DECLARE_REGISTRY_RESOURCEID(IDR_MYOVERLAYICON)


BEGIN_COM_MAP(CMyOverlayIcon)
	COM_INTERFACE_ENTRY(IMyOverlayIcon)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(IShellIconOverlayIdentifier)
END_COM_MAP()


	DECLARE_PROTECT_FINAL_CONSTRUCT()

  // IShellIconOverlayIdentifier Methods
  STDMETHOD(GetOverlayInfo)(LPWSTR pwszIconFile,int cchMax,int *pIndex,DWORD* pdwFlags);
  STDMETHOD(GetPriority)(int* pPriority);
  STDMETHOD(IsMemberOf)(LPCWSTR pwszPath,DWORD dwAttrib);

	HRESULT FinalConstruct()
	{
		return S_OK;
	}
	
	void FinalRelease() 
	{
	}

public:

};

OBJECT_ENTRY_AUTO(__uuidof(MyOverlayIcon), CMyOverlayIcon)
