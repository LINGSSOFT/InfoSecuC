#include "StdAfx.h"
#include "ResourceManager.h"

CResourceManager::CResourceManager(void)
{
}

CResourceManager::~CResourceManager(void)
{
}

CSkinResource * CResourceManager::FindSkinResource(STRING & szResourceKey)
{
	CResourceMap::iterator it = m_mapResource.find(szResourceKey);
	if(it == m_mapResource.end())
		return NULL;
	return it->second;
}

CSkinImageMap * CResourceManager::QueryImageMap(LPCTSTR pszFileName)
{
	STRING szKey;
	CSkinImageMap::GenerateResourceKey(pszFileName, szKey);

	CSkinResource * pResource = FindSkinResource(szKey);
	if(pResource)
	{
		pResource->AddRef();
		return (CSkinImageMap *)pResource;
	}

	CSkinImageMap * pImageMap = new CSkinImageMap(szKey, pszFileName);
	if(pImageMap->GetHandle() == NULL)
	{
		pImageMap->Release();
		return NULL;
	}
	m_mapResource[szKey] = pImageMap;
	return pImageMap;
}

CSkinFont * CResourceManager::QueryFont(LPCTSTR pszFontName, int nFontSize, DWORD dwFontStyle)
{
	STRING szKey;
	CSkinFont::GenerateResourceKey(pszFontName, nFontSize, dwFontStyle, szKey);

	CSkinResource * pResource = FindSkinResource(szKey);
	if(pResource)
	{
		pResource->AddRef();
		return (CSkinFont *)pResource;
	}

	CSkinFont * pFont = new CSkinFont(szKey, pszFontName, nFontSize, dwFontStyle);
	if(pFont->GetHandle() == NULL)
	{
		pFont->Release();
		return NULL;
	}
	m_mapResource[szKey] = pFont;
	return pFont;
}

void CResourceManager::AddRefResource(CSkinResource * pResource)
{
	pResource->AddRef();
}

void CResourceManager::ReleaseResource(CSkinResource * pResource)
{
	if(pResource->m_nRefCount == 1)
	{
		CResourceMap::iterator it = m_mapResource.find(pResource->GetResourceKey());
		if(it != m_mapResource.end())
			m_mapResource.erase(it);
	}
	pResource->Release();
}

