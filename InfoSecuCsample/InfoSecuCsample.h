#pragma once
#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <ctype.h>

#define FALSE 0
#define TRUE 1

struct bookInfo
{
	char title[40];
	char author[25];
	float price;
	int pages;
};

//////////////////////////////////////////////////////////////
// Draw Pocker Program
void	printGreeting();
int		getBet();
char	getSuit(int suit);
char	getRank(int rank);
void	getFirstand(int cardRank[], int cardSuit[]);
void	getFinalHand(int cardRank[], int cardSuit[], int finalRank[], int finalSuit[], int ranksinHand[], int suitsinHand[]);
int		analyzeHand(int ranksinHand[], int suitsinHand[]);
