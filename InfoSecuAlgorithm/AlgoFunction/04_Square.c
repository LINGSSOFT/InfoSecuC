#include "04_Square.h"

// 04 ������ �۷ι� 2���� �迭 ���� ����
int g_nSquare[MAX_SIZE][MAX_SIZE] = { 0, };

////////////////////////////////////////////////////////////////////
// 04. ������ Magic Square
// ���� 3,5,7,9...Ȧ���� ������ ������ �ذ��ϴ� ���
// 1. ù��° ���ڸ� 1���� �߾ӿ��� �ִ´�.
// 2. �밢�� ����(�� ĭ ���� �� ĭ ��)���� �̵��� ���� ���� ���ڸ� �ִ´�. ���� ��ġ��
//    ������ ��� ��� �ݴ� ���̳� ���� ���ڸ� �ִ´�.
// 3. ���� �ܰ踦 �̹� ���ڰ� ����Ǿ� �ִ� ĭ�� ���� ������ nȸ ��ŭ �ݺ��Ѵ�.
// 4. 3�ܰ��� ������ ���ڸ� ���� ������ �ٷ� �Ʒ� ĭ�� ���� ���ڸ� �ְ� ���� 2�ܰ����
//    �ٽ� �ݺ��Ѵ�.
// 4-1 Ȧ���� ������ 
void InitSquare()
{
	for (int i = 0; i < MAX_SIZE; i++)
	{
		for (int j = 0; j < MAX_SIZE; j++)
			g_nSquare[i][j] = 0;
	}
}

void GetOddSquare(int n)
{
	int row = 0;
	int col = 0;
	int nValue = 1;

	col = n / 2;

	while (nValue <= n * n)
	{
		g_nSquare[row][col] = nValue;

		if (nValue % n == 0)
			row++;
		else
		{
			row--;
			col++;
		}

		if (row < 0)
			row = n - 1;
		if (row >= n)
			col = n - 1;
		if (col >= n)
			col = 0;

		nValue++;
	}
}

void ShowSquare(int n)
{
	for (int i = 0; i < n; i++)
	{
		for (int j = 0; j < n; j++)
			printf("%02d", g_nSquare[i][j]);
		printf("\n");
	}
	printf("\n");
}
// 4-2 4�� ��� ������ �׸� ����� ����
int IsInBlock(int n, int row, int col)
{
	return 0;
}
/*void InitSquare()
{
	printf("Test\n");
}
void GetQuaterSquare(int n)
{
	printf("Test\n");
}
void ShowSquare(int n)
{
	printf("Test\n");
}
*/
