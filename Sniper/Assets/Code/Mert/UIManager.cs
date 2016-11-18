using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIManager : MonoBehaviour { 
    public bool _padIsTouched;
    public float _startTime;
	public float _elapsedTime;
	public int _bonusTime;
	public int _playerScore;

    public Text _timerText;
    public Text _scoreText;

    public ParticleSystem _scorePFX;
    public ParticleSystem _timerPfx;
    
    void Start () {
		_startTime = Time.time;
		_scoreText.text = "0";		
	}

    void Update() {
        _elapsedTime = 100 - Time.time + _startTime + _bonusTime;
        _timerText.text = _elapsedTime.ToString();
        PlayerPrefs.SetInt("score", GetCurrentScore());
        PlayerPrefs.SetFloat("time", GetCurrentTime());
    }

    public int GetCurrentScore() {
        return _playerScore;
    }

    public float GetCurrentTime() {
        return _elapsedTime;
    }
}
