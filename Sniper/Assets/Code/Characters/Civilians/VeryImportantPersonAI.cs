using UnityEngine;
using System.Collections;

public class VeryImportantPersonAI : BaseAI
{
    [SerializeField] private Transform _myHunter = null;
    private bool _vipIsWalking;

    protected override void OnEnable()
    {
        transform.LookAt(WaypointNavigator.CurrentWaypoint);
        Animator.SetTrigger("WalkTrigger");
        _vipIsWalking = true;
    }

    protected override void OnUpdate()
    {
        if (_vipIsWalking)
        {
            transform.LookAt(WaypointNavigator.CurrentWaypoint);
            if (Vector3.Distance(transform.position, WaypointNavigator.CurrentWaypoint.position) < 1)
            {
                Animator.SetTrigger("StandIdleTrigger");
                _vipIsWalking = false;
            }
        }
    }

    protected override void OnTakeAction()
    {
    }

    protected override void OnDamage(DamageInfo info)
    {
        if (info.Damage > 0)
            StartCoroutine(VipGetsHit());
    }

    protected override void OnDie()
    {
        StopAllCoroutines();
        Animator.SetTrigger("DeathTrigger");
    }

    private IEnumerator VipGetsHit()
    {
        yield return new WaitForSeconds(0.3f);
        
        if (Health.CurrentHealth > 0)
        {
            Animator.SetTrigger("GetHitTrigger");
        }
        else
        {
            Animator.SetTrigger("DeathTrigger");
            _myHunter.transform.GetComponent<HunterAI>().SetShouldAttack(false);
        }
    }
}
