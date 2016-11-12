using System;
using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    [SerializeField] private float _stoppingDistance = 1f;
    [SerializeField] private Transform[] _waypoints = new Transform[0];
    private Transform _currentWaypoint;

    public int CurrentIndex { get; private set; }

    public Transform CurrentWaypoint
    {
        get
        {
            if (_currentWaypoint != null)
            {
                return _currentWaypoint;
            }

            _currentWaypoint = new GameObject().transform;
            Next();

            return _currentWaypoint;
        }
    }

    public float RemainingDistance
    {
        get { return Vector3.Distance(transform.position, _currentWaypoint.position); }
    }

    public bool InStoppingDistance
    {
        get { return RemainingDistance < _stoppingDistance; }
    }

    public void Next()
    {
        ++CurrentIndex;

        if (CurrentIndex >= _waypoints.Length)
        {
        CurrentIndex = 0;
        }

        _currentWaypoint.position = _waypoints.Length > 0 ? _waypoints[CurrentIndex].position : transform.position;
    }
}