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
	public string _leftOrRight;

    [SerializeField] private Rifle _rifle;
	[SerializeField] private SpotterTool _spotterTool;
    [SerializeField] private UIManager _UIManager;

    void Start() {
        if (_spotterTool != null && _UIManager != null && _rifle != null) {
            _spotterTool = FindObjectOfType<SpotterTool>();
            _UIManager = FindObjectOfType<UIManager>();
            _rifle = FindObjectOfType<Rifle>();
        }
        
    }

    // Use this for initialization
    void OnEnable () {
		SteamVR_TrackedController controller = GetComponent<SteamVR_TrackedController>();
		controller.TriggerClicked += OnClickTrigger;
		controller.TriggerUnclicked += OnUnclickTrigger;
		controller.PadClicked += OnPadClicked;
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
			//_sceneManager.ScopeSwitch();
		}
	}


	void OnClickTrigger(object sender, ClickedEventArgs e) 
	{
		Debug.Log("Clicked trigger!");
		//_sceneManager._triggerIsDown = true;

		// pull Trigger on Right Controller
		if (_leftOrRight == "right")
		{
            _rifle.Fire();
		}
		else
		{
			_spotterTool.ActivateSpotter();
		}
	}

	void OnUnclickTrigger(object sender, ClickedEventArgs e) 
	{
		Debug.Log("Unclicked trigger!");
		//_sceneManager._triggerIsDown = false;
		//_sceneManager.ClearUI();
		_rifle.TriggerUp();

	}

	void OnPadTouched(object sender, ClickedEventArgs e)
	{
		Debug.Log("pad touched: " + e.padX + " " + e.padY);
		//_sceneManager._padIsTouched = true;
		//_sceneManager._padX = e.padX;
		//_sceneManager._padY = e.padY;

		if (_leftOrRight == "right")
		{
			//_sceneManager._rightPadY = e.padY;
			//Debug.Log("rPadY = " + _sceneManager._rightPadY);
		}
	}

}