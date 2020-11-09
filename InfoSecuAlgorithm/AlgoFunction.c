#include "AlgoFunction.h"

////////////////////////////////////////////////////////////////////
// Show Case AlgoFunc 1~13 exit 9999
void ShowAlgorithm()
{
	printf("Yeah! ~@.@~\n");

	// Plan
	int nSelect, n = 0;
	while (TRUE)
	{
		printf("������ �˰����� �����Ͽ��ּ���\n");
		printf("Input number : ");
		scanf_s("%d", &nSelect);

		if (nSelect == 9999)
		{
			printf("�������� ���ư��ϴ�\n\n");
			break;
		}

		switch (nSelect)
		{
		case 1:
			printf("GCD�˰���\n");
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
			printf("�ϳ��̾˰���\n");
			printf("Input board number : ");
			scanf_s("%d", &n);
			printf("---------------------------------\n");
			Hanoi(n, 'A', 'B', 'C');
			printf("---------------------------------\n");
			break;
		case 3:
			printf("�Ҽ��˰���\n");
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
			printf("�������˰���\n");
			printf("SELECT 3,5,7 : ");
			scanf_s("%d", &n);
			printf("---------------------------------\n");
			InitSquare();
			GetOddSquare(n);
			ShowSquare(n);
			printf("---------------------------------\n");
			break;
		default:
			printf("�ٽ� �������ּ���!\n");
			break;
		}
	}
}

