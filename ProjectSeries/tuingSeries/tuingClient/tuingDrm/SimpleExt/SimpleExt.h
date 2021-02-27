

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 8.01.0622 */
/* at Tue Jan 19 12:14:07 2038
 */
/* Compiler settings for SimpleExt.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 8.01.0622 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */



/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 500
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif /* __RPCNDR_H_VERSION__ */

#ifndef COM_NO_WINDOWS_H
#include "windows.h"
#include "ole2.h"
#endif /*COM_NO_WINDOWS_H*/

#ifndef __SimpleExt_h__
#define __SimpleExt_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __ISimpleShlExt_FWD_DEFINED__
#define __ISimpleShlExt_FWD_DEFINED__
typedef interface ISimpleShlExt ISimpleShlExt;

#endif 	/* __ISimpleShlExt_FWD_DEFINED__ */


#ifndef __SimpleShlExt_FWD_DEFINED__
#define __SimpleShlExt_FWD_DEFINED__

#ifdef __cplusplus
typedef class SimpleShlExt SimpleShlExt;
#else
typedef struct SimpleShlExt SimpleShlExt;
#endif /* __cplusplus */

#endif 	/* __SimpleShlExt_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 


#ifndef __ISimpleShlExt_INTERFACE_DEFINED__
#define __ISimpleShlExt_INTERFACE_DEFINED__

/* interface ISimpleShlExt */
/* [unique][helpstring][dual][uuid][object] */ 


EXTERN_C const IID IID_ISimpleShlExt;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("5E2121ED-0300-11D4-8D3B-444553540000")
    ISimpleShlExt : public IDispatch
    {
    public:
    };
    
    
#else 	/* C style interface */

    typedef struct ISimpleShlExtVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            ISimpleShlExt * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            _COM_Outptr_  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            ISimpleShlExt * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            ISimpleShlExt * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            ISimpleShlExt * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            ISimpleShlExt * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            ISimpleShlExt * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            ISimpleShlExt * This,
            /* [annotation][in] */ 
            _In_  DISPID dispIdMember,
            /* [annotation][in] */ 
            _In_  REFIID riid,
            /* [annotation][in] */ 
            _In_  LCID lcid,
            /* [annotation][in] */ 
            _In_  WORD wFlags,
            /* [annotation][out][in] */ 
            _In_  DISPPARAMS *pDispParams,
            /* [annotation][out] */ 
            _Out_opt_  VARIANT *pVarResult,
            /* [annotation][out] */ 
            _Out_opt_  EXCEPINFO *pExcepInfo,
            /* [annotation][out] */ 
            _Out_opt_  UINT *puArgErr);
        
        END_INTERFACE
    } ISimpleShlExtVtbl;

    interface ISimpleShlExt
    {
        CONST_VTBL struct ISimpleShlExtVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define ISimpleShlExt_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define ISimpleShlExt_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define ISimpleShlExt_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define ISimpleShlExt_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define ISimpleShlExt_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define ISimpleShlExt_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define ISimpleShlExt_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __ISimpleShlExt_INTERFACE_DEFINED__ */



#ifndef __SIMPLEEXTLib_LIBRARY_DEFINED__
#define __SIMPLEEXTLib_LIBRARY_DEFINED__

/* library SIMPLEEXTLib */
/* [helpstring][version][uuid] */ 


EXTERN_C const IID LIBID_SIMPLEEXTLib;

EXTERN_C const CLSID CLSID_SimpleShlExt;

#ifdef __cplusplus

class DECLSPEC_UUID("5E2121EE-0300-11D4-8D3B-444553540000")
SimpleShlExt;
#endif
#endif /* __SIMPLEEXTLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


