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
	Rigidbody rb;
	[SerializeField] Transform baseTr;
	[SerializeField] Transform cannonShootPosition;

	[SerializeField] GameObject bulletPrefab;
	[SerializeField] GameObject waterPrefab;

	[Header("Stats")]
	public int waterLevel = 100; //HP
	public float moveSpeed;
	public float rotationSpeed;
	public float bulletSpeed;
	public float cooldownShoot = 0.5f;
	public float cooldownDash = 0.5f;

	public float knockbackMagnitude = 2;
	public int damage = 10;
	public float slideFactor;
	private Vector3 slideVector;

	//Buttons
	private Vector3 shootDir;
	private bool onShootCooldown; 
	private bool releasedShootTrigger = true;

	private Vector3 dashDir;
	private bool onDashCooldown; 
	private bool releasedDashTrigger = true;

	void Start () 
	{
		rb = GetComponent<Rigidbody>(); 
		tr = GetComponent<Transform>();
		player = ReInput.players.GetPlayer(playerID);
	}
	
	void Update () 
	{
		InputMouvement();
		InputAim();
		RotateBase();
		InputShoot();
		InputDash();
	}

	void InputMouvement()
	{
		Vector3 moveVector = new Vector3(player.GetAxis("MoveX"),0,player.GetAxis("MoveY"));

		if(moveVector.magnitude > 1)
			moveVector.Normalize();


		if(moveVector.magnitude == 0)
		{		
			slideVector *= slideFactor;
		}
		else
		{
			slideVector += moveVector * slideFactor;
			if(slideVector.magnitude > 1)
				slideVector.Normalize();
		}

		tr.position += (moveVector + slideVector) * moveSpeed * Time.deltaTime;

	
	}
	void InputAim()
	{
		Vector3 aimVector = new Vector3(player.GetAxis("AimX"),0,player.GetAxis("AimY"));

		if(aimVector.magnitude < .3)
			return;

		aimVector.Normalize();
		shootDir = aimVector;
	}
	void RotateBase()
	{
		float step = rotationSpeed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(baseTr.forward, shootDir, step, 0.0f);
		baseTr.rotation = Quaternion.LookRotation(newDir);
	}
	#region Shoot
	void InputShoot()
	{
		if(player.GetAxis("Shoot") > .5f)
		{
			if(!onShootCooldown && releasedShootTrigger)
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
		onShootCooldown = true;
		yield return new WaitForSeconds(cooldownShoot);
		onShootCooldown = false;
	}
	void InputDash()
	{
		if(player.GetAxis("Dash") > .5f)
		{
			if(!onDashCooldown && releasedDashTrigger)
			{
				Dash();
				StartCoroutine(CooldownDash());
			}
		}
		else
		{
			releasedDashTrigger = true;
		}

	}
	void Dash()
	{

	}
	IEnumerator CooldownDash()
	{
		onDashCooldown = true;
		yield return new WaitForSeconds(cooldownDash);
		onDashCooldown = false;
	}



	void ShootBullet()
	{
		GameObject bullet = Instantiate(bulletPrefab, cannonShootPosition.position, Quaternion.identity);
		bullet.GetComponent<Bullet>().InitialiseBullet(playerID,baseTr.forward,bulletSpeed,damage);
	}
	#endregion

	void OnTriggerEnter(Collider col)
	{
		if(col.CompareTag("Bullet"))
		{
			HitByBullet(col.GetComponent<Bullet>());
		}
	}

	#region Bullet
	void HitByBullet(Bullet bullet)
	{
		if(bullet.playerID != playerID)
		{
			//knockback
			rb.AddForce((bullet.transform.position - tr.position) * knockbackMagnitude);
			Damaged(bullet);
		
			Stunned();
			LooseWater();
		}
	}
	void Damaged(Bullet bullet)
	{
		waterLevel -= bullet.damage;
		if(waterLevel <= 0)
		{
			DeathPlayer();
		}
	}
	void Stunned()
	{

	}
	void LooseWater()
	{

	}
	void DeathPlayer()
	{

	}
	#endregion
}
