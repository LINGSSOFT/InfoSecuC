using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CBubbleGenerator : MonoBehaviour
{
	// 스테이지 문자담기
	public string[] HanString = {
		"나는/해야한다/가다/교회에", "우리는/해야한다/예의를 차리다/연장자를", "너는/해야한다/일어나다/일찍",
		"너는/해야한다/아마/가져가다/우산을", "우리는/해야한다/정숙하다/도서관에서", "나의 차는/해야한다/고쳐지다/13일까지",
		"너는/해야한다/생각하다/두번/~하기 전/너가/말하다", "우리는/해야한다/하다/숙제를/왜냐하면/마감기한이/내일이다",
		"너는/해야한다/전화하다/그에게/저녁을 먹기위해/함께", "그녀는/해야한다/소비하다/돈을/파티에 참가하기 위해/내일"
	};

	public string[] EngString = {
		"I/should/go/to church", "We/should/be polite/to old people", "You/should/get up/early",
		"You/should/probably/bring/an umbrella", "We/should/be polite/in the library",
		"My car/should/be fixed/by 13th May", "You/should/think/twice/before/you/speak",
		"We/should/do/homework/because/the deadline/is tomorrow",
		"You/should/call/him/to have dinner/together", "She/should/spend/money/to join the party/tomorrow"
	};
		
	public Transform[] _spawnPoints; 	// 방울 생성 포인트들

	// 생성 지연 시간
	public float _minDelayTime;
	public float _maxDelayTime;

	public string[] inText;
	public bool bStatus = false;
	public int nCurrSec = 0;

	private AudioSource myAudio;
	private AudioClip stageClear;
	private AudioClip sentenceClear;

	int nCount = 0;
	float nSize = 0.0f;
	int nRandom = 0;

	GameObject gmObj;					// 방울 오브젝트 복사본 객체
	public GameObject[] _bubblePrefabs;	// 방울 프리팹
			
	// Use this for initialization
	void Start () {
		// 문자 네트워크 또는 디비에서 받기
		myAudio = GameObject.Find ("BackGround").GetComponent<AudioSource> ();
		sentenceClear = GameObject.Find ("BackGround").GetComponent<CBackBlockDestroy> ().sentenceClear;
		stageClear = GameObject.Find ("BackGround").GetComponent<CBackBlockDestroy> ().stageClear;
	}

	// 방울 생성
	public void BubbleStart()
	{
		StartCoroutine("GenCoroutine");
	}


	IEnumerator GenCoroutine()
	{
		for (nCurrSec = 0; nCurrSec < HanString.Length; nCurrSec++)
		{
			GameObject.Find ("KText").GetComponent<Text>().text = (string)HanString.GetValue(nCurrSec);

			nCount = 0;
			bStatus = false;
			// 방울안 글자 세팅 후 메니저에게 알림
			inText = EngString[nCurrSec].Split('/');
			GameObject.Find ("GameManager").GetComponent<GameManager>().totCnt = inText.Length;

			while (true) {
				// 생성 위치 선정
				int posNum = Random.Range (0, _spawnPoints.Length);

				// 가변적인 생성 지연 시간을 설정함
				float delayTime = Random.Range (_minDelayTime, _maxDelayTime);

				// 생성 지연
				yield return new WaitForSeconds (delayTime);

				// 오브젝트 생성 무작위
				nRandom = Random.Range(0, inText.Length);
				gmObj = Instantiate(_bubblePrefabs[0], _spawnPoints[posNum].position, Quaternion.identity) as GameObject;
				// 글자갯수 별로 사이즈 조정
				if (inText [nRandom].Length > 5)
					nSize = 1.0f;
				else
					nSize = 0.8f;

				// 오브젝트 생성 및 설정
				gmObj.transform.localScale = new Vector3(nSize, nSize);
				gmObj.transform.Find("Canvas").transform.Find("English").GetComponent<Text>().text = inText[nRandom];
				gmObj.GetComponent<CBubbleDestroy>().myStatus = nRandom;

				nCount += 1;
				if (nCount >= inText.Length)
					nCount = 0;
				// 게임 계산 적용

				// 1문장 클리어면 와일 탈출하고 다음문장
				if (bStatus) {
					// 배경 흔들기
					// 클리어 음악 고
					myAudio.PlayOneShot (sentenceClear, sentenceClear.length);

					// 다음 문장 세팅
					break;
				}
			}

		}
		// 클리어 음악 고
		myAudio.PlayOneShot (stageClear, stageClear.length);
		// Stage Clear
		GameObject.Find ("GameManager").GetComponent<GameManager>().StageClear();
	}
}