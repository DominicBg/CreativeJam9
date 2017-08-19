using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	void Awake()
	{
		instance = this;
	}

	public void SpawnWater(Vector3 fromPosition, Vector3 toPosition)
	{

	}
}
