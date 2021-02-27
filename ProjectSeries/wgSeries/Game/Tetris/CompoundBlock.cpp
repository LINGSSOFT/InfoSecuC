#include "StdAfx.h"
#include "CompoundBlock.h"
#include "../libSimpleSkin/ResourceManager.h"


struct C_BLOCKINFO
{
	int nWidth;
	int nHeight;
	int nCenterX;
	int nCenterY;
	int nRotateType;
	int BlockInfo[4][4];
};


C_BLOCKINFO _BlockTable[] = {

	{ 2, 2,	0, 0, 1, {
		{ 1, 1, 0, 0 },
		{ 1, 1, 0, 0 },
		{ 0, 0, 0, 0 },
		{ 0, 0, 0, 0 } }
	},
	{ 3, 2, 1, 1, 2, {
		{ 2, 2, 0, 0 },
		{ 0, 2, 2, 0 },
		{ 0, 0, 0, 0 },
		{ 0, 0, 0, 0 } }
	},
	{ 3, 2, 1, 1, 2, {
		{ 0, 3, 3, 0 },
		{ 3, 3, 0, 0 },
		{ 0, 0, 0, 0 },
		{ 0, 0, 0, 0 } }
	},
	{ 3, 2, 1, 1, 4, {
		{ 0, 0, 4, 0 },
		{ 4, 4, 4, 0 },
		{ 0, 0, 0, 0 },
		{ 0, 0, 0, 0 } }
	},
	{ 3, 2, 1, 1, 4, {
		{ 5, 0, 0, 0 },
		{ 5, 5, 5, 0 },
		{ 0, 0, 0, 0 },
		{ 0, 0, 0, 0 } }
	},
	{ 4, 1, 1, 0, 2, {
		{ 6, 6, 6, 6 },
		{ 0, 0, 0, 0 },
		{ 0, 0, 0, 0 },
		{ 0, 0, 0, 0 } }
	},
	{ 3, 2, 1, 1, 4, {
		{ 0, 7, 0, 0 },
		{ 7, 7, 7, 0 },
		{ 0, 0, 0, 0 },
		{ 0, 0, 0, 0 } }
	}
};

CompoundBlock::CompoundBlock(int nBlockType)
{
	memset(m_pBlockArray, 0, sizeof(m_pBlockArray));
	C_BLOCKINFO * pInfo = &(_BlockTable[nBlockType]);
	m_nLeft = 0;
	m_nTop = 0;
	m_nWidth = pInfo->nWidth;
	m_nHeight = pInfo->nHeight;
	m_nRotateType = pInfo->nRotateType;
	m_bClockwise = TRUE;

	TCHAR szSkinFile[MAX_PATH];
	GetModuleFileName(GetModuleHandle(NULL), szSkinFile, MAX_PATH);
	_tcsrchr(szSkinFile, '\\')[0] = 0;
	_tcsrchr(szSkinFile, '\\')[0] = 0;
	_tcscat(szSkinFile, _T("\\DoTetris\\skin\\SoloBlock.bmp"));
	CSkinImageMap * pImageMap = sg_pResourceMngr->QueryImageMap(szSkinFile);

	int nBlockImage;
	CSkinImageControl * pControl;
	RECT rtControl;
	RECT rtImage;
	for(int j = 0; j < m_nHeight; j++)
	{
		for(int i = 0; i < m_nWidth; i++)
		{
			nBlockImage = pInfo->BlockInfo[j][i];
			if(nBlockImage == 0)
				m_pBlockArray[j * m_nWidth + i] = NULL;
			else
			{
				nBlockImage--;
				pControl = new CSkinImageControl();
				m_pBlockArray[j * m_nWidth + i] = pControl;
				pControl->SetAnchors(ANK_LEFT | ANK_TOP);
				SetRect(&rtControl, i * 22, j * 22, (i + 1) * 22, (j + 1) * 22);
				pControl->SetControlRect(&rtControl);
				SetRect(&rtImage, nBlockImage * 23, 0, nBlockImage * 23 + 22, 22);
				pControl->SetImageMap(pImageMap);
				pControl->SetImageRect(&rtImage);
				pControl->SetAutoDelete(TRUE);

				if(i == pInfo->nCenterX && j == pInfo->nCenterY)
					pControl->SetData((void *)1);
			}
		}
	}

	sg_pResourceMngr->ReleaseResource(pImageMap);
}

CompoundBlock::~CompoundBlock(void)
{
}

void CompoundBlock::Rotate(BOOL bClockwise)
{
	switch(m_nRotateType)
	{
		case 1:
			return;

		case 2:
			m_bClockwise = !m_bClockwise;
			bClockwise = m_bClockwise;
			break;
	}

	CSkinControl * pTempArray[16];
	CSkinControl * pCurrent;
	memset(pTempArray, 0, sizeof(pTempArray));

	int nNewWidth = m_nHeight;
	int nNewHeight = m_nWidth;
	int i, j;
	int nLeftOffset = 0;
	int nTopOffset = 0;

	for(j = 0; j < m_nHeight; j++)
	{
		for(i = 0; i < m_nWidth; i++)
		{
			if(bClockwise)
			{
				pCurrent = m_pBlockArray[i + j * m_nWidth];
				pTempArray[(m_nHeight - j - 1) + nNewWidth * i] = pCurrent;
				if(pCurrent && pCurrent->GetData() != NULL)
				{
					nLeftOffset = (m_nHeight - j - 1) - i;
					nTopOffset = i - j;
				}
			} else
			{
				pCurrent = m_pBlockArray[i + j * m_nWidth];
				pTempArray[(m_nWidth - i - 1) * nNewWidth + j] = pCurrent;
				if(pCurrent && pCurrent->GetData() != NULL)
				{
					nLeftOffset = j - i;
					nTopOffset = (m_nWidth - i - 1) - j;
				}
			}
		}
	}

	m_nLeft -= nLeftOffset;
	m_nTop -= nTopOffset;

	RECT rtControl;
	int nPos;
	for(j = 0; j < nNewHeight; j++)
	{
		for(i = 0; i < nNewWidth; i++)
		{
			nPos = i + j * nNewWidth;
			m_pBlockArray[nPos] = pTempArray[nPos];
			SetRect(&rtControl, (i + m_nLeft) * 22, (j + m_nTop) * 22, (i + m_nLeft + 1) * 22, (j + m_nTop + 1) * 22);
			if(m_pBlockArray[nPos])
				m_pBlockArray[nPos]->SetControlRect(&rtControl);
		}
	}

	m_nWidth = nNewWidth;
	m_nHeight = nNewHeight;
}

int CompoundBlock::GetLeft()
{
	return m_nLeft;
}

int CompoundBlock::GetTop()
{
	return m_nTop;
}

int CompoundBlock::GetWidth()
{
	return m_nWidth;
}

int CompoundBlock::GetHeight()
{
	return m_nHeight;
}

CSkinControl ** CompoundBlock::GetBlockArray()
{
	return m_pBlockArray;
}

void CompoundBlock::SetParent(CSkinControl * pParent)
{
	CSkinControl * pControl;
	for(int j = 0; j < m_nHeight; j++)
	{
		for(int i = 0; i < m_nWidth; i++)
		{
			pControl = m_pBlockArray[j * m_nWidth + i];
			if(pControl)
			{
				pControl->SetAutoDelete(FALSE);
				if(pControl->GetParent())
					pControl->GetParent()->RemoveChildControl(pControl);
				pParent->AddChildControl(pControl);
				pControl->SetAutoDelete(TRUE);
			}
		}
	}
}

void CompoundBlock::SetLeft(int nLeft)
{
	OffsetBlock(nLeft - m_nLeft, 0);
	m_nLeft = nLeft;
}

void CompoundBlock::SetTop(int nTop)
{
	OffsetBlock(0, nTop - m_nTop);
	m_nTop = nTop;
}

void CompoundBlock::OffsetBlock(int nOffsetX, int nOffsetY)
{
	CSkinControl * pControl;
	RECT rtControl;
	for(int j = 0; j < m_nHeight; j++)
	{
		for(int i = 0; i < m_nWidth; i++)
		{
			pControl = m_pBlockArray[j * m_nWidth + i];
			if(pControl)
			{
				pControl->GetControlRect(&rtControl);
				OffsetRect(&rtControl, nOffsetX * 22, nOffsetY * 22);
				pControl->SetControlRect(&rtControl);
			}
		}
	}
}
