using UnityEngine;

public class HunterAI : BaseAI
{
    [SerializeField] private Transform _myVipTarget = null;
    private bool _isWalking;
    private bool _shouldAttack;
    private float _lastAttackTime;

    public void SetShouldAttack(bool shouldAttack)
    {
        _shouldAttack = shouldAttack;
    }

    protected override void OnEnable()
    {
        transform.LookAt(_myVipTarget);
        Animator.SetTrigger("WalkTrigger");
        _isWalking = true;
    }

    protected override void OnUpdate()
    {
        if (_isWalking)
        {
            transform.LookAt(_myVipTarget);

            if (Vector3.Distance(transform.position, _myVipTarget.position) < 2)
            {
                _isWalking = false;
                _shouldAttack = true;
            }
        }

        if (_shouldAttack)
        {
            if (Time.time - _lastAttackTime > 3f)
            {
                transform.LookAt(_myVipTarget);
                Animator.SetTrigger("HunterAttackTrigger");
                _myVipTarget.transform.GetComponent<IDamageable>().Damage(new DamageInfo(1));
                _lastAttackTime = Time.time;
            }
        }
    }

    protected override void OnDamage(DamageInfo info)
    {
    }

    protected override void OnDie()
    {
        StopAllCoroutines();
        Animator.SetTrigger("DeathTrigger");
    }

    protected override void OnTakeAction()
    {
    }
}