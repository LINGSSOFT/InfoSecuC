#pragma once
#define _CRT_OBSOLETE_NO_WARNINGS
#include <stdio.h>
#include <stdbool.h>
#include <malloc.h>

#define TRUE    1
#define FALSE   0
#define MAX_QUEUE_SIZE 100

typedef struct QNode
{
	int nValue;
	struct QNode* pNext;
}QNode;

////////////////////////////////////////////////////////////////////
// 08. 큐 Queue
// 큐(Queue)는 여러 개의 데이터 항목들이 일정한 순서로 나열된 자료로서 스택과는 달리
// 한쪽 끝에서는 삽입만 할 수 있고 삭제는 반대쪽 끝에서만 할 수 있는 자료구조입니다.
// 데이터가 입력되는 위치와 출력되는 위치가 다르기 때문에 입력된 순서대로 데이터가
// 출력되도록 구현합니다. 따라서 이러한 특성 때문에 큐 자료구조를
// FIFO(First In First Out) 구조라고 합니다.
//
// 큐의 원리
// 큐에 저장된 데이터 항목들 중에서 먼저 입력된 것이 먼저 출력되고 입력된 순서대로
// 출력됩니다. 그래서 큐를 FIFO구조 또는 선입 선출 리스트라고 합니다.
//
void QueueSample();
bool QarrIsEmpty();
bool QarrIsFull();
bool QarrPut(int nValue);
bool QarrGet(int* pnValue);
int QarrGetDataCount();
void QarrPrintQueue();

bool QlinkIsFull();
bool QlinkIsEmpty();
bool QlinkPut(int nValue);
bool QlinkGet(int* pnValue);
int QlinkGetDataCount();
void QlinkPrintQueue();
void QlinkRemoveQueue();
