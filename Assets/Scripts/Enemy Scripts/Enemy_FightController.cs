using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//FP - Fight Property (label to help find methods easier in inspector)

public class Enemy_FightController : MonoBehaviour 
{
	bool cancelWindow = false;
	GameObject activeHitbox;
	[SerializeField] Enemy_HitBox[] Enemy_HitBoxes;
	//List<GameObject> hitboxes = new List<GameObject>();
	Technique currentTechnique;
	Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
		GetHitboxes();
		SetupHitboxes();
	}


	void FP_HitboxOn(string techname)
	{
		if (Enemy_HitBoxes != null)
		{
			foreach (Enemy_HitBox hitbox in Enemy_HitBoxes)
			{
				if (hitbox.name == techname)
				{
					hitbox.GetComponent<Collider>().enabled = true;
					activeHitbox = hitbox.gameObject;
					break;
				}
			}
		}
	}

	void FP_HitboxOff()
	{
		if (activeHitbox != null)
		{
			activeHitbox.GetComponent<Collider>().enabled = false;	
			activeHitbox = null;
		}
	}

	void FP_AttackName(Technique tech)
	{
		currentTechnique = tech;
	}

	void GetHitboxes()
	{
		Enemy_HitBoxes = GetComponentsInChildren<Enemy_HitBox>();
	}

	void SetupHitboxes()
	{
		foreach (Enemy_HitBox hitbox in Enemy_HitBoxes)
		{
			hitbox.GetComponent<Collider>().enabled = false;
		}
	}

	void SpecialPropertiesCheck(Technique tech, GameObject target)
	{
		if (target.GetComponent<Rigidbody>())
		{
			if (tech.launch)
			{
				target.GetComponent<Rigidbody>().velocity = Vector3.zero;
				target.GetComponent<Rigidbody>().AddForce(transform.forward * tech.force, ForceMode.Impulse);
			}
			else if (tech.juggle)
			{
				target.GetComponent<Rigidbody>().velocity = Vector3.zero;
				target.GetComponent<Rigidbody>().AddForce(Vector3.up * tech.force, ForceMode.Impulse);
			}
			else if (tech.dizzy)
			{
				//TODO Implement dizzy
			}

		}
	}

	void OnTriggerEnter(Collider c)
	{
		GameObject g = c.gameObject;

		if(g.GetComponent<PlayerController>())
		{
			PlayerController target = g.GetComponent<PlayerController>();
			Debug.Log("Hit the target: " + target.name);
			//target.CurrentHealthPoints = target.CurrentHealthPoints - 30f; TODO make enemy damage player

		}
	}

}
