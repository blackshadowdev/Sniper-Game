using UnityEngine;
using System.Collections;

public class PlayerHealthTrigger : MonoBehaviour {

	public sceneManager _sceneManager;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Bullet")
		{
			_sceneManager.PlayerIsHit();
		}	

		
	}
}
