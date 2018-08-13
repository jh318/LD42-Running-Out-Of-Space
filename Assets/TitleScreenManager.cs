using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour {

	[SerializeField] Text score;

	void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		float minutes = Mathf.Floor(ScoreTracker.instance.bestTime / 60);
		float seconds = ScoreTracker.instance.bestTime % 60;
		score.text = "Best Time: " + minutes + ":" + Mathf.RoundToInt(seconds);
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene(1);
		}
	}
}
