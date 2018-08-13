using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//FP - Fight Property (label to help find methods easier in inspector)

public class FightController : MonoBehaviour 
{
	bool cancelWindow = false;
	GameObject activeHitbox;
	[SerializeField] PlayerHitBox[] playerHitboxes;
	//List<GameObject> hitboxes = new List<GameObject>();
	Technique currentTechnique;
	Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
		GetHitboxes();
		SetupHitboxes();
	}


	void Update()
	{
		if (cancelWindow)
		{
			Cancel();
		}
	}

	void FP_HitboxOn(string nameOfHitbox)
	{
		if (playerHitboxes != null)
		{
			foreach (PlayerHitBox hitbox in playerHitboxes)
			{
				if (hitbox.name == nameOfHitbox)
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

	void FP_CancelWindowOn()
	{
		cancelWindow = true;
	}

	void FP_CancelWindowOff()
	{
		cancelWindow = false;
	}

	void Cancel()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			animator.SetTrigger("Cancel"); //TODO Set up to interace with player anim
		}
	}

	void FP_AttackName(Technique tech)
	{
		currentTechnique = tech;
		UserVelocity(tech);
	}

	void OnTriggerEnter(Collider c)
	{
		GameObject g = c.gameObject;

		if(g.GetComponent<Enemy>())
		{
			Enemy enemy = g.GetComponent<Enemy>();
			enemy.hitStun = true;
			enemy.CurrentHealthPoints = enemy.CurrentHealthPoints - 10f; // TODO let tech decide damage
			if(enemy.GetComponent<NavMeshAgent>()) // TODO move this to the enemy if possible
			{
				enemy.GetComponent<NavMeshAgent>().enabled = false;
			}
			SpecialPropertiesCheck(currentTechnique, enemy.gameObject);
		}
	}

	void GetHitboxes()
	{
		playerHitboxes = GetComponentsInChildren<PlayerHitBox>();
	}

	void SetupHitboxes()
	{
		foreach (PlayerHitBox hitbox in playerHitboxes)
		{
			hitbox.GetComponent<Collider>().enabled = false;
		}
	}

	void SpecialPropertiesCheck(Technique tech, GameObject target)
	{
		target.GetComponent<Enemy>().hitStunTimer = tech.hitStun;

		if (target.GetComponent<Rigidbody>())
		{
			Rigidbody targetBody = target.GetComponent<Rigidbody>();
			targetBody.constraints = RigidbodyConstraints.None;
				targetBody.velocity = Vector3.zero;

			if (tech.launch)
			{
				targetBody.AddForce(transform.forward * tech.force, ForceMode.Impulse);
			}
			else if (tech.juggle)
			{
				target.GetComponent<Enemy>().juggled = true;
				target.GetComponent<Enemy>().JuggleState();
				targetBody.AddForce(Vector3.up * tech.force, ForceMode.Impulse);
			}
			else if (tech.dizzy)
			{
				//TODO Implement dizzy
			}

		}
	}

	void UserVelocity(Technique tech)
	{
		if (tech.alterSelfVelocity)
		{
			if (gameObject.GetComponent<Rigidbody>())
			{
				Rigidbody body = gameObject.GetComponent<Rigidbody>();

				body.velocity = tech.setVelocity;
			}
		}
	}


}
