#pragma once
#define _CRT_OBSOLETE_NO_WARNINGS
#include <stdio.h>

#define TRUE    1
#define FALSE   0
#define MAX_SIZE 10
#define MATRIX_SIZE 5

////////////////////////////////////////////////////////////////////
// 05. 배열 Array
// 자료구조와 알고리즘
// 컴퓨터는 입력되는 데이터를 빠르게 연산하여 정확한 결과를 출력하는 기능을 수행합니다.
// 많은 양의 데이터에 대해서 복잡한 연산을 수행하면서도 빠르고 정확한 처리를 하는데에는
// 데이터를 어떻게 다루고 저장하느냐에 따라서 많은 차이를 보이게 됩니다.
// 컴퓨터는 데ㅣ터 처리를 위해서 메모리라는 저장공간으로부터 CPU(중앙처리장치)를 거쳐
// 어떠한 연산을 거치게 됩니다. 메모리는 낮은 번지에서 높은 번지까지 수많은 저장공간으로
// 구성되어 있습니다. 자료를 다루는 목적에 따라 어떠한 위치에 어떠한 방법으로 저장하는
// 것이 가장 효율적인지 결정됩니다. 컴퓨터 알고리즘 서적에서 자료구조를 빠뜨리지 않고
// 설명하는 이유가 바로 여기에 있습니다. 데이터를 효율적으로 합리적으로 다루는 것이
// 프로그램에 있어서 가장 기본적이고 궁극적으로 중요한 부분이기 때문입니다.
//
// 배열의 필요성
// 프로그램에서 처리에 필요한 데이터를 저장하기 위해서 변수를 선언하고 이를 이용해서 값을
// 저장하고 읽습니다. 하지만 처리해야 할 데이터가 매우 많다면 일일이 변수를 선언해서 각각
// 변수명을 정리해줘야 할까요? 이와 같이 동일한 자료형으로 구성된 많은 데이터를 저장하기
// 위하여 제공되는 것이 바로 배열이며, 이는 가장 기본적인 자료구조입니다.
// 프로그래밍에서 배열(Array)은 동일한 성질을 가지고 있는 자료들의 집합을 의미합니다.
//
// 배열의 특징
// 1. 배열은 색인(index)과 값(value)을 가지는 집합이다.
// 2. 배열은 선언시 크기가 정해져 있어야 한다.
//
// 배열의 선언
// int arrScore[10];	int(자료형) arrScore(배열명)[10](배열의 크기);
// 
// 배열값 저장
// arrScore[0] = 5;		arrScore(배열명)[0](첨자)
// 
// ex> 배열 삽입/삭제/찾기 Sample
//
// 배열의 장점
// 일반적으로 데이터란 복수개의 자료가 모여진 집합을 의미하므로 배열은 이러한 여러 개의
// 데이터를 저장하고 처리하기 위한 가장 기본적인 자료구조로 사용됩니다.흔히 배열은 리스트
// 구조와 비교해서 아래와 같은 장점과 단점을 가집니다.
// 장점
// 1. 배열내의 특정 데이터에 빠르게 접근할 수 있습니다.
// 2. 정적인 데이터 처리에서 기억장소를 효율적으로 활용할 수 있습니다.
// 단점
// 1. 배열내의 특정 위치에 데이터를 삽입하거나 삭제할 경우 많은 작업을 필요로 합니다.
// 2. 배열의 크기를 최대로 선언해야 합니다.(메모리 측면세서 비효율적이며 낭비임)
//
// 2차원 배열을 이용하여 출력하는 프로그램
// *****
//
// *****
//
// *****

// 배열기본 초기화, 추가, 삭제, 찾기
void arrayBasic();
void Insert(int nArray[], int nIndex, int nValue);
void Delete(int nArray[], int nIndex);
int Find(int nArray[], int nValue);
void Print(int nArray[]);

// 행렬예제
void ArrayExample01();
void ArrayExample02();
void ArrayExample03();
void PrintcArray(char cArray[][MATRIX_SIZE]);
void PrintnArray(int nArray[][MATRIX_SIZE]);
