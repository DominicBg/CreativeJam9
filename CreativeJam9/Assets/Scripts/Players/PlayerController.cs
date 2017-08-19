using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour {

	[Header("Info")]
	public int playerID;
	Player player;

	[Header("Component")]
	Transform tr;
	[SerializeField] Transform baseTr;
	[SerializeField] Transform cannonShootPosition;

	[SerializeField] GameObject bulletPrefab;

	[Header("Stats")]
	public float moveSpeed;
	public float rotationSpeed;
	public float bulletSpeed;
	public float cooldownShoot = 0.5f;


	private bool shootCooldown; 
	private bool releasedShootTrigger = true;

	void Start () 
	{
		tr = GetComponent<Transform>();
		player = ReInput.players.GetPlayer(playerID);
	}
	
	void Update () 
	{
		InputMouvement();
		InputAim();
		InputShoot();
	}

	void InputMouvement()
	{
		Vector3 moveVector = new Vector3(player.GetAxis("MoveX"),0,player.GetAxis("MoveY"));

		if(moveVector.magnitude > 1)
			moveVector.Normalize();

		tr.position += moveVector * moveSpeed * Time.deltaTime;
	}
	void InputAim()
	{
		Vector3 aimVector = new Vector3(player.GetAxis("AimX"),0,player.GetAxis("AimY"));

		if(aimVector.magnitude == 0)
			return;

		aimVector.Normalize();
		float step = rotationSpeed * Time.deltaTime;

		Vector3 newDir = Vector3.RotateTowards(baseTr.forward, aimVector, step, 0.0f);

		baseTr.rotation = Quaternion.LookRotation(newDir);
	}
	void InputShoot()
	{
		if(player.GetAxis("Shoot") > .5f)
		{
			if(!shootCooldown && releasedShootTrigger)
			{
				ShootBullet();
				StartCoroutine(CooldownShoot());
			}
		}
		else
		{
			releasedShootTrigger = true;
		}
	}
	IEnumerator CooldownShoot()
	{
		shootCooldown = true;
		yield return new WaitForSeconds(cooldownShoot);
		shootCooldown = false;
	}
	void ShootBullet()
	{
		GameObject bullet = Instantiate(bulletPrefab, cannonShootPosition.position, Quaternion.identity);
		bullet.GetComponent<Bullet>().InitialiseBullet(playerID,baseTr.forward,bulletSpeed);
	}

}
