#pragma once
#define _CRT_OBSOLETE_NO_WARNINGS
#include <stdio.h>


#define TRUE    1
#define FALSE   0

////////////////////////////////////////////////////////////////////
// 03. 소수 Prime Number
// 소수(Prime Number)의 사전적인 의미는 "1보다 큰 정수 p가 1과 p자신 이외의 양의
// 약수를 가지지 않을 때의 p"입니다. 이를테면 2,3,5,7,11,13,17,...등을 소수라
// 부릅니다. 반대로 다른 숫자의 곱으로 이루어진 4(2*2), 6(2*3)...등 소수가 아닌
// 자연수를 "합성수"라고 합니다. 참고로 1은 소수도 아니고 합성수도 아닙니다.
int IsPrimeNumber(int n);
