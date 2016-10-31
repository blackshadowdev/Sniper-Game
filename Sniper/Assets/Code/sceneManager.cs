using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // for text?


public class sceneManager : MonoBehaviour {


	public bool _laptopTesting;
	public bool _scopeOn;
	public Camera _camera;
	public Camera _scopeCamera;
	public Transform _bloodPfxPrefab;
	public Transform _leftController;
	public Transform _rightController;
	public Transform _head;
	public AudioSource _rifleAudioSource;
	public AudioClip _rifleShot;
	public AudioClip _rifleChamber;
	public AudioClip _magazineReload;
	public float _rightPadY;
	private float _lastRightPadY;
	public bool _padIsTouched;
	public bool _rifleCanFire;
	public EnemyManager _enemyManagerScript;

	public SpriteRenderer _bulletIcon1;
	public SpriteRenderer _bulletIcon2;
	public SpriteRenderer _bulletIcon3;
	public SpriteRenderer _bulletIcon4;

	public Transform _scopePivot;
	public Light _lightSource;
	public Renderer _fadeToBlack;

	public int _playerHealth;
	public SpriteRenderer _heartIcon1;
	public SpriteRenderer _heartIcon2;
	public SpriteRenderer _heartIcon3;
	public Renderer _bloodSplatter;

	public Text _timerText;
	private float _startTime;
	private float _elapsedTime;
	private int _bonusTime;
	public ParticleSystem _timerPfx;
	public Text _scoreText;
	private int _playerScore;
	public ParticleSystem _scorePFX;




	void Start () 
	{
		_startTime = Time.time;
		_rifleCanFire = true;
		_scoreText.text = "0";

		if (_laptopTesting)
		{
			_leftController.gameObject.SetActive(true);
			_rightController.gameObject.SetActive(true);
			_head.position = new Vector3(_head.position.x+1,_head.position.y+1.6f,_head.position.z);
			_leftController.position = new Vector3(_leftController.position.x+1,_leftController.position.y+1.5f,_leftController.position.z-1.5f);
			_rightController.position = new Vector3(_rightController.position.x+1,_rightController.position.y+1.5f,_rightController.position.z-0.1f);
		}
	}


	void Update () 
	{
		if (Input.GetKeyDown("space"))
		{
			ScopeSwitch();
		}

		if (Input.GetKeyDown("f"))
		{
			//Fire();
			Invoke("TriggerUp", 0.5f);
		}

		if (Input.GetKey("e"))
		{
			_head.transform.Translate(Vector3.right * 0.1f);
		}

		if (Input.GetKey("q"))
		{
			_head.transform.Translate(Vector3.right * -0.1f);
		}

		_elapsedTime = 100 - Time.time + _startTime + _bonusTime;
		_timerText.text = _elapsedTime.ToString();

		if (_bloodSplatter.material.color.a > 0)
		{
			float _tempAlpha = _bloodSplatter.material.color.a - 0.01f;
			_bloodSplatter.material.color = new Color(1,1,1,_tempAlpha);

		}

		/*
		 * 
		 * this all works fine;
		 * changing to The Nest style scope
		 * 
		// if right hand is near head, engage scope
		float _rHandToHeadDist = Vector3.Distance(_head.position, _rightController.position);
		if (_rHandToHeadDist < 0.4f && _scopeOn == false)
		{
			ScopeSwitch();												// changes the camera clearing flags
		} 
		else if (_rHandToHeadDist > 0.4f && _scopeOn == true)
		{
			ScopeSwitch();
		}

		AdjustZoom();

		// fade to black effect
		if (_rHandToHeadDist > 0.4f && _rHandToHeadDist < 0.45f)
		{
			//_lightSource.intensity = (_rHandToHeadDist - 0.4f) * 20;
			float _alpha = (0.45f - _rHandToHeadDist) * 20;
			_fadeToBlack.material.color = new Color(0,0,0,_alpha);
		}
		if (_rHandToHeadDist < 0.4f)
		{
			//_lightSource.intensity = 1;
			_fadeToBlack.material.color = new Color(0,0,0,0);
		}
		if (_rHandToHeadDist > 0.45f)
		{
			//_lightSource.intensity = 1;
			_fadeToBlack.material.color = new Color(0,0,0,0);
		}
		*/

		/* zoom with 2 hands
		// adjust zoom
		float _handDist = Vector3.Distance(_leftController.position, _rightController.position);
		float _newFov = 65 - _handDist*100;
		if (_newFov < 1)
		{
			_newFov = 1;
		} 
		else if (_newFov > 50)
		{
			_newFov = 50;
		}
		_scopeCamera.fieldOfView = _newFov;
		*/

	} // end of Update()
		

