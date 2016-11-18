using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAI : BaseAI
{
    private bool _bossIsEntering;
    private bool _bossMovingToWaypoint;
    private float _bossNewWaypointTime;
	[SerializeField] Transform _raycastOrigin;
    [SerializeField]
    private VRCameraFade _cameraFade;                 // This fades the scene out when a new scene is about to be loaded.
    private Transform _startingWaypoint;



	private IEnumerator AvoidCollision(bool _rollLeft)
	{
		ImBusy = true;
		Animator.SetTrigger("StandIdleTrigger");
		yield return new WaitForSeconds(0.5f);
		if (_rollLeft)
			Animator.SetTrigger("RollLeftTrigger");
		else
			Animator.SetTrigger("RollRightTrigger");
		yield return new WaitForSeconds(1.5f);
		StartCoroutine(BossFireSequence());
	}


	protected override void OnEnable()
    {
        transform.LookAt(WaypointNavigator.CurrentWaypoint);
        Animator.SetTrigger("RunTrigger");
        _bossIsEntering = true;
		_startingWaypoint = WaypointNavigator.CurrentWaypoint;			// trying to add fix waypoint bug
    }

    protected override void OnUpdate()
    {
        if (!_bossIsEntering)
        {
			Raycast();
            if (WaypointNavigator.InStoppingDistance)
            {
                if (!ImBusy)
                {
                    ImBusy = true;
                    StartCoroutine(BossFireSequence());
                }
            }
        }
        else if (_bossIsEntering)
        {
            if (WaypointNavigator.InStoppingDistance)
            {
                _bossIsEntering = false;
                StartCoroutine(BossFireSequence());
            }
        }
    }

    protected override void OnDamage(DamageInfo info)
    {
    }

    protected override void OnTakeAction()
    {
    }

    protected override void OnDie()
    {
        ImBusy = false;
        Animator.SetTrigger("GetHitTrigger");

        if (Health.CurrentHealth > 0)
        {
            StartCoroutine(BossWaitThenResumeMoving());
        }
        else
        {
            Animator.SetTrigger("DeathTrigger");
            StartCoroutine(_cameraFade.BeginFadeOut(true));
        }
    }

    
	private void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.tag != "Floor") && (collision.gameObject.tag != "Bullet"))
        {
            ImBusy = true;
			Animator.SetTrigger("RollBackTrigger");
            StartCoroutine(BossWaitThenResumeMoving());
        }
    }
    

    private IEnumerator BossWaitThenResumeMoving()
    {
        yield return new WaitForSeconds(1);
        BossFindNewWaypoint();
        BossResumeMoving();
    }
    

    private IEnumerator BossFireSequence()
    {
        ImBusy = true;
        Animator.SetTrigger("StandIdleTrigger");
        yield return new WaitForSeconds(0.5f);
        GetComponent<BulletSpawner>().Fire();
        yield return new WaitForSeconds(0.5f);
        BossFindNewWaypoint();
        BossResumeMoving();
    }

    private void BossFindNewWaypoint()
    {
		//var waypoint = WaypointNavigator.CurrentWaypoint;					// i want every new waypoint to be based off of the original waypoint, not the last waypoint
		var waypoint = _startingWaypoint;									// but this doesn't work, either.


		var tempX = _startingWaypoint.position.x + Random.Range(-5f, 5f);
		var tempZ = _startingWaypoint.position.z + Random.Range(-5f, 5f);
		waypoint.position = new Vector3(tempX, waypoint.position.y, tempZ);
		Debug.Log("boss new waypoint = " + waypoint.position);
		transform.LookAt(waypoint);
    }

    private void BossResumeMoving()
    {
        if (Health.CurrentHealth == 3)
            Animator.SetTrigger("SlowWalkTrigger");
        else if (Health.CurrentHealth == 2)
            Animator.SetTrigger("WalkTrigger");
        else if (Health.CurrentHealth == 1)
            Animator.SetTrigger("RunTrigger");
		ImBusy = false;
    }

	private void Raycast()
	{
		Vector3 _fwd = _raycastOrigin.TransformDirection(Vector3.forward);
		RaycastHit _hit;
		Debug.DrawRay(_raycastOrigin.position, _fwd * 10, Color.green, 1);
		if (Physics.Raycast(_raycastOrigin.position, _fwd, out _hit)) 
		{
			if (Vector3.Distance(transform.position, _hit.point) < 1 && !ImBusy)
			{
				float _leftDistance = 100;												// had to assign these a value, or i'd get errors... not sure why
				float _rightDistance = 100;
				Vector3 _left = _raycastOrigin.TransformDirection(Vector3.left);
				Debug.DrawRay(_raycastOrigin.position, _left * 10, Color.blue, 1);
				if (Physics.Raycast(_raycastOrigin.position, _left, out _hit)) 
					_leftDistance = Vector3.Distance(transform.position, _hit.point);
				Vector3 _right = _raycastOrigin.TransformDirection(Vector3.right);
				Debug.DrawRay(_raycastOrigin.position, _right * 10, Color.red, 1);
				if (Physics.Raycast(_raycastOrigin.position, _right, out _hit)) 
					_rightDistance = Vector3.Distance(transform.position, _hit.point);
				bool _rollLeft = false;
				if (_leftDistance > _rightDistance)
					_rollLeft = true;
				StartCoroutine(AvoidCollision(_rollLeft));
			}
		}
	}


    private IEnumerator TempEndLevel()
    {
        //Wait for camera to fade code
        yield return StartCoroutine(_cameraFade.BeginFadeOut(true));

        SceneManager.LoadScene("YouWin");
    }
}