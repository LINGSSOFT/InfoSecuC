// SimpleShlExt.h : Declaration of the CSimpleShlExt

#ifndef __SIMPLESHLEXT_H_
#define __SIMPLESHLEXT_H_

/////////////////////////////////////////////////////////////////////////////
// CSimpleShlExt

class ATL_NO_VTABLE CSimpleShlExt : 
    public CComObjectRootEx<CComSingleThreadModel>,
    public CComCoClass<CSimpleShlExt, &CLSID_SimpleShlExt>,
    public IShellExtInit,
    public IContextMenu
{
public:
    CSimpleShlExt();
	~CSimpleShlExt();
    DECLARE_REGISTRY_RESOURCEID(IDR_SIMPLESHLEXT)

    BEGIN_COM_MAP(CSimpleShlExt)
        COM_INTERFACE_ENTRY(IShellExtInit)
        COM_INTERFACE_ENTRY(IContextMenu)
    END_COM_MAP()

public:
    // IShellExtInit
    STDMETHODIMP Initialize(LPCITEMIDLIST, LPDATAOBJECT, HKEY);

    // IContextMenu
    STDMETHODIMP GetCommandString(UINT, UINT, UINT*, LPSTR, UINT);
    STDMETHODIMP InvokeCommand(LPCMINVOKECOMMANDINFO);
    STDMETHODIMP QueryContextMenu(HMENU, UINT, UINT, UINT, UINT);

protected:
	  HBITMAP     m_hRegBmp;
	  HBITMAP     m_hUnregBmp;
	  HBITMAP     m_hViewBmp;

    TCHAR m_szFile [MAX_PATH];
	int s_drm;
};

#endif //__SIMPLESHLEXT_H_
