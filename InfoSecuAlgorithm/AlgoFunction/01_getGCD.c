#include "01_getGCD.h"

////////////////////////////////////////////////////////////////////
// 01.�ִ����� Greatest Common Divisor

int getGCD(int nFir, int nSec)
{
	printf("GCD(%d, %d)\n", nFir, nSec);

	while (nSec != 0)
	{
		int temp = nSec;
		nSec = nFir % nSec;
		nFir = temp;

		printf("GCD(%d, %d)\n", nFir, nSec);
	}

	return nFir;
}
