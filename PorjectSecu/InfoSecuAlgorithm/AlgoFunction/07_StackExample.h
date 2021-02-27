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
// 07. 스택 Stack
// 스택이란?
// 스택이란 여러개의 데이터 항목들이 일정한 순서로 나열된 자료 구조로서 한쪽
// 끝에서만 새로운 항목을 삽입하거나 기존 항목을 삭제할 수 있는 자료구조입니다.
// 데이터가 입력되는 위치와 출력되는 위치가 동일하기 때문에 가장 최근에 입력된
// 데이터가 출력되는 형태입니다. 따라서 이러한 특성 때문에 스택 자료구조를
// LIFO(Last In First Out) 구조라고 합니다.
//
// 스택의 원리
// 앞에 다뤘던 하노이탑 문제도 이러한 스택 구조에 기반을 하고 있습니다. 3개의
// 기둥에 64개의 서로 다른 크기의 원판을 다른 기둥으로 옮기는 문제였는데, 이들
// 원판이 차곡차곡 쌓여 있는 형태이기 때문에 위에서부터 원판을 차례대로
// 빼내야 하는 형태였습니다. 스택은 기저(BASE)로부터 1,2,3,4,5..이러한 순서로
// 차례대로 데이터가 쌓여진 모습을 하고 있습니다.
// 데이터의 입력과 출력될 위치는 이를 가리키고 있는 top포인터에 의해서 제어됩니다.
// 새러운 데이터인 6이 입력되면 새로운 데이터가 저장되고 top포인터는 한 칸 위로
// 이동을 하고, 반대로 가장 상위에 있는 5가 출력되면 top포인터는 한칸 아래로
// 이동하게 됩니다. 스택에 데이터를 추가하는 행위를 푸쉬(push)라고 하며, 스택에
// 있는 데이터를 빼내는 행위를 팝(pop)이라고 합니다. 스택에는 저장할 수 있는
// 데이터의 한계(limit)가 있는데 이를 초과해서 데이터를 푸시하는 경우를
// 오버플로우(overflow)라 하며, 반대로 데이터가 더 이상 없는 상태에서 데이터를
// 팝하려는 경우를 언더플로우(underflow)라고 합니다.
//
// 스택구현하기
// 배열/연결리스트를 이용하여 스택구현하기 차이점
// 배열을 이용해서 구현한 스택의 경우에는 배열을 선언할 때 최대 크기를 예상해서
// 배열의 크기를 지정해야 하므로 실제로 저장되는 데이터의 양이 적은 경우에는
// 그만큼 메모리 사용의 효율성이 떨어집니다. 반면 리스트를 이용해서 구현할 경우에는
// 사용할 메모리 공간이 일렬로 존재할 필요가 없으며 저장 공간이 필요할 때마다
// 할당해서 저장할 수 있으므로 메모리를 보다 효율적으로 사용할 수 있습니다.
// 다만 연결 리스트 구성에 필요한 링크 정보를 저장할 공간이 추가적으로 필요하게
// 되며 속도가 배열에 비해 느립니다.

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
