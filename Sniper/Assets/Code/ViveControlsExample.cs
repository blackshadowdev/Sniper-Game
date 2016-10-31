using UnityEngine;
using System.Collections;
//
//	1) In your scene you should have controllers attached to the camera rig, eg:
//	[CameraRig]
//	-- Controller (Left)
//
//	2) Ensure that controller has both a "SteamVR_TrackedObject" script AND "SteamVR_TrackedController" script
//
//	3) Add this script to the controller, and modify it as necessary
//
[RequireComponent(typeof(SteamVR_TrackedController))]
public class ViveControlsExample : MonoBehaviour {

	public sceneManager _sceneManager;
    private MenuManager _menuManager;
	public string _leftOrRight;
    public Laser _laser;


	// Use this for initialization
	void OnEnable () {
		SteamVR_TrackedController controller = GetComponent<SteamVR_TrackedController>();
		controller.TriggerClicked += OnClickTrigger;
		controller.TriggerUnclicked += OnUnclickTrigger;
		controller.PadClicked += OnPadClicked;

		_sceneManager = GameObject.Find("Scene Manager").GetComponent<sceneManager>();
        _menuManager = FindObjectOfType<MenuManager>();
	}

	void OnDisable(){
		SteamVR_TrackedController controller = GetComponent<SteamVR_TrackedController>();
		controller.TriggerClicked -= OnClickTrigger;
		controller.TriggerUnclicked -= OnUnclickTrigger;
		controller.PadClicked -= OnPadClicked;
	}

	void OnPadClicked(object sender, ClickedEventArgs e){
		Debug.Log ("Pad Clicked! X: " + e.padX + " " + e.padY);

		if (_leftOrRight == "left")
		{
			_sceneManager.ScopeSwitch();
		}
	}


	void OnClickTrigger(object sender, ClickedEventArgs e) 
	{
		Debug.Log("Clicked trigger!");
		//_sceneManager._triggerIsDown = true;

		// pull Trigger on Right Controller
		if (_leftOrRight == "right")
		{
			if (_laser) {
				if (_laser._newGameButtonClicked) {
					_menuManager.LoadScene ("MissionBriefing");
				}

				if (_laser._loadLevelButtonClicked) {
					_menuManager.LoadScene (""); //TODO Mert: Add load level scene name
				}

				if (_laser._normalOptionButtonClicked) {
					_menuManager.LoadScene (""); //TODO Mert: Add game scene name
				}

				if (_laser._hardOptionButtonClicked) {
					_menuManager.LoadScene (""); //TODO Mert: Add game scene name
				}
			}
            _sceneManager.Fire();
		}
	}

	void OnUnclickTrigger(object sender, ClickedEventArgs e) 
	{
		Debug.Log("Unclicked trigger!");
		//_sceneManager._triggerIsDown = false;
		//_sceneManager.ClearUI();
		_sceneManager.TriggerUp();

	}

	void OnPadTouched(object sender, ClickedEventArgs e)
	{
		Debug.Log("pad touched: " + e.padX + " " + e.padY);
		//_sceneManager._padIsTouched = true;
		//_sceneManager._padX = e.padX;
		//_sceneManager._padY = e.padY;

		if (_leftOrRight == "right")
		{
			_sceneManager._rightPadY = e.padY;
			Debug.Log("rPadY = " + _sceneManager._rightPadY);
		}
	}

}