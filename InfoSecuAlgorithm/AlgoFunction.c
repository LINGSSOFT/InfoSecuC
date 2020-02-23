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
// 하노이탑 알고리즘
// 순서대로 1부터 n까지 n개의 원팡이 있고 A,B,C 3개의 막대가 있는 경우 하노이탑 문제를
// 해결하는 방법은 다음과 같이 정리할 수 있습니다.
// 1. A막대기에서 2번부터 n번까지 n-1개의 원판을 B 막대기로 이동시킨다.
// 2. A막대기에서 1번 원판을 C막대기로 이동시킨다.
// 3. B막대기에서 2번부터 n번까지 n-1개의 원판을 C로 이동시킨다.
// 여기서 1,3번 방법은 매우 막연하게 서술되어 있다는 것을 알 수 있습니다. 자세한 처리
// 방법이 설명된 것이 아니라 어떻게 보면 "될때까지 옮긴다"라는 것처럼 간단히 설명하였기
// 때문입니다. 1,3번은 앞서 원판이 1개,2개,3개일 때 원판을 옮기는 방법을 n의 개수로
// 작은 숫자로 줄이면 그리 막연한 내용이 아닙니다. 따라서 이는 내부적으로 재귀적인 의미를
// 가지고 있습니다. 원판이 4개일 때에는 원판을 15번 이동시켜야 한다는 것을 알 수 있습니다.
// 원판이 n개의 경우 원판의 이동 횟수는 2의n승-1이 됩니다. 64개의 원판을 A에서 C로 모두
// 옮기려면 1개 이동시키는데 1초로 가정하여 약 5833억년의 시간이 걸린다는것을 알수 있습니다.
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
// 자연수 n이 소수인지 아닌지 판단하는 방법!
// 1. 1부터 n까지 반복해서 n의 약수인지 확인한다.
// 2. 만약 새로운 약수를 찾으면 count변수를 +1한다.
// 3. 모든 확인이 끝난 뒤 약수의 개수가 2이면 소수이고 그렇지 않으면 수수가 아니다.
bool IsPrimeNumber(int n)
{
	// n을 인자로 받아서 소수인지 아닌지 확인해서 결과 리턴
	int count = 0;

	if (n % 2 == 0)
		return FALSE;
		
	for(int i = 0; i <=n; i++)
	{
		if(n % i == 0)
			count++;
		if(count > 2)
			return FALSE;
	}

	if(count == 2)
		return TRUE;

	return FALSE;
}

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
// 4-2 4의 배수 마방진 그림 관계로 생략
bool IsInBlock(int n, int row, int col)
{
	return 0;
}
void InitSquare()
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

////////////////////////////////////////////////////////////////////
// 05. 배열 Array
void ArrayExample()
{
	char Array[5],[5];

	// 배열 초기화
	for(int i = 0; i < 5; i++)
	{
		for(int j = 0; j < 5; j++)
			Array[i][j] = '';
	}

	// 배열 입력
	for(int i = 0; i < 5; i=i+2)
		for(int j = 0; j < 5; j++)
			Array[i][j] = '*';
	
	// 배열 출력
	for(int i = 0; i < 5; i++)
	{
		for(int j = 0; j < 5; j++)
			printf("%c", Array[i][j]);
		printf("\n");
	}
}

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
