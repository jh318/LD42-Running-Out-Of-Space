using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour 
{
	Text[] textUI;

	void OnEnable()
	{
		PlayerHealth.playerDeath += GameOver;
	}

	void OnDisable()
	{
		PlayerHealth.playerDeath -= GameOver;
	}

	void Start()
	{
		textUI = GetComponentsInChildren<Text>();
		foreach (Text t in textUI)
		{
			t.enabled = false;
		}
	}

	void GameOver()
	{
		foreach (Text t in textUI)
		{
			t.enabled = true;
		}
	}
}
