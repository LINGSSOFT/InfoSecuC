#include "06_LinkExample.h"

////////////////////////////////////////////////////////////////////
// 06. 링크 Link

void LinkedList()
{
	printf("\n---------------------------------\n");
	printf("Yeah! ~@.@~\n");

	// Plan
	int nSelect, n = 0;
	while (TRUE)
	{
		printf("---------------------------------\n");
		printf("실행할 알고리즘을 선택하여주세요\n");
		printf("01. 단순연결리스트 \n");
		printf("02. 다중연결리스트 \n");
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
			printf("단순연결리스트\n");
			SingleNode shead, stail;
			shead.nData = 10;
			stail.nData = 20;
			shead.pNext = &stail;
			stail.pNext = NULL;
			singleTraverseNode(&shead);
			printf("\n");
			printf("---------------------------------\n");
			printf("삽입 5, 24, 1978\n");
			singleInsertNode(&shead, 5);
			singleInsertNode(&shead, 24);
			singleInsertNode(&shead, 1978);
			singleTraverseNode(&shead);
			printf("\n");
			printf("---------------------------------\n");
			printf("삭제 2회\n");
			singleDeleteNode(&shead);
			singleDeleteNode(&shead);
			singleTraverseNode(&shead);
			printf("\n");
			printf("---------------------------------\n");
			break;
		case 2:
			printf("다중연결리스트\n");
			DoubleNode dhead, dtail;
			dhead.nData = 30;
			dtail.nData = 40;
			dhead.pPrev = NULL;
			dhead.pNext = &dtail;
			dtail.pPrev = &dhead;
			dtail.pNext = NULL;
			doubleTraverseNodeForward(&dhead);
			printf("\n");
			printf("---------------------------------\n");
			printf("삽입 5, 24, 1978\n");
			doubleInsertNode(&dhead, 5);
			doubleInsertNode(&dhead, 24);
			doubleInsertNode(&dhead, 1978);
			doubleTraverseNodeForward(&dhead);
			printf("\n");
			doubleTraverseNodeReverse(&dtail);
			printf("\n");
			printf("---------------------------------\n");
			printf("삭제 2회\n");
			doubleDeleteNode(dhead.pNext);
			doubleDeleteNode(dhead.pNext);
			doubleTraverseNodeForward(&dhead);
			printf("\n");
			printf("---------------------------------\n");
			break;
		default:
			printf("준비중입니다. 1-4에서 다시 선택해주세요!\n");
			break;
		}
	}
}

void singleInsertNode(SingleNode* pPrev, int nData)
{
	// 새로운 노드를 생성
	SingleNode* pNode = NULL;
	pNode = (SingleNode *)malloc(sizeof(SingleNode));
	pNode->nData = nData;

	// 이전 노드의 링크를 저장
	pNode->pNext = pPrev->pNext;

	// 이전 노드의 링크에 새로운 노드 주소를 저장한다.
	pPrev->pNext = pNode;
}

void singleDeleteNode(SingleNode* pPrev)
{
	// 삭제할 노드
	SingleNode* pDelNode = pPrev->pNext;

	// 삭제할 노드 링크 정보를 저장
	pPrev->pNext = pDelNode->pNext;

	// 노드를 삭제
	free(pDelNode);
}

void singleTraverseNode(SingleNode* pNode)
{
	SingleNode* pPointer = pNode;
	// 노드의 데이터를 출력한다.
	printf("%d ", pPointer->nData);
	// 다음 노드 정보가 있으면 반복한다. 재귀호출
	if (pPointer->pNext != NULL)
		singleTraverseNode(pPointer->pNext);
}

void doubleInsertNode(DoubleNode* pTarget, int nData)
{
	// 새로운 노드를 생성
	DoubleNode* pNode = NULL;
	pNode = (DoubleNode*)malloc(sizeof(DoubleNode));
	pNode->nData = nData;

	// 새로운 노드의 링크를 구성
	pNode->pNext = pTarget->pNext;
	pNode->pPrev = pTarget;

	// 기존 노드의 링크에 새로운 노드 정보를 저장
	if (pNode->pPrev != NULL)
		pNode->pPrev->pNext = pNode;
	if (pNode->pNext != NULL)
		pNode->pNext->pPrev = pNode;
}

void doubleDeleteNode(DoubleNode* pTarget)
{
	// 앞 노드의 링크를 수정한다.
	if (pTarget->pPrev != NULL)
		pTarget->pPrev->pNext = pTarget->pNext;

	// 뒤 노드의 링크를 수정한다.
	if (pTarget->pNext != NULL)
		pTarget->pNext->pPrev = pTarget->pPrev;

	// 노드를 삭제한다.
	free(pTarget);
}

void doubleTraverseNodeForward(DoubleNode* pNode)
{
	DoubleNode* pPointer = pNode;

	// 노드의 데이터를 출력한다.
	printf("%d ", pPointer->nData);
	
	// 다음 노드 정보가 있으면 반복한다. 재귀호출
	if (pPointer->pNext != NULL)
		doubleTraverseNodeForward(pPointer->pNext);

}

void doubleTraverseNodeReverse(DoubleNode* pNode)
{
	DoubleNode* pPointer = pNode;

	// 노드의 데이터를 출력한다.
	printf("%d ", pPointer->nData);

	// 다음 노드 정보가 있으면 반복한다. 재귀호출
	if (pPointer->pPrev != NULL)
		doubleTraverseNodeReverse(pPointer->pPrev);

}
