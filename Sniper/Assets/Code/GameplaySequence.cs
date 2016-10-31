using UnityEngine;
using System.Collections;
using System.Collections.Generic; // so that I can use Lists


public class GameplaySequence : MonoBehaviour {

	public List<GameObject> _firstSpawn;
	public List<GameObject> _secondSpawn;
	private bool _firstWaveSpawned;
	private bool _secondWaveSpawned;
	private float _startTime;


	// Use this for initialization
	void Start () {
	
		SpawnWave(_firstSpawn);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - _startTime > 1 && !_firstWaveSpawned)
		{
			SpawnWave(_firstSpawn);
			_firstWaveSpawned = true;
		}

		if (Time.time - _startTime > 10 && !_secondWaveSpawned)
		{
			SpawnWave(_secondSpawn);
			_secondWaveSpawned = true;
		}
	}

	private void SpawnWave (List<GameObject> _thisWave)
	{
		for (int i = 0; i < _thisWave.Count; i ++)
		{
			Debug.Log("spawnwave called");
			_thisWave[i].SetActive(true);
		}
	}
}
