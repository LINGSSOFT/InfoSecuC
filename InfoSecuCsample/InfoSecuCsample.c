#include "InfoSecuCsample.h"

int main()
{
    FILE* fptr;
    int ctr;
    struct bookInfo books[3];
    for (ctr = 0; ctr < 3; ctr++)
    {
        printf("책 #%d의 이름은?\n", (ctr + 1));
        gets(books[ctr].title);
        puts("책의 저자는?");
        gets(books[ctr].author);
        puts("책의 가격은?");
        scanf_s("%f", &books[ctr].price);
        puts("책의 페이지수는?");
        scanf_s("%d", &books[ctr].pages);
        getchar();
    }

    fptr = fopen("c:\\aa\\cdata.txt", "w");
    if (fptr == 0)
    {
        printf("오류-파일이 열리지 않았음.\n");
        exit(1);
    }
    fprintf(fptr, "\n\n책의 컬렉션 정보: \n");
    for (ctr = 0; ctr < 3; ctr++)
    {
        fprintf(fptr, "#%d: %s 저자는 %s", (ctr + 1), books[ctr].title, books[ctr].author);
        fprintf(fptr, "\n%d 페이지와 가격은 $%.2f", books[ctr].pages, books[ctr].price);

        fprintf(fptr, "\n\n");
    }

    fclose(fptr);
    return 0;

}