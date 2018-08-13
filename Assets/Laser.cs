using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	void OnParticleCollision(GameObject other)
	{
		if (other.GetComponent<PlayerController>())
		{
			Debug.Log("HIT THE PLAYER");
			other.GetComponent<PlayerHealth>().DamagePlayer(10f);
		}
	}
}
