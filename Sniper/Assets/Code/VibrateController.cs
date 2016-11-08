using UnityEngine;
using System.Collections;

public class VibrateController : MonoBehaviour {

	private int _deviceIndex;
	[SerializeField] private bool _leftController;

	[SerializeField] private int _leftControllerIndex;
	[SerializeField] private int _rightControllerIndex;

	// Use this for initialization
	void Start () {
		if (_leftController)
			_deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
		else
			_deviceIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);

		_leftControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
		_rightControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);

	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (_deviceIndex != -1 && SteamVR_Controller.Input (_deviceIndex).GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) 
		{
			StartCoroutine(HapticVibration(0.2f, 3000, 0.01f));
		}
		*/
	}

	public void VibrateForFiring ()
	{
		StartCoroutine(HapticVibration(0.05f, 3500, 0.01f));
	}

	public void VibrateForDamage ()
	{
		StartCoroutine(HapticVibration(0.5f, 1000, 0.03f));
	}
		

	private IEnumerator HapticVibration (float _duration, ushort _hapticPulseStrength, float _pulseInterval)
	{
		if (_pulseInterval <= 0) 
		{
			yield break;
		}

		while (_duration > 0) 
		{
			//SteamVR_Controller.Input(_deviceIndex).TriggerHapticPulse(_hapticPulseStrength);
			SteamVR_Controller.Input(_leftControllerIndex).TriggerHapticPulse(_hapticPulseStrength);
			SteamVR_Controller.Input(_rightControllerIndex).TriggerHapticPulse(_hapticPulseStrength);
			yield return new WaitForSeconds (_pulseInterval);
			_duration -= _pulseInterval;
		}
	}
}
