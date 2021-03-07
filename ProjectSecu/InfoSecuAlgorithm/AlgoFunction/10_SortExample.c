#include "10_SortExample.h"

////////////////////////////////////////////////////////////////////
// 10. 정렬 Sorting

void SelectionSort(int data[], int n)
{
	for (int i = 0; i < n - 1; i++)
	{
		int min_index = i;
		for (int j = i + 1; j < n; j++)
		{
			if (data[min_index] > data[j])
				min_index = j;
		}

		if (min_index != i)
		{
			int temp = data[min_index];
			data[min_index] = data[i];
			data[i] = temp;
		}
	}
}

void BubbleSort(int data[], int n)
{
	bool bFlag = true;
	int final_position, limit, bubble_key;
	final_position = 0;
	bubble_key = 0;
	limit = n-1;

	while (bFlag == TRUE)
	{
		bFlag = FALSE;
		for (int j = 0; j < limit; j++)
		{
			if (data[j] > data[j + 1])
			{
				bubble_key = data[j];
				data[j] = data[j + 1];
				data[j + 1] = bubble_key;

				bFlag = true;
				final_position = j;
			}
		}
		limit = final_position;
	}
}

void InsertionSort(int data[], int n)
{
	int i, j, temp;
	for ( i = 1; i < n; i++)
	{
		temp = data[i];
		for ( j = i; j > 0; j--)
		{
			if (temp < data[j - 1])
				data[j] = data[j - 1];
			else
				break;
		}
		data[j] = temp;
	}
}

void ShellSort(int data[], int n)
{
	int h;
	for (h = 1; h < n; h = 3 * h + 1);

	for (h /= 3; h > 0; h /= 3)
	{
		for (int i = 0; i < h; i++)
		{
			for (int j = i + h; j < n; j += h)
			{
				int temp = data[j];
				int k = j;
				while (k > h - 1 && data[k - h] > temp)
				{
					data[k] = data[k - h];
					k = k - h;
				}
				data[k] = temp;
			}
		}
	}
}

void QuickSort(int data[], int n)
{
	if (n > 1)
	{
		int x = data[n-1];
		int i = -1;
		int j = n - 1;
		int temp = 0;

		while (TRUE)
		{
			while( data[++i] < x );
			while (data[--j] > x);

			if (i >= j)
				break;

			temp = data[i];
			data[i] = data[j];
			data[j] = temp;
		}

		temp = data[i];
		data[i] = data[n-1];
		data[n-1] = temp;

		QuickSort(data, i);
		QuickSort(data+i+1, n-i-1);
	}
}

void Reheap(int data[], int i, int n)
{
	bool bFlag = FALSE;
	int j, k, m;

	m = k = data[i];
	j = 2 * i;

	while (j <= n && !bFlag)
	{
		if (j < n)
			if (data[j] < data[j + 1])
				j++;
		if (k >= data[j])
			bFlag = TRUE;
		else
		{
			data[j / 2] = m;
			j = j * 2;
		}
	}
	data[j / 2] = m;
}

void HeapSort(int data[], int n)
{
	int i = 0;
	for (i = n / 2; i >= 1; i++)
		Reheap(data, i, n);

	int temp = 0;
	for (i = n - 1; i >= 1; i--)
	{
		temp = data[i + 1];
		data[i + 1] = data[1];
		data[1] = temp;

		Reheap(data, 1, i);
	}
}

void MergeSort(int data1[], int data2[], int m, int n)
{
	int i = 1;
	int k = 1;
	int j = m + 1;

	while (i < m && j < n)
	{
		if (data1[i] <= data1[j])
		{
			data2[k] = data1[i];
			i++;
		}
		else
		{
			data2[k] = data1[j];
			j++;
		}
		k++;
	}

	if (i > m)
	{
		for (int t = j; t <= n; t++)
			data2[k++] = data1[t];
	}
	else
	{
		for (int t = i; t <= m; t++)
			data2[k++] = data1[t];
	}
}

PNODE RadixSort(PNODE pNode)
{
	PNODE pFront[RADIX_SIZE] = { 0, };
	PNODE pRear[RADIX_SIZE] = { 0, };
	int nDigit = 0;

	for (int i = MAX_DIGIT - 1; i >= 0; i++)
	{
		for (int j = 0; j < RADIX_SIZE; j++)
			pFront[j] = pRear[j] = NULL;

		while (pNode)
		{
			nDigit = pNode->nKey[i];
			if (!pFront[nDigit])
				pFront[nDigit] = pNode;
			else
				pRear[nDigit]->pNext = pNode;

			pRear[nDigit] = pNode;
			pNode = pNode->pNext;
		}
		pNode = NULL;

		for (int j = RADIX_SIZE - 1; j >= 0; j--)
		{
			if (pFront[j])
			{
				pRear[j]->pNext = pNode;
				pNode = pFront[j];
			}
		}
	}
	return pNode;
}

