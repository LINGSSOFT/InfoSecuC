#include "StdAfx.h"
#include "Stage.h"
#include <time.h>
#include "resource.h"

//#include "FSound.h"

#define IDT_BLOCK_DOWN		(100)

#define INITIAL_DOWN_TIME	(1000)
#define SPEED_UP_TIME		(50)	//	50, no 3000 score  
#define LEVEL_UP_COUNT		(10)

Stage::Stage(void)
{
	m_bGameOver = FALSE;
	srand((unsigned int)time(NULL));
	m_pSkinWindow = NULL;
	m_pCurrentBlock = NULL;
	m_dwDownTime = INITIAL_DOWN_TIME;
	m_dwLineCount = 0;
	m_nMusicNum = 0;
	m_nCombo = 0;
	m_bPaused = FALSE;
}

Stage::~Stage(void)
{
	if(m_pCurrentBlock)
		delete m_pCurrentBlock;
	if(m_pNextBlock)
		delete m_pNextBlock;
}

void Stage::StartGame(CSkinWindow * pSkinWindow)
{
	m_pSkinWindow = pSkinWindow;
	m_pSkinWindow->FindControl(_T("pause"), NULL)->SetVisible(m_bPaused);
	SetGameOver(FALSE);
	m_bGameOver = TRUE;
}

void Stage::RelayMessage(UINT uMessage, WPARAM wParam, LPARAM lParam)
{
	switch(uMessage)
	{
	case WM_KEYDOWN:
		OnWMKeyDown(wParam, lParam);
		break;

	case WM_TIMER:
		OnWMTimer(wParam, lParam);
		break;

	case WM_COMMAND:
		OnWMCommand(wParam, lParam);
		break;
	}
}

void Stage::OnWMKeyDown(WPARAM wParam, LPARAM lParam)
{
	if(wParam == 'N')
	{
		//FSound::StopBackSound();
		m_nMusicNum = 0;
		m_bPaused = FALSE;
		CSkinStaticControl * pStatic = (CSkinStaticControl *)m_pSkinWindow->FindControl(_T("pause"), NULL);
		pStatic->SetVisible(m_bPaused);
		Restart();
		return;
	}
	if(!m_bGameOver)
	{
		if(wParam == 'P')
		{
			TogglePause();
			return;
		}
		if(!m_bPaused)
		{
			switch(wParam)
			{
			case VK_LEFT:
				if(IsMovable(-1, 0))
					m_pCurrentBlock->SetLeft(m_pCurrentBlock->GetLeft() - 1);
				break;

			case VK_RIGHT:
				if(IsMovable(1, 0))
					m_pCurrentBlock->SetLeft(m_pCurrentBlock->GetLeft() + 1);
				break;

			case VK_DOWN:
				if(IsMovable(0, 1))
				{
					m_pCurrentBlock->SetTop(m_pCurrentBlock->GetTop() + 1);
					KillTimer(m_pSkinWindow->GetHWND(), IDT_BLOCK_DOWN);
					SetTimer(m_pSkinWindow->GetHWND(), IDT_BLOCK_DOWN, m_dwDownTime, NULL);
				}
				break;

			case VK_UP:
					m_pCurrentBlock->Rotate(FALSE);
					if(!IsMovable(0, 0))
						m_pCurrentBlock->Rotate(TRUE);
				break;

			case VK_SPACE:
				while(IsMovable(0, 1))
					m_pCurrentBlock->SetTop(m_pCurrentBlock->GetTop() + 1);
				OnWMTimer(IDT_BLOCK_DOWN, 0);
				break;
			}
		}
	}
	else
	{
		if(wParam == 'R')
		{
			//FSound::StopBackSound();
			m_nMusicNum = 0;
			m_bPaused = FALSE;
			CSkinStaticControl * pStatic = (CSkinStaticControl *)m_pSkinWindow->FindControl(_T("pause"), NULL);
			pStatic->SetVisible(m_bPaused);
			Restart();
		}
	}

}

void Stage::OnWMTimer(WPARAM wParam, LPARAM lParam)
{
//	CSkinStaticControl * pStatic;
//	TCHAR szText[MAX_PATH];
//	memset(szText, NULL, sizeof(szText));
//	int n = m_pCurrentBlock->GetTop() + m_pCurrentBlock->GetHeight();
//	_stprintf(szText, _T("%d"), n);
//	pStatic = (CSkinStaticControl *)m_pSkinWindow->FindControl(_T("great"), NULL);
//	pStatic->SetText(LPCTSTR(szText));
//	pStatic->Invalidate();

	switch(wParam)
	{
	case IDT_BLOCK_DOWN:
		KillTimer(m_pSkinWindow->GetHWND(), IDT_BLOCK_DOWN);

		if(IsMovable(0, 1))
			m_pCurrentBlock->SetTop(m_pCurrentBlock->GetTop() + 1);
		else
		{
			AddBlock(m_pCurrentBlock);

			LineCheck();

			m_pCurrentBlock = m_pNextBlock;
			m_pCurrentBlock->SetParent(m_pSkinWindow->FindControl(_T("grid")));
			m_pCurrentBlock->SetLeft(4);
			m_pCurrentBlock->SetTop(1 - m_pCurrentBlock->GetHeight());

			CreateNextBlock();

			if(IsMovable(0, 0) == FALSE)
			{
				SetGameOver(TRUE);
				return;
			}
		}
		SetTimer(m_pSkinWindow->GetHWND(), IDT_BLOCK_DOWN, m_dwDownTime, NULL);
		break;
	}
}

BOOL Stage::IsMovable(int nOffsetX, int nOffsetY)
{
	int nLeft = m_pCurrentBlock->GetLeft();
	int nTop = m_pCurrentBlock->GetTop();
	int nWidth = m_pCurrentBlock->GetWidth();
	int nHeight = m_pCurrentBlock->GetHeight();

	if(nLeft + nOffsetX < 0)
		return FALSE;
	if(nLeft + nWidth + nOffsetX > 10)
		return FALSE;
	if(nTop + nHeight + nOffsetY > 20)
		return FALSE;

	if(IsOverlap(m_pCurrentBlock, nOffsetX, nOffsetY))
		return FALSE;

	return TRUE;
}

BOOL Stage::IsOverlap(CompoundBlock * pBlock, int nOffsetX, int nOffsetY)
{
	int nLeft = pBlock->GetLeft() + nOffsetX;
	int nTop = pBlock->GetTop() + nOffsetY;
	CSkinControl ** ppArray = pBlock->GetBlockArray();
	CSkinControl * pControl;
	for(int j = 0; j < pBlock->GetHeight(); j++)
	{
		if(j + nTop < 0)
			continue;

		for(int i = 0; i < pBlock->GetWidth(); i++)
		{
			int nPos = j * pBlock->GetWidth() + i;
			if(nPos < 0)
				continue;

			pControl = ppArray[j * pBlock->GetWidth() + i];
			if(pControl != NULL && m_pBlockGrid[j + nTop + 5][i + nLeft] != NULL)
				return TRUE;
		}
	}

	return FALSE;
}

void Stage::AddBlock(CompoundBlock * pBlock)
{
	int nLeft = pBlock->GetLeft();
	int nTop = pBlock->GetTop();
	CSkinControl ** ppArray = pBlock->GetBlockArray();
	CSkinControl * pControl;
	for(int j = 0; j < pBlock->GetHeight(); j++)
	{
		for(int i = 0; i < pBlock->GetWidth(); i++)
		{
			pControl = ppArray[j * pBlock->GetWidth() + i];
			if(pControl)
				m_pBlockGrid[j + nTop + 5][i + nLeft] = pControl;
		}
	}
}

void Stage::LineCheck()
{
	BOOL bLineCompleted;
	DWORD dwIncrement = 10;
	int nStageIncrement = 0;
	BOOL bComChk = FALSE;
	for(int j = 0; j < 20; j++)
	{
		bLineCompleted = TRUE;
		for(int i = 0; i < 10; i++)
		{
			if(m_pBlockGrid[j + 5][i] == NULL)
			{
				bLineCompleted = FALSE;
				break;
			}
		}

		if(bLineCompleted)
		{
			m_dwScore += dwIncrement;
			dwIncrement += 10;
			DeleteLine(j);
			m_dwLineCount++;
			bComChk = TRUE;

			if((m_dwLineCount % LEVEL_UP_COUNT) == 0)
			{
				nStageIncrement++;		// Stage
				m_nMusicNum++;			// Music

				//FSound::StopBackSound();
				if(m_nMusicNum > 11)
					m_nMusicNum -= 12;
				//FSound::PlayBackSound(m_nMusicNum);

				if(m_dwDownTime > SPEED_UP_TIME)
				{
					if(m_dwDownTime > 80)
						m_dwDownTime -= SPEED_UP_TIME;
					KillTimer(m_pSkinWindow->GetHWND(), IDT_BLOCK_DOWN);
					SetTimer(m_pSkinWindow->GetHWND(), IDT_BLOCK_DOWN, m_dwDownTime, NULL);
				}
			}
		}
	}
//	FSound::BlockDown(dwIncrement);
	UpdateScore();

	CSkinStaticControl * pStatic;
	TCHAR szText[MAX_PATH];
	memset(szText, NULL, sizeof(szText));
	pStatic = (CSkinStaticControl *)m_pSkinWindow->FindControl(_T("stage"), NULL);
	if(nStageIncrement != 0)
	{
		m_dwStage += 1;
		_stprintf_s(szText, _T("%d Stage Clear"), m_dwStage);
	}
	pStatic->SetText(LPCTSTR(szText));
	pStatic->Invalidate();

	memset(szText, NULL, sizeof(szText));
	if(dwIncrement == 50)
		_tcscpy_s(szText, _T("Great~~!!"));

	pStatic = (CSkinStaticControl *)m_pSkinWindow->FindControl(_T("great"), NULL);
	pStatic->SetText(LPCTSTR(szText));
	pStatic->Invalidate();

	memset(szText, NULL, sizeof(szText));
	pStatic = (CSkinStaticControl *)m_pSkinWindow->FindControl(_T("combo"), NULL);
	if(bComChk)
	{
		m_nCombo++;
		_stprintf_s(szText, _T("%d Combo"), m_nCombo);
	}
	else
		m_nCombo = 0;

	pStatic->SetText(LPCTSTR(szText));
	pStatic->Invalidate();

	m_pSkinWindow->FindControl(_T("grid"))->Invalidate();
}

void Stage::DeleteLine(int nLine)
{
	CSkinControl * pParent = m_pSkinWindow->FindControl(_T("grid"));
	int i;
	for(i = 0; i < 10; i++)
		pParent->RemoveChildControl(m_pBlockGrid[nLine + 5][i]);

	RECT rtControl;
	for(int j = nLine - 1; j >= 0; j--)
	{
		for(i = 0; i < 10; i++)
		{
			m_pBlockGrid[j + 1 + 5][i] = m_pBlockGrid[j + 5][i];
			if(m_pBlockGrid[j + 1 + 5][i])
			{
				SetRect(&rtControl, i * 22, (j + 1) * 22, (i + 1) * 22, (j + 2) * 22);
				m_pBlockGrid[j + 1 + 5][i]->SetControlRect(&rtControl);
			}
		}
	}
}

void Stage::SetGameOver(BOOL bGameOver)
{
	m_bGameOver = bGameOver;
	CSkinControl * pControl = m_pSkinWindow->FindControl(_T("gameover"), NULL);
	if(pControl)
		pControl->SetVisible(m_bGameOver);

	pControl = m_pSkinWindow->FindControl(_T("retry"), NULL);
	if(pControl)
		pControl->SetVisible(m_bGameOver);
//
//	pControl = m_pSkinWindow->FindControl(_T("frame_top"), NULL);
//	if(pControl)
//		pControl->SetVisible(m_bGameOver);
	
//	HMENU hMainMenu = GetMenu(m_pSkinWindow->GetHWND());
//	HMENU hSubMenu = GetSubMenu(hMainMenu, 0);
//	EnableMenuItem(hSubMenu, ID_FILE_RETRY, MF_BYCOMMAND | (bGameOver ? MF_ENABLED : MF_GRAYED));
	//FSound::StopBackSound();
}

