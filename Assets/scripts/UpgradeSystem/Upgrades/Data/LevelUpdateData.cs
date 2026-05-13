using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public enum ModifierType
{
    Flat,
    Multiple
}
[Serializable]
public class StatModifierData
{

    [SerializeField] private ModifierType _modifierType;
    [SerializeField] private float modifier = 1;
    public ModifierType ModifierType => _modifierType;
    public float Modifier => modifier;
}
[Serializable]
public class LevelUpdateData
{
    [SerializeField] private int _level;
    [SerializeField] private StatType _statType;
    [SerializeField] private List<StatModifierData> _statModifiers;

    public int Level => _level;
    public StatType StatType => _statType;
    public List<StatModifierData> StatModifiers => _statModifiers;
    public void ResetLevel()
    {
        _level = 1;
    }
}
