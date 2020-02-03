#include "InfoSecuAlgorithm.h"
#include "AlgoFunction.h"

int main()
{
	// Plan
	int nSelect = 0;
	while (1)
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
		default:
			printf("다시 선택해주세요!\n");
			break;
		}
		
	}

	return 0;
}