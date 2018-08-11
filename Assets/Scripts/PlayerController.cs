using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{

	public bool thirdPerson = true;
	public bool toggleSword = false;
	
	public float Horizontal { get { return m_horizontal; } }
	public float Vertical { get { return m_vertical; } }

	[SerializeField] float maxTurnSpeed = 500f;
	[SerializeField] float jumpForce = 5f;
	[SerializeField] GameObject Sword;
	
	float rotation = 0.0f;
	float m_horizontal = 0.0f;
	float m_vertical = 0.0f;
	float m_horizontalRaw = 0.0f;
	float m_verticalRaw = 0.0f;
	
	Vector3 targetDirection;
	Vector3 axisDirection;
	Vector3 forwardToTargetCrossProduct;
	Vector3 targetVector;

	Animator animator;
	ThirdPersonCamera thirdPersonCamera;
	Rigidbody body;

	void Awake()
	{
		
	}

	void Start()
	{
		animator = GetComponent<Animator>(); // Assuming both are on root
		body = GetComponentInChildren<Rigidbody>();

		if (thirdPerson)
		{
			thirdPersonCamera = GameObject.FindObjectOfType<ThirdPersonCamera>().GetComponent<ThirdPersonCamera>();
		}
	}

	void Update()
	{
		GetAxes();
		SetAnimator();
		ToggleSword();
		Jump();

		if (Input.GetButtonDown("Fire1"))
		{
			animator.SetTrigger("Attack");
			var x = FindObjectOfType<Enemy>();
			//Debug.Log(x.healthAsPercentage);
			//x.healthAsPercentage = x.healthAsPercentage - 30f;
			x.CurrentHealthPoints = x.CurrentHealthPoints - 30f;
			//x.healthAsPercentage = 50f;
			
			Debug.Log(x.healthAsPercentage);
		}

		if (Input.GetButtonDown("Fire2"))
		{
			// animator.SetTrigger("Attack2");
		}

		if (thirdPerson)
		{
			ThirdPersonCamera();
		}

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

	void ThirdPersonCamera()
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

	void ToggleSword()
	{
		if(toggleSword)
		{
			Sword.gameObject.SetActive(true);
		}
		else
		{
			Sword.gameObject.SetActive(false);
		}
	}

	void Jump()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if(animator.GetBool("CanJump") && !animator.GetBool("IsJumping"))
			{
				animator.applyRootMotion = false;
				body.velocity += Vector3.up * jumpForce;
			}
		}
	}

	void Land()
	{
		
	}

	void OnCollisionEnter(Collision c)
	{
		float dotProduct = Vector3.Dot(c.gameObject.transform.up, transform.up);
		if (dotProduct > 0.9f)
		{
			Debug.Log("Landed");
			animator.SetBool("CanJump", true);
			animator.SetBool("IsJumping", false);
			animator.SetTrigger("Landing");
			animator.applyRootMotion = true;
		}
	}

}
