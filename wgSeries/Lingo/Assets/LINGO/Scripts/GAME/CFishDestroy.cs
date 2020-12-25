using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CFishDestroy : MonoBehaviour
{
	public GameObject _damageEffectPrefab;
	public int _hp; // 체력 수치
	public Image _hpProgress; // 체력바

	private AudioSource myAudio;
	private AudioClip enemyTouch;
	private AudioClip enemyDead;

	public string _targetTagName;

	//public CHealth _health; // 체력 관리 컴포넌트

	public Animator _animator; // 애니메이터 컴포넌트
	// Use this for initialization

	void Awake()
	{
		_animator = GetComponent<Animator>();
	}
	void Start () {
		myAudio = GameObject.Find ("BackGround").GetComponent<AudioSource> ();
		enemyTouch = GameObject.Find ("BackGround").GetComponent<CBackBlockDestroy> ().enemyTouch;
		enemyDead = GameObject.Find ("BackGround").GetComponent<CBackBlockDestroy> ().enemyDead;
	}

	// Update is called once per frame
	void Update () {

	}
	void OnMouseDown ()
	{
		//_animator.Play ("Damage");

		int damage = 10; //GetComponent <CAttackInfo> ().GetDamage ();

		_hpProgress.fillAmount -= (damage * 0.02f);

		_hp -= damage; // 체력 수치 감소
		myAudio.PlayOneShot (enemyTouch, enemyTouch.length);

		if (_hp <= 0) { // 체력이 0이되면
			Destroyer (); // 오브젝트를 파괴함
		}


	}

	// 파괴 처리
	public void Destroyer ()
	{
		myAudio.PlayOneShot (enemyDead, enemyDead.length);
		// 스코어 증가

		// 마지막 피격 또는 파괴 이펙트를 생성함
		Instantiate (_damageEffectPrefab, transform.position, Quaternion.identity);

		// 오브젝트를 파괴함
		Destroy (gameObject);
	}

	// Bubble Toouch
	protected virtual void OnTriggerEnter2D(Collider2D col)
	{
		// Bubble Touch
		if (col.tag == _targetTagName)
		{
			Hit(col); // 데미지 피격 처리를 수행함
		}
	}

	// Bubble Destroy
	protected void Hit(Collider2D col)
	{
		Instantiate(_damageEffectPrefab, col.transform.position, Quaternion.identity);

		// 피격된 오브젝트를 파괴함
		Destroy(col.gameObject);
	}


}
