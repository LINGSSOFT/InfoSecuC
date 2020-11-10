#include "03_IsPrimeNumber.h"

////////////////////////////////////////////////////////////////////
// 03. 소수 Prime Number

int IsPrimeNumber(int n)
{
	// n을 인자로 받아서 소수인지 아닌지 확인해서 결과 리턴
	int count = 0;

	if (n % 2 == 0)
		return FALSE;

	for (int i = 1; i <= n; i++)
	{
		if (n % i == 0)
			count++;
	}

	if (count == 2)
		return TRUE;

	return FALSE;
}

/*
* 위 코드는 1-n까지 약수가 존재하는지 확인하고 약수의 개수가 2인지 판단하는데
* 만약 반복문 중간에 약수의 개수가 2 이상이 된다면 더이상 약수가 있는지 확인할
* 필요가 없을것입니다. 그리고 2를 제외한 모든 짝수는 소수가 될수 없기 때문에
* 1부터 n까지 반복하기 전에 미리 이를 확인해서 처리할 수도 있습니다. 이렇게
* 알고리즘을 보완함으로써 처리속도를 보다 빠르게 할 수 있을 것입니다.
*/

/* 개선코드
int IsPrimeNumber(int n)
{
	int count = 0;

	if (n % 2 == 0)
		return FALSE;

	for (int i = 1; i <= n; i++)
	{
		if (n % i == 0)
			count++;
		if (count > 2)			// <-- Check
			return FALSE;
	}

	if (count == 2)
		return TRUE;

	return FALSE;
}
*/