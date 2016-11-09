using UnityEngine;
using System.Collections;

public class BulletUI : MonoBehaviour {


	[SerializeField] private SpriteRenderer _bulletIcon1;
	[SerializeField] private SpriteRenderer _bulletIcon2;
	[SerializeField] private SpriteRenderer _bulletIcon3;
	[SerializeField] private SpriteRenderer _bulletIcon4;


	void Start () 
	{
	}
	

	void Update () 
	{
	}


	public void DecreaseBulletIcons() 
	{
		if (_bulletIcon4.enabled) {
			_bulletIcon4.enabled = false;
		}
		else if (_bulletIcon3.enabled) {
			_bulletIcon3.enabled = false;
		}
		else if (_bulletIcon2.enabled) {
			_bulletIcon2.enabled = false;
		}
		else if (_bulletIcon1.enabled) {
			_bulletIcon1.enabled = false;
		}
	}


	public void RestoreBulletIcons() 
	{
		_bulletIcon1.enabled = true;
		_bulletIcon2.enabled = true;
		_bulletIcon3.enabled = true;
		_bulletIcon4.enabled = true;
	}
}
