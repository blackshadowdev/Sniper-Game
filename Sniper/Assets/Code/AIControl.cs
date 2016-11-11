using System.Collections;													
using UnityEngine;
using UnityEngine.SceneManagement;


public class AIControl : MonoBehaviour
{
	
	[SerializeField] private AudioClip _tellAudioClip;
	[SerializeField] private Transform _player;
	[SerializeField] private bool _typeCrouch;
	[SerializeField] private bool _typeStrafe;
	[SerializeField] private bool _typeBoss;
	[SerializeField] private bool _typeVip;
	[SerializeField] private bool _typeHunter;


	[SerializeField] private Transform _waypoint1;					
	[SerializeField] private Transform _waypoint2;
	[SerializeField] private Transform _waypiont3;					
	[SerializeField] private Transform _waypoint4;
	private GameObject _currentWaypoint;

	private bool _shouldCrouch = true;
	private bool _isAlive = true;
	private bool _isStrafing;
	private bool _imBusy;
	private bool _vipIsWalking;


	private bool _bossIsEntering;
	private bool _bossMovingToWaypoint;
	private int _bossHealth = 3;
	private float _bossNewWaypointTime;

	private BulletSpawner _bulletSpawner;
	private AudioSource _audioSource;
	private Animator _animator;

	//for the Hunter
	private bool _hunterIsWalking;
	private bool _hunterShouldAttack;
	private float _lastAttackTime;
	[SerializeField] private Transform _myVipTarget;


	// for the VIP
	private int _timesHit;
	[SerializeField] private Transform _myHunter;




	private void Start()
	{
		_animator = transform.GetComponent<Animator>();
		if (_typeCrouch || _typeStrafe || _typeHunter || _typeBoss)
			_bulletSpawner = transform.GetComponent<BulletSpawner>();
		_audioSource = transform.GetComponent<AudioSource>();
		_currentWaypoint = new GameObject();

		if (_typeStrafe)
		{
			_currentWaypoint.transform.position = _waypoint2.transform.position;
		} 
			
		gameObject.SetActive(false);															// enemies are activated by the spawn manager
	}

	private void OnEnable ()
	{
		if (_typeBoss)
		{
			transform.LookAt(_waypoint1);
			_animator.SetTrigger("RunTrigger");
			_bossIsEntering = true;
		}

		if (_typeCrouch)
			_animator.SetTrigger("CrouchTrigger");												// these animation triggers cause an error, but still function.  Error = Object reference not set to an instance of an object.
	
		if (_typeVip)
		{
			transform.LookAt(_waypoint1);
			_animator.SetTrigger("WalkTrigger");
			_vipIsWalking = true;
		}

		if (_typeHunter)
		{
			transform.LookAt(_myVipTarget);
			_animator.SetTrigger("WalkTrigger");
			_hunterIsWalking = true;
		}
	}


	private void Update()
	{
		if (_typeStrafe)
			StrafeUpdate();
		else if (_typeBoss)
			BossUpdate();
		else if (_typeVip)
			VipUpdate();
		else if (_typeHunter)
			HunterUpdate();
	}

	private void StrafeUpdate ()
	{
		if (_isStrafing)
		{
			if (Vector3.Distance(transform.position, _currentWaypoint.transform.position) < 1)
			{
				_isStrafing = false;
				_animator.ResetTrigger("StrafeLeft");
				_animator.ResetTrigger("StrafeRight");
				_animator.SetTrigger("StandIdleTrigger");

				if (_currentWaypoint.transform.position == _waypoint1.transform.position)
				{
					_currentWaypoint.transform.position = _waypoint2.transform.position;
					_imBusy = false;
				}
				else
				{
					_currentWaypoint.transform.position = _waypoint1.transform.position;
					StartCoroutine(PauseTellPauseFire());
				}
			}
		} 
	}
		

	private void HunterUpdate ()
	{
		if (_hunterIsWalking)
		{
			transform.LookAt(_myVipTarget);
			if (Vector3.Distance(transform.position, _myVipTarget.position) < 2)
			{
					_hunterIsWalking = false;
					_hunterShouldAttack = true;
			}
		}



		if (_hunterShouldAttack)
		{
			if (Time.time - _lastAttackTime > 3)
			{
				transform.LookAt(_myVipTarget);
				_animator.SetTrigger("HunterAttackTrigger");
				_myVipTarget.transform.GetComponent<AIControl>().StartVipHitCoroutine();
				_lastAttackTime = Time.time;
			}
		}
	}

	public void StartVipHitCoroutine ()
	{
		StartCoroutine(VipGetsHit());
	}

	public IEnumerator VipGetsHit ()
	{
		yield return new WaitForSeconds(0.3f);
		_timesHit ++;
		if (_timesHit < 3)
			_animator.SetTrigger("GetHitTrigger");
		else
		{
			_animator.SetTrigger("DeathTrigger");
			_myHunter.transform.GetComponent<AIControl>()._hunterShouldAttack = false;
		}
	}

	private void VipUpdate ()
	{
		if (_vipIsWalking)
		{
			transform.LookAt(_waypoint1);
			if (Vector3.Distance(transform.position, _waypoint1.position) < 1)
			{
				_animator.SetTrigger("StandIdleTrigger");
				_vipIsWalking = false;
			}
		}


	}