void Stage::OnWMCommand(WPARAM wParam, LPARAM lParam)
{
/*	switch(LOWORD(wParam))
	{
		case ID_FILE_RETRY:
			FSound::StopBackSound();
			m_nMusicNum = 0;
			m_bPaused = FALSE;
			CSkinStaticControl * pStatic = (CSkinStaticControl *)m_pSkinWindow->FindControl(_T("pause"), NULL);
			pStatic->SetVisible(m_bPaused);
			Restart();
			break;
	}
*/
}

void Stage::Restart()
{
	ClearAll();

	SetGameOver(FALSE);

	m_pCurrentBlock = new CompoundBlock(rand() % 7);
	m_pCurrentBlock->SetParent(m_pSkinWindow->FindControl(_T("grid")));
	m_pCurrentBlock->SetLeft(4);
	m_pCurrentBlock->SetTop(1 - m_pCurrentBlock->GetHeight());

	CreateNextBlock();

	SetTimer(m_pSkinWindow->GetHWND(), IDT_BLOCK_DOWN, m_dwDownTime, NULL);

	//FSound::PlayBackSound(m_nMusicNum);
}

void Stage::ClearAll()
{
	if(m_pCurrentBlock)
	{
		delete m_pCurrentBlock;
		m_pCurrentBlock = NULL;
	}

	if(m_pNextBlock)
	{
		delete m_pNextBlock;
		m_pNextBlock = NULL;
	}

	CSkinControl * pParent = m_pSkinWindow->FindControl(_T("grid"), NULL);
	CSkinControl * pChild;
	while(pChild = pParent->GetFirstChild())
		pParent->RemoveChildControl(pChild);
	pParent->Invalidate();

	pParent = m_pSkinWindow->FindControl(_T("next"), NULL);
	while(pChild = pParent->GetFirstChild())
		pParent->RemoveChildControl(pChild);
	pParent->Invalidate();

	for(int j = 0; j < 20; j++)
		for(int i = 0; i < 10; i++)
			m_pBlockGrid[j + 5][i] = NULL;

	m_dwDownTime = INITIAL_DOWN_TIME;
	m_dwScore = 0;
	m_dwStage = 0;
	m_dwLineCount = 0;
	UpdateScore();
}

void Stage::UpdateScore()
{
	TCHAR szText[255];
	_stprintf_s(szText, _T("%u"), m_dwScore);
	CSkinStaticControl * pStatic = (CSkinStaticControl *)m_pSkinWindow->FindControl(_T("score"), NULL);
	pStatic->SetText(szText);
	pStatic->Invalidate();

	_stprintf_s(szText, _T("%u"), m_dwLineCount);
	pStatic = (CSkinStaticControl *)m_pSkinWindow->FindControl(_T("clearblock"), NULL);
	pStatic->SetText(szText);
	pStatic->Invalidate();
}

void Stage::TogglePause()
{
	m_bPaused = !m_bPaused;
	if(m_bPaused)
	{
		KillTimer(m_pSkinWindow->GetHWND(), IDT_BLOCK_DOWN);
		//FSound::StopBackSound();
	}
	else
	{
		SetTimer(m_pSkinWindow->GetHWND(), IDT_BLOCK_DOWN, m_dwDownTime, NULL);
		//FSound::PlayBackSound(m_nMusicNum);
	}
	m_pSkinWindow->FindControl(_T("pause"), NULL)->SetVisible(m_bPaused);
}


void Stage::FrameClose()
{
	if(!m_bGameOver)
	{
		if(m_bPaused == FALSE)
			TogglePause();
	}
}

void Stage::StageClear()
{
}

void Stage::CreateNextBlock()
{
	CSkinControl * pParent = m_pSkinWindow->FindControl(_T("next"));

	m_pNextBlock = new CompoundBlock(rand() % 7);
	m_pNextBlock->SetParent(pParent);
	m_pNextBlock->SetLeft(1);
	m_pNextBlock->SetTop(1);
	pParent->Invalidate();
}

