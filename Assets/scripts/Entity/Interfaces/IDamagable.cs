using UnityEngine;
using System.Collections;
using System;

public interface IDamageable
{
    public abstract void TakeDamage(DamageData damageData);
    public abstract void TakeDamage(DamageData damageData, float knockback);
    public abstract void RestoreHP(int intHeal);
    public abstract void RestoreHP(float percent);
}
