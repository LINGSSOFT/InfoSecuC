#pragma once

#include "../libSimpleSkin/SkinFileReader.h"

class CompoundBlock
{
public:
	CompoundBlock(int nBlockType);
	~CompoundBlock(void);

	void Rotate(BOOL bClockwise);
	int GetLeft();
	int GetTop();
	int GetWidth();
	int GetHeight();
	CSkinControl ** GetBlockArray();
	void SetParent(CSkinControl * pParent);
	void SetLeft(int nLeft);
	void SetTop(int nTop);

protected:
	void OffsetBlock(int nOffsetX, int nOffsetY);

	int m_nLeft;
	int m_nTop;
	int m_nWidth;
	int m_nHeight;
	int m_nRotateType;
	BOOL m_bClockwise;

	CSkinControl * m_pBlockArray[16];
};
