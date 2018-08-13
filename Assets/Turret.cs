using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	[SerializeField] ParticleSystem gun;
	bool fire = false;

	PlayerController player;

	void Start()
	{
		player = FindObjectOfType<PlayerController>();

	}
	
	void Update()
	{
		LookAtPlayer();

		if (Input.GetKeyDown(KeyCode.T))
		{
			fire = !fire;
			FireGun(fire);
		}
	}

	void FireGun(bool f)
	{
		var emission = gun.emission;
		emission.enabled = f;
	}

	void LookAtPlayer()
	{
		transform.LookAt(player.transform.position);
	}
}
