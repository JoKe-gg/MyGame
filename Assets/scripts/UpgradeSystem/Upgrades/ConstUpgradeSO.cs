using System;
using UnityEngine;
using System.Collections.Generic;

public enum ConstUpgradeType
{
    Defence,
    Damage,
    MovementSPD,
    MaxHP,
    EXPBonus,
    RegenerationHP,
    Shield
}
[Serializable]
public class ConstUpgradeData
{
    [Header("Level data")]
    [SerializeField, Min(1)] private int _level;
    [SerializeField] private StatModifierData _statModifierData;
    [Header("Price")]
    [SerializeField, Min(1)] private int _price;

    public int Level => _level;
    public StatModifierData StatModifierData => _statModifierData;
    public int Price => _price;
}
[CreateAssetMenu(fileName = "ConstUpgradeSO", menuName = "Scriptable Objects/ConstUpgradeSO")]
public class ConstUpgradeSO : ScriptableObject
{
    [Header("ID")]
    [SerializeField] private string _name = "someUpgradeName";
    [SerializeField] private Sprite _sprite = null;
    [Header("Const Upgrade stats")]
    [SerializeField] private ConstUpgradeType _constUpgradeType;
    [SerializeField] private List<ConstUpgradeData> _constantUpgradeData;
    public string Name => _name;
    public Sprite Sprite => _sprite;
    public List<ConstUpgradeData> ConstUpgradeData => _constantUpgradeData;
    public ConstUpgradeType ConstUpgradeType => _constUpgradeType;
}