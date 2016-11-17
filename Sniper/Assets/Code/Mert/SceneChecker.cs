using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChecker : MonoBehaviour {

    //[SerializeField] private BriefingMenuManager _manager;
    static bool spawned = false;

    public string _prevScene;
    public string _currentScene;
    public int index = 0;

	// Use this for initialization
	void Start () {
        /* if(spawned == false) {
             spawned = true;
             DontDestroyOnLoad( gameObject );
         }
         else {
             DestroyImmediate( gameObject );
         }*/
        _currentScene = GetCurrentScene( );

    }

    // Update is called once per frame
    void Update() {
       // _manager = GameObject.Find( "MissionBriefingMenuUI" ).GetComponent<BriefingMenuManager>( );

        // UpdateInfo( );
        SaveCurrentScene( );
    }

   /* void UpdateInfo() {
        switch(_prevScene) {
            case "StartMenu":
                index = 1;
                break;
            case "Lunch Interrupted":
                index = 0;
                break;
        }

        if(_manager)
        _manager.UpdateBriefingMenu( index );
    }*/

    public string GetCurrentScene() {
        return SceneManager.GetActiveScene().name;
    }

    public void SaveCurrentScene() {
        PlayerPrefs.SetString( "scene", _currentScene );
    }


}