	private void AdjustZoom ()
	{
		float _tempRightPadY = _rightPadY;
		if (_tempRightPadY > 0.5f)
		{
			_tempRightPadY = 0.5f;
		} else if (_tempRightPadY < -0.5f)
		{
			_tempRightPadY = -0.5f;
		}
		_tempRightPadY += 0.5f;
		float _newFov = _tempRightPadY*50;
		_scopeCamera.fieldOfView = _newFov;

		_scopePivot.LookAt(_head);
	}

	public void ScopeSwitch ()
	{
		// if Scope View, switch to Normal View
		if (_scopeOn)
		{
			_camera.cullingMask = (1 << 0);
			_camera.clearFlags = CameraClearFlags.Skybox;

			_scopeOn = false;
		}
		// if Normal View, switch to Scope View
		else if (!_scopeOn)
		{
			_camera.cullingMask = (1 << LayerMask.NameToLayer("Scope Image"));
			_camera.clearFlags = CameraClearFlags.SolidColor;
			_scopeOn = true;
		}
	} // end of ScopeSwitch()

	public void Fire ()
	{
		if (_rifleCanFire)
		{
			_rifleAudioSource.PlayOneShot(_rifleShot);
			Vector3 _fwd = _scopeCamera.transform.TransformDirection(Vector3.forward);
			RaycastHit _hit;
			Debug.DrawRay(_scopeCamera.transform.position, _fwd * 100, Color.green, 1);
			if (Physics.Raycast(_scopeCamera.transform.position, _fwd, out _hit)) 
			{
				if (_hit.transform.tag == "Enemy")
				{
					
					for (int _killedEnemy = 0; _killedEnemy < _enemyManagerScript._enemies.Count; _killedEnemy ++)
					{
						if (_hit.transform.gameObject == _enemyManagerScript._enemies[_killedEnemy])
						{
							_enemyManagerScript._enemies.RemoveAt(_killedEnemy);
							Debug.Log("removing killed " + _killedEnemy);
						}
					}

					Instantiate(_bloodPfxPrefab, _hit.point, Quaternion.identity);
					//Destroy(_hit.transform.gameObject);
					//RPGCharacterControllerFREE _enemyController = _hit.transform.GetComponent<RPGCharacterControllerFREE>();
					AIControl _enemyController = _hit.transform.GetComponent<AIControl>();
					_enemyController.Die();
					_bonusTime += 15;
					_timerPfx.Emit(50);
					_playerScore += 250;
					_scoreText.text = _playerScore.ToString();
					_scorePFX.Emit(50);
				} // end of IF HIT ENEMY

				if (_hit.transform.tag == "Civilian")
				{
					CivilianAI _civilianController = _hit.transform.GetComponent<CivilianAI>();
					_civilianController.Die();
					_playerScore -= 500;
					_scoreText.text = _playerScore.ToString();
					_scorePFX.Emit(50);
				}
			} else {
				Debug.Log("Miss!");
			}

			SetBulletIcons();


			_rifleCanFire = false;
		}
	} // end of Fire()

	private void SetBulletIcons ()
	{
		if (_bulletIcon4.enabled)
		{
			_bulletIcon4.enabled = false;
		}
		else if (_bulletIcon3.enabled)
		{
			_bulletIcon3.enabled = false;
		} 
		else if (_bulletIcon2.enabled)
		{
			_bulletIcon2.enabled = false;
		} 
		else if (_bulletIcon1.enabled)
		{
			_bulletIcon1.enabled = false;
		}
	}

	public void TriggerUp ()
	{
		if (_bulletIcon1.enabled == false)
		{
			_rifleAudioSource.PlayOneShot(_magazineReload);
			Invoke("MagazineReloaded", 3.7f);
		} 
		else
		{
			_rifleAudioSource.PlayOneShot(_rifleChamber);
			Invoke("RoundChambered", 1);
		}
	}

	public void RoundChambered ()
	{
		_rifleCanFire = true;
	}

	private void MagazineReloaded ()
	{
		_bulletIcon1.enabled = true;
		_bulletIcon2.enabled = true;
		_bulletIcon3.enabled = true;
		_bulletIcon4.enabled = true;
		_rifleCanFire = true;
	}

	public void PlayerIsHit ()
	{
		_playerHealth--;
		if (_heartIcon3.enabled)
		{
			_heartIcon3.enabled = false;
			_bloodSplatter.material.color = new Color(1,1,1,0.5f);
		}
		else if (_heartIcon2.enabled)
		{
			_heartIcon2.enabled = false;
			_bloodSplatter.material.color = new Color(1,1,1,1);
		} 
		else if (_heartIcon1.enabled)
		{
			_heartIcon1.enabled = false;
			//_bloodSplatter.material.color = new Color(1,1,1,1);
			if (Application.loadedLevelName == "rifle and npcs")
				SceneManager.LoadScene("rifle and npcs");
			if (Application.loadedLevelName == "city 1")
				SceneManager.LoadScene("city 1");
		}
	}
}
