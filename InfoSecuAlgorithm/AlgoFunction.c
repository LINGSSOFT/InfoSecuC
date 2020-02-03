#include "AlgoFunction.h"

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