#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
//XOR256Stream.cpp

#include "stdafx.h"
#include "XOR256Stream.h"
#include "SHA.h"
#include "DoubleBuffering.h"

#include <exception>
#include <strstream>

using namespace std;

char const* CXOR256Stream::sm_szErrorMsgXS1 = "Illegal Internal Rounds!";

//CONSTRUCTOR
CXOR256Stream::CXOR256Stream() : m_iRounds(-1), m_pucSeqX(NULL), m_pucSeqM(NULL)
{
	m_blockSize = -1;
	m_keylength = KEY_MAX;
}

//DESTRUCTOR
CXOR256Stream::~CXOR256Stream()
{
	if(m_pucSeqX != NULL)
		delete [] m_pucSeqX;
	if(m_pucSeqM != NULL)
		delete [] m_pucSeqM;
}

void CXOR256Stream::Initialize(char const* keydata, int keydatalength, int iRounds)
{
	//Check Initialization Data
	if(NULL == keydata)
		throw runtime_error(string(sm_szErrorMsg4));
	if(keydatalength<1)
		throw runtime_error(string(sm_szErrorMsg5));
	if(iRounds<1)
		throw runtime_error(string(sm_szErrorMsgXS1));
	if(m_iRounds != iRounds)
	{
		m_iRounds = iRounds;
		m_iRounds1 = m_iRounds-1;
		if(m_pucSeqX != NULL)
			delete [] m_pucSeqX;
		if(m_pucSeqM != NULL)
			delete [] m_pucSeqM;
		m_pucSeqX = new unsigned char[m_iRounds];
		m_pucSeqM = new unsigned char[m_iRounds];
	}
	//Create the Key from Key Data
	int i, j;
	char key[KEY_MAX];
	for(i=0,j=0; i<m_keylength; i++,j=(j+1)%keydatalength)
		key[i] = keydata[j];
	bool bSameKey = false;
	if(true == m_bInit)
	{
		//Check the Key if is the same
		if(0 == memcmp(m_apKey.get(), key, m_keylength))
			bSameKey = true;
	}
	if(true == bSameKey)
	{
		//Fast Initialization
		m_oArcfourPRNG.Reset();
		return;
	}
	m_apKey = auto_ptr<char>(new char[m_keylength]);
	memcpy(m_apKey.get(), key, m_keylength);
	m_oArcfourPRNG.SetKey((unsigned char*)key, m_keylength);
	m_ucPrev0 = m_oArcfourPRNG.Rand();
	m_ucPrev = m_ucPrev0;
	//Initialization Flag
	m_bInit = true;
}

void CXOR256Stream::ResetChain()
{
	if(false==m_bInit)
		throw runtime_error(string(sm_szErrorMsg1));
	m_oArcfourPRNG.Reset();
	m_oArcfourPRNG.Rand();
	m_ucPrev = m_ucPrev0;
}



//Compute Signature
void CXOR256Stream::Signature(char* pcSig)
{
	//12+256+3+1
	char acSigData[273] = {0};
	strcat(acSigData, "XOR256STREAM");
	int iLen = strlen(acSigData);
	memcpy(acSigData+iLen, m_apKey.get(), m_keylength);
	sprintf(acSigData+iLen+m_keylength, "%d", m_iRounds);
	CSHA oSHA;
	oSHA.AddData(acSigData, strlen(acSigData));
	oSHA.FinalDigest(pcSig);
}

//Encryption for a string of chars
void CXOR256Stream::Encrypt(char const* in, char* result, size_t n)
{
	if(false==m_bInit)
		throw runtime_error(string(sm_szErrorMsg1));
	//n should be > 0
	if(n<1)
		throw runtime_error(string(sm_szErrorMsg6));
	unsigned char ucCipher;
	for(int i=0; i<n; i++,in++,result++)
	{
		//The first round
		m_ucPrev ^= *in;
		ucCipher = (m_ucPrev ^ m_oArcfourPRNG.Rand()) + m_oArcfourPRNG.Rand();
		//The last m_iRounds-1 rounds
		for(int j=1; j<m_iRounds; j++)
		{
			ucCipher ^= m_oArcfourPRNG.Rand();
			ucCipher += m_oArcfourPRNG.Rand();
		}
		*result = ucCipher;
	}
}

//Decryption for a string of chars
void CXOR256Stream::Decrypt(char const* in, char* result, size_t n)
{
	if(false==m_bInit)
		throw runtime_error(string(sm_szErrorMsg1));
	//n should be > 0
	if(n<1)
		throw runtime_error(string(sm_szErrorMsg6));
	unsigned char* pucSeqX;
	unsigned char* pucSeqM;
	unsigned char ucPlain;
	int i, j;
	for(i=0; i<n; i++,in++,result++)
	{
		//Calculate the constants
		pucSeqX = m_pucSeqX;
		pucSeqM = m_pucSeqM;
		for(j=0; j<m_iRounds; j++)
		{
			*(pucSeqX++) = m_oArcfourPRNG.Rand();
			*(pucSeqM++) = m_oArcfourPRNG.Rand();
		}
		//The last m_iRounds-1 rounds
		ucPlain = *in;
		pucSeqX = m_pucSeqX + m_iRounds1;
		pucSeqM = m_pucSeqM + m_iRounds1;
		for(j=m_iRounds-1; j>0; j--,pucSeqX--,pucSeqM--)
		{
			if(*pucSeqM <= ucPlain)
				ucPlain -= *pucSeqM;
			else
				(ucPlain += ~*pucSeqM)++;
			ucPlain ^= *pucSeqX;
		}
		//First round
		if(*m_pucSeqM <= ucPlain)
			ucPlain -= *m_pucSeqM;
		else
			(ucPlain += ~*m_pucSeqM)++;
		ucPlain ^= m_ucPrev ^ *m_pucSeqX;
		*result = ucPlain;
		m_ucPrev ^= ucPlain;
	}
}

