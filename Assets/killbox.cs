using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killbox : MonoBehaviour {

	void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<PlayerHealth>())
		{
			c.GetComponent<PlayerHealth>().DamagePlayer(999);
		}
	}
}
