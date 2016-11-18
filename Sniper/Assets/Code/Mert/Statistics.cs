using UnityEngine;
using UnityEngine.UI;
using System;

public class Statistics : MonoBehaviour {
    
    [SerializeField] private Text _kills;
    [SerializeField] private Text _headshots;
    [SerializeField] private Text _objectsDestroyed;
    [SerializeField] private Text _civilliansRescued;
    [SerializeField] private Text _civilliansLost;
    [SerializeField] private Text _score;
    [SerializeField] private Text _grade;
    [SerializeField] private int _totalPossiblePoints = 100;

    public void Start() {
        _kills.text = "Targets Defeated: " + PlayerPrefs.GetInt("kills").ToString();
        _headshots.text = "Headshots: " + PlayerPrefs.GetInt("headshots").ToString();
        _objectsDestroyed.text = "Objects Destroyed: " + PlayerPrefs.GetInt("objectDestroyed");
        _civilliansRescued.text = "Civillian Rescued: " + PlayerPrefs.GetInt("rescued");
        _civilliansLost.text = "Civillian Lost: " + PlayerPrefs.GetInt("lost");
        _score.text     = "Total Score: " + CalculateTotalScore().ToString();
        _grade.text      = "Grade: "  + CalculateGrade().ToUpper().ToString();
    }

    public int CalculateTotalScore() {
        return PlayerPrefs.GetInt("kills") + PlayerPrefs.GetInt("headshots") + PlayerPrefs.GetInt("objectDestroyed") + 
               PlayerPrefs.GetInt("rescued") + PlayerPrefs.GetInt("lost");
    }

    public string CalculateGrade() {
        int _totalScore = CalculateTotalScore();
        float _grade = _totalScore / _totalPossiblePoints * 100;
        if(_grade < 70)
            return "D";
        else if(_grade >= 70 && _grade <= 79)
            return "C";
        else if (_grade >= 80 && _grade <= 89)
            return "B";
        else if (_grade >= 90 && _grade <= 95)
            return "A";
        else
            return "S";
    }
}
