using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

    private int _numberOfKills;
    private int _numberOfHeadshots;
	private float _startTime;
	private float _elapsedTime;
	private float _timeLastAction;
	private int _lastWaveSpawned;
	[SerializeField] private float _actionTimer;
	public List<GameObject> _activeEnemies;							// made this public so that SpotterTool can access it.
	[SerializeField] private List<GameObject> _firstWave;
	[SerializeField] private List<GameObject> _firstSpecials;
	[SerializeField] private List<GameObject> _secondWave;
	[SerializeField] private List<GameObject> _secondSpecials;
    [SerializeField] private UIManager _UIManager;

    public Transform _bloodPfxPrefab;

    void Update () {
		
		if (Time.time - _timeLastAction > _actionTimer)
		{
			TakeAction();
		}

		if (Time.time - _startTime > 1 && _lastWaveSpawned == 0)
		{
			SpawnWave(_firstWave, true);
			SpawnWave(_firstSpecials, false);
			_lastWaveSpawned = 1;
		}

	}

	private void TakeAction ()
	{
		_timeLastAction = Time.time;
		BaseAI _thisEnemyScript = _activeEnemies[Random.Range(0, _activeEnemies.Count)].GetComponent<BaseAI>();
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

	public void KillEnemy (InteractiveItem _enemy)
	{
        if (_enemy) {
            Instantiate(_bloodPfxPrefab, _enemy.transform.position, Quaternion.identity);
            var enemyHealth = _enemy.GetComponent<IHealth>();
            var info = new DamageInfo(enemyHealth.CurrentHealth);
            _enemy.GetComponent<IDamageable>().Damage(info);
            _numberOfKills++;
            PlayerPrefs.SetInt("kills", GetNumberOfKills());
            _activeEnemies.Remove(_enemy.gameObject);
            if (_activeEnemies.Count == 0 && _lastWaveSpawned == 1) {
                SpawnWave(_secondWave, true);
                SpawnWave(_secondSpecials, false);
                _lastWaveSpawned = 2;
            }

            UpdatePlayerData();
        }
	}

    public int GetNumberOfKills() {
        return _numberOfKills;
    }

    public void UpdatePlayerData() {
        _UIManager._bonusTime += 15;
        _UIManager._timerPfx.Emit(50);
        _UIManager._playerScore += 250;
        _UIManager._scoreText.text = _UIManager._playerScore.ToString();
        _UIManager._scorePFX.Emit(50);


    }
}
