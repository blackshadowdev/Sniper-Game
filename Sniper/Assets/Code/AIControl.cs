using System.Collections;													// removed to prevent issue with two Randoms
using UnityEngine;
//using UnityStandardAssets.Characters.ThirdPerson;

//[RequireComponent(typeof (NavMeshAgent))]
//[RequireComponent(typeof (ThirdPersonCharacter))]



// Notes
// 1) don't put the strafing enemy's waypoints too far apart, or he get's called by EnemyManager before finishing his action.  Alternatively, you could make the enemy manager not call an activated enemy.



public class AIControl : MonoBehaviour
{
	public NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding


	public Transform _bulletPrefab;
	public Transform _bulletSpawnPos;
	public ParticleSystem _muzzleFlashPfx;
	public AudioSource _gunAudioSource;
	public AudioClip _gunAudioclip;
	public AudioClip _tellAudioClip;
	public Transform _player;

	private bool _shouldCrouch = true;
	public bool _isAlive = true;

	private Animator _animator;

	public string _myType;


	public Transform _waypiont1;					// start here; shoot here.  (this will reverse when action is triggered by player's first shot)
	public Transform _waypoint2;					// cover here
	private Transform _currentWaypoint;

	private bool _isStrafing;
	public bool _imBusy;

	private void Start()
	{
		// get the components on the object we need ( should not be null due to require component so no need to check )
		//agent = GetComponentInChildren<NavMeshAgent>();
		//character = GetComponent<ThirdPersonCharacter>();

		//agent.updateRotation = false;
		//agent.updatePosition = true;


		//_characterController.Crouch();
		//_animator.SetBool("StrafeLeft", true);

		_animator = transform.GetComponent<Animator>();

		if (_myType == "StandFireCrouch")
			_animator.SetTrigger("CrouchTrigger");
		else if (_myType == "StrafeFireStrafe")
		{
			_currentWaypoint = _waypoint2;
		}
	}


	private void Update()
	{
		if (_myType == "StrafeFireStrafe" && _isStrafing)
		{

			// if near target waypoint, stand idle and switch waypoints
			if (Vector3.Distance(transform.position, _currentWaypoint.transform.position) < 1)
			{
				_isStrafing = false;
				_animator.SetTrigger("StandIdleTrigger");

				// if arriving at Waypoint 1, do thiss...
				if (_currentWaypoint == _waypiont1)
				{
					_currentWaypoint = _waypoint2;
					_imBusy = false;
				}
				// if arriving at Waypoint 2, do this...
				else
				{
					_currentWaypoint = _waypiont1;
					StartCoroutine(PauseTellPauseFire());
				}
			}
		}
	} // end of Update()

	public void TakeAction ()
	{
		if (_myType == "StandFireCrouch")
			StartCoroutine(StandFireCrouch());
		else if (_myType == "StrafeFireStrafe")
			StrafeToWaypoint();
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
		_gunAudioSource.PlayOneShot(_tellAudioClip);											// "TELL!"
		float _timeUntilFire = Random.Range(2.0f, 3.0f);										// [rand]
		yield return new WaitForSeconds(_timeUntilFire);										// wait
		Fire();																					// fire

		if (_myType == "StrafeFireStrafe")
		{
			yield return new WaitForSeconds(1);
			StrafeToWaypoint();
		}
	}

	private IEnumerator StandFireCrouch ()
	{
		_animator.SetTrigger("StandIdleTrigger");												// stand up
		StartCoroutine(PauseTellPauseFire());														// pause, "Tell!", pause
		float _timeUntilCrouch = Random.Range(2.0f, 3.0f);										// [rand]
		yield return new WaitForSeconds(_timeUntilCrouch);										// wait
		_animator.SetTrigger("CrouchTrigger");													// crouch

	}

	public void Fire ()
	{
		_bulletSpawnPos.LookAt(_player);														// aim
		Instantiate(_bulletPrefab, _bulletSpawnPos.position, _bulletSpawnPos.rotation);			// spawn bullet
		_muzzleFlashPfx.Emit(10);																// muzzle flash PFX
		_gunAudioSource.PlayOneShot(_gunAudioclip);												// gunshot audio
	}

	public void Die ()
	{
		StopAllCoroutines();
		_animator.SetTrigger("DeathTrigger");
	}


	public Vector3 getRelativePosition(Transform origin, Vector3 position) {				// used to be public STATIC.... ?
		Vector3 distance = position - origin.position;
		Vector3 relativePosition = Vector3.zero;
		relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
		relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
		relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

		return relativePosition;
	}
}
