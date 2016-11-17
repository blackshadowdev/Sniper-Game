using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChecker : MonoBehaviour {
    static bool spawned = false;

    public string _prevScene;
    public string _currentScene;
    public int index = 0;

	// Use this for initialization
	void Start () {
        _currentScene = GetCurrentScene( );
    }

    // Update is called once per frame
    void Update() {
        SaveCurrentScene( );
    }

    public string GetCurrentScene() {
        return SceneManager.GetActiveScene().name;
    }

    public void SaveCurrentScene() {
        PlayerPrefs.SetString( "scene", _currentScene );
    }


}
