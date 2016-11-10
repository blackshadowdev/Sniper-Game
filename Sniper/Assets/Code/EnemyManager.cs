using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	private float _startTime;
	private float _elapsedTime;
	private float _timeLastAction;
	private int _lastWaveSpawned;
	[SerializeField] private float _actionTimer;
	public List<GameObject> _activeEnemies;							// made this public so that SpotterTool can access it.
	[SerializeField] private List<GameObject> _firstWave;
	[SerializeField] private List<GameObject> _secondWave;
	[SerializeField] private List<GameObject> _secondSpecials;

	void Start () {
	}
	
	void Update () {
		
		if (Time.time - _timeLastAction > _actionTimer)
		{
			TakeAction();
		}

		if (Time.time - _startTime > 1 && _lastWaveSpawned == 0)
		{
			SpawnWave(_firstWave, true);
			_lastWaveSpawned = 1;
		}

	}

	private void TakeAction ()
	{
		_timeLastAction = Time.time;
		AIControl _thisEnemyScript = _activeEnemies[Random.Range(0, _activeEnemies.Count)].GetComponent<AIControl>();
		_thisEnemyScript.TakeAction();
	}

	private void SpawnWave (List<GameObject> _thisWave, bool _addToActiveEnemiesList)
	{
		for (int i = 0; i < _thisWave.Count; i ++)
		{
			_thisWave[i].SetActive(true);
			if (_addToActiveEnemiesList)
				_activeEnemies.Add(_thisWave [i]);
		}
	}

	public void KillEnemy (GameObject _hitEnemy)
	{
		_hitEnemy.GetComponent<AIControl>().Die();
		_activeEnemies.Remove(_hitEnemy);
		if (_activeEnemies.Count == 0 && _lastWaveSpawned == 1)
		{
			SpawnWave(_secondWave, true);
			SpawnWave(_secondSpecials, false);
			_lastWaveSpawned = 2;
		}
	}
}
