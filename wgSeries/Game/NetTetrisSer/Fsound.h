#pragma once

#include "fmod.hpp"
#pragma comment (lib,"fmodex_vc.lib")

class FSound
{
public:
	FSound(void);
	~FSound(void);
	void static StopBackSound();
	void static PlayBackSound(int nMusicNum);
	void static BlockDown(DWORD dwIncrement);

protected:

};
