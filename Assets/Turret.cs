using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	[SerializeField] Vector3 goToPosition;
	[SerializeField] float speed = 1f;

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

		MoveToPosition();
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

	public void DestroyTurret()
	{
		Destroy(gameObject);
	}

	void MoveToPosition()
	{
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, goToPosition, step);
	}



}
