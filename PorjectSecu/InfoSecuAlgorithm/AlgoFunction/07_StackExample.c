#include "07_StackExample.h"

////////////////////////////////////////////////////////////////////
// 07. 스택 Stack

void StackSample()
{
	printf("\n---------------------------------\n");
	printf("Yeah! ~@.@~\n");

	// Plan
	int nSelect, n = 0;
	while (TRUE)
	{
		printf("---------------------------------\n");
		printf("실행할 알고리즘을 선택하여주세요\n");
		printf("01. 배열을 이용한 스택 \n");
		printf("02. 연결리스트를 이용한 스택 \n");
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
			printf("배열을 이용한 스택\n");
			int nValue = 0;
			arrPush(19);
			arrPush(28);
			arrPush(74);
			arrPush(10);
			arrPrintStack();
			printf("Current data count = %d\n", arrGetDataCount());
			printf("\n");
			printf("---------------------------------\n");
			arrPop(&nValue);
			arrPop(&nValue);
			arrPop(&nValue);
			arrPop(&nValue);
			arrPop(&nValue);
			printf("\n");
			printf("---------------------------------\n");
			break;
		case 2:
			printf("연결리스트를 이용한 스택\n");
			nValue = 0;
			linkPush(19);
			linkPush(28);
			linkPush(74);
			linkPush(10);
			linkPrintStack();
			printf("Current data count = %d\n", linkGetDataCount());
			printf("\n");
			printf("---------------------------------\n");
			linkPop(&nValue);
			linkPop(&nValue);
			linkPop(&nValue);
			linkPop(&nValue);
			linkPop(&nValue);
			linkRemoveStack();
			printf("\n");
			printf("---------------------------------\n");
			break;
		default:
			printf("[1,2] 중 선택해주세요!\n");
			break;
		}
	}
}

// Array를 이용한 스택
int g_nArray[MAX_STACK_SIZE] = { 0, };
int g_nTop = -1;
bool arrIsEmpty()
{
	if (g_nTop == -1)
		return TRUE;
	return FALSE;
}

bool arrIsFull()
{
	if (g_nTop >= MAX_STACK_SIZE - 1)
		return TRUE;
	return FALSE;
}

bool arrPush(int nValue)
{
	if (arrIsFull() == TRUE)
	{
		printf("[error] stack overflow.\n");
		return FALSE;
	}
	g_nTop++;
	g_nArray[g_nTop] = nValue;

	printf("Push(%d) \n", nValue);
	return TRUE;
}

bool arrPop(int* pnValue)
{
	if (pnValue == NULL)
	{
		printf("[error] invalid parameter. \n");
		return FALSE;
	}

	if (arrIsEmpty() == TRUE)
	{
		printf("[error] stack underflow.\n");
		return FALSE;
	}

	*pnValue = g_nArray[g_nTop];
	g_nTop--;

	printf("Pop(%d) \n", *pnValue);

	return TRUE;
}

int arrGetDataCount()
{
	return g_nTop + 1;
}

void arrPrintStack()
{
	printf("PrintStack() : ");

	for (int i = g_nTop; i >= 0; i--)
		printf("%d ", g_nArray[i]);

	printf("\n");
}

// Link를 이용한 스택
Node* g_pTop = NULL;
int g_nCount = 0;
bool linkIsFull()
{
	if (g_nCount >= MAX_STACK_SIZE)
		return TRUE;
	return FALSE;
}

bool linkIsEmpty()
{
	if (g_nCount == 0)
		return TRUE;
	return FALSE;
}

bool linkPush(int nValue)
{
	if (linkIsFull() == TRUE)
	{
		printf("[error] stack overflow.\n");
		return FALSE;
	}
	Node* pNode = (Node*)malloc(sizeof(Node));
	if (pNode == NULL)
	{
		printf("[error] memory allocation failed.\n");
		return FALSE;
	}
	pNode->nValue = nValue;
	pNode->pNext = g_pTop;

	g_pTop = pNode;
	g_nCount++;

	printf("Push(%d) \n", nValue);
	return TRUE;
}

bool linkPop(int* pnValue)
{
	if (pnValue == NULL)
	{
		printf("[error] invalid parameter. \n");
		return FALSE;
	}

	if (linkIsEmpty() == TRUE)
	{
		printf("[error] stack underflow.\n");
		return FALSE;
	}

	*pnValue = g_pTop->nValue;

	Node* pDelNode = g_pTop;
	g_pTop = g_pTop->pNext;
	free(pDelNode);
	g_nCount--;

	printf("Pop(%d) \n", *pnValue);

	return TRUE;
}

bool linkGetDataCount()
{
	return g_nCount;
}

void linkPrintStack()
{
	printf("PrintStack() : ");

	Node* pNode = g_pTop;
	while (pNode != NULL)
	{
		printf("%d ", pNode->nValue);
		pNode = pNode->pNext;
	}
	printf("\n");
}

void linkRemoveStack()
{
	Node* pNode = g_pTop;
	while (pNode != NULL)
	{
		Node* pDelNode = pNode;
		pNode = pNode->pNext;

		free(pDelNode);
	}
}
