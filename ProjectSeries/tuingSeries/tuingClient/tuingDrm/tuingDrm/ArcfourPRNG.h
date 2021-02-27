
//ArcfourPRNG.h

#ifndef __ARCFOURPRNG_H__
#define __ARCFOURPRNG_H__

//This class implements an algorithm here called Arcfour that is 
//believed to be fully interoperable with the RC4 algoritm. RC4 
//is trademark of RSA Data Security, Inc.
//
//          Description of Algorithm
//
//          1 Key Setup
//
//          1. Allocate an 256 element array of 8 bit bytes to be used as an 
//             S-box, label it
//
//               S[0] .. S[255].
//
//          2. Initialize the S-box.  Fill each entry first with it's index:
//
//               S[0] = 0; S[1] = 1; etc. up to S[255] = 255;
//
//          3. Set j to zero and initialize the S-box with the key like this, repeating 
//             key bytes as necessary.:
//
//               for(i = 0; i &lt; 256; i = i + 1)
//               {
//                 j = (j + S [i] + key[i % keylen]) % 256;
//                 temp = S[i];
//                 S[i] = S[j];
//                 S[j] = temp;
//               }
//
//          4. Initialize i and j to zero.
//
//          2 Random Byte Generation
//
//          A pseudorandom byte K is generated:
//
//               i = (i+1) % 256;
//               j = (j + S[i]) % 256;
//               temp = S[i];
//               S[i] = S[j];
//               S[j] = temp;
//               t = (S[i] + S[j]) % 256;
//               K = S[t];
//

class CArcfourPRNG
{
public:
	//CONSTRUCTOR
	CArcfourPRNG();
	void SetKey(unsigned char *pucKeyData, int iKeyLen);
	void Reset();
	unsigned char Rand();
	
private:
	bool m_bInit;
	unsigned char m_aucState0[256];
	unsigned char m_aucState[256];
	unsigned char m_ucI;
	unsigned char m_ucJ;
	unsigned char* m_pucState1;
	unsigned char* m_pucState2;
	//To keep temporary value
	unsigned char m_ucTemp;
	static char const* m_szMessage;
};

#endif // __ARCFOURPRNG_H__

