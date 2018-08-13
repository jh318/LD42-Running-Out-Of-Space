using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreTracker : MonoBehaviour {

	public static ScoreTracker instance;
	public float timeScore = 0f;
	public float bestTime = 0f;
	bool trackTime = false;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

	void OnEnable()
	{
		SceneManager.sceneLoaded += UpdateBestScore;
		SceneManager.sceneLoaded += ResetScoreTracker;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded -= UpdateBestScore;
		SceneManager.sceneLoaded -= ResetScoreTracker;

	}


	void Start()
	{
		
		
		
	}

	void Update()
	{
		if(SceneManager.GetActiveScene().buildIndex == 1)
		{
			timeScore += Time.deltaTime;	
		}
	}

	void ResetScoreTracker(Scene name, LoadSceneMode mode)
	{
		if(SceneManager.GetActiveScene().buildIndex == 1)
		{
			timeScore = 0;
		}
	}

	void UpdateBestScore(Scene name, LoadSceneMode mode)
	{
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			if (timeScore > bestTime)
			{
				bestTime = timeScore;
			}
		}
	}

}
