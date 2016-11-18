using UnityEngine;
using System.Collections;

public class ConstrainRigidbody : MonoBehaviour {

	private float _startPosX;
	private float _startPosY;
	private float _startPosZ;
	private float _startRotX;
	private float _startRotY;
	private float _startRotZ;

	private Vector3 _startPos;
	private Quaternion _startRot;

	// Use this for initialization
	void Start () {
		_startPos = transform.localPosition;
		_startRot = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate ()
	{
		transform.localPosition = _startPos;
		transform.localRotation = _startRot;
	}
}
