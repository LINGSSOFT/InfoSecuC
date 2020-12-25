using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBackBlockDestroy : MonoBehaviour {

	public AudioClip clipGo;
	public AudioClip clipTrue;
	public AudioClip clipFalse;
	public AudioClip enemyTouch;
	public AudioClip enemyDead;
	public AudioClip stageClear;
	public AudioClip sentenceClear;

	// Use this for initialization
	void Start () {
		// Ready Sound Start
		GetComponent<AudioSource> ().Play ();

	}
	
	void BackGroundEnd()
	{
		// Go Sound Start
		GetComponent<AudioSource> ().PlayOneShot (clipGo, 1.0f);
		GameObject.Find ("GameManager").GetComponent<GameManager> ().Ready();
	}

	void OnTriggerExit2D(Collider2D col)
	{
		AreaOut(col.gameObject);
	}

	// 영역을 벗어난 오브젝트들은 파괴함
	void AreaOut(GameObject destroyObject)
	{
		Destroy(destroyObject);
	}
}
