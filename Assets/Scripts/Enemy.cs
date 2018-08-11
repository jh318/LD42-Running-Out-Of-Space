using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	[SerializeField] float maxHealthPoints = 100f;

	float currentHealthPoints = 100f;

	float m_horizontalVelocity;
	float m_forwardVelocity;

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

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		player = FindObjectOfType<PlayerController>();

		Debug.Log(agent);
	}

	void Update()
	{
		SetDestination(player.transform.position);
		Debug.Log(agent.velocity);
		m_horizontalVelocity = agent.velocity.x;
		m_forwardVelocity = agent.velocity.z;
		UpdateAnimator();
	}

	void SetDestination(Vector3 targetPosition)
	{
		agent.SetDestination(targetPosition);
	}

	void UpdateAnimator()
	{
		animator.SetFloat("HorizontalVelocity", m_horizontalVelocity);
		animator.SetFloat("ForwardVelocity", m_forwardVelocity);
	}

}
