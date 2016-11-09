using UnityEngine;
using System.Collections;

public class Scope : MonoBehaviour {

	private bool _scopeOn;
	[SerializeField] private Camera _camera;
	[SerializeField] private float _rightPadY;					// this is set by ViveControlsExample, but has been disabled for now in that script.
	[SerializeField] private Camera _scopeCamera;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//_scopePivot.LookAt(_head);
	}

	public void ScopeSwitch() {
		// if Scope View, switch to Normal View
		if (_scopeOn) {
			_camera.cullingMask = (1 << 0);
			_camera.clearFlags = CameraClearFlags.Skybox;
			_scopeOn = false;
		}
		// if Normal View, switch to Scope View
		else if (!_scopeOn) {
			_camera.cullingMask = (1 << LayerMask.NameToLayer("Scope Image"));
			_camera.clearFlags = CameraClearFlags.SolidColor;
			_scopeOn = true;
		}
	}

	private void AdjustZoom() {
		float _tempRightPadY = _rightPadY;
		if (_tempRightPadY > 0.5f) {
			_tempRightPadY = 0.5f;
		}
		else if (_tempRightPadY < -0.5f) {
			_tempRightPadY = -0.5f;
		}
		_tempRightPadY += 0.5f;
		float _newFov = _tempRightPadY * 50;
		_scopeCamera.fieldOfView = _newFov;
	}
}
