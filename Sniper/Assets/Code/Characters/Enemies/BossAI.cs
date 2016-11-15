using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAI : BaseAI
{
    private bool _bossIsEntering;
    private bool _bossMovingToWaypoint;
    private float _bossNewWaypointTime;
	[SerializeField] Transform _raycastOrigin;
	private bool _busyAvoidingCollision;

	private void Raycast()
	{
		Vector3 _fwd = _raycastOrigin.TransformDirection(Vector3.forward);
		RaycastHit _hit;
		Debug.DrawRay(_raycastOrigin.position, _fwd * 10, Color.green, 1);
		if (Physics.Raycast(_raycastOrigin.position, _fwd, out _hit)) {
			//InteractiveItem interactible = _hit.collider.GetComponent<InteractiveItem>();   //attempt to get the InteractiveItem on the hit object                                                                                // if (_rifle._fired) 
			{
				if (Vector3.Distance(transform.position, _hit.point) < 2 && !_busyAvoidingCollision)
				{
					StartCoroutine(AvoidCollision());
				}
			}
		}
	}

	private IEnumerator AvoidCollision()
	{
		Debug.Log("starting AvoidCollision");
		_busyAvoidingCollision = true;
		Animator.SetTrigger("StandIdleTrigger");
		yield return new WaitForSeconds(0.5f);
		int _randomDirection = Random.Range(0,1);
		if (_randomDirection == 0)
			Animator.SetTrigger("RollLeftTrigger");
		if (_randomDirection == 1)
			Animator.SetTrigger("RollRightTrigger");
		yield return new WaitForSeconds(1);
		StartCoroutine(BossFireSequence());
		yield return new WaitForSeconds(1);
		_busyAvoidingCollision = false;
	}


	protected override void OnEnable()
    {
        transform.LookAt(WaypointNavigator.CurrentWaypoint);
        Animator.SetTrigger("RunTrigger");
        _bossIsEntering = true;
    }

    protected override void OnUpdate()
    {
        if (!_bossIsEntering)
        {
			Raycast();



			//transform.LookAt(WaypointNavigator.CurrentWaypoint);
            if (WaypointNavigator.InStoppingDistance)
            {
                if (!ImBusy)
                {
                    ImBusy = true;
                    Debug.Log("boss near waypoint; stop and fire");
                    StopAllCoroutines();
                    StartCoroutine(BossFireSequence());
                }
            }

            if (Time.time - _bossNewWaypointTime > 12f)
            {
                if (!ImBusy)
                {
                    Debug.Log("boss time out must fire");
                    StartCoroutine(BossFireSequence());
                    ImBusy = true;
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
            Invoke("TempEndLevel", 2);
        }
    }

    
	private void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.tag != "Floor") && (collision.gameObject.tag != "Bullet"))
        {
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
        ImBusy = false;
    }

    private void BossFindNewWaypoint()
    {
        var waypoint = WaypointNavigator.CurrentWaypoint;
		Debug.Log("starting waypoint = " + waypoint.position);
		var tempX = waypoint.position.x + Random.Range(-5f, 5f);
		var tempZ = waypoint.position.z + Random.Range(-5f, 5f);
        waypoint.position = new Vector3(tempX, waypoint.position.y, tempZ);
		Debug.Log("boss new waypoint = " + waypoint.position);
        _bossNewWaypointTime = Time.time;
		transform.LookAt(waypoint);
    }

    private void BossResumeMoving()
    {
        if (Health.CurrentHealth == 3)
        {
            Animator.SetTrigger("SlowWalkTrigger");
        }
        else if (Health.CurrentHealth == 2)
        {
            Animator.SetTrigger("WalkTrigger");
        }
        else if (Health.CurrentHealth == 1)
        {
            Animator.SetTrigger("RunTrigger");
        }
    }

    private void TempEndLevel()
    {
        SceneManager.LoadScene("YouWin");
    }
}