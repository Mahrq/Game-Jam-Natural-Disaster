using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void ModifyHealth(int amount);
    event System.Action OnDeath;
}

public interface IDamageable<T>
{
    void ModifyHealth(T damageData);
    event System.Action OnDeath;
}
