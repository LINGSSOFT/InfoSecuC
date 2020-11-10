#include "05_ArrayExample.h"

////////////////////////////////////////////////////////////////////
// 05. 배열 Array
void ArrayExample()
{
	char Array[5][5];

	// 배열 초기화
	for (int i = 0; i < 5; i++)
	{
		for (int j = 0; j < 5; j++)
			Array[i][j] = ' ';
	}

	// 배열 입력
	for (int i = 0; i < 5; i = i + 2)
		for (int j = 0; j < 5; j++)
			Array[i][j] = '*';

	// 배열 출력
	for (int i = 0; i < 5; i++)
	{
		for (int j = 0; j < 5; j++)
			printf("%c", Array[i][j]);
		printf("\n");
	}
}
