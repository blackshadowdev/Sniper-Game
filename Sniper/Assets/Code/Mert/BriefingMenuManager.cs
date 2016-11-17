using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BriefingMenuManager : MonoBehaviour
{
    public MenuButton[] Buttons;
    public string _prevScene;
    public Text[] _missionText;
    public SpriteRenderer[] _targetImage;
    public int sceneIndex;
    public string sceneNameToLoad;

    public void Start() {
        _prevScene = PlayerPrefs.GetString( "scene" );
        
        switch(_prevScene) {
            case "StartMenu":
                sceneIndex = 0;
                sceneNameToLoad = "Cartoon City 1";
                
                break;
            case "Cartoon City 1":
                sceneIndex = 1;
                sceneNameToLoad = "Lunch Interrupted";
                break;

            case "Lunch Interrupted":
                sceneIndex = 2;
                sceneNameToLoad = "Lighthouse at Night";
                break;


        }

        UpdateBriefingMenu( sceneIndex );
        SetSceneToLoad( sceneNameToLoad );


        Debug.Log( "Previous scene: " + _prevScene );
        Debug.Log( "Scene index:" + sceneIndex );
    }

    private void UpdateBriefingMenu(int index) {

        for(int i = 0; i < 3; i++) {
            if(i == index) {
                _missionText[i].enabled = true;
                _targetImage[i].enabled = true;
            }
            else {
                _missionText[i].enabled = false;
                _targetImage[i].enabled = false;
            }
        }
    }

    public void SetSceneToLoad(string scene) {
        foreach(MenuButton _button in Buttons) {
            _button._sceneToLoad = scene;
        }
    }
}
