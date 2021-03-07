#include "05_ArrayExample.h"

////////////////////////////////////////////////////////////////////
// 05. 배열 Array

// 배열기본 초기화, 추가, 삭제, 찾기
void Insert(int nArray[], int nIndex, int nValue)
{
	if (nIndex < 0 || nIndex >= MAX_SIZE)
	{
		printf("Invalid Parameter.\n");
		return;
	}

	for (int i = MAX_SIZE - 1; i > nIndex; i--)
		nArray[i] = nArray[i - 1];

	nArray[nIndex] = nValue;
}

void Delete(int nArray[], int nIndex)
{
	if (nIndex < 0 || nIndex >= MAX_SIZE)
	{
		printf("Invalid Parameter.\n");
		return;
	}

	for (int i = nIndex; i < MAX_SIZE; i++)
		nArray[i] = nArray[i + 1];

	nArray[MAX_SIZE - 1] = 0;
}

int Find(int nArray[], int nValue)
{
	int i = 0;
	int nIndex = -1;

	while (i < MAX_SIZE)
	{
		if (nArray[i] == nValue)
		{
			nIndex = i;
			break;
		}
		i++;
	}
	return nIndex;
}

void Print(int nArray[])
{
	printf("->");
	for (int i = 0; i < MAX_SIZE; i++)
		printf("%02d ", nArray[i]);
	printf("\n");
}

void arrayBasic()
{
	int nArray[MAX_SIZE] = { 0, };
	Print(nArray);

	printf("set value\n");
	for (int i = 0; i < MAX_SIZE; i++)
		nArray[i] = i + 1;
	Print(nArray);

	printf("insert 10 at 3\n");
	Insert(nArray, 3, MAX_SIZE);
	Print(nArray);

	printf("delete at 5\n");
	Delete(nArray, 5);
	Print(nArray);

	int nIndex = Find(nArray, 6);
	if (nIndex > 0)
		printf("found 6 at %d\n", nIndex);

}

// 행렬 예제
void ArrayExample01()
{
	char cArray[MATRIX_SIZE][MATRIX_SIZE];

	// 배열 초기화
	for (int i = 0; i < MATRIX_SIZE; i++)
	{
		for (int j = 0; j < MATRIX_SIZE; j++)
			cArray[i][j] = ' ';
	}

	// 배열 입력
	for (int i = 0; i < MATRIX_SIZE; i = i + 2)
		for (int j = 0; j < MATRIX_SIZE; j++)
			cArray[i][j] = '*';

	// 배열 출력
	for (int i = 0; i < MATRIX_SIZE; i++)
	{
		for (int j = 0; j < MATRIX_SIZE; j++)
			printf("%c", cArray[i][j]);
		printf("\n");
	}
	printf("\n");
}

void ArrayExample02()
{
	char cArray[MATRIX_SIZE][MATRIX_SIZE];

	// 배열 초기화
	for (int i = 0; i < MATRIX_SIZE; i++)
	{
		for (int j = 0; j < MATRIX_SIZE; j++)
		{
			if ((i + j) % 2 == 0)
				cArray[i][j] = '*';
			else
				cArray[i][j] = ' ';
		}
	}

	PrintcArray(cArray);
	printf("\n");
}

void ArrayExample03()
{
	int nValue = 1;
	int nArray[MATRIX_SIZE][MATRIX_SIZE] = { 0, };

	for (int i = 0; i < MATRIX_SIZE; i++)
	{
		if (i % 2 == 0)
		{
			for (int j = 0; j < MATRIX_SIZE; j++)
				nArray[i][j] = nValue++;
		}
		else
		{
			for (int j = MATRIX_SIZE - 1; j >= 0; j--)
				nArray[i][j] = nValue++;
		}
	}

	PrintnArray(nArray);
	printf("\n");
}

void PrintcArray(char cArray[][MATRIX_SIZE])
{
	for (int i = 0; i < MATRIX_SIZE; i++)
	{
		for (int j = 0; j < MATRIX_SIZE; j++)
			printf("%c", cArray[i][j]);

		printf("\n");
	}
}

void PrintnArray(int nArray[][MATRIX_SIZE])
{
	for (int i = 0; i < MATRIX_SIZE; i++)
	{
		for (int j = 0; j < MATRIX_SIZE; j++)
			printf("%02d ", nArray[i][j]);

		printf("\n");
	}
}
