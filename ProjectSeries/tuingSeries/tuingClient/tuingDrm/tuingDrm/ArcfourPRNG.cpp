
//ArcfourPRNG.cpp

#include "stdafx.h"
#include "ArcfourPRNG.h"

#include <exception>
#include <cstring>

using namespace std;

char const* CArcfourPRNG::m_szMessage = "PRNG Not Initialized";

//CONSTRUCTOR
CArcfourPRNG::CArcfourPRNG() : m_bInit(false)
{
}

void CArcfourPRNG::SetKey(unsigned char *pucKeyData, int iKeyLen)
{
	if(iKeyLen < 1)
		throw exception("Key Length should be at least 1");
	int i;
	for(i=0; i<256; i++)
		m_aucState0[i] = i;
	m_ucI = 0;
	m_ucJ = 0;
	for(i=0; i<256; i++)
	{
		m_pucState1 = m_aucState0 + i;
		m_ucJ += *m_pucState1 + *(pucKeyData+m_ucI);
		m_pucState2 = m_aucState0 + m_ucJ;
		//Swaping
		m_ucTemp = *m_pucState1;
		*m_pucState1 = *m_pucState2;
		*m_pucState2 = m_ucTemp;
		m_ucI = (m_ucI + 1) % iKeyLen;
	}
	memcpy(m_aucState, m_aucState0, 256);
	//Initialize Indexes
	m_ucI = 0;
	m_ucJ = 0;
	//Initialization Finished
	m_bInit = true;
}

void CArcfourPRNG::Reset()
{
	if(false == m_bInit)
		throw exception(m_szMessage);
	memcpy(m_aucState, m_aucState0, 256);
	//Reinitialize Indexes
	m_ucI = 0;
	m_ucJ = 0;
}

unsigned char CArcfourPRNG::Rand()
{
	if(false == m_bInit)
		throw exception(m_szMessage);
	m_ucI++;
	m_pucState1 = m_aucState + m_ucI;
	m_ucJ += *m_pucState1;
	m_pucState2 = m_aucState + m_ucJ;
	//Swapping
	m_ucTemp = *m_pucState1;
	*m_pucState1 = *m_pucState2;
	*m_pucState2 = m_ucTemp;
	return m_aucState[(*m_pucState1 + *m_pucState2) & 0xFF];
}

