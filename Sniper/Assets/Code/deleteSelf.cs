using UnityEngine;
using System.Collections;

public class deleteSelf : MonoBehaviour {

	public float _timeToDelete;

	// Use this for initialization
	void Start () {
		Destroy(gameObject, _timeToDelete);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
