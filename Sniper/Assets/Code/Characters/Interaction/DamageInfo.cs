public struct DamageInfo
{
    public DamageInfo(int damage)
    {
        _damage = damage;
    }

    private int _damage;

    public int Damage { get { return _damage; } }
}
