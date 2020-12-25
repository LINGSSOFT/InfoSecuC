using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CUILifeTime : MonoBehaviour {

	//public bool i;

	/////////////////////////////
	// info manager
	// total hp;
	// total speed;

	public Image _hpProgress;

	// Use this for initialization
	public void TimeStart()
	{
		StartCoroutine ("Count");
	}

	IEnumerator Count()
	{
		for (int i=142; i >=0; i--) 
		{
			//Debug.Log (i);
			yield return new WaitForSeconds (1);
			_hpProgress.fillAmount -= (i * 0.0001f);

		}

	}
}
