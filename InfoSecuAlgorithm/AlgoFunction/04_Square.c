#include "04_Square.h"

// 04 마방진 글로벌 2차원 배열 변수 선언
int g_nSquare[MAX_SIZE][MAX_SIZE] = { 0, };

////////////////////////////////////////////////////////////////////
// 04. 마방진 Magic Square
// 먼저 3,5,7,9...홀수차 마방진 문제를 해결하는 방법
// 1. 첫번째 숫자를 1행의 중앙열에 넣는다.
// 2. 대각선 방향(한 칸 위와 한 칸 옆)으로 이동한 곳에 다음 숫자를 넣는다. 만약 위치가
//    영역을 벗어날 경우 반대 행이나 열에 숫자를 넣는다.
// 3. 위의 단계를 이미 숫자가 저장되어 있는 칸을 만날 때까지 n회 만큼 반복한다.
// 4. 3단계의 마지막 숫자를 넣은 곳에서 바로 아래 칸에 다음 숫자를 넣고 위의 2단계부터
//    다시 반복한다.
// 4-1 홀수차 마방진 
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
// 4-2 4의 배수 마방진 그림 관계로 생략
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
