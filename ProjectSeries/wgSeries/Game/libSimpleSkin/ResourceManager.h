#pragma once

#include "Singleton.h"
#include "SkinFont.h"
#include "SkinImageMap.h"
#include <map>
#pragma warning( disable: 4786 )

// GDI Resource Manager
class CResourceManager
{
public:
	CResourceManager(void);
	virtual ~CResourceManager(void);

	CSkinImageMap * QueryImageMap(LPCTSTR pszFileName);
	CSkinFont * QueryFont(LPCTSTR pszFontName, int nFontSize, DWORD dwFontStyle);

	void AddRefResource(CSkinResource * pResource);
	void ReleaseResource(CSkinResource * pResource);

protected:
	CSkinResource * FindSkinResource(STRING & szResourceKey);

	typedef std::map<STRING, CSkinResource *> CResourceMap;
	CResourceMap m_mapResource;
};


#define sg_pResourceMngr		(Singleton<CResourceManager>::Instance())