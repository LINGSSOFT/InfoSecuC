#pragma once
#define _CRT_OBSOLETE_NO_WARNINGS
#include <stdio.h>

#define TRUE    1
#define FALSE   0
#define MAX_SIZE 10								// 마방진 배열 사이즈

////////////////////////////////////////////////////////////////////
// 04. 마방진 Magic Square
// 중국 전설의 주인공인 한나라의 우임금은 홍수를 다스리려고 황하강의 지류에서 물길을
// 고치다가 거북 등껍질에 새겨진 이상한 그림을 얻었다고 합니다. 낙서라 불리는 이 그림에는
// 1부터 9까지의 숫자가 3*3의 정사각형에 배열되 있었다고 합니다. 그런데 신기한 것은
// 늘어선 숫자들이 가로,세로, 대각선 어느 방향으로 더해도 그 합이 15가 되는 것이었습니다.
// 그래서 고대 중국에서는 이 낙서가 우주의 원리를 포함하고 있다고 여기고 신비한 힘을
// 가지고 있다고 생각하게 되었습니다. 이처럼 1부터 연이은 숫자를 정사각형 모양으로 배열해
// 가로와 세로, 대각선 방향의 합이 모두 같도록 배열한 것을 "마방진"이라고 합니다.
// 마방진에는 가로,세로를 구성하는 항목의 개수에 따라서 여러 차수의 마방진이 존재합니다.
// 즉 가로 세로가 3개의 항목으로 구성된 낙서는 3차 마방진이 되며, 4개의 항목으로
// 구성되면 4차 마방진이라고 합니다. 옛날부터 많은 사람들이 이와 같이 신비로운 마방진에
// 몰두해왔는데 지금 이 순간에도 새로운 마방진 해법이 연구되고 있으며 세상에 소개되고
// 있습니다. 여기에서는 가장 많이 쓰이고 있는 홀수 마방진과 4의 배수 마방진을 알아봅니다.
//
//		4 9 2
//		3 5 7	위아래 대각선 합이 같은값이 나옵니다.
//		8 1 6
//
// 홀수차 마방진
// 먼저 3,5,7,9...홀수차 마방진 문제를 해결하는 방법
// 1. 첫번째 숫자를 1행의 중앙열에 넣는다.
// 2. 대각선 방향(한 칸 위와 한 칸 옆)으로 이동한 곳에 다음 숫자를 넣는다. 만약 위치가
//    영역을 벗어날 경우 반대 행이나 열에 숫자를 넣는다.
// 3. 위의 단계를 이미 숫자가 저장되어 있는 칸을 만날 때까지 n회 만큼 반복한다.
// 4. 3단계의 마지막 숫자를 넣은 곳에서 바로 아래 칸에 다음 숫자를 넣고 위의 2단계부터
//    다시 반복한다.
//
//		0 1 0	0 1 0	0 1 0
//	->	0 0 0	0 0 0	3 0 0	->
//		0 0 0	0 0 2	0 0 2
//
//		0 1 0	0 1 0	0 1 6 
//	->	3 0 0	3 5 0	3 5 0	->
//		4 0 2	4 0 2	4 0 2
//
//		0 1 6	8 1 6		8 1 6
//	->	3 5 7	3 5 7	=	3 5 7
//		4 0 2	4 0 2		4 9	2
//

void GetOddSquare(int n);
void InitSquare();
void ShowSquare(int n);

// 4의 배수 마방진
// 홀수차 마방진만큼이나 자주 사용되는 4의 배수 마방진의 풀이 방법에 대해서 알아보도록
// 하겠습니다. 4의 배수 마방진이란 n의 값이 4,8,12...와 같이 4의 배수로 구성된 마방진을 말합니다.
// 4의 배수 마방진 문제를 해결하는 방법
// 1. 다음과 같이 1 : 2 : 1의 비율로 가로,세로를 3등분하여 9개의 구역으로 나눕니다.
// 2. A,C,E,G,I영역은 정방향으로 1부터 숫자를 차례대로 길록합니다.
// 3. 나머지 영역은 끝에서부터 역방향으로 1부터 숫자를 차례대로 기록합니다.
//
//		A B B C			1  B  B  4		1  15 14 4
//		D E E F			D  6  7  F		12 6  7  9
//	->	D E E F		->	D  10 11 F	->	8  10 11 5
//		G H H I			13 H  H  16		13 3  2  16
//

/* 4의 배수 마방진
int IsInBlock(int n, int row, int col);
void InitSquare();
void GetQuaterSquare(int n);
void ShowSquare(int n);
*/
