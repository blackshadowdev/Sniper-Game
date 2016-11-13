using UnityEngine;
using UnityEngine.UI;
using System;

public class Statistics : MonoBehaviour {
    
    [SerializeField] private Text _score;
    [SerializeField] private Text _time;
    [SerializeField] private Text _kills;

    public void Start() {
        _score.text = "Score: " + PlayerPrefs.GetInt("score").ToString() + " pts";
        _time.text  = "Time: "  + PlayerPrefs.GetFloat("time").ToString() + " seconds";
        _kills.text = "Kills: " + PlayerPrefs.GetInt("kills").ToString() + " kills";
    }
}
