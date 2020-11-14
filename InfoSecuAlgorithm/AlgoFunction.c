#include "AlgoFunction.h"

////////////////////////////////////////////////////////////////////
// Show Case AlgoFunc 1~13 exit 9999
void ShowAlgorithm()
{
	printf("\n---------------------------------\n");
	printf("Yeah! ~@.@~\n");

	// Plan
	int nSelect, n = 0;
	while (TRUE)
	{
		printf("---------------------------------\n");
		printf("실행할 알고리즘을 선택하여주세요\n");
		printf("01. 최대공약수 \n");
		printf("02. 하노이탑 \n");
		printf("03. 소수 \n");
		printf("04. 마방진 \n");
		printf("05. 배열 \n");
		printf("06. 링크 샘플 \n");
		printf("07. 스택 샘플 \n");
		printf("08. 큐 샘플 \n");
		printf("09. 트리 샘플 \n");
		printf("10. 정렬 샘플 \n");
		printf("11. 찾기 샘플 \n");
		printf("12. 암호화 샘플 \n");
		printf("13. 압축 샘플 \n");
		printf("99. 이전으로 \n");
		printf("---------------------------------\n");
		printf("Input number : ");
		scanf_s("%d", &nSelect);

		if (nSelect == 99)
		{
			printf("이전으로 돌아갑니다\n\n");
			return;
		}

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
			printf("Input board number : ");
			scanf_s("%d", &n);
			printf("---------------------------------\n");
			Hanoi(n, 'A', 'B', 'C');
			printf("---------------------------------\n");
			break;
		case 3:
			printf("소수알고리즘\n");
			printf("Input number : ");
			scanf_s("%d", &n);
			printf("---------------------------------\n");
			if(IsPrimeNumber(n) == TRUE)
				printf("%d is a prime number.\n", n);
			else
				printf("%d is not prime number.\n", n);
			printf("---------------------------------\n");
			break;
		case 4:
			printf("마방진알고리즘\n");
			printf("SELECT 3,5,7 : ");
			scanf_s("%d", &n);
			printf("---------------------------------\n");
			InitSquare();
			GetOddSquare(n);
			// GetQuaterSquare(n);
			ShowSquare(n);
			printf("---------------------------------\n");
			break;
		case 5:
			printf("배열 예제\n");
			arrayBasic();
			printf("---------------------------------\n\n");
			printf("행렬 예제 01\n");
			ArrayExample01();
			printf("행렬 예제 02\n");
			ArrayExample02();
			printf("행렬 예제 03\n");
			ArrayExample03();
			printf("---------------------------------\n");
			break;
		case 6:
			printf("링크 예제\n");
			printf("---------------------------------\n\n");
			printf("다양한 링크\n");
			LinkedList();
			printf("---------------------------------\n");
			break;
		case 7:
			printf("스택 예제\n");
			printf("---------------------------------\n\n");
			printf("다양한 스택\n");
			StackSample();
			printf("---------------------------------\n");
			break;
		case 8:
			printf("큐 예제\n");
			printf("---------------------------------\n\n");
			printf("다양한 큐\n");
			QueueSample();
			printf("---------------------------------\n");
			break;
		case 9:
			printf("트리 예제\n");
			printf("---------------------------------\n\n");
			printf("트리운행\n");
			TreeBasic();
			printf("---------------------------------\n");
			break;
		default:
			printf("준비중입니다. 1-8에서 다시 선택해주세요!\n");
			break;
		}
	}
}

