using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public int playerID;
	public int damage;

	Transform tr;
	float speed;

	public void InitialiseBullet(int _playerID, Vector3 direction, float _speed, int _damage)
	{
		tr = GetComponent<Transform>();

		tr.LookAt(tr.position + direction);
		speed = _speed;
		playerID = _playerID;
		damage = _damage;
	}

	void Update()
	{
		tr.position += tr.forward * Time.deltaTime * speed;
	}
}
