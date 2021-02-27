using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishGenerator : MonoBehaviour {

	public Transform[] _spawnPoints; // Fish 생성 포인트들

	// 생성 지연 시간
	public float _minDelayTime;
	public float _maxDelayTime;
	public GameObject[] _fishPrefabs; // Fish 프리팹

	// Fish 생성
	public void FishStart()
	{
		StartCoroutine("GenCoroutine");
	}

	IEnumerator GenCoroutine()
	{
		while (true)
		{
			// 생성 위치 선정
			int posNum = Random.Range(0, _spawnPoints.Length);

			// 가변적인 생성 지연 시간을 설정함
			float delayTime = Random.Range(_minDelayTime, _maxDelayTime);

			// 생성 지연
			yield return new WaitForSeconds(delayTime);

			// 적기 생성
			Instantiate(_fishPrefabs[0], _spawnPoints[posNum].position,
				Quaternion.identity);
		}
	}	
}
