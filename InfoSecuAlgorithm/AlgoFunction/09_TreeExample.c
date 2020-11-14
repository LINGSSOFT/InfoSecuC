#include "09_TreeExample.h"

////////////////////////////////////////////////////////////////////
// 09. Ʈ�� Tree

TNode* CreateNode(char cValue)
{
	TNode* pNode = NULL;
	pNode = (TNode*)malloc(sizeof(TNode));
	if (pNode == NULL)
		return NULL;

	pNode->cValue = cValue;
	pNode->pLeft = NULL;
	pNode->pRight = NULL;

	printf("%c ��尡 �����Ǿ����ϴ�.\n", pNode->cValue);

	return pNode;
}

void DeleteTree(TNode* pNode)
{
	if (pNode != NULL)
	{
		if (pNode->pLeft != NULL)
			DeleteTree(pNode->pLeft);
		if (pNode->pRight != NULL)
			DeleteTree(pNode->pRight);

		printf("%c ��带 �����մϴ�.\n", pNode->cValue);
		free(pNode);
	}
}

void TraversePreorder(TNode* pNode)
{
	if (pNode != NULL)
	{
		printf("%c ", pNode->cValue);
		TraversePreorder(pNode->pLeft);
		TraversePreorder(pNode->pRight);
	}
}

void TraverseInorder(TNode* pNode)
{
	if (pNode != NULL)
	{
		TraverseInorder(pNode->pLeft);
		printf("%c ", pNode->cValue);
		TraverseInorder(pNode->pRight);
	}
}

void TraversePostorder(TNode* pNode)
{
	if (pNode != NULL)
	{
		TraversePostorder(pNode->pLeft);
		TraversePostorder(pNode->pRight);
		printf("%c ", pNode->cValue);
	}
}

void TreeBasic()
{
	TNode* pRoot = NULL;
	pRoot = CreateNode('A');

	pRoot->pLeft = CreateNode('B');
	pRoot->pRight = CreateNode('C');

	pRoot->pLeft->pLeft = CreateNode('D');
	pRoot->pLeft->pRight = CreateNode('E');

	pRoot->pRight->pLeft = CreateNode('F');
	pRoot->pRight->pRight = CreateNode('G');

	pRoot->pLeft->pRight->pLeft = CreateNode('H');
	pRoot->pLeft->pRight->pRight = CreateNode('I');

	printf("Inorder : ");
	TraverseInorder(pRoot);
	printf("\n");

	printf("Preorder : ");
	TraversePreorder(pRoot);
	printf("\n");

	printf("Inorder : ");
	TraversePostorder(pRoot);
	printf("\n");

	DeleteTree(pRoot);
}
