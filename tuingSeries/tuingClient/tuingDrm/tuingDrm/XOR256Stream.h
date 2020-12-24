
//XOR256Stream.h

#ifndef __XOR256STREAM_H__
#define __XOR256STREAM_H__

//Typical DISCLAIMER:
//The code in this project is Copyright (C) 2003 by George Anescu. You have the right to
//use and distribute the code in any way you see fit as long as this paragraph is included
//with the distribution. No warranties or claims are made as to the validity of the
//information and code contained herein, so use it at your own risk.

#include "ArcfourPRNG.h"
#include "Method.h"
#include <memory>

using namespace std;

class CXOR256Stream : public IMethod
{
private:
	enum { KEY_MAX = 256 };
	enum { BUFF_LEN=1024, DATA_LEN=384 };

public:
	//CONSTRUCTOR
	CXOR256Stream();
	//DESTRUCTOR
	~CXOR256Stream();
	//Expand a user-supplied key material into a session key.
	// keydata - The key material expanded to 256 bytes user-key to use.
	// keydatalength - how much to take from keydata
	// iRounds - the number of Internal Rounds
	// key length is fixed to 256
	void Initialize(char const* keydata, int keydatalength, int iRounds=4);
	//Resetting the Initialization Vector
	void ResetChain();
	//Encryption for a string of chars
	void Encrypt(char const* in, char* result, size_t n);
	//Decryption for a string of chars
	void Decrypt(char const* in, char* result, size_t n);
	//Encryption for a File
	void EncryptFile(string const& rostrFileIn, string const& rostrFileOut);
	//Decryption for a File
	void DecryptFile(string const& rostrFileIn, string const& rostrFileOut);


private:
	//Compute Signature
	void Signature(char* pcSig);
	//PRNG
	CArcfourPRNG m_oArcfourPRNG;
	//Key
	auto_ptr<char> m_apKey;
	//Number of Rounds
	int m_iRounds;
	int m_iRounds1; //m_iRounds-1
	unsigned char* m_pucSeqX;
	unsigned char* m_pucSeqM;
	unsigned char m_ucPrev0;
	unsigned char m_ucPrev;
	static char const* sm_szErrorMsgXS1;

	
	
};

#endif //__XOR256STREAM_H__

