#include "StdAfx.h"
#include "FSound.h"

#define NUM_MUSIC_FOR_FSOUND	12

#include "MMsystem.h"
#pragma comment (lib ,"winmm.lib")

static char BackSoundArray[NUM_MUSIC_FOR_FSOUND][MAX_PATH] = 
{
	"sound\\stage01.mp3", "sound\\stage02.mp3",
	"sound\\stage03.mp3", "sound\\stage04.mp3",
	"sound\\stage05.mp3", "sound\\stage06.mp3",
	"sound\\stage07.mp3", "sound\\stage08.mp3",
	"sound\\stage09.mp3", "sound\\stage10.mp3",
	"sound\\stage11.mp3", "sound\\stage12.mp3"
};


FMOD::System *System;

struct tagSound
{
	FMOD::Channel *Channel;
	FMOD::Sound  *Sound;
};

FSound::FSound(void)
{
}

FSound::~FSound(void)
{
	System->close();
	System->release();
}

void FSound::PlayBackSound(int nMusicNum)
{
	FMOD_RESULT result;
	unsigned int version;

	result = FMOD::System_Create(&System);
	result = System->getVersion(&version);
	result = System->init(100, FMOD_INIT_NORMAL, 0);
	result = System->release();
	FMOD::System_Create(&System);
	System->init(100, FMOD_INIT_NORMAL, 0);
	tagSound* SoundStruct;
	SoundStruct = new tagSound[3];

	System->createStream(BackSoundArray[nMusicNum], FMOD_HARDWARE | FMOD_LOOP_NORMAL | FMOD_2D, NULL, &SoundStruct[0].Sound);
	System->playSound(FMOD_CHANNEL_FREE, SoundStruct[0].Sound, false, &SoundStruct[0].Channel);

	unsigned int pLenth = 0;

	SoundStruct[0].Sound->getLength(&pLenth, FMOD_TIMEUNIT_MS);
}


void FSound::StopBackSound()
{
	System->close();
	System->release();
}

void FSound::BlockDown(DWORD dwIncrement)
{
	switch(dwIncrement)
	{
		case 20:
			sndPlaySound(_T("sound\\blockclear1.wav"), SND_ASYNC);
			break;
		case 30:
			sndPlaySound(_T("sound\\blockclear2.wav"), SND_ASYNC);
			break;
		case 40:
			sndPlaySound(_T("sound\\blockclear3.wav"), SND_ASYNC);
			break;
		case 50:
			sndPlaySound(_T("sound\\blockclear4.wav"), SND_ASYNC);
			break;
		default:
			sndPlaySound(_T("sound\\blockadd.wav"), SND_ASYNC);
			break;
	}

}
