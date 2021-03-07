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
			int nValue = 0;
			QarrPut(19);
			QarrPut(28);
			QarrPut(74);
			QarrPut(10);
			QarrPrintQueue();
			printf("Current data count = %d\n", QarrGetDataCount());
			printf("\n");
			printf("---------------------------------\n");
			QarrGet(&nValue);
			QarrGet(&nValue);
			QarrGet(&nValue);
			QarrGet(&nValue);
			QarrGet(&nValue);
			printf("---------------------------------\n");
			break;
		case 2:
			printf("연결리스트를 이용한 큐\n");
			nValue = 0;
			QlinkPut(19);
			QlinkPut(28);
			QlinkPut(74);
			QlinkPut(10);
			QlinkPrintQueue();
			printf("Current data count = %d\n", QlinkGetDataCount());
			printf("\n");
			printf("---------------------------------\n");
			QlinkGet(&nValue);
			QlinkGet(&nValue);
			QlinkGet(&nValue);
			QlinkGet(&nValue);
			QlinkGet(&nValue);
			QlinkRemoveQueue();
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
int g_nqArray[MAX_QUEUE_SIZE] = { 0, };
int g_nFront = 0;
int g_nRear = -1;
bool QarrIsEmpty()
{
	if (g_nFront > g_nRear)
		return TRUE;

	return FALSE;
}

bool QarrIsFull()
{
	if (g_nRear >= MAX_QUEUE_SIZE)
		return TRUE;

	return FALSE;
}

bool QarrPut(int nValue)
{
	if (QarrIsFull() == TRUE)
	{
		printf("[error] queue overflow.\n");
		return FALSE;
	}

	g_nRear++;
	g_nqArray[g_nRear] = nValue;

	printf("Push(%d) \n", nValue);
	return TRUE;
}

bool QarrGet(int* pnValue)
{
	if (pnValue == NULL)
	{
		printf("[error] invalid parameter.\n");
		return FALSE;
	}

	if (QarrIsEmpty() == TRUE)
	{
		printf("[error] queue underflow.\n");
		return FALSE;
	}

	*pnValue = g_nqArray[g_nFront];
	g_nFront++;

	printf("Pop(%d) \n", *pnValue);
	return TRUE;
}

int QarrGetDataCount()
{
	return (g_nRear - g_nFront + 1);
}

void QarrPrintQueue()
{
	printf("PrintQueue() : ");

	for (int i = g_nFront; i <= g_nRear; i++)
		printf("%d ", g_nqArray[i]);
	
	printf("\n");
}

// Link를 이용한 큐
QNode* g_pqFront = NULL;
QNode* g_pqRear = NULL;
int g_nqCount = 0;
bool QlinkIsFull()
{
	if (g_nqCount >= MAX_QUEUE_SIZE)
		return TRUE;

	return FALSE;
}

bool QlinkIsEmpty()
{
	if (g_nqCount == 0)
		return TRUE;

	return FALSE;
}

bool QlinkPut(int nValue)
{
	if (QlinkIsFull() == TRUE)
	{
		printf("[error] queue overflow\n");
		return FALSE;
	}

	QNode* pNode = (QNode*)malloc(sizeof(QNode));
	if (pNode == NULL)
	{
		printf("[error] memory allocation failed.\n");
		return FALSE;
	}

	pNode->nValue = nValue;
	pNode->pNext = NULL;

	if (g_pqRear == NULL)
	{
		g_pqRear = pNode;
		g_pqFront = pNode;
	}
	else
	{
		g_pqRear->pNext = pNode;
		g_pqRear = pNode;
	}

	g_nqCount++;

	printf("Push(%d)\n", nValue);

	return TRUE;
}

bool QlinkGet(int* pnValue)
{
	if (pnValue == NULL)
	{
		printf("[error] invalid parameter.\n");
		return FALSE;
	}

	if (QlinkIsEmpty() == TRUE)
	{
		printf("[error] queue underflow.\n");
		return FALSE;
	}

	*pnValue = g_pqFront->nValue;

	QNode* pDelNode = g_pqFront;
	g_pqFront = g_pqFront->pNext;
	free(pDelNode);

	g_nqCount--;
	if (g_nqCount == 0)
	{
		g_pqFront = NULL;
		g_pqRear = NULL;
	}

	printf("Pop(%d) \n", *pnValue);

	return TRUE;
}

int QlinkGetDataCount()
{
	return g_nqCount;
}

void QlinkPrintQueue()
{
	printf("PrintStack() : ");

	QNode* pNode = g_pqFront;
	while (pNode != NULL)
	{
		printf("%d ", pNode->nValue);
		pNode = pNode->pNext;
	}
	printf("\n");
}

void QlinkRemoveQueue()
{
	QNode* pNode = g_pqFront;
	while (pNode != NULL)
	{
		QNode* pDelNode = pNode;
		pNode = pNode->pNext;

		free(pDelNode);
	}
}
