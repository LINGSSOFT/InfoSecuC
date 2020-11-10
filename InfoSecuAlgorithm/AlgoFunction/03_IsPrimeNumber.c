#include "03_IsPrimeNumber.h"

////////////////////////////////////////////////////////////////////
// 03. �Ҽ� Prime Number

int IsPrimeNumber(int n)
{
	// n�� ���ڷ� �޾Ƽ� �Ҽ����� �ƴ��� Ȯ���ؼ� ��� ����
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
* �� �ڵ�� 1-n���� ����� �����ϴ��� Ȯ���ϰ� ����� ������ 2���� �Ǵ��ϴµ�
* ���� �ݺ��� �߰��� ����� ������ 2 �̻��� �ȴٸ� ���̻� ����� �ִ��� Ȯ����
* �ʿ䰡 �������Դϴ�. �׸��� 2�� ������ ��� ¦���� �Ҽ��� �ɼ� ���� ������
* 1���� n���� �ݺ��ϱ� ���� �̸� �̸� Ȯ���ؼ� ó���� ���� �ֽ��ϴ�. �̷���
* �˰����� ���������ν� ó���ӵ��� ���� ������ �� �� ���� ���Դϴ�.
*/

/* �����ڵ�
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