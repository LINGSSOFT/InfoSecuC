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
		printf("������ �˰����� �����Ͽ��ּ���\n");
		printf("01. �ִ����� \n");
		printf("02. �ϳ���ž \n");
		printf("03. �Ҽ� \n");
		printf("04. ������ \n");
		printf("05. �迭 \n");
		printf("06. ��ũ ���� \n");
		printf("07. ���� ���� \n");
		printf("08. ť ���� \n");
		printf("09. Ʈ�� ���� \n");
		printf("10. ���� ���� \n");
		printf("11. ã�� ���� \n");
		printf("12. ��ȣȭ ���� \n");
		printf("13. ���� ���� \n");
		printf("99. �������� \n");
		printf("---------------------------------\n");
		printf("Input number : ");
		scanf_s("%d", &nSelect);

		if (nSelect == 99)
		{
			printf("�������� ���ư��ϴ�\n\n");
			return;
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
			// GetQuaterSquare(n);
			ShowSquare(n);
			printf("---------------------------------\n");
			break;
		case 5:
			printf("�迭 ����\n");
			arrayBasic();
			printf("---------------------------------\n\n");
			printf("��� ���� 01\n");
			ArrayExample01();
			printf("��� ���� 02\n");
			ArrayExample02();
			printf("��� ���� 03\n");
			ArrayExample03();
			printf("---------------------------------\n");
			break;
		case 6:
			printf("��ũ ����\n");
			printf("---------------------------------\n\n");
			printf("�پ��� ��ũ\n");
			LinkedList();
			printf("---------------------------------\n");
			break;
		case 7:
			printf("���� ����\n");
			printf("---------------------------------\n\n");
			printf("�پ��� ����\n");
			StackSample();
			printf("---------------------------------\n");
			break;
		case 8:
			printf("ť ����\n");
			printf("---------------------------------\n\n");
			printf("�پ��� ť\n");
			QueueSample();
			printf("---------------------------------\n");
			break;
		case 9:
			printf("Ʈ�� ����\n");
			printf("---------------------------------\n\n");
			printf("Ʈ������\n");
			TreeBasic();
			printf("---------------------------------\n");
			break;
		default:
			printf("�غ����Դϴ�. 1-8���� �ٽ� �������ּ���!\n");
			break;
		}
	}
}

