using System.Collections;													
using UnityEngine;

public class AIControl : MonoBehaviour
{
	
	[SerializeField] private AudioClip _tellAudioClip;
	[SerializeField] private Transform _player;
	[SerializeField] private bool _typeCrouch;
	[SerializeField] private bool _typeStrafe;
	[SerializeField] private bool _typeBoss;

	[SerializeField] private Transform _waypoint1;					
	[SerializeField] private Transform _waypoint2;
	[SerializeField] private Transform _waypiont3;					
	[SerializeField] private Transform _waypoint4;
	private GameObject _currentWaypoint;

	private bool _shouldCrouch = true;
	private bool _isAlive = true;
	private bool _isStrafing;
	private bool _imBusy;

	private bool _bossIsEntering;
	private bool _bossMovingToWaypoint;
	private int _bossHealth = 3;

	private BulletSpawner _bulletSpawner;
	private AudioSource _audioSource;
	private Animator _animator;

	private void Start()
	{
		_animator = transform.GetComponent<Animator>();
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
			_animator.SetTrigger("CrouchTrigger");												// these 2 triggers cause an error, but still function.  Object reference not set to an instance of an object.
	}


	private void Update()
	{
		if (_typeStrafe && _isStrafing)
		{

			// if near target waypoint, stand idle and switch waypoints
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
		} else if (_typeBoss)
		{
			BossUpdate();
		}
	}

	private void BossUpdate ()
	{
		if (!_bossIsEntering)
		{
			transform.LookAt(_currentWaypoint.transform.position);
			if (Vector3.Distance(transform.position, _currentWaypoint.transform.position) < 1)
			{
				int _randBossAction = Random.Range(0,2);
				if (_randBossAction == 0)
				{
					StartCoroutine(BossFireSequence());
				} else 
				{
					BossFindNewWaypoint();
				}
			}	
		}

		else if (_bossIsEntering)
		{
			if (Vector3.Distance(transform.position, _waypoint1.transform.position) < 1)
			{
				_bossIsEntering = false;
				_animator.SetTrigger("SlowWalkTrigger");
				BossFindNewWaypoint();
			}
		}
	}

	private IEnumerator BossFireSequence ()
	{
		_animator.SetTrigger("StandIdleTrigger");
		yield return new WaitForSeconds(0.5f);
		transform.LookAt(_player);
		Fire();
		yield return new WaitForSeconds(0.5f);
		BossFindNewWaypoint();


		if (_bossHealth == 3)
		{
			_animator.SetTrigger("SlowWalkTrigger");
		}
		else if (_bossHealth == 2)
		{
			_animator.SetTrigger("WalkTrigger");
		} else if (_bossHealth == 1)
		{
			_animator.SetTrigger("RunTrigger");
		}
	}

	private void BossFindNewWaypoint ()
	{
		float _tempX = _waypoint1.position.x + Random.Range(-15,15);
		float _tempZ = _waypoint1.position.z + Random.Range(-15,15);
		_currentWaypoint.transform.position = new Vector3(_tempX, _waypoint1.position.y, _tempZ);
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
			}
			BossFindNewWaypoint();
		}

	}


	public Vector3 getRelativePosition(Transform origin, Vector3 position) {				
		Vector3 distance = position - origin.position;
		Vector3 relativePosition = Vector3.zero;
		relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
		relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
		relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

		return relativePosition;
	}
}
