﻿using UnityEngine;
using System.Collections;

public class PlayerHealthTrigger : MonoBehaviour {

    public Rifle _rifle;
	[SerializeField] private PlayerHealth _playerHealth;


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
			//_rifle.PlayerIsHit();
			_playerHealth.PlayerIsHit();
		}	

		
	}
}
