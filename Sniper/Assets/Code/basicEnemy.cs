using UnityEngine;
using System.Collections;

public class basicEnemy : MonoBehaviour {

	private Vector3 waypoint;
	private Rigidbody rb;
	private Quaternion directionToWaypoint;
	private Vector3 velocity = Vector3.zero;
	private float waypointTime;
	public float _speed;

	// Use this for initialization
	void Start () {
		rb = transform.GetComponent<Rigidbody>();
		//_audioScream = transform.FindChild("screamSource").GetComponent<AudioSource>();
		//_bloodParticles = transform.FindChild("bloodSource").GetComponent<ParticleSystem>();
		//_gameManagerScript = GameObject.Find("Game Manager").GetComponent<gameManager>();
		//_storedDataScript = GameObject.Find("storedData").GetComponent<storedData>();

		NewWaypoint();
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Time.time - waypointTime > 3) 
		{
			NewWaypoint();
		}

		if (transform.position.y < -10) 
		{
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
			rb.velocity = Vector3.zero;
		}
	}

	void FixedUpdate () 
	{
		//rb.AddRelativeForce(Vector3.forward * 1500.0f * _speed);
		transform.Translate(Vector3.forward * _speed);
		Vector3 relativePos = waypoint - transform.position;
		directionToWaypoint = Quaternion.LookRotation(relativePos);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, directionToWaypoint, 1.0f);
	}

	void NewWaypoint()
	{
		waypoint.x = transform.position.x + Random.Range(-20.0f, 20.0f);
		waypoint.y = transform.position.y;
		waypoint.z = transform.position.z + Random.Range(-20.0f, 20.0f);
		waypointTime = Time.time;

		/*
		Vector3 playerPos = GameObject.Find("[CameraRig]").transform.position;
		waypoint.x = playerPos.x + Random.Range(-_aimingErrorRange, _aimingErrorRange);
		waypoint.y = 1;
		waypoint.z = playerPos.z + Random.Range(-_aimingErrorRange, _aimingErrorRange);
		*/
	}
}
