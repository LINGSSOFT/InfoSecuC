#pragma once
#define _CRT_OBSOLETE_NO_WARNINGS
#include <stdio.h>


#define TRUE    1
#define FALSE   0

////////////////////////////////////////////////////////////////////
// 01.최대공약수 Greatest Common Divisor
// 유클리드 호제법 Euclidean algorithm
// 유클리드(B.C.365~B.C.275)는 기하학의 아버지라고 불리는 유명한 고대 수학자입니다.
// 유클리드의 저서 중에서 가장 유명한 것이 총 13권으로 구성된 '기하학 원본'입니다.
// 이 책은 그의 선배인 피타고라스,플라톤,히포크라테스 등이 연구한 여러 가지 자료를
// 정선하고 거기에 자신의 창작을 가미하여 조직적인 교과서로 편찬한 것으로서
// 수학사상 최고의 성전이라고도 할 수 있습니다.
int getGCD(int nFir, int nSec);
