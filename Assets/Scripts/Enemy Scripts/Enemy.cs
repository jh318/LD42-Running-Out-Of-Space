using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour 
{
	[HideInInspector] public bool hitStun = false;
	[HideInInspector] public float hitStunTimer = 0f;
	[HideInInspector] public bool juggled = false;


	[SerializeField] float maxHealthPoints = 100f;
	[SerializeField] GameObject Sword;
	[SerializeField] float swordDrawTime = 3f;

	float swordTimer;
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
		StartCoroutine(SwordStatus());

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
		swordTimer = swordDrawTime;
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

	void RecoveryState()
	{
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
	}

	public void JuggleState()
	{
		StartCoroutine(JuggleStun());
	}

	IEnumerator JuggleStun()
	{
		while(juggled)
		{
			hitStunTimer = 5;
			Debug.Log(hitStunTimer);
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator HitStunTime(float time)
	{
		hitStunTimer = time;
		while(hitStunTimer >= 0)
		{
			hitStunTimer -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		animator.SetBool("IsHitStun", false);
		hitStun = false;
		RecoveryState();
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

	IEnumerator SwordStatus()
	{
		while (true)
		{
			if(swordTimer >= 0)
			{
				ToggleSword(true);
				while(swordTimer >= 0)
				{
					swordTimer -= Time.deltaTime;
					yield return new WaitForEndOfFrame();
				}
				ToggleSword(false);
			}
			yield return new WaitForEndOfFrame();
		}
		
	}

	void ToggleSword(bool toggleSword)
	{
		if(toggleSword)
		{
			var x = Sword.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem p in x)
			{
				var emission = p.emission;
				emission.enabled = true;
			}
		}
		else
		{
			var x = Sword.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem p in x)
			{
				var emission = p.emission;
				emission.enabled = false;
			}
		}
	}

	void OnCollisionEnter(Collision c)
	{
		float dotProduct = Vector3.Dot(c.gameObject.transform.up, transform.up);
		if (dotProduct > 0.9f && c.gameObject.tag == "Ground")
		{
			juggled = false;
		}
	}
}
