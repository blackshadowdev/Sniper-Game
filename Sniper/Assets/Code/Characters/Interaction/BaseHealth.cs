using System;
using UnityEngine;

public abstract class BaseHealth : MonoBehaviour, IDamageable, IHealth
{
    [SerializeField] private int _startHealth = 1;
    [SerializeField] private int _maxHealth = 1;

    public int MaxHealth
    {
        get { return _maxHealth; }
    }

    public event Action<DamageInfo> DamageEvent = delegate { };

    public bool Damage(DamageInfo info)
    {
        if ((info.Damage < 1) || (CurrentHealth <= 0))
        {
            return false;
        }

        CurrentHealth -= info.Damage;
        DamageEvent(info);
        HealthChangeEvent();

        if (CurrentHealth > 0)
        {
            return false;
        }

        DieEvent();
        CurrentHealth = 0;

        return true;
    }

    public event Action HealthChangeEvent = delegate { };
    public event Action DieEvent = delegate { };

    public int CurrentHealth { get; private set; }

    public void ModifyHealth(int amount)
    {
        CurrentHealth += amount;
        HealthChangeEvent();
    }

    protected virtual void Awake()
    {
        DamageEvent += OnDamage;
        HealthChangeEvent += OnHealthChange;
        DieEvent += OnDie;
    }

    protected virtual void Start()
    {
        CurrentHealth = _startHealth;
        HealthChangeEvent();
    }

    protected virtual void OnDestroy()
    {
        DamageEvent -= OnDamage;
        HealthChangeEvent -= OnHealthChange;
        DieEvent -= OnDie;
    }

    protected abstract void OnDamage(DamageInfo info);
    protected abstract void OnHealthChange();
    protected abstract void OnDie();
}