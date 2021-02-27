#include "DrawPoker.h"

// DrawPoker ���� �Լ�
void	DrawPoker()
{
	int bet, i, winnings;
	int bank = 100;
	int cardRank[5];
	int cardSuit[5];
	int finalRank[5];
	int finalSuit[5];
	int ranksinHand[13];
	int suitsinHand[4];
	time_t t;
	char suit, rank, stillPlay;

	// �λ縻 ���, �ѹ��� ����Ͽ� �ݺ����� �ܺη� ������
	printGreeting();

	// ����ڰ� ��� ��Ŀ�� �ѹ� �� ������ �ݺ��Ѵ�.
	do
	{
		bet = getBet();

		srand(time(&t));
		getFirstHand(cardRank, cardSuit);

		printf("�������� 5���� ī��: \n");

		for (i = 0; i < 5; i++)
		{
			suit = getSuit(cardSuit[i]);
			rank = getRank(cardRank[i]);
			printf("ī�� #%d: %c%c\n", i + 1, rank, suit);
		}

		// ������� �տ� �ִ� ī�尪�� ����� �� ���
		// �迭 �ʱ�ȭ
		for (i = 0; i < 4; i++)
			suitsinHand[i] = 0;
		for (i = 0; i < 13; i++)
			ranksinHand[i] = 0;

		getFinalHand(cardRank, cardSuit, finalRank, finalSuit, ranksinHand, suitsinHand);

		printf("�������� �������� ī��:\n");

		for (i = 0; i < 5; i++)
		{
			suit = getSuit(finalSuit[i]);
			rank = getRank(finalRank[i]);
			printf("ī�� #%d: %c%c\n", i + 1, rank, suit);
		}

		winnings = analyzeHand(ranksinHand, suitsinHand);

		printf("�������� �̰���ϴ�:%d\n", bet * winnings);

		bank = bank - bet + (bet * winnings);

		printf("\n�������� ���� �ܾ��� %d\n", bank);

		printf("\n����Ͻðڽ��ϱ�?");
		scanf_s("%c", &stillPlay, 1);

	} while (toupper(stillPlay) == 'Y');
}

// �λ縻�� �Բ� �������� ī�� ���տ� ���� ������ �˷��ش�.
void printGreeting()
{
	printf("*************************************************************************\n");
	printf("\n\n���� �ʺ����� ī���뿡 ���� ���� ȯ���մϴ�.\n\n");
	printf("\t��� ��Ŀ�� Ȩ�Դϴ�.\n\n");
	printf("*************************************************************************\n");
	printf("��Ģ�� ������ �����ϴ�:\n");
	printf("100������ �����մϴ�. �ѹ� ������");
	printf("1������ 5�� �����Դϴ�.\n");
	printf("ó���� 5���� ī�带 �޽��ϴ�.");
	printf("��� ī�带 ������ � ī�带 ����������");
	printf("������ �� �ֽ��ϴ�.\n");
	printf("\nī�� ���տ� ���� ������ ������ �����ϴ�. (1���� �����Ѵٰ�");
	printf("�����մϴ�.):");
	printf("\n���\t\t\t\t1��");
	printf("\n�� ��� \t\t\t2��");
	printf("\n���� ī��\t\t\t3��");
	printf("\n��Ʈ����Ʈ\t\t\t4��");
	printf("\n�÷���\t\t\t\t5��");
	printf("\nǮ �Ͽ콺\t\t\t8��");
	printf("\n��Ŀ\t\t\t10��");
	printf("\n��Ʈ����Ʈ �÷���\t\t\t20��");
	printf("\n\n��ſ� �ð� ��������!!\n\n");
}

// ù ��° 5���� ó���ϴ� �Լ�
void getFirstHand(int cardRank[], int cardSuit[])
{
	int i, j;
	int cardDup;

	for (i = 0; i < 5; i++)
	{
		cardDup = 0;
		do
		{
			cardRank[i] = (rand() % 13);
			cardSuit[i] = (rand() % 4);

			for (j = 0; j < i; j++)
			{
				if ((cardRank[i] == cardRank[j]) && (cardSuit[i] == cardSuit[j]))
					cardDup = 1;
			}
		} while (cardDup == 1);
	}
}

// ī�� ���̿� ���� ���ڸ� �ο��ϴ� �Լ�
char getSuit(int suit)
{
	switch (suit)
	{
	case 0:
		return('c');
	case 1:
		return('d');
	case 2:
		return('h');
	case 3:
		return('s');
	}
}

// ī�� ���ڸ� ��ũ ���ڷ� �����ϴ� �Լ�
char getRank(int rank)
{
	switch (rank)
	{
	case 0:
		return('A');
	case 1:
		return('2');
	case 2:
		return('3');
	case 3:
		return('4');
	case 4:
		return('5');
	case 5:
		return('6');
	case 6:
		return('7');
	case 7:
		return('8');
	case 8:
		return('9');
	case 9:
		return('T');
	case 10:
		return('J');
	case 11:
		return('Q');
	case 12:
		return('K');
	}
}

