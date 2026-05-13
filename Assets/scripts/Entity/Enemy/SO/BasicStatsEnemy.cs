using System;
using UnityEngine;

[Serializable]
public class BasicStatsEnemy
{
    [SerializeField] private int _basicMaxHealth = 100;
    [SerializeField] private int _basicResist = 0;
    [SerializeField] private Vector2 _expForKillingRange = new Vector2(5, 10);
    [SerializeField] private Vector2 _coinsForKillingRange = new Vector2(2, 8);
    [SerializeField] private DamageData _basicDamageData;
    [SerializeField] private float _basicMovementSpeed = 2;
    [SerializeField] private float _intervalBetweenAttacks = 0.1f;

    public int BasicMaxHealth => _basicMaxHealth;
    public int BasicResist => _basicResist;
    public Vector2 ExpForKillingRange => _expForKillingRange;
    public Vector2 CoinsForKillingRange => _coinsForKillingRange;
    public DamageData DamageData => _basicDamageData;
    public float BasicMovementSpeed => _basicMovementSpeed;
    public float IntervalBetweenAttacks => _intervalBetweenAttacks;
}
