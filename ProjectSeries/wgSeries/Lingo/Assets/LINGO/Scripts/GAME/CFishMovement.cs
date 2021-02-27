using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFishMovement : MonoBehaviour
{
	public float _speed; // 이동 속도
	float _originSpeed; // 원래 속도
	public Vector2 _direction; // 이동 방향

	Transform _target = null;
	Vector3 _originTargetPos; // 처음 설정한 타겟의 위치

	// 타겟 방향 설정
	public void Init(Transform target)
	{
		// 타겟이 살아있으면
		if (_target != null)
		{
			_target = target;

			// 타겟을 향한 방향을 계산함
			_originTargetPos = _target.position;
			_direction = _originTargetPos - transform.position;
		}
		else // 타겟이 존재하지 않는다면
		{
			Destroy(gameObject);
		}

	}

	void Start()
	{
		// 오리지널 속도를 보관
		_originSpeed = _speed;
		_speed += Random.Range (0, 3);
	}

	public void Stop() // 이동 정지
	{
		_speed = 0;
	}

	public void Resume() // 이동 재개
	{
		_speed = _originSpeed;
	}

	// Update is called once per frame
	void Update ()
	{
		Move();
	}

	public void Move()
	{
		if (_target == null) {
			// 처음 생성시 설정된 방향으로 이동함
			transform.Translate (_direction.normalized * _speed * Time.deltaTime);
			return;
		}

		// 타겟의 방향을 다시 구해서 이동함
		_direction = _target.position - transform.position;
		transform.Translate (_direction.normalized * _speed * Time.deltaTime);
	}
}