// ����ڷκ��� 1���� 5������ ������ �Է��ϴ� �Լ�
int getBet()
{
	int bet;

	// ����ڰ� 0���� 5�� �Է��ϴ� �� ��� �ݺ�
	do
	{
		printf("�󸶳� ���ðڽ��ϱ�?(���ڷ� �Է� ");
		printf("1���� 5����, �����Ϸ��� 0): ");
		scanf_s("%d", &bet);

		if (bet >= 1 && bet <= 5)
		{
			return(bet);
		}
		else if (bet == 0)
		{
			exit(1);
		}
		else
		{
			printf("\n\n1���� 5������ ���ڸ� �Է����ּ��� ");
			printf("�����Ϸ��� 0�� �Է����ּ���.\n");
		}
	} while ((bet < 0) || (bet > 5));
}

// ������ �Լ��ν� ���� ī�带 �м��Ͽ� ������ ����Ѵ�.
int analyzeHand(int ranksinHand[], int suitsinHand[])
{
	int num_consec = 0;
	int i, rank, suit;
	int straight = FALSE;
	int flush = FALSE;
	int four = FALSE;
	int three = FALSE;
	int pairs = 0;

	for (suit = 0; suit < 4; suit++)
		if (suitsinHand[suit] == 5)
			flush = TRUE;
	rank = 0;
	while (ranksinHand[rank] == 0)
		rank++;

	for (; rank = 13 && ranksinHand[rank]; rank++)
		num_consec++;

	if (num_consec == 5)
		straight = TRUE;

	for (rank = 0; rank < 13; rank++)
	{
		if (ranksinHand[rank] == 4)
			four = TRUE;
		if (ranksinHand[rank] == 3)
			three = TRUE;
		if (ranksinHand[rank] == 2)
			pairs++;
	}

	if (straight && flush)
	{
		printf("��Ʈ����Ʈ �÷���\n\n");
		return(20);
	}
	else if (four)
	{
		printf("��Ŀ\n\n");
		return(10);
	}
	else if (three && pairs == 1)
	{
		printf("Ǯ �Ͽ콺\n\n");
		return (8);
	}
	else if (flush)
	{
		printf("�÷���\n\n");
		return (5);
	}
	else if (straight)
	{
		printf("��Ʈ����Ʈ\n\n");
		return (4);
	}
	else if (three)
	{
		printf("���� ī��\n\n");
		return (3);
	}
	else if (pairs == 2)
	{
		printf("�� ���\n\n");
		return (2);
	}
	else if (pairs == 1)
	{
		printf("pair\n\n");
		return (1);
	}
	else
	{
		printf("High Card\n\n");
		return (0);
	}
}

// �� �Լ��� ó�� �տ� �� 5���� ī�带 �ϳ��� ������
// ����ڿ��� �� ī�带 ������ �������� �����Ѵ�.
// ����ڰ� NO��� �ϸ� ī�带 ��ü�Ѵ�.
void getFinalHand(int cardRank[], int cardSuit[], int finalRank[], int finalSuit[], int ranksinHand[], int suitsinHand[])
{
	int i, j, cardDup;
	char suit, rank, ans;

	for (i = 0; i < 5; i++)
	{
		suit = getSuit(cardSuit[i]);
		rank = getRank(cardRank[i]);

		printf("�� ī�带 �����Ͻðڽ��ϱ� #%d: %c%c?", i + 1, rank, suit);
		printf("\n�亯���ּ���(Y/N): ");
		scanf_s("%c", &ans);

		if (toupper(ans) == 'Y')
		{
			finalRank[i] = cardRank[i];
			finalSuit[i] = cardSuit[i];
			ranksinHand[finalRank[i]]++;
			suitsinHand[finalSuit[i]]++;
			continue;
		}
		else if (toupper(ans) == 'N')
		{
			cardDup = 0;
			do {
				cardDup = 0;

				finalRank[i] = (rand() % 13);
				finalSuit[i] = (rand() % 4);

				// ���ο� ī�带 5���� �� ī��� ��ġ�� �ʴ����� �˻��Ѵ�.
				for (j = 0; j < 5; j++)
				{
					if ((finalRank[i] == cardRank[j]) && (finalSuit[i] == cardSuit[j]))
					{
						cardDup = 1;
					}
				}

				// ���� ���� ī�峢�� ��ġ�� �ʴ����� �˻��Ѵ�.
				for (j = 0; j < i; j++)
				{
					if ((finalRank[i] == finalRank[j]) && (finalSuit[i] == finalSuit[j]))
					{
						cardDup = 1;
					}
				}
			} while (cardDup == 1);

			ranksinHand[finalRank[i]]++;
			suitsinHand[finalSuit[i]]++;
		}
	}
}
