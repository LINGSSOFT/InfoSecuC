#include "InfoSecuCsample.h"
#include "DrawPoker.h"

int main()
{
	int nSelect;
	printf("//////////////////////\n");
	printf("// 1. Drawpoker \n");
	printf("// 2. none \n");
	printf("// 3. none \n");
	printf("// 4. none \n");
	printf("//////////////////////\n");
	scanf("%d", &nSelect);

	switch (nSelect)
	{
	case 1:
		DrawPoker();
		break;
	case 2:
	case 3:
	case 4:
	default:
		break;
	}

	return 0;
}
