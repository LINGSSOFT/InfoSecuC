///////////////////////////////////////////////////
// Draw Pocker Program
// �� ���α׷��� Draw Poker�� �����Ѵ�. ����ڴ� 1����
// 5������ ���� ������ �� �ִ�. 5���� ī�带 �ް� � 
// ī�带 �����ϰ� � ī�带 ������������ �����Ѵ�.
// ���� ī�带 �м��Ͽ��� ������ �ִ� ī�忡 ����
// ���� ���޵ȴ�. ������� ���� �ܾ��� ǥ�õǰ� �����
// �������� ������ �� �ִ�.
// �Ʒ� 2���� ����� ������ �ִ� ī�尡 �÷��ó� �Ǵ�
// ��Ʈ����Ʈ�ĸ� ������ �� ����

#pragma once
#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <ctype.h>

#define TRUE 1
#define FALSE 0

void	DrawPoker();	// ��ο���Ŀ ���� �Լ�
void	printGreeting();
int		getBet();
char	getSuit(int suit);
char	getRank(int rank);
void	getFirstHand(int cardRank[], int cardSuit[]);
void	getFinalHand(int cardRank[], int cardSuit[], int finalRank[], int finalSuit[], int ranksinHand[], int suitsinHand[]);
int		analyzeHand(int ranksinHand[], int suitsinHand[]);
