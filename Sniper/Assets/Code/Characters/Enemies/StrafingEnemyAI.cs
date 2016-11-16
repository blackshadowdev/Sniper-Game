using UnityEngine;
using System.Collections;

public class StrafingEnemyAI : BaseAI
{
    [SerializeField] private AudioClip _tellAudioClip;
    private bool _isStrafing;

    protected override void OnTakeAction()
    {
        StrafeToWaypoint();
    }

    protected override void OnEnable()
    {
    }

    protected override void OnUpdate()
    {
        if (!_isStrafing)
        {
            return;
        }

        if (!WaypointNavigator.InStoppingDistance)
        {
            return;
        }

        _isStrafing = false;
        Animator.ResetTrigger("StrafeLeft");
        Animator.ResetTrigger("StrafeRight");
        Animator.SetTrigger("StandIdleTrigger");

        if (WaypointNavigator.CurrentIndex == 0)
        {
            ImBusy = false;
			transform.position = WaypointNavigator.CurrentWaypoint.position;				// reset to starting waypoint.position to prevent drift;
        }
        else
        {
            StartCoroutine(PauseTellPauseFire());
        }

        WaypointNavigator.Next();
    }

    protected override void OnDamage(DamageInfo info)
    {
    }

    protected override void OnDie()
    {
        StopAllCoroutines();
        Animator.SetTrigger("DeathTrigger");
    }

    private void StrafeToWaypoint()
    {
        var relPos = GetRelativePosition(transform, WaypointNavigator.CurrentWaypoint.position);
        Animator.SetTrigger(relPos.x < 0f ? "StrafeLeft" : "StrafeRight");
        _isStrafing = true;
        ImBusy = true;
    }

    private IEnumerator PauseTellPauseFire()
    {
        yield return new WaitForSeconds(0.2f);
        AudioSource.PlayOneShot(_tellAudioClip);
        yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
        GetComponent<BulletSpawner>().Fire();
        yield return new WaitForSeconds(1f);
        StrafeToWaypoint();
    }

    private static Vector3 GetRelativePosition(Transform origin, Vector3 position)
    {
        var distance = position - origin.position;
        var relativePosition = Vector3.zero;
        relativePosition.x = Vector3.Dot(distance, origin.right.normalized);
        relativePosition.y = Vector3.Dot(distance, origin.up.normalized);
        relativePosition.z = Vector3.Dot(distance, origin.forward.normalized);

        return relativePosition;
    }
}
