#pragma once

#include "StlString.h"

class CResourceManager;

class CSkinResource
{
friend class CResourceManager;

public:
	CSkinResource(STRING & szKey);

	STRING & GetResourceKey();

protected:
	virtual ~CSkinResource(void);

	STRING m_szResourceKey;
	int m_nRefCount;

private:

	int AddRef();
	int Release();
};
