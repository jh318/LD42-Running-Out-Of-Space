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

	PlayerUI playerUI;

	void Start()
	{
		playerUI = GetComponentInChildren<PlayerUI>();
		Debug.Log(GetComponentInChildren<PlayerUI>());
	}

	void Update()
	{
		currentEnergy += energyRegeneration * Time.deltaTime;
		currentEnergy = Mathf.Clamp(currentEnergy, 0f, maxEnergy);
		SetEnergyBar();
		Debug.Log(currentEnergy); 
	}

	public void DamagePlayer(float damage)
	{
		currentHealth -= damage;
		SetHealthBar(damage);
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

	
}
