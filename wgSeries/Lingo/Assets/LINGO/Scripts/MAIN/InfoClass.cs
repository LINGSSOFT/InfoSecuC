using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoClass : MonoBehaviour
{
	// Persononal Infomation
	public class PersonalInfo
	{
		public int minimum;
		public int maximum;

		public PersonalInfo(int min, int max)
		{
			minimum = min;
			maximum = max;
		}
		// etc
	}

	// Game Infomation
	public class GameInfo
	{
		// etc
		public int score = 80;
	}

	// Cash Infomation
	public class CashlInfo
	{
		// etc
	}

	public PersonalInfo personalcount = new PersonalInfo(5,9); // 5 < min < 9
	public GameInfo gameInfo = new GameInfo();
	public CashlInfo cashInfo = new CashlInfo ();

}
