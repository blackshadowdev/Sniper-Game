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
	public List<GameObject> _enemies;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Time.time - _timeLastAction > _actionTimer)
		{
			TakeAction();
		}
	}

	private void TakeAction()
	{
		_timeLastAction = Time.time;
		GameObject _thisEnemy;
		_thisEnemy = _enemies[Random.Range(0, _enemies.Count)];
		_thisEnemyScript = _thisEnemy.GetComponent<AIControl>();
		if (_thisEnemyScript._imBusy == false)
		_thisEnemyScript.TakeAction();
	}
}
