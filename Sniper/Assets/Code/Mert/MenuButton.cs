using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;


public class MenuButton : MonoBehaviour {
    public event Action<MenuButton> OnButtonShot;                   //This event is triggered when the selection button has been shot at.

    [SerializeField] private string _sceneToLoad;
    [SerializeField] private InteractiveItem _interactiveItem;      // The interactive item for where the user should shoot to load the level.
    [SerializeField] private Rifle  _rifle;

    private void OnEnable() {
        _interactiveItem.OnShoot += HandleSelection;
    }

    private void HandleSelection() {
        StartCoroutine(ActivateButton());
    }

    private IEnumerator ActivateButton() {
        if(OnButtonShot != null)
            OnButtonShot(this);

        //TODO Mert: Add wait for camera to fade code
        yield return null; //This is temporary

        //Load Level
        SceneManager.LoadScene(_sceneToLoad);
    }

}
