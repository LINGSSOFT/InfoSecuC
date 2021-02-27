#pragma once

#include "../libSimpleSkin/SkinFileReader.h"
#include "CompoundBlock.h"

class Stage
{
public:
	void FrameClose();
	Stage(void);
	~Stage(void);

	void StartGame(CSkinWindow * pSkinWindow);
	void RelayMessage(UINT uMessage, WPARAM wParam, LPARAM lParam);
	void TogglePause();

protected:
	void OnWMCommand(WPARAM wParam, LPARAM lParam);
	void OnWMKeyDown(WPARAM wParam, LPARAM lParam);
	void OnWMTimer(WPARAM wParam, LPARAM lParam);
	BOOL IsMovable(int nOffsetX, int nOffsetY);
	BOOL IsOverlap(CompoundBlock * pBlock, int nOffsetX, int nOffsetY);
	void AddBlock(CompoundBlock * pBlock);
	void LineCheck();
	void DeleteLine(int nLine);
	void SetGameOver(BOOL bGameOver);
	void Restart();
	void ClearAll();
	void UpdateScore();
	void CreateNextBlock();
	void StageClear();

	CSkinWindow * m_pSkinWindow;
	CompoundBlock * m_pCurrentBlock;
	CompoundBlock * m_pNextBlock;
	DWORD m_dwDownTime;
	CSkinControl * m_pBlockGrid[25][10];
	DWORD m_dwScore;
	DWORD m_dwLineCount;
	DWORD m_dwStage;
	BOOL m_bPaused;
	BOOL m_bGameOver;
	int m_nMusicNum;
	int m_nCombo;
};
