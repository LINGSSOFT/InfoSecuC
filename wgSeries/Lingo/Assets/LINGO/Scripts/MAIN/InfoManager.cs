using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InfoManager : MonoBehaviour
{
	public static InfoManager instance = null;
	public InfoClass infoScript;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		infoScript = GetComponent<InfoClass> ();
		InitInfo ();
	}

	void InitInfo()
	{
		// InfoScript Initialization
		// infoScript.xxx .....
	}

	void Update()
	{
		
	}
}
