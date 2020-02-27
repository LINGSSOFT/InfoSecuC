///////////////////////////////////////////////////
// Draw Pocker Program
// 이 프로그램은 Draw Poker를 구현한다. 사용자는 1부터
// 5사이의 돈을 베팅할 수 있다. 5개의 카드를 받고 어떤 
// 카드를 유지하고 어떤 카드를 버릴것인지를 선택한다.
// 최종 카드를 분석하여서 가지고 있는 카드에 따라서
// 돈이 지급된다. 사용자의 현재 잔액이 표시되고 계속할
// 것인지를 선택할 수 있다.
// 아래 2개의 상수는 가지고 있는 카드가 플러시냐 또는
// 스트레이트냐를 결정할 때 사용됨

#pragma once
#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <ctype.h>

#define TRUE 1
#define FALSE 0

void	DrawPoker();	// 드로우포커 실행 함수
void	printGreeting();
int		getBet();
char	getSuit(int suit);
char	getRank(int rank);
void	getFirstHand(int cardRank[], int cardSuit[]);
void	getFinalHand(int cardRank[], int cardSuit[], int finalRank[], int finalSuit[], int ranksinHand[], int suitsinHand[]);
int		analyzeHand(int ranksinHand[], int suitsinHand[]);
