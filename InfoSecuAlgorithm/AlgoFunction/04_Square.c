#include "04_Square.h"

////////////////////////////////////////////////////////////////////
// 04. ������ Magic Square

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

void InitSquare()
{
	for (int i = 0; i < MAX_SIZE; i++)
	{
		for (int j = 0; j < MAX_SIZE; j++)
			g_nSquare[i][j] = 0;
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


// 4�� ��� ������
/*
int IsInBlock(int n, int row, int col)
{
	int nQuater = 0;
	int nHalf = 0;
	nQuater = nHalf / 2;

	if ((row < nQuater) || (row >= nHalf + nQuater))
	{
		if ((col < nQuater) || (col >= nHalf + nQuater))
		{
			return 1;
		}

	}

	if ((row >= nQuater) || (row < nHalf + nQuater))
	{
		if ((col >= nQuater) || (col < nHalf + nQuater))
		{
			return 1;
		}
	}
	return 0;
}

void GetQuaterSquare(int n)
{
	int row = 0;
	int col = 0;
	int nValue = 1;

	for (row = 0; row < n; row++)
	{
		for (col = 0; col < n; col++)
		{
			if (IsInBlock(n, row, col) == 1)
				g_nSquare[row][col] = nValue;
			nValue++;
		}
	}

	nValue = 1;
	for (row = n - 1; row >= 0; row--)
	{
		for (col = n - 1; col >= 0; col--)
		{
			if (IsInBlock(n, row, col) == 0)
				g_nSquare[row][col] = nValue;
			nValue++;
		}
	}
}

void InitSquare()
{
	for (int i = 0; i < MAX_SIZE; i++)
	{
		for (int j = 0; j < MAX_SIZE; j++)
			g_nSquare[i][j] = 0;
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
*/