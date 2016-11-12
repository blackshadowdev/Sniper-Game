using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossAI : BaseAI
{
    private bool _bossIsEntering;
    private bool _bossMovingToWaypoint;
    private float _bossNewWaypointTime;

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
            transform.LookAt(WaypointNavigator.CurrentWaypoint);
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

    private void OnCollisionEnter(Collision collision)
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
        var tempX = waypoint.position.x + Random.Range(-10f, 10f);
        var tempZ = waypoint.position.z + Random.Range(-10f, 10f);
        waypoint.position = new Vector3(tempX, waypoint.position.y, tempZ);
        _bossNewWaypointTime = Time.time;
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