using System;

public interface IDamageable
{
    event Action<DamageInfo> DamageEvent;
    bool Damage(DamageInfo info);
}