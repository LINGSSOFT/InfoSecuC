#include "InfoSecuAlgorithm.h"

int main()
{
	int nSelect = 0;

	while(TRUE)
	{
		printf("---------------------------------\n");
		printf("������ �˰����� �����Ͽ��ּ���\n");
		printf("01. �˰��� 13�� \n");
		printf("02. ����\n");
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
			printf("�ٽ� �������ּ���!\n\n");
			break;
		}
	}

	return 0;
}