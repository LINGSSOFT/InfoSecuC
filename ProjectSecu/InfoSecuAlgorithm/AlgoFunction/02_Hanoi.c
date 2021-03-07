#include "02_Hanoi.h"

////////////////////////////////////////////////////////////////////
// 02.하노이탑 Hanoi Tower

void Hanoi(int n, char from, char middle, char to)
{
	if (n == 1)											// 원판이 1일때
		printf("board #%02d %c -> %c\n", n, from, to);	
	else
	{
		Hanoi(n - 1, from, to, middle);					// 1번규칙
		printf("board #%02d %c -> %c\n", n, from, to);	// 2번규칙
		Hanoi(n - 1, middle, from, to);					// 3번규칙
	}
}
