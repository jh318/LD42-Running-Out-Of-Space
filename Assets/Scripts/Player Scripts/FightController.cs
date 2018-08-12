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
			FP_Cancel();
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

	void FP_Cancel()
	{
		if (Input.GetButtonDown("Fire1") && animator.GetBool("IsAttacking"))
		{
			animator.SetTrigger("Cancel"); //TODO Set up to interace with player anim
		}
	}

	void FP_AttackName(Technique tech)
	{
		currentTechnique = tech;
	}

	void OnTriggerEnter(Collider c)
	{
		GameObject g = c.gameObject;

		if(g.GetComponent<Enemy>())
		{
			Enemy enemy = g.GetComponent<Enemy>();
			enemy.CurrentHealthPoints = enemy.CurrentHealthPoints - 30f;
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
		if (target.GetComponent<Rigidbody>())
		{
			Debug.Log("Performing special attack");
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


}
