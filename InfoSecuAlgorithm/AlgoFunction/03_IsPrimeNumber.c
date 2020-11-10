#include "03_IsPrimeNumber.h"

////////////////////////////////////////////////////////////////////
// 03. 소수 Prime Number
// 자연수 n이 소수인지 아닌지 판단하는 방법!
// 1. 1부터 n까지 반복해서 n의 약수인지 확인한다.
// 2. 만약 새로운 약수를 찾으면 count변수를 +1한다.
// 3. 모든 확인이 끝난 뒤 약수의 개수가 2이면 소수이고 그렇지 않으면 수수가 아니다.
int IsPrimeNumber(int n)
{
	// n을 인자로 받아서 소수인지 아닌지 확인해서 결과 리턴
	int count = 0;

	if (n % 2 == 0)
		return FALSE;

	for (int i = 0; i <= n; i++)
	{
		if (n % i == 0)
			count++;
		if (count > 2)
			return FALSE;
	}

	if (count == 2)
		return TRUE;

	return FALSE;
}
