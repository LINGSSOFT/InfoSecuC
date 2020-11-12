#include "08_QueueExample.h"

////////////////////////////////////////////////////////////////////
// 08. 큐 Queue

void QueueSample()
{
	printf("\n---------------------------------\n");
	printf("Yeah! ~@.@~\n");

	// Plan
	int nSelect, n = 0;
	while (TRUE)
	{
		printf("---------------------------------\n");
		printf("실행할 알고리즘을 선택하여주세요\n");
		printf("01. 배열을 이용한 큐 \n");
		printf("02. 연결리스트를 이용한 큐 \n");
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
			printf("배열을 이용한 큐\n");
			printf("\n");
			printf("---------------------------------\n");
			printf("---------------------------------\n");
			break;
		case 2:
			printf("연결리스트를 이용한 큐\n");
			printf("\n");
			printf("---------------------------------\n");
			printf("\n");
			printf("---------------------------------\n");
			break;
		default:
			printf("[1,2] 중 선택해주세요!\n");
			break;
		}
	}
}

// Array를 이용한 큐
int g_nArray[MAX_QUEUE_SIZE] = { 0, };
int g_nFront = 0;
int g_nRear = -1;
bool arrIsEmpty()
{

}

bool arrIsFull()
{

}

bool arrPut(int nValue)
{

}

bool arrGet(int* pnValue)
{

}

int arrGetDataCount()
{

}

void arrPrintQueue()
{

}

// Link를 이용한 큐

