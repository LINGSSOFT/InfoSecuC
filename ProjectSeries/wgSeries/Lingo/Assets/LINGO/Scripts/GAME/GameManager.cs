using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Data;
using Mono.Data.SqliteClient;

public class GameManager : MonoBehaviour {
	
	public int totCnt = 0;
	public int nCompare = 0;

	public Text _scoreText;
	public Text _testDBText;

	// 스코어 증가
	public void ScoreUp(int value)
	{
		// 증가할 스코어 값을 받아 UI에 표시함
		int score = int.Parse(_scoreText.text);
		score += value;
		_scoreText.text = score.ToString();

		// PlayerPrefs에 스코어를 저장함
		PlayerPrefs.SetString("SCORE", _scoreText.text);
		PlayerPrefs.Save();
	}

	// Use this for initialization
	void Start ()
	{
		testFunc ();	
	}

	public void Ready()
	{
		// 레디 없어지고 고나오기
		//GameObject.Find ("ReadyText").SetActive(false);
		//GameObject.Find("GameUI").transform.Find("Canvas").transform.Find("GoText").gameObject.SetActive(true);
		//GameObject.Find ("GoText").SetActive(true);

		// 고 조금 기다렸다 스타트하기
		StartCoroutine("GameStart");
	}

	IEnumerator GameStart()
	{
		//GameObject.Find ("DoozyUI").transform.Find ("UI Container").transform.Find ("EArea").gameObject.SetActive (false);
		yield return new WaitForSeconds (6.0f);
		//GameObject.Find ("EnglishArea").SetActive (false);
		//GameObject.Find ("GoText").SetActive(false);
		GameObject.Find("DoozyUI").transform.Find("UI Container").transform.Find("KroeaArea").gameObject.SetActive(true);

		ManageStart();

		// 백그라운드 사운드 시작
		GetComponent<AudioSource> ().Play ();
		GetComponent<AudioSource> ().loop = true;
	}

	public void ManageStart()
	{
		// 적기, 방울, 문자창 생성
		GameObject.Find ("BubbleGenerator").GetComponent<CBubbleGenerator> ().BubbleStart();
		GameObject.Find ("FishGeneratorRight").GetComponent<CFishGenerator> ().FishStart ();
		GameObject.Find ("FishGeneratorLeft").GetComponent<CFishGenerator> ().FishStart ();
		GameObject.Find ("Life").GetComponent<CUILifeTime> ().TimeStart ();
	}
	public void StageClear()
	{
		// 종료 되면 팝업 나와
		StartCoroutine("GameEnd");
	}

	IEnumerator GameEnd()
	{
		yield return new WaitForSeconds (0.1f);
		GameObject.Find("GameUI").transform.Find("Canvas").transform.Find("ResultBOX").gameObject.SetActive(true);

		GameObject.Find ("PauseController").GetComponent<PauseManager> ().OnPause ();

	}

	public bool ScoreProcess(int currBubble)
	{
		if (nCompare == currBubble)
		{
			// 참
			nCompare += 1;

			// 상태창 문자 변경하기 카운트 갯수만큼 색깔변경 해서
			int sec = GameObject.Find ("BubbleGenerator").GetComponent<CBubbleGenerator>().nCurrSec;
			string strStatus = (string)GameObject.Find ("BubbleGenerator").GetComponent<CBubbleGenerator>().HanString[sec];
			string[] strSwitch = strStatus.Split('/');

			GameObject.Find ("Life").GetComponent<CUILifeTime> ()._hpProgress.fillAmount += 0.001f;

			// next Stage
			if (nCompare == totCnt)
			{
				// 문자 색깔 전체 바꾸고..
				for (int i = 0; i < totCnt; i++)
				{
					if (i == 0)
						strStatus = "<color='#0000ffaa'>";
					if (i == totCnt -1)
						strStatus = strStatus + strSwitch [i] + "</color>";
					else
						strStatus = strStatus + strSwitch [i] + "/";
				} 
				GameObject.Find ("KText").GetComponent<Text> ().text = strStatus;
		
				// 1문장 소리 내주고...
				// Sound..
			
				// 다음판 꼬...
				GameObject.Find ("BubbleGenerator").GetComponent<CBubbleGenerator> ().bStatus = true;
				//GameObject.Find ("GameController").GetComponent<CScore> ().ScoreUp (10);
				ScoreUp(10);
				nCompare = 0;
			}
			else
			{
				// 현재 문자 색깔 바꾸기
				for (int i = 0; i < totCnt; i++)
				{
					if (i == 0)
						strStatus = "<color='#0000ffaa'>";
					if (i == nCompare)
						strStatus += "</color>";
					if (i == totCnt -1)
						strStatus = strStatus + strSwitch [i];
					else
						strStatus = strStatus + strSwitch [i] + "/";
				} 
				GameObject.Find ("KText").GetComponent<Text> ().text = strStatus;
			}
			return true;
		}
		else
		{
			// 거짓
			GameObject.Find ("Life").GetComponent<CUILifeTime> ()._hpProgress.fillAmount -= 0.001f;

			return false;

		}
	}




	// DBTest
	public void testFunc()
	{
		//		string result = "";

		string _constr = "URI=file:" + "test.lms.db";
		IDbConnection _dbc;
		IDbCommand _dbcm;
		IDataReader _dbr;

		_dbc = new SqliteConnection(_constr);
		_dbc.Open();
		_dbcm = _dbc.CreateCommand();
		_dbcm.CommandText = "select read1 from wowread";
		// "SELECT txt FROM mymytable WHERE id='" + "4" + "'"; // mymytable 테이블의 txt 칼럼의 값을 가져오는데, 조건은 id 칼럼의 값이 4이다.
		_dbr = _dbcm.ExecuteReader();

/*		while( _dbr.Read())
		{
//			DBtest.text += _dbr.GetString(0);	//_dbr.GetInt16(0);
		}
*/
		if (_dbr.Read()) // 결과 row가 하나일 때는 _dbr.Read() 한 번으로 끝난다.
		{
			//result = _dbr.GetInt16(0);
			_testDBText.text = _dbr.GetString(0); // 이제 현재 row의 0번째 칼럼의 값 결과물을 문자열로 받아온 것을 result 에 넣어준다.
		}

		//_dbr.NextResult(); // 만일 결과 row가 여러 줄일 때는 .NextResult() 메서드를 사용하여 다음 row를 다루도록 하자.

		//		DBtest.text = result.ToString();	// 유니티 UI 라벨 () 의 캡션에 방금 받아온 스트링을 쏴준다.

		//+ 추가 (DB 닫는 용도)
		_dbr.Close (); 
		_dbr = null;
		_dbcm.Dispose (); 
		_dbcm = null; 
		_dbc.Close (); 
		_dbc = null; 
	}
}

