#include "InfoSecuAlgorithm.h"

int main()
{
	int nSelect = 0;

	while(TRUE)
	{
		printf("---------------------------------\n");
		printf("실행할 알고리즘을 선택하여주세요\n");
		printf("01. 알고리즘 13종 \n");
		printf("02. 종료\n");
		printf("---------------------------------\n");
		printf("Input number : ");
		scanf_s("%d", &nSelect);

		if (nSelect == 2) return 0;

		switch (nSelect)
		{
		case 1:
			ShowAlgorithm();
			break;
		case 2:
			break;
		default:
			printf("다시 선택해주세요!\n\n");
			break;
		}
	}

	return 0;
}