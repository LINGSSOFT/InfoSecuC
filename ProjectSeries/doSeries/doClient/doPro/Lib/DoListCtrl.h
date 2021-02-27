/*----------------------------------------------------------------------
Copyright (C)2009 Lings Entertainment. All Rights Reserved.
          This source may be used freely as long as it is not sold for
					profit and this copyright information is not altered or removed.
					Visit the web-site at http://www.lings.co.kr
					e-mail comments to jjang010@lings.co.kr
File:     DoListCtrl.h
Purpose:  Provides the header control, with drawing of the arrows, for
          the list control.
----------------------------------------------------------------------*/

#if !defined(AFX_DOLISTCTRL_H__42EB9BD7_E8E5_4E5A_8B2C_FF01718FF5E9__INCLUDED_)
#define AFX_DOLISTCTRL_H__42EB9BD7_E8E5_4E5A_8B2C_FF01718FF5E9__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifdef _DEBUG
	#define ASSERT_VALID_STRING( str ) ASSERT( !IsBadStringPtr( str, 0xfffff ) )
#else	//	_DEBUG
	#define ASSERT_VALID_STRING( str ) ( (void)0 )
#endif	//	_DEBUG

// DoListCtrl.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CDoHeaderCtrl window

class CDoHeaderCtrl : public CHeaderCtrl
{
// Construction
public:
	CDoHeaderCtrl();

// Attributes
public:

// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDoHeaderCtrl)
	public:
	virtual void Serialize(CArchive& ar);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CDoHeaderCtrl();

	void SetSortArrow( const int iColumn, const BOOL bAscending );

	// Generated message map functions
protected:
	void DrawItem( LPDRAWITEMSTRUCT lpDrawItemStruct );

	int m_iSortColumn;
	BOOL m_bSortAscending;

	//{{AFX_MSG(CDoHeaderCtrl)
		// NOTE - the ClassWizard will add and remove member functions here.
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////
// ListCtrl
class CDoListCtrl : public CListCtrl
{
// Construction
public:
	CDoListCtrl();

// Attributes
public:

// Operations
public:
	BOOL SetHeadings( UINT uiStringID );
	BOOL SetHeadings( const CString& strHeadings );

	int AddItem( int nImage, LPCTSTR pszText, ... );
	BOOL DeleteItem( int iItem );
	BOOL DeleteAllItems();
	void LoadColumnInfo();
	void SaveColumnInfo();
	BOOL SetItemText( int nItem, int nSubItem, LPCTSTR lpszText );
	void Sort( int iColumn, BOOL bAscending );
	BOOL SetItemData(int nItem, DWORD dwData);
	DWORD GetItemData(int nItem) const;

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CDoListCtrl)
	protected:
	virtual void PreSubclassWindow();
	//}}AFX_VIRTUAL

// Implementation
public:
	void Resize (int cx, int cy, int ntype);
	virtual ~CDoListCtrl();

	// Generated message map functions
protected:
	static int CALLBACK CompareFunction( LPARAM lParam1, LPARAM lParam2, LPARAM lParamData );
	void FreeItemMemory( const int iItem );
	BOOL CDoListCtrl::SetTextArray( int iItem, LPTSTR* arrpsz );
	LPTSTR* CDoListCtrl::GetTextArray( int iItem ) const;

	CDoHeaderCtrl m_ctlHeader;

	int m_iNumColumns;
	int m_iSortColumn;
	BOOL m_bSortAscending;

	//{{AFX_MSG(CSortListCtrl)
	afx_msg void OnColumnClick(NMHDR* pNMHDR, LRESULT* pResult);
	afx_msg void OnDestroy();
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_DOLISTCTRL_H__42EB9BD7_E8E5_4E5A_8B2C_FF01718FF5E9__INCLUDED_)
