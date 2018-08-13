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

	PlayerUI playerUI;

	void Start()
	{
		playerUI = GetComponentInChildren<PlayerUI>();
		Debug.Log(GetComponentInChildren<PlayerUI>());
	}

	public void DamagePlayer(float damage)
	{
		currentHealth -= damage;
		SetHealthBar(damage);
	}

	void SetHealthBar(float damage)
	{
		playerUI.HealthBar.fillAmount = (currentHealth - damage) / maxHealth;
	}

	
}
