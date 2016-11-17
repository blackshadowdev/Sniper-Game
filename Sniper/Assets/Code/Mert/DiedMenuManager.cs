using UnityEngine;
using System.Collections;

public class DiedMenuManager : MonoBehaviour {
    public MenuButton _button;
    public string _prevScene;
    public string _sceneToLoad;

    void Start() {
        _prevScene = PlayerPrefs.GetString("scene");
        switch (_prevScene) {
            case "Lunch Interrupted":
                _sceneToLoad = "Lunch Interrupted";

                break;
            case "Cartoon City 1":
                _sceneToLoad = "Cartoon City 1";
                break;

            case "Lighthouse at Night":
                _sceneToLoad = "Lighthouse at Night";
                break;
        }

        SetSceneToLoad(_sceneToLoad);
    }

    public void SetSceneToLoad(string scene) {
        _button._sceneToLoad = scene;
    }
}
