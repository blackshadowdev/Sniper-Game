using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	private float _startTime;
	private float _elapsedTime;
	private float _timeLastAction;
	private int _lastWaveSpawned;
	[SerializeField] private float _actionTimer;
	[SerializeField] private List<GameObject> _activeEnemies;
	[SerializeField] private List<GameObject> _firstWave;
	[SerializeField] private List<GameObject> _secondWave;




	void Start () {
	}
	
	void Update () {
		
		if (Time.time - _timeLastAction > _actionTimer)
		{
			TakeAction();
		}

		if (Time.time - _startTime > 1 && _lastWaveSpawned == 0)
		{
			SpawnWave(_firstWave);
			_lastWaveSpawned = 1;
		}

	}

	private void TakeAction ()
	{
		_timeLastAction = Time.time;
		AIControl _thisEnemyScript = _activeEnemies[Random.Range(0, _activeEnemies.Count)].GetComponent<AIControl>();
		if (_thisEnemyScript._imBusy == false)
			_thisEnemyScript.TakeAction();
	}

	private void SpawnWave (List<GameObject> _thisWave)
	{
		for (int i = 0; i < _thisWave.Count; i ++)
		{
			_thisWave[i].SetActive(true);
			_activeEnemies.Add(_thisWave [i]);
		}
	}

	public void KillEnemy (GameObject _hitEnemy)
	{
		_hitEnemy.GetComponent<AIControl>().Die();
		_activeEnemies.Remove(_hitEnemy);
		if (_activeEnemies.Count == 0 && _lastWaveSpawned == 1)
		{
			SpawnWave(_secondWave);
			_lastWaveSpawned = 2;
		}
	}
}
