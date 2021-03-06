﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{

	public bool thirdPerson = true;
	
	public float Horizontal { get { return m_horizontal; } }
	public float Vertical { get { return m_vertical; } }

	public Vector3 TargetDirection{
		get { return targetDirection; }
	}

	[SerializeField] float maxTurnSpeed = 500f;
	[SerializeField] float jumpForce = 5f;
	[SerializeField] GameObject Sword;
	[SerializeField] float swordDrawTime = 3f;
	[SerializeField] float boostPower = 10f;

	
	float rotation = 0.0f;
	float m_horizontal = 0.0f;
	float m_vertical = 0.0f;
	float m_horizontalRaw = 0.0f;
	float m_verticalRaw = 0.0f;
	float swordTimer = 0.0f;
	
	Vector3 targetDirection;
	Vector3 axisDirection;
	Vector3 forwardToTargetCrossProduct;
	Vector3 targetVector;

	Animator animator;
	ThirdPersonCamera thirdPersonCamera;
	Rigidbody body;

	void Start()
	{
		animator = GetComponent<Animator>(); // Assuming both are on root
		body = GetComponentInChildren<Rigidbody>();
		CheckForThirdPersion();
		StartCoroutine(SwordStatus());
	}

	void Update()
	{
		GetAxes();
		SetAnimator();
		Jump();
		AttackInputs();
		DodgeShift();

		ThirdPersonCamera();
	}

	void FixedUpdate()
	{
	}

	void GetAxes()
	{
		m_horizontal = Input.GetAxis("Horizontal");
		m_vertical = Input.GetAxis("Vertical");
		m_horizontalRaw = Input.GetAxisRaw("Horizontal");
		m_verticalRaw = Input.GetAxisRaw("Vertical");
	}

	void SetAnimator()
	{
		if (animator != null)
		{
			animator.SetFloat("Horizontal", m_horizontal);
			animator.SetFloat("Vertical", m_vertical);
			animator.SetFloat("Speed", Mathf.Abs(m_horizontal) + Mathf.Abs(m_vertical));
			animator.SetFloat("JumpVelocity", body.velocity.y);
		}
	}

	void ThirdPersonCamera() // Player movement dictated by a third person camera
	{
		if(thirdPerson)
		{
			if (thirdPersonCamera != null)
			{
				axisDirection = new Vector3(m_horizontalRaw, 0.0f, m_verticalRaw);
				targetDirection = 
				(
					thirdPersonCamera.targetCamera.transform.forward
					* axisDirection.z
					+ thirdPersonCamera.targetCamera.transform.right
					* axisDirection.x
				);
				forwardToTargetCrossProduct = Vector3.Cross(transform.forward, targetDirection);
				rotation = forwardToTargetCrossProduct.y;

				if (Vector3.Dot(transform.forward, targetDirection) < 0.0f)
				{
					rotation = Mathf.Sign(rotation);
				}

				targetVector = new Vector3(1.0f, rotation, 1.0f); // TODO see if this is needed
				transform.eulerAngles += Vector3.up * rotation * maxTurnSpeed * Time.deltaTime;

			}
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
			//Sword.gameObject.SetActive(true);
		}
		else
		{
			var x = Sword.GetComponentsInChildren<ParticleSystem>();
			foreach (ParticleSystem p in x)
			{
				var emission = p.emission;
				emission.enabled = false;
			}
			//Sword.gameObject.SetActive(false);
		}
	}

	void Jump()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if(animator.GetBool("CanJump") && !animator.GetBool("IsJumping"))
			{
				animator.applyRootMotion = false;
				animator.SetBool("IsJumping", true);
				body.velocity += Vector3.up * jumpForce;

			}
		}
	}

	void OnCollisionEnter(Collision c)
	{
		float dotProduct = Vector3.Dot(c.gameObject.transform.up, transform.up);
		if (dotProduct > 0.9f)
		{
			Land();
		}
	}

	void OnCollisionStay(Collision c)
	{
		if (c.gameObject.tag == "Ground")
		{
			//animator.applyRootMotion = false;
			animator.SetBool("IsJumping", false);
			animator.SetTrigger("Landing");
		}
	}

	void OnCollisionExit(Collision c)
	{
		float dotProduct = Vector3.Dot(c.gameObject.transform.up, transform.up);
		if (dotProduct > 0.9f && c.gameObject.tag == "Ground")
		{
			animator.applyRootMotion = false;
			animator.SetBool("IsJumping", true);
		}
	}

	void Land()
	{
		Debug.Log("Landed");
		animator.SetBool("CanJump", true);
		animator.SetBool("IsJumping", false);
		animator.SetTrigger("Landing");
		animator.applyRootMotion = true;
	}

	void CheckForThirdPersion()
	{
		if (thirdPerson)
		{
			thirdPersonCamera = GameObject.FindObjectOfType<ThirdPersonCamera>().GetComponent<ThirdPersonCamera>();
		}
	}

	void AttackInputs()
	{
		Attack1();
		Attack2();
	}

	void Attack1()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if(!animator.GetBool("IsRunning"))
			{
				SetAttackDirectionToCamera();
			}
			animator.SetTrigger("Attack");		
			swordTimer = swordDrawTime;
		}
	}

	void SetAttackDirectionToCamera()
	{
		transform.eulerAngles = new Vector3(
			0f,
			thirdPersonCamera.Pivot.transform.eulerAngles.y,
			0f
		);
	}

	void Attack2()
	{
		if (Input.GetButtonDown("Fire2"))
		{
			SetAttackDirectionToCamera();
			animator.SetTrigger("SpecialAttack");
		}
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

	void DodgeShift()
	{
		PlayerHealth energy = GetComponentInChildren<PlayerHealth>();
		if(Input.GetKeyDown(KeyCode.LeftShift) && energy.currentEnergy >= 25f)
		{
			GetComponentInChildren<PlayerHealth>().UseEnergy(25f);
			StartCoroutine((DodgeShiftTime(0.01f)));
		}
	}

	IEnumerator DodgeShiftTime(float timer)
	{
		animator.applyRootMotion = false;
		body.velocity = Vector3.zero;
		body.velocity += (targetDirection) * boostPower;
		//body.AddForce(targetDirection * boostPower * body.mass, ForceMode.Impulse);
		animator.SetBool("IsDodgeShift", true);
		animator.SetTrigger("DodgeShift");

		while(timer >= 0)
		{
			timer -= Time.deltaTime;
			// if (Mathf.Abs(body.velocity.z) < 1f || Mathf.Abs(body.velocity.x) < 1f)
			// {
			// 	break;
			// }
			yield return new WaitForEndOfFrame();
		}

		animator.SetBool("IsDodgeShift", false);

		
	}

}
