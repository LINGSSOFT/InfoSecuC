#include "02_Hanoi.h"

////////////////////////////////////////////////////////////////////
// 02.�ϳ���ž Hanoi Tower

void Hanoi(int n, char from, char middle, char to)
{
	if (n == 1)											// ������ 1�϶�
		printf("board #%02d %c -> %c\n", n, from, to);	
	else
	{
		Hanoi(n - 1, from, to, middle);					// 1����Ģ
		printf("board #%02d %c -> %c\n", n, from, to);	// 2����Ģ
		Hanoi(n - 1, middle, from, to);					// 3����Ģ
	}
}
