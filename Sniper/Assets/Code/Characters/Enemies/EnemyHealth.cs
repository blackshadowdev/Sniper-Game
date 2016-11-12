using System;

public class EnemyHealth : BaseHealth
{
    public static event Action<EnemyHealth> EnemyDieEvent = delegate { };

    protected override void OnDamage(DamageInfo info)
    {
    }

    protected override void OnHealthChange()
    {
    }

    protected override void OnDie()
    {
        EnemyDieEvent(this);
        gameObject.SetActive(false);
    }
}