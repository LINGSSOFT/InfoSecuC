#pragma once
#define _CRT_OBSOLETE_NO_WARNINGS
#include <stdio.h>
#include <malloc.h>

#define TRUE    1
#define FALSE   0

typedef struct TNode
{
	char cValue;
	struct TNode* pLeft;
	struct TNode* pRight;
}TNode;

/* ex> define error
#ifdef typedef struct Node
{
	char cValue;
	struct Node* pLeft;
	struct Node* pRight;
}Node;
#endif
*/
////////////////////////////////////////////////////////////////////
// 09. 트리 Tree
// 트리란?
// 트리(Tree)는 여러 개의 데이터들이 루트를 중심으로 계층구조를 이루고 있는 비선형
// 자료구조입니다. 계층적으로 구성하고 있는 모양이 나무의 뿌리에서 줄기가 뻗어나가는
// 모습과 비슷하다고 해서 트리 구조라는 이름으로 지어졌습니다. 일반적으로 우리나라
// 에서는 트리 또는 나무구조라고 부릅니다.
// 트리는 하나의 루트 노드를 중심으로 그 아래에 수많은 서브트리(SubTree)를 가지고
// 있습니다. 트리를 구성하는 노드는 링크(Link)를 이용해서 서로 연결되어 있습니다.
//
// 트리의 용어
// 루트노드(root node) : 트리의 시작지점을 의미합나다.
// 부모노드(parent node) : 특정 노드의 상위 노드를 의미합니다.
// 자식노드(child node) : 특정 노드의 하위 노드를 의미합니다.
// 형제노드(sibling node) : 부모 노드가 같은 노드들을 의미합니다.
// 차수(degree) : 특정 노드의 서브 트리의 개수를 의미합니다.
// 단노드(terminal node) : 차수가 0인 노드를 의미합니다.
// 간노드(nonterminal node) : 단 노드와 반대로 차수가 0이 아닌 노드를 말합니다.
// 깊이(depth) : 최대 레벨 수를 의미합니다. root 1
// 포레스트(forest) : 루트 노드를 제거했을때의 서브트리의 개수를 의미합니다.
// 조상(ancestor) : 특정 노드에서 루트 노드까지 올라가는 동안에 존재하는 모든 노드를 말합니다.
//
// 이진트리란?
// 이진트리(binary tree)란 각 노드의 차수가 2이하로 구성된 트리를 의미합니다. 모든
// 노드의 자식 노드의 개수가 0,1,2개 내에서 구성된 트리를 이진 트리라 합니다.
//
// 이진트리의 종류
// 1. 정 이진트리(full binary tree) : 모든 노드가 채워진 형태의 이진트리입니다.
// 2. 전 이진트리(complete binary tree) : 단 노드(terminal node)를 제외한 모든 노드가
//                                        채워진 형태의 이진 트리입니다.
// 3. 사향 이진트리(skewed binary tree) : 이진 트리 중에서 왼쪽 서브 트리나 오른쪽 서브
//                                        트리가 없이 한쪽 방향으로 치우친 트리입니다.
//
// 트리 운행?
// 배열이나 리스트 구조는 시작 지점부터 일정한 순서에 따라 모든 데이터를 참조할 수 있지만
// 트리 구조는 선형이 아니라 계층구조이므로 각 노드를 방문하는 방법이 독특합니다 이렇듯
// 트리를 구성하는 노드를 방문하는 것을 트리의 운행(traversal)이라고 합니다. 트리를 
// 운행하는 데에는 크게 전위 운행, 중위 운행, 후위 운행의 3가지 방법이 있습니다.

// 전위 운행(preorder traversal)											A
// 루트->왼쪽 자식노드->오른쪽 자식노드 순으로 운행합니다.				____|____
// (A-B-D-E-H-I-C-F-G)													|		|
// 중위 운행(inorder traversal)											B		C
// 왼쪽 자식노드->루트->오른쪽 자식 노드 순으로 운행합니다.			____|		|____
// (D-B-H-E-I-A-F-C-G)												D	E		F	G
// 후위 운행(postorder traversal)										|___
// 왼쪽 자식노드->오른쪽 자식노드->루트 순으로 운행합니다.				H	I
// (D-H-I-E-B-F-G-C-A)

TNode* CreateNode(char cValue);
void TreeBasic();
void DeleteTree(TNode* pNode);
void TraversePreorder(TNode* pNode);
void TraverseInorder(TNode* pNode);
void TraversePostorder(TNode* pNode);
