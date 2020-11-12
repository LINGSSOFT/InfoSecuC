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
// 08. ť Queue
// ť(Queue)�� ���� ���� ������ �׸���� ������ ������ ������ �ڷ�μ� ���ð��� �޸�
// ���� �������� ���Ը� �� �� �ְ� ������ �ݴ��� �������� �� �� �ִ� �ڷᱸ���Դϴ�.
// �����Ͱ� �ԷµǴ� ��ġ�� ��µǴ� ��ġ�� �ٸ��� ������ �Էµ� ������� �����Ͱ�
// ��µǵ��� �����մϴ�. ���� �̷��� Ư�� ������ ť �ڷᱸ����
// FIFO(First In First Out) ������� �մϴ�.
//
// ť�� ����
// ť�� ����� ������ �׸�� �߿��� ���� �Էµ� ���� ���� ��µǰ� �Էµ� �������
// ��µ˴ϴ�. �׷��� ť�� FIFO���� �Ǵ� ���� ���� ����Ʈ��� �մϴ�.
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
