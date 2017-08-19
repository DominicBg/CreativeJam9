using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	Transform tr;
	float speed;

	public void InitialiseBullet(int playerID, Vector3 direction, float startSpeed)
	{
		tr = GetComponent<Transform>();

		tr.LookAt(tr.position + direction);
		speed = startSpeed;
	}

	void Update()
	{
		tr.position += tr.forward * Time.deltaTime * speed;
	}
}
