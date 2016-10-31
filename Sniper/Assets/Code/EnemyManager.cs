using UnityEngine;
using System.Collections;
using System.Collections.Generic; // so that I can use Lists

public class EnemyManager : MonoBehaviour {

	//public GameObject[] _enemies;
	private float _startTime;
	private float _elapsedTime;
	public float _actionTimer;
	private float _timeLastAction;
	public AIControl _thisEnemyScript;
	public List<GameObject> _activeEnemies;

	public List<GameObject> _firstWave;
	public List<GameObject> _secondWave;

	private bool _firstWaveSpawned;


	// Use this for initialization
	void Start () {
		//SpawnWave(_firstWave);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Time.time - _timeLastAction > _actionTimer)
		{
			TakeAction();
		}

		if (Time.time - _startTime > 1 && !_firstWaveSpawned)
		{
			SpawnWave(_firstWave);
			_firstWaveSpawned = true;
		}

	}

	private void TakeAction ()
	{
		_timeLastAction = Time.time;
		GameObject _thisEnemy;
		_thisEnemy = _activeEnemies[Random.Range(0, _activeEnemies.Count)];
		_thisEnemyScript = _thisEnemy.GetComponent<AIControl>();
		if (_thisEnemyScript._imBusy == false)
		_thisEnemyScript.TakeAction();
	}

	private void SpawnWave (List<GameObject> _thisWave)
	{
		for (int i = 0; i < _thisWave.Count; i ++)
		{
			Debug.Log("spawnwave called");
			_thisWave[i].SetActive(true);
			_activeEnemies.Add(_thisWave [i]);
		}
	}

	public void KillEnemy (GameObject _hitEnemy)
	{
		Debug.Log ("kıll enemy called");
		for (int _killedEnemy = 0; _killedEnemy < _activeEnemies.Count; _killedEnemy ++)
		{
			if (_hitEnemy.transform.gameObject == _activeEnemies[_killedEnemy])
			{
				_activeEnemies.RemoveAt(_killedEnemy);
				if (_activeEnemies.Count == 0) 
				{
					SpawnWave (_secondWave);
				}
			}
		}
	}
}
