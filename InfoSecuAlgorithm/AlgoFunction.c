#include "AlgoFunction.h"

////////////////////////////////////////////////////////////////////
// Show Case AlgoFunc 1~13 exit 9999
void ShowAlgorithm()
{
	printf("Yeah! ~@.@~\n");

	// Plan
	int nSelect = 0;
	while (TRUE)
	{
		printf("실행할 알고리즘을 선택하여주세요\n");
		scanf_s("%d", &nSelect);

		if (nSelect == 9999) break;

		switch (nSelect)
		{
		case 1:
			printf("GCD알고리즘\n");
			int a = 0, b = 0;
			int gcd = 0;
			printf("input a : ");
			scanf_s("%d", &a);
			printf("input b : ");
			scanf_s("%d", &b);
			printf("---------------------------------\n");
			gcd = getGCD(a, b);
			printf("---------------------------------\n");
			printf("result GCD : %d\n", gcd);
			break;
		case 2:
			printf("하노이알고리즘\n");
			int n = 0;
			printf("Input board number : ");
			scanf_s("%d", &n);
			printf("---------------------------------\n");
			Hanoi(n, 'A', 'B', 'C');
			printf("---------------------------------\n");
			break;
		case 3:
			printf("소수알고리즘\n");
			int n = 0;
			printf("Input number : ");
			scanf_s("%d", &n);
			printf("---------------------------------\n");
			if(IsPrimeNumber(n) == TRUE)
				printf("%d is a prime number.\n");
			else
				printf("%d is not prime number.\n");
			printf("---------------------------------\n");
			break;
		case 4:
			printf("마방진알고리즘\n");
			int n = 0;
			printf("SELECT 3,5,7 : ");
			scanf_s("%d", &n);
			printf("---------------------------------\n");
			InitSquare();
			GetOddSquare(n);
			ShowSquare(n);
			printf("---------------------------------\n");
			break;
		default:
			printf("다시 선택해주세요!\n");
			break;
		}
	}
}

////////////////////////////////////////////////////////////////////
// 01.최대공약수 Greatest Common Divisor
// 최대공약수란? 정수 a,b의 약수 중에서 공통된 약수를
// 공약수라고 하며 공약수 중에서 가장 큰 수를 최대 공약수라 합니다.
//
// 최대공약수를 구하기 위한 과정
// 1.정수 a의 약수로 구성된 집합 A를 구한다.
// 2.정수 b의 약수로 구성된 집합 B를 구한다.
// 3.정수 a와 b의 공약수로 구성된 집합 C를 구한다(집합 A와 B의 교집합).
// 4.집합 C에서 가장 큰수인 c가 정수 a와 b의 최대공약수이다.
//
// 한계점
// 1.정수의 약수를 구하는데 그 값이 매우 큰 경우에는 많은 시간이 걸린다.
// 2.A집합과 B집합의 교집합을 구해서 공약수 집합인 C를 만드는데 오래
//   걸린다. A와 B집합의 모든 요소들을 비교해서 같은 값을 찾는 작업이
//   집합내의 요소의 수가 많을 경우 많은 시간을 필요로 한다.
// 3.공약수의 집합인 C에서 최대공약수인 C를 찾는데 대소 비교를 통해
//   가장 큰 수를 찾아야 하므로 많은 시간이 소요된다.
//
// 유클리드 알고리즘
// 주어진 서로 소가 아닌 두 수 a,b에대해 최대공약수를 찾는 것으로
// 두개의 규칙을 전제로 하고 있다.
// 1.b가 a로 나눠서 떨어지면 두수의 최대공약수는 b이다.
// 2.a를 b로 나웠을 때 나머지가 r이면, 두수의 최대공약수는 r과 b의 최대공약수와 같다.
// GCD(120, 50) = GCD(50, 20) = GCD(20, 10) = GCD(10, 0)
// 120을 50으로 나누면 r=20, 50을 20으로 나누면 r=10
// 20을 10으로 나누면 r=0, 최대공약수는 10 = b
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

////////////////////////////////////////////////////////////////////
// 02.하노이탑 Hanoi Tower
//
void Hanoi(int n, char from, char middle, char to)
{
	if (n == 1)
		printf("board #%02d %c -> %c\n", n, from, to);
	else
	{
		Hanoi(n - 1, from, to, middle);
		printf("board #%02d %c -> %c\n", n, from, to);
		Hanoi(n - 1, middle, from, to);
	}
}

////////////////////////////////////////////////////////////////////
// 03. 소수 Prime Number
void IsPrimeNumber(int n)
{
	int count = 0;

	for(int i = 0; i <=n; i++)
	{
		if(n % i == 0)
			count++;
	}

	if(count == 2)
		return TRUE;

	return FALSE;
}

////////////////////////////////////////////////////////////////////
// 04. 마방진 Magic Square
void InitSquare()
{
	for(int i = 0; i < MAX_SIZE; i++)
	{
		for(int j = 0; j < MAX_SIZE; j++)
			g_nSquare[i][j] = 0;
	}
}

void GetOddSquare(int n)
{
	int row = 0;
	int col = 0;
	int nValue = 1;

	col = n/2;

	while(nValue <= n*n)
	{
		g_nSquare[row][col] = nValue;

		if(nValue % n == 0)
			row++;
		else
		{
			row--;
			col++;
		}

		if(row < 0)
			row = n - 1;
		if(row >= n)
			col = n-1;
		if(col >= n)
			col = 0;
		
		nValue++;
	}
}

void ShowSquare(int n)
{
	for(int i = 0; i < n; i++)
	{
		for(int j = 0; j < n; j++)
			printf("%02d", g_nSquare[i][j]);
		printf("\n");
	}
	printf("\n");
}

////////////////////////////////////////////////////////////////////
// 05. 배열 Array

////////////////////////////////////////////////////////////////////
// 06. 링크 Link

////////////////////////////////////////////////////////////////////
// 07. 스택 Stack

////////////////////////////////////////////////////////////////////
// 08. 큐 Queue

////////////////////////////////////////////////////////////////////
// 09. 트리 Tree

////////////////////////////////////////////////////////////////////
// 10. 정렬 Sorting

////////////////////////////////////////////////////////////////////
// 11. 검색 Searching

////////////////////////////////////////////////////////////////////
// 12. 암호화 Cryptography

////////////////////////////////////////////////////////////////////
// 13. 압축 Compress
