using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour 
{
	// public float SetHealth{
	// 	set { currentHealth -= currentHealth - value; }
	// }

	public float currentHealth = 100f;
	[SerializeField] float maxHealth = 100f;
	public float currentEnergy = 100f;
	[SerializeField] float maxEnergy = 100f;
	[SerializeField] float energyRegeneration = 1f;
	[SerializeField] float healthLoss = 1f;

	PlayerUI playerUI;
	Animator animator;

	public delegate void PlayerDeath();
	public static event PlayerDeath playerDeath;

	void Start()
	{
		animator = GetComponent<Animator>();
		playerUI = GetComponentInChildren<PlayerUI>();
		Debug.Log(GetComponentInChildren<PlayerUI>());
	}

	void Update()
	{
		if (!animator.GetBool("IsJumping"))
		{
			currentEnergy += energyRegeneration * Time.deltaTime;
		}
		currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
		SetEnergyBar();

		DamagePlayer(healthLoss * Time.deltaTime);
	}

	public void DamagePlayer(float damage)
	{
		currentHealth -= damage;
		SetHealthBar(damage);
		if(currentHealth <= 0)
		{
			playerDeath();
			animator.SetTrigger("Dead");
		}
	}

	void SetHealthBar(float damage)
	{
		playerUI.HealthBar.fillAmount = (currentHealth) / maxHealth;
	}

	public void UseEnergy(float energy)
	{
		currentEnergy -= energy;
		SetEnergyBar();

	}

	void SetEnergyBar()
	{
		playerUI.EnergyBar.fillAmount = (currentEnergy) / maxEnergy;
	}

	public void HealPlayer(float heal)
	{
		currentHealth += heal;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
		SetEnergyBar();
	}

	
}
