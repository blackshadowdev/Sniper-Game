using UnityEngine;

public abstract class BaseAI : MonoBehaviour
{
    protected bool ImBusy;

    protected IHealth Health { get; private set; }
    protected IDamageable Damageable { get; private set; }
    protected WaypointNavigator WaypointNavigator { get; private set; }
    protected Animator Animator { get; private set; }
    protected AudioSource AudioSource { get; private set; }

    public void TakeAction()
    {
        if (ImBusy)
        {
            return;
        }

        OnTakeAction();
    }

    protected abstract void OnEnable();
    protected abstract void OnTakeAction();

    protected abstract void OnUpdate();
    protected abstract void OnDamage(DamageInfo info);
    protected abstract void OnDie();

    private void Awake()
    {
        Health = GetComponent<IHealth>();
        Health.DieEvent += OnDie;

        Damageable = GetComponent<IDamageable>();
        Damageable.DamageEvent += OnDamage;

        WaypointNavigator = GetComponent<WaypointNavigator>();
        AudioSource = GetComponent<AudioSource>();
        Animator = GetComponent<Animator>();

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Damageable.DamageEvent -= OnDamage;
        Health.DieEvent -= OnDie;
    }

    private void Update()
    {
        OnUpdate();
    }
}