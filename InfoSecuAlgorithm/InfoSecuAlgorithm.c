#include "InfoSecuAlgorithm.h"
#include "AlgoFunction.h"

int main()
{
	int nSelect = 0;

	while(TRUE)
	{
		printf("---------------------------------\n");
		printf("������ �˰����� �����Ͽ��ּ���\n");
		printf("1. �˰��� 13�� \n");
		printf("2. ����\n");
		printf("---------------------------------\n");
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
			printf("�ٽ� �������ּ���!\n");
			break;
		}
	}

	return 0;
}