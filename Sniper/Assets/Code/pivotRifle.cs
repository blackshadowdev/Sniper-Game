using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class pivotRifle : MonoBehaviour {


	public Transform _leftController;
	private bool _usingTwoHands;
	private Quaternion _startingRotation;

	// Use this for initialization
	void Start () 
	{
		_usingTwoHands = true;
		_startingRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_usingTwoHands)
			transform.LookAt(_leftController);
		else
			transform.rotation = _startingRotation;

		#if UNITY_EDITOR
		_leftController.Translate(Vector3.right * Input.GetAxis("Mouse X") * 0.1f);
		_leftController.Translate(Vector3.up * Input.GetAxis("Mouse Y") * 0.1f);
		#endif
	}

	public void OneOrTwoHands ()
	{
		if (_usingTwoHands)
			_usingTwoHands = false;
		else
			_usingTwoHands = true;
	}

}