void CXOR256Stream::EncryptFile(string const& rostrFileIn, string const& rostrFileOut)
{
	if(false==m_bInit)
		throw runtime_error(string(sm_szErrorMsg1));
	//Check if the same file for input and output
	if(rostrFileIn == rostrFileOut)
	{
		ostrstream ostr;
		ostr << sm_szErrorMsg8 << rostrFileIn << "!" << ends;
		string ostrMsg = ostr.str();
		ostr.freeze(false);
		throw runtime_error(ostrMsg);
	}
	//Open Input File
	ifstream in(rostrFileIn.c_str(), ios::binary);
	if(!in)
	{
		ostrstream ostr;
		ostr << sm_szErrorMsg7 << rostrFileIn << "!" << ends;
		string ostrMsg = ostr.str();
		ostr.freeze(false);
		throw runtime_error(ostrMsg);
	}
	//Open Output File
	ofstream out(rostrFileOut.c_str(), ios::binary);
	if(!out)
	{
		ostrstream ostr;
		ostr << sm_szErrorMsg7 << rostrFileOut << "!" << ends;
		string ostrMsg = ostr.str();
		ostr.freeze(false);
		throw runtime_error(ostrMsg);
	}
	//Computing the signature
	char acSig[32];
	Signature(acSig);
	//Writing the Signature
	out.write(m_Authority, 14);
	out.write(acSig, 32);
	//Resetting the chain
	ResetChain();
	//Reading from file
	char szLargeBuff[BUFF_LEN+1] = {0};
	char szBuffIn[DATA_LEN+1] = {0};
	char szBuffOut[DATA_LEN+1] = {0};
	CDoubleBuffering oDoubleBuffering(in, szLargeBuff, BUFF_LEN, DATA_LEN);
	int iRead;
	while((iRead=oDoubleBuffering.GetData(szBuffIn)) > 0)
	{
		//Encrypting
		Encrypt(szBuffIn, szBuffOut, iRead);
		out.write(szBuffOut, iRead);
	}
	in.close();
	out.close();
}

void CXOR256Stream::DecryptFile(string const& rostrFileIn, string const& rostrFileOut)
{
	if(false==m_bInit)
		throw runtime_error(string(sm_szErrorMsg1));
	//Check if the same file for input and output
	if(rostrFileIn == rostrFileOut)
	{
		ostrstream ostr;
		ostr << sm_szErrorMsg8 << rostrFileIn << "!" << ends;
		string ostrMsg = ostr.str();
		ostr.freeze(false);
		throw runtime_error(ostrMsg);
	}
	//Open Input File
	ifstream in(rostrFileIn.c_str(), ios::binary);
	if(!in)
	{
		ostrstream ostr;
		ostr << sm_szErrorMsg7 << rostrFileIn << "!" << ends;
		string ostrMsg = ostr.str();
		ostr.freeze(false);
		throw runtime_error(ostrMsg);
	}
	//Open Output File
	ofstream out(rostrFileOut.c_str(), ios::binary);
	if(!out)
	{
		ostrstream ostr;
		ostr << sm_szErrorMsg7 << rostrFileOut << "!" << ends;
		string ostrMsg = ostr.str();
		ostr.freeze(false);
		throw runtime_error(ostrMsg);
	}
	//Computing the signature
	char acSig[32];
	Signature(acSig);
	char acSig1[32];
	//Reading the Signature
	in.read(m_Authority, 14);
	OutputDebugString(m_Authority);
	in.read(acSig1, 32);
	//Compare the signatures
	if(memcmp(acSig1, acSig, 32) != 0)
	{
		ostrstream ostr;
		ostr << sm_szErrorMsg9 << rostrFileIn << sm_szErrorMsg10 << ends;
		string ostrMsg = ostr.str();
		ostr.freeze(false);
		throw runtime_error(ostrMsg);
	}
	//Resetting the chain
	ResetChain();
	//Reading from file
	char szLargeBuff[BUFF_LEN+1] = {0};
	char szBuffIn[DATA_LEN+1] = {0};
	char szBuffOut[DATA_LEN+1] = {0};
	CDoubleBuffering oDoubleBuffering(in, szLargeBuff, BUFF_LEN, DATA_LEN);
	int iRead;
	while((iRead=oDoubleBuffering.GetData(szBuffIn)) > 0)
	{
		//Encrypting
		Decrypt(szBuffIn, szBuffOut, iRead);
		out.write(szBuffOut, iRead);
	}
	in.close();
	out.close();
}

