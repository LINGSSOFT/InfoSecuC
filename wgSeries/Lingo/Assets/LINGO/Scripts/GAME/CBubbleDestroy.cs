using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CBubbleDestroy : MonoBehaviour,IPointerDownHandler
{
	public GameObject _destroyEffectPrefab;
	public GameObject _damageEffectPrefab;
	public int myStatus = 0;

	public AudioClip clipTrue;
	public AudioClip clipFalse;

	private AudioSource myAudio;

	// Use this for initialization
	void Start () {
		myAudio = GameObject.Find ("BackGround").GetComponent<AudioSource> ();
		clipTrue = GameObject.Find ("BackGround").GetComponent<CBackBlockDestroy> ().clipTrue;
		clipFalse = GameObject.Find ("BackGround").GetComponent<CBackBlockDestroy> ().clipFalse;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		Destroy (gameObject);
		Destroyer ();
	}

	// 파괴 처리
	public void Destroyer ()
	{
		// 스코어 증가
		bool bChk = GameObject.Find ("GameManager").GetComponent<GameManager> ().ScoreProcess(myStatus);

		// 마지막 피격 또는 파괴 이펙트를 생성함
		if (bChk)
		{	// 참
			myAudio.PlayOneShot (clipTrue, clipFalse.length);
			Instantiate (_destroyEffectPrefab, transform.position, Quaternion.identity);
		}
		else
		{	// 거짓
			myAudio.PlayOneShot (clipFalse, clipFalse.length);
			Instantiate (_damageEffectPrefab, transform.position, Quaternion.identity);
		}
	}

}
