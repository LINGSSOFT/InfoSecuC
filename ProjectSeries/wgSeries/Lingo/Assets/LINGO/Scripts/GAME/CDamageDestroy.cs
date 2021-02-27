using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDamageDestroy : MonoBehaviour {

	public float _destroyTime;
	// Use this for initialization
	void Start () {
		Destroy (gameObject, _destroyTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
