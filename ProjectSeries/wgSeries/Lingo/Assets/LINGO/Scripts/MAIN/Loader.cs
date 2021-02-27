using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
	public GameObject infoManager;

	void Awake()
	{
		if (InfoManager.instance == null)
			Instantiate (infoManager);
	}
}
