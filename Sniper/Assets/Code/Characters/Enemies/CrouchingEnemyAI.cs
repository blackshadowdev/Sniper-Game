using System.Collections;
using UnityEngine;

public class CrouchingEnemyAI : BaseAI
{
    [SerializeField] private AudioClip _tellAudioClip;


    protected override void OnEnable()
    {
		
		Animator.SetTrigger("CrouchTrigger");
    }

    protected override void OnTakeAction()
    {
        StartCoroutine(StandFireCrouch());
        ImBusy = true;
    }

    protected override void OnUpdate()
    {
    }

    protected override void OnDamage(DamageInfo info)
    {
    }

    protected override void OnDie()
    {
        StopAllCoroutines();
        Animator.SetTrigger("DeathTrigger");
    }

    private IEnumerator StandFireCrouch()
    {
        Animator.SetTrigger("StandIdleTrigger");
        yield return new WaitForSeconds(0.2f);
        AudioSource.PlayOneShot(_tellAudioClip);
        yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
        GetComponent<BulletSpawner>().Fire();
        yield return new WaitForSeconds(Random.Range(2.0f, 3.0f));
        Animator.SetTrigger("CrouchTrigger");
        ImBusy = false;
    }
}
