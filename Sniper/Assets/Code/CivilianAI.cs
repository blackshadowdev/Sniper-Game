using UnityEngine;
using System.Collections;

public class CivilianAI : MonoBehaviour {


	private Rigidbody _rb;
	private Animator _animator;
	public Transform _waypoint1;
	public Transform _waypoint2;
	public bool _shouldRun = true;
	private GameObject _currentWaypoint;
	private float _lastFindWaypointTime;


	// Use this for initialization
	void Start () {
		_currentWaypoint = new GameObject();
		_animator = GetComponentInChildren<Animator>();
		_rb = GetComponent<Rigidbody>();

		_currentWaypoint.transform.position = new Vector3(0,0,0);							// necessary to initialize bot if running between 2 waypoints

		FindNewWaypoint();
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(_currentWaypoint.transform);
		if (Vector3.Distance(transform.position, _currentWaypoint.transform.position) < 1 || Time.time - _lastFindWaypointTime > 10)
		{
			FindNewWaypoint();
		}
	
		//ControlMotion();
	}

	public void ControlMotion ()
	{
		/*
		transform.LookAt(_currentWaypoint.transform);
		if (Vector3.Distance(transform.position, _currentWaypoint.transform.position) < 1)
		{
			if (_currentWaypoint == _waypoint1)
				_currentWaypoint = _waypoint2;
			else
				_currentWaypoint = _waypoint1;
		}
		*/
	}

	public void Die ()
	{
		_animator.SetTrigger("DeathTrigger");

	}

	void OnCollisionEnter (Collision collision)
	{
		FindNewWaypoint();
	}
		
	private void FindNewWaypoint ()
	{
		_currentWaypoint.transform.position = new Vector3 (Random.Range(-25,25),0,Random.Range(0,50));
		_lastFindWaypointTime = Time.time;
	}
}
