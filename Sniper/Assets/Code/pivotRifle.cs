using UnityEngine;
using System.Collections;

public class pivotRifle : MonoBehaviour {

	public Transform _leftController;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt(_leftController);


		// simulate Left Controller movement on PC
		_leftController.Translate(Vector3.right * Input.GetAxis("Mouse X") * 0.1f);
		_leftController.Translate(Vector3.up * Input.GetAxis("Mouse Y") * 0.1f);
		// delete when testing is complete


	}
}
