using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	[SerializeField] private float _speed;
	[SerializeField] private Transform _hitCoverPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * _speed);
	}

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Cover")
		{
			Instantiate(_hitCoverPrefab, transform.position, transform.rotation);
			Destroy(gameObject);
		}
	}
}
