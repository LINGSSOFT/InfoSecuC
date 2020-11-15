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
