#include "03_IsPrimeNumber.h"

////////////////////////////////////////////////////////////////////
// 03. �Ҽ� Prime Number
// �ڿ��� n�� �Ҽ����� �ƴ��� �Ǵ��ϴ� ���!
// 1. 1���� n���� �ݺ��ؼ� n�� ������� Ȯ���Ѵ�.
// 2. ���� ���ο� ����� ã���� count������ +1�Ѵ�.
// 3. ��� Ȯ���� ���� �� ����� ������ 2�̸� �Ҽ��̰� �׷��� ������ ������ �ƴϴ�.
int IsPrimeNumber(int n)
{
	// n�� ���ڷ� �޾Ƽ� �Ҽ����� �ƴ��� Ȯ���ؼ� ��� ����
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
