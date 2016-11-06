using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

    private GameObject _scopeCamera;
    [SerializeField]
    private MenuManager _menuManager;
    private ViveControlsExample _viveController;

    public bool _newGameButtonClicked = false;
    public bool _loadLevelButtonClicked = false;
    public bool _normalOptionButtonClicked = false;
    public bool _hardOptionButtonClicked = false;
    public bool _againButtonClicked = false;
    public bool _mainMenuButtonClicked = false;
    public bool _retryButtonClicked = false;
    public bool _nextButtonClicked = false;

    // Use this for initialization
    void Start() {
        _scopeCamera = GameObject.Find("Scope Camera");
        _menuManager = FindObjectOfType<MenuManager>();
        _viveController = FindObjectOfType<ViveControlsExample>();
    }

    public void OnTriggerEnter(Collider other) {
        if(other.name == "Start New Game Button") {
            _newGameButtonClicked = true;
        }

        if(other.name == "Load Level Button") {
            _loadLevelButtonClicked = true;
        }

        if (other.name == "Normal Button") {
            _normalOptionButtonClicked = true;
        }

        if (other.name == "Hard Button") {
            _hardOptionButtonClicked = true;
        }

        if (other.name == "Again Button") {
            _againButtonClicked = true;
        }

        if(other.name == "Main Menu Button") {
            _mainMenuButtonClicked = true;
        }

        if (other.name == "Retry Button") {
            _retryButtonClicked = true;
        }

        if (other.name == "Next Button") {
            _nextButtonClicked = true;
        }
    }
}