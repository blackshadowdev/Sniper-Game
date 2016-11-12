using System;

public interface IHealth
{
    event Action HealthChangeEvent;
    event Action DieEvent;

    int CurrentHealth { get; }

    void ModifyHealth(int amount);
}
