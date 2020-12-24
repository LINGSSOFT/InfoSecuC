#include "StdAfx.h"
#include "SkinResource.h"

CSkinResource::CSkinResource(STRING & szKey)
{
	m_nRefCount = 1;
	m_szResourceKey = szKey;
}

CSkinResource::~CSkinResource(void)
{
}

STRING & CSkinResource::GetResourceKey()
{
	return m_szResourceKey;
}

int CSkinResource::AddRef()
{
	return ++m_nRefCount;
}

int CSkinResource::Release()
{
	int nRet = --m_nRefCount;
	if(nRet == 0)
		delete this;
	return nRet;
}