	private void BossUpdate ()
	{
		if (!_bossIsEntering)
		{
			transform.LookAt(_currentWaypoint.transform.position);
			if (Vector3.Distance(transform.position, _currentWaypoint.transform.position) < 1)
			{
				if (!_imBusy)
				{
					_imBusy = true;
					Debug.Log("boss near waypoint; stop and fire");
					StopAllCoroutines();
					StartCoroutine(BossFireSequence());
				}
			}

			if (Time.time - _bossNewWaypointTime > 12)
			{
				if (!_imBusy)
				{
					Debug.Log("boss time out must fire");
					StartCoroutine(BossFireSequence());
					_imBusy = true;
				}
			}
		}




		else if (_bossIsEntering)
		{
			if (Vector3.Distance(transform.position, _waypoint1.transform.position) < 1)
			{
				_bossIsEntering = false;
				StartCoroutine(BossFireSequence());

			}
		}
	} // end of BossUpdate()

	private IEnumerator BossFireSequence ()
	{
		Debug.Log("starting boss fire sequence");
		_imBusy = true;
		_animator.SetTrigger("StandIdleTrigger");
		yield return new WaitForSeconds(0.5f);
		Fire();
		yield return new WaitForSeconds(0.5f);
		BossFindNewWaypoint();
		BossResumeMoving();

		

		_imBusy = false;
	}

	private void BossFindNewWaypoint ()
	{
		Debug.Log("boss found a new waypoint");
		float _tempX = _waypoint1.position.x + Random.Range(-10,10);
		float _tempZ = _waypoint1.position.z + Random.Range(-10,10);
		_currentWaypoint.transform.position = new Vector3(_tempX, _waypoint1.position.y, _tempZ);
		_bossNewWaypointTime = Time.time;
	}

	private void BossResumeMoving ()
	{
		Debug.Log("BossResumeMoving() called");
		Debug.Log("boss health = " + _bossHealth);
		//StopAllCoroutines();


		if (_bossHealth == 3)
			_animator.SetTrigger("SlowWalkTrigger");
		else if (_bossHealth == 2)
			_animator.SetTrigger("WalkTrigger");
		else if (_bossHealth == 1)
			_animator.SetTrigger("RunTrigger");
		
	}

	public void TakeAction ()
	{
		if (!_imBusy)
		{
			if (_typeCrouch)
			{
				StartCoroutine(StandFireCrouch());
				_imBusy = true;
			}
				
			else if (_typeStrafe)
				StrafeToWaypoint();
		}
	}

	public void StrafeToWaypoint ()
	{
		if (_currentWaypoint)
		{
			Vector3 _relPos = getRelativePosition(transform, _currentWaypoint.transform.position);
			if (_relPos.x < 0)
				_animator.SetTrigger("StrafeLeft");
			else
				_animator.SetTrigger("StrafeRight");

			_isStrafing = true;																		// need to set this true so that Update stops checking distance

			if (_currentWaypoint == _waypoint2)														// should this be true for either waypoint???
				_imBusy = true;
		}
	}
		

	private IEnumerator PauseTellPauseFire ()
	{
		yield return new WaitForSeconds(0.2f);													// wait
		_audioSource.PlayOneShot(_tellAudioClip);												// "TELL!"
		float _timeUntilFire = Random.Range(2.0f, 3.0f);										// [rand]
		yield return new WaitForSeconds(_timeUntilFire);										// wait
		Fire();																					// fire

		if (_typeStrafe)
		{
			yield return new WaitForSeconds(1);
			StrafeToWaypoint();
		}
	}

	private IEnumerator StandFireCrouch ()
	{
		_animator.SetTrigger("StandIdleTrigger");												// stand up
		StartCoroutine(PauseTellPauseFire());													// pause, "Tell!", pause
		float _timeUntilCrouch = Random.Range(2.0f, 3.0f);										// [rand]
		yield return new WaitForSeconds(_timeUntilCrouch);										// wait
		_animator.SetTrigger("CrouchTrigger");													// crouch
		_imBusy = false;

	}

	public void Fire ()
	{
		_bulletSpawner.Fire();
	}

	public void Die ()
	{
		if (!_typeBoss)
		{
			StopAllCoroutines();
			_animator.SetTrigger("DeathTrigger");	
		} else if (_typeBoss)
		{
			_bossHealth--;
			if (_bossHealth == 2)
			{
				_animator.SetTrigger("WalkTrigger");
			} else if (_bossHealth == 1)
			{
				_animator.SetTrigger("RunTrigger");
			} else if (_bossHealth == 0)
			{
				_animator.SetTrigger("DeathTrigger");
				Invoke ("TempEndLevel", 2);
				Debug.Log ("boss ıs dead");
			}
			BossFindNewWaypoint();
		}
	}


	private void TempEndLevel()
	{
		Debug.Log ("temp end level called");
		SceneManager.LoadScene("YouWin");
	}

	public Vector3 getRelativePosition(Transform origin, Vector3 position) {				
		Vector3 distance = position - origin.position;
		Vector3 relativePosition = Vector3.zero;
		relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
		relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
		relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

		return relativePosition;
	}


	void OnCollisionEnter (Collision collision)
	{
		
		if (collision.gameObject.tag != "Floor" && collision.gameObject.tag != "Bullet")
		{
			if (_typeBoss)
			{
				_animator.SetTrigger("RollBackTrigger");
				//BossFindNewWaypoint(); //we need a pause here so that roll animation can play, then make boss walk/run to next waypoint
				StartCoroutine(BossWaitThenResumeMoving());
			}
				
		}

	}

	private IEnumerator BossWaitThenResumeMoving ()
	{
		yield return new WaitForSeconds(1);
		BossFindNewWaypoint();
		BossResumeMoving();
	}



}
