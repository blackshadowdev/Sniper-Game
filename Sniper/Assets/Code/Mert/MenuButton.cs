using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;


public class MenuButton : MonoBehaviour {
    public event Action<MenuButton> OnButtonShot;                   //This event is triggered when the selection button has been shot at.

    public string _sceneToLoad;
    [SerializeField] private InteractiveItem _interactiveItem;      // The interactive item for where the user should shoot to load the level.
    [SerializeField] private Rifle  _rifle;
    [SerializeField] private VRCameraFade _cameraFade;                 // This fades the scene out when a new scene is about to be loaded.

    private void OnEnable() {
        _interactiveItem.OnShoot += HandleSelection;
    }

    private void HandleSelection() {
        StartCoroutine(ActivateButton());
    }

    private IEnumerator ActivateButton() {
        
        // If anything is subscribed to the OnButtonShot event, call it.
        if (OnButtonShot != null)
            OnButtonShot(this);

        //Wait for camera to fade code
        yield return StartCoroutine(_cameraFade.BeginFadeOut(true));

        //Load Level
        SceneManager.LoadScene(_sceneToLoad);
    }

}
