using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class pivotRifle : MonoBehaviour {

	#if UNITY_EDITOR
	public Transform _leftController;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt(_leftController);
		_leftController.Translate(Vector3.right * Input.GetAxis("Mouse X") * 0.1f);
		_leftController.Translate(Vector3.up * Input.GetAxis("Mouse Y") * 0.1f);
	}
	#endif
}
