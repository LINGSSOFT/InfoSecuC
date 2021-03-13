#include "stdafx.h"
#include "SceneMgr.h"

SceneMgr::SceneMgr(void)
{
	// Todo...
	m_stage = NULL;
}

SceneMgr::~SceneMgr(void)
{
	// Todo...
	if (m_stage == NULL)
		return;

	delete m_stage;
}
