#include "05_ArrayExample.h"

////////////////////////////////////////////////////////////////////
// 05. �迭 Array
void ArrayExample()
{
	char Array[5][5];

	// �迭 �ʱ�ȭ
	for (int i = 0; i < 5; i++)
	{
		for (int j = 0; j < 5; j++)
			Array[i][j] = ' ';
	}

	// �迭 �Է�
	for (int i = 0; i < 5; i = i + 2)
		for (int j = 0; j < 5; j++)
			Array[i][j] = '*';

	// �迭 ���
	for (int i = 0; i < 5; i++)
	{
		for (int j = 0; j < 5; j++)
			printf("%c", Array[i][j]);
		printf("\n");
	}
}
