#pragma once
#define _CRT_OBSOLETE_NO_WARNINGS
#include <stdio.h>
#include <stdbool.h>
#include <malloc.h>

#define TRUE    1
#define FALSE   0
#define MAX_STACK_SIZE 100

typedef struct Node
{
	int nValue;
	struct Node *pNext;
}Node;

////////////////////////////////////////////////////////////////////
// 07. ���� Stack
// �����̶�?
// �����̶� �������� ������ �׸���� ������ ������ ������ �ڷ� �����μ� ����
// �������� ���ο� �׸��� �����ϰų� ���� �׸��� ������ �� �ִ� �ڷᱸ���Դϴ�.
// �����Ͱ� �ԷµǴ� ��ġ�� ��µǴ� ��ġ�� �����ϱ� ������ ���� �ֱٿ� �Էµ�
// �����Ͱ� ��µǴ� �����Դϴ�. ���� �̷��� Ư�� ������ ���� �ڷᱸ����
// LIFO(Last In First Out) ������� �մϴ�.
//
// ������ ����
// �տ� �ٷ�� �ϳ���ž ������ �̷��� ���� ������ ����� �ϰ� �ֽ��ϴ�. 3����
// ��տ� 64���� ���� �ٸ� ũ���� ������ �ٸ� ������� �ű�� �������µ�, �̵�
// ������ �������� �׿� �ִ� �����̱� ������ ���������� ������ ���ʴ��
// ������ �ϴ� ���¿����ϴ�. ������ ����(BASE)�κ��� 1,2,3,4,5..�̷��� ������
// ���ʴ�� �����Ͱ� �׿��� ����� �ϰ� �ֽ��ϴ�.
// �������� �Է°� ��µ� ��ġ�� �̸� ����Ű�� �ִ� top�����Ϳ� ���ؼ� ����˴ϴ�.
// ������ �������� 6�� �ԷµǸ� ���ο� �����Ͱ� ����ǰ� top�����ʹ� �� ĭ ����
// �̵��� �ϰ�, �ݴ�� ���� ������ �ִ� 5�� ��µǸ� top�����ʹ� ��ĭ �Ʒ���
// �̵��ϰ� �˴ϴ�. ���ÿ� �����͸� �߰��ϴ� ������ Ǫ��(push)��� �ϸ�, ���ÿ�
// �ִ� �����͸� ������ ������ ��(pop)�̶�� �մϴ�. ���ÿ��� ������ �� �ִ�
// �������� �Ѱ�(limit)�� �ִµ� �̸� �ʰ��ؼ� �����͸� Ǫ���ϴ� ��츦
// �����÷ο�(overflow)�� �ϸ�, �ݴ�� �����Ͱ� �� �̻� ���� ���¿��� �����͸�
// ���Ϸ��� ��츦 ����÷ο�(underflow)��� �մϴ�.
//
// ���ñ����ϱ�
// �迭/���Ḯ��Ʈ�� �̿��Ͽ� ���ñ����ϱ� ������
// �迭�� �̿��ؼ� ������ ������ ��쿡�� �迭�� ������ �� �ִ� ũ�⸦ �����ؼ�
// �迭�� ũ�⸦ �����ؾ� �ϹǷ� ������ ����Ǵ� �������� ���� ���� ��쿡��
// �׸�ŭ �޸� ����� ȿ������ �������ϴ�. �ݸ� ����Ʈ�� �̿��ؼ� ������ ��쿡��
// ����� �޸� ������ �Ϸķ� ������ �ʿ䰡 ������ ���� ������ �ʿ��� ������
// �Ҵ��ؼ� ������ �� �����Ƿ� �޸𸮸� ���� ȿ�������� ����� �� �ֽ��ϴ�.
// �ٸ� ���� ����Ʈ ������ �ʿ��� ��ũ ������ ������ ������ �߰������� �ʿ��ϰ�
// �Ǹ� �ӵ��� �迭�� ���� �����ϴ�.

void StackSample();
bool arrIsEmpty();
bool arrIsFull();
bool arrPush(int nValue);
bool arrPop(int* pnValue);
int arrGetDataCount();
void arrPrintStack();

bool linkIsFull();
bool linkIsEmpty();
bool linkPush(int nValue);
bool linkPop(int* pnValue);
bool linkGetDataCount();
void linkPrintStack();
void linkRemoveStack();