using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	[SerializeField] Vector3 goToPosition;
	[SerializeField] float speed = 1f;
	[SerializeField] float cooldown = 5f;
	[SerializeField] float firePeriod = 10f;

	[SerializeField] ParticleSystem gun;

	bool fire = false;

	PlayerController player;

	void Start()
	{
		player = FindObjectOfType<PlayerController>();
		StartCoroutine(FireRoutine());
	}
	
	void Update()
	{
		LookAtPlayer();
		MoveToPosition();
	}

	IEnumerator FireRoutine()
	{
		while(true)
		{
			FireGun(false);
			yield return new WaitForSeconds(cooldown);
			FireGun(true);
			yield return new WaitForSeconds(firePeriod);
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

	public void DestroyTurret()
	{
		FindObjectOfType<GameManager>().turretCount--; //TODO change if we make gamemanager static
		StopAllCoroutines();
		Destroy(gameObject);
	}

	void MoveToPosition()
	{
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, goToPosition, step);
	}



}
