using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	[HideInInspector] public int turretCount = 1;
	[SerializeField] int maxTurrets = 1; 

	[SerializeField] float spawnEnemyRoutinePeriod = 5f;
	[SerializeField] float spawnTurretPeriod = 10f;
	[SerializeField] int maxEnemyCount = 10;

	[SerializeField] GameObject turretPrefab;
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] Transform turretSpawnPoint;

	PlayerController player;
	
	void Start()
	{
		if (FindObjectOfType<PlayerController>())
		{
			player = FindObjectOfType<PlayerController>();
		}

		

		StartCoroutine(SpawnEnemyRoutine());
		StartCoroutine(SpawnTurretRoutine());

	}

	IEnumerator SpawnEnemyRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnEnemyRoutinePeriod);
			if(FindObjectsOfType<Enemy>().Length < maxEnemyCount)
			{
				SpawnEnemy();
			}
		}
	}

	void SpawnEnemy()
	{
		Instantiate(enemyPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
	}

	IEnumerator SpawnTurretRoutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(spawnTurretPeriod);
			if(turretCount < maxTurrets)
			{
				SpawnTurret();
				Debug.Log("turretCount" + turretCount);
			}
		}
	}

	void SpawnTurret()
	{
		Instantiate(turretPrefab, turretSpawnPoint.transform.position, Quaternion.identity);
		turretCount++;
	}
}