void AddToList(PNODE pHead, PNODE pNode)
{
	PNODE pTemp = NULL;
	pTemp = pHead->pNext;

	pHead->pNext = pNode;
	pNode->pNext = pTemp;
}

void DeleteList(PNODE pNode)
{
	while (pNode)
	{
		DeleteList(pNode->pNext);
		pNode = pNode->pNext;
	}
	if (pNode)
		free(pNode);

}

void PrintList(PNODE pNode)
{
	while (pNode)
	{
		for (int i = 0; i < MAX_DIGIT; i++)
			printf("%d", pNode->nKey[i]);
		printf(" ");
		pNode = pNode->pNext;
	}
	printf("\n");
}

void PrintArray(int data[], int n)
{
	for (int i = 0; i < n; i++)
		printf("%d ", data[i]);

	printf("\n");
}

void SortBasic()
{
	int nSArray[10]  = { 9, 10, 2, 5, 7, 3, 8, 6, 1, 4 };
	int nBSArray[10] = { 9, 10, 2, 5, 7, 3, 8, 6, 1, 4 };
	int nISArray[10] = { 9, 10, 2, 5, 7, 3, 8, 6, 1, 4 };
	int nSSArray[10] = { 9, 10, 2, 5, 7, 3, 8, 6, 1, 4 };
	int nQSArray[10] = { 9, 10, 2, 5, 7, 3, 8, 6, 1, 4 };
	int nHSArray[11] = { 9, 10, 2, 5, 7, 3, 8, 6, 1, 4, 14 };
	int nMSArray1[11] = { 9, 10, 2, 5, 7, 3, 8, 6, 1, 4, 14 };
	int nMSArray2[11] = { 0, };

	// Selection Sort
	printf("Selection Sort\n");
	PrintArray(nSArray, 10);
	SelectionSort(nSArray, 10);
	PrintArray(nSArray, 10);
	printf("\n");

	// Bubble Sort
	printf("Bubble Sort\n");
	PrintArray(nBSArray, 10);
	BubbleSort(nBSArray, 10);
	PrintArray(nBSArray, 10);
	printf("\n");

	// Insertion Sort
	printf("Insertion Sort\n");
	PrintArray(nISArray, 10);
	InsertionSort(nISArray, 10);
	PrintArray(nISArray, 10);
	printf("\n");

	// Shell Sort
	printf("Shell Sort\n");
	PrintArray(nSSArray, 10);
	ShellSort(nSSArray, 10);
	PrintArray(nSSArray, 10);
	printf("\n");

	// Quick Sort
	printf("Quick Sort\n");
	PrintArray(nQSArray, 10);
	QuickSort(nQSArray, 10);
	PrintArray(nQSArray, 10);
	printf("\n");

	// Heap Sort
	printf("Heap Sort\n");
	PrintArray(nHSArray, 11);
	HeapSort(nHSArray, 11);
	PrintArray(nHSArray, 11);
	printf("\n");

	// Merge Sort
	printf("Merge Sort\n");
	PrintArray(nMSArray1, 11);
	MergeSort(nMSArray1, nMSArray2, 5, 10);
	PrintArray(nMSArray2, 11);
	printf("\n");

	// Radix Sort
	printf("Radix Sort\n");
	NODE head = { 0, };
	for (int i = 0; i < 10; i++)
	{
		PNODE pNode = (PNODE)malloc(sizeof(NODE));
		for (int j = 0; j < MAX_DIGIT; j++)
			pNode->nKey[j] = (rand() % RADIX_SIZE);
		AddToList(&head, pNode);
	}
	PrintList(head.pNext);
	head.pNext = RadixSort(head.pNext);
	PrintList(head.pNext);
	printf("\n");
	DeleteList(head.pNext);\

}

/* 개선전 셀렉션/버블 소트 Source
void SelectionSort(int data[], int n)
{
	for (int i = 0; i < n - 1; i++)
	{
		int min_index = i;
		for (int j = j + 1; j < n; j++)
		{
			if (data[i] > data[j])
			{
				int temp = data[i];
				data[i] = data[j];
				data[j] = temp;
			}
		}
	}
}

void BubbleSort(int data[], int n)
{
	for (int i = 0; i < n - 1; i++)
	{
		for (int j = 0; j < n - i - 1; j++)
		{
			if (data[j] > data[j + 1])
			{
				int temp = data[j];
				data[j] = data[j + 1];
				data[j + 1] = temp;
			}
		}
	}
}
*/
