using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour 
{
	[HideInInspector] public bool hitStun = false;
	[HideInInspector] public float hitStunTimer = 0f;


	[SerializeField] float maxHealthPoints = 100f;


	float currentHealthPoints = 100f;

	float m_horizontalVelocity;
	float m_forwardVelocity;

	GameObject target;

	PlayerController player;
	NavMeshAgent agent;
	Animator animator;



	public float CurrentHealthPoints
	{
		get { return currentHealthPoints; }
		set { currentHealthPoints = value; }
	}

	public float healthAsPercentage
	{
		get { return currentHealthPoints / maxHealthPoints; }
		set { currentHealthPoints = value; }
	}

	enum States {Chase, Attack};


	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		player = FindObjectOfType<PlayerController>();
		target = player.gameObject;

		StartCoroutine(StartAI());
	}

	void Update()
	{

		UpdateAnimator();
	}

	void SetDestination(Vector3 targetPosition)
	{
		if(agent.isActiveAndEnabled)
		{
			agent.SetDestination(targetPosition);
		}
	}

	void UpdateAnimator()
	{
		animator.SetFloat("HorizontalVelocity", m_horizontalVelocity);
		animator.SetFloat("ForwardVelocity", m_forwardVelocity);
	}

	void CheckState()
	{
		float targetDistance = Vector3.Distance(transform.position, target.transform.position);
		if(currentHealthPoints <= 0)
		{
			DestroyEnemy();
		}
		else if(hitStun)
		{
			HitStunState();
		}
		else if (targetDistance <= agent.stoppingDistance)
		{
			agent.enabled = false;
			AttackState();
			Debug.Log("I should attack");
		}
		else if (targetDistance >= agent.stoppingDistance)
		{
			Debug.Log("Chasing");
			agent.enabled = true;
			ChaseState();
		}
	}

	void ChaseState()
	{
		animator.SetBool("NearPlayer", false);
		animator.SetBool("CanAttack", false);
		SetDestination(player.transform.position);
		m_horizontalVelocity = agent.velocity.x;
		m_forwardVelocity = agent.velocity.z;
	}

	void AttackState()
	{
		Debug.Log("I have arrived!");
		animator.SetBool("NearPlayer", true);
	}

	void HitStunState()
	{
		if(!animator.GetBool("IsHitStun"))
		{
			animator.SetTrigger("Hit");
			animator.SetBool("IsHitStun", true);
			StartCoroutine(HitStunTime(hitStunTimer));
		}
		else
		{
			//Wait for end of coroutine
		}
	}

	IEnumerator HitStunTime(float time)
	{
		while(time >= 0)
		{
			Debug.Log(time);
			time -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		animator.SetBool("IsHitStun", false);
		hitStun = false;
	}

	IEnumerator NearPlayer()
	{
		while(animator.GetBool("NearPlayer"))
		{
			Debug.Log("I am near the player!");
			yield return new WaitForSeconds(1f);
		}
	}


	IEnumerator StartAI()
	{
		yield return new WaitForSeconds(1f); // Need to wait for navmesh stuff to initialize

		while(true)
		{
			CheckState();
			yield return new WaitForEndOfFrame();
		}
	}

	void DestroyEnemy()
	{
		StopAllCoroutines();
		Destroy(gameObject);
	}

}
