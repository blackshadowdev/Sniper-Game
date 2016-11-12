using System;

public class CivilianHealth : BaseHealth
{
    public static event Action<CivilianHealth> CivilianDieEvent = delegate { };

    protected override void OnDamage(DamageInfo info)
    {
    }

    protected override void OnHealthChange()
    {
    }

    protected override void OnDie()
    {
        CivilianDieEvent(this);
        gameObject.SetActive(false);
    }
}