#include "06_LinkExample.h"

////////////////////////////////////////////////////////////////////
// 06. ��ũ Link

void LinkedList()
{
	printf("\n---------------------------------\n");
	printf("Yeah! ~@.@~\n");

	// Plan
	int nSelect, n = 0;
	while (TRUE)
	{
		printf("---------------------------------\n");
		printf("������ �˰����� �����Ͽ��ּ���\n");
		printf("01. �ܼ����Ḯ��Ʈ \n");
		printf("02. ���߿��Ḯ��Ʈ \n");
		printf("99. �������� \n");
		printf("---------------------------------\n");
		printf("Input number : ");
		scanf_s("%d", &nSelect);

		if (nSelect == 99)
		{
			printf("�������� ���ư��ϴ�\n\n");
			return;
		}

		switch (nSelect)
		{
		case 1:
			printf("�ܼ����Ḯ��Ʈ\n");
			SingleNode shead, stail;
			shead.nData = 10;
			stail.nData = 20;
			shead.pNext = &stail;
			stail.pNext = NULL;
			singleTraverseNode(&shead);
			printf("\n");
			printf("---------------------------------\n");
			printf("���� 5, 24, 1978\n");
			singleInsertNode(&shead, 5);
			singleInsertNode(&shead, 24);
			singleInsertNode(&shead, 1978);
			singleTraverseNode(&shead);
			printf("\n");
			printf("---------------------------------\n");
			printf("���� 2ȸ\n");
			singleDeleteNode(&shead);
			singleDeleteNode(&shead);
			singleTraverseNode(&shead);
			printf("\n");
			printf("---------------------------------\n");
			break;
		case 2:
			printf("���߿��Ḯ��Ʈ\n");
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
			printf("���� 5, 24, 1978\n");
			doubleInsertNode(&dhead, 5);
			doubleInsertNode(&dhead, 24);
			doubleInsertNode(&dhead, 1978);
			doubleTraverseNodeForward(&dhead);
			printf("\n");
			doubleTraverseNodeReverse(&dtail);
			printf("\n");
			printf("---------------------------------\n");
			printf("���� 2ȸ\n");
			doubleDeleteNode(dhead.pNext);
			doubleDeleteNode(dhead.pNext);
			doubleTraverseNodeForward(&dhead);
			printf("\n");
			printf("---------------------------------\n");
			break;
		default:
			printf("�غ����Դϴ�. 1-4���� �ٽ� �������ּ���!\n");
			break;
		}
	}
}

void singleInsertNode(SingleNode* pPrev, int nData)
{
	// ���ο� ��带 ����
	SingleNode* pNode = NULL;
	pNode = (SingleNode *)malloc(sizeof(SingleNode));
	pNode->nData = nData;

	// ���� ����� ��ũ�� ����
	pNode->pNext = pPrev->pNext;

	// ���� ����� ��ũ�� ���ο� ��� �ּҸ� �����Ѵ�.
	pPrev->pNext = pNode;
}

void singleDeleteNode(SingleNode* pPrev)
{
	// ������ ���
	SingleNode* pDelNode = pPrev->pNext;

	// ������ ��� ��ũ ������ ����
	pPrev->pNext = pDelNode->pNext;

	// ��带 ����
	free(pDelNode);
}

void singleTraverseNode(SingleNode* pNode)
{
	SingleNode* pPointer = pNode;
	// ����� �����͸� ����Ѵ�.
	printf("%d ", pPointer->nData);
	// ���� ��� ������ ������ �ݺ��Ѵ�. ���ȣ��
	if (pPointer->pNext != NULL)
		singleTraverseNode(pPointer->pNext);
}

void doubleInsertNode(DoubleNode* pTarget, int nData)
{
	// ���ο� ��带 ����
	DoubleNode* pNode = NULL;
	pNode = (DoubleNode*)malloc(sizeof(DoubleNode));
	pNode->nData = nData;

	// ���ο� ����� ��ũ�� ����
	pNode->pNext = pTarget->pNext;
	pNode->pPrev = pTarget;

	// ���� ����� ��ũ�� ���ο� ��� ������ ����
	if (pNode->pPrev != NULL)
		pNode->pPrev->pNext = pNode;
	if (pNode->pNext != NULL)
		pNode->pNext->pPrev = pNode;
}

void doubleDeleteNode(DoubleNode* pTarget)
{
	// �� ����� ��ũ�� �����Ѵ�.
	if (pTarget->pPrev != NULL)
		pTarget->pPrev->pNext = pTarget->pNext;

	// �� ����� ��ũ�� �����Ѵ�.
	if (pTarget->pNext != NULL)
		pTarget->pNext->pPrev = pTarget->pPrev;

	// ��带 �����Ѵ�.
	free(pTarget);
}

void doubleTraverseNodeForward(DoubleNode* pNode)
{
	DoubleNode* pPointer = pNode;

	// ����� �����͸� ����Ѵ�.
	printf("%d ", pPointer->nData);
	
	// ���� ��� ������ ������ �ݺ��Ѵ�. ���ȣ��
	if (pPointer->pNext != NULL)
		doubleTraverseNodeForward(pPointer->pNext);

}

void doubleTraverseNodeReverse(DoubleNode* pNode)
{
	DoubleNode* pPointer = pNode;

	// ����� �����͸� ����Ѵ�.
	printf("%d ", pPointer->nData);

	// ���� ��� ������ ������ �ݺ��Ѵ�. ���ȣ��
	if (pPointer->pPrev != NULL)
		doubleTraverseNodeReverse(pPointer->pPrev);

}
