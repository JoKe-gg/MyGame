using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public enum UpgradeChoiceType
{
    Poison,
    Burn,
    DamageIncrease,
    HealthIncrease
}

[CreateAssetMenu(fileName = "SelectableUpgradeSO", menuName = "Scriptable Objects/SelectableUpgradeSO")]
public class SelectableUpgradeSO : ScriptableObject
{
    [Header("Selectable choice id")]
    [SerializeField] private string _name;
    [SerializeField] private UpgradeChoiceType _upgradeChoiceType;
    [Header("Selectable choice stats")]
    [SerializeField] private bool _readUpgrade = true;
    [SerializeField] private LevelUpdateData _upgrade;
    [SerializeField] private bool _readEffect = true;
    [SerializeField] private NegativeEffectData _effectData;

    private void OnValidate()
    {
        if (_readUpgrade)
        {
            _readEffect = false;
            if (_upgrade.Level <= 0)
            {
                _upgrade.ResetLevel();
            }
            _effectData = null;
        }
        else
        {
            _readEffect = true;
            _readUpgrade = false;
            if (_effectData.Level <= 0)
            {
                _effectData.ResetLevel();
            }
            _upgrade = null;
        }
    }
    public new string name => _name;
    public UpgradeChoiceType UpgradeChoiceType => _upgradeChoiceType;
    public LevelUpdateData Upgrade => _upgrade;
    public bool ReadUpgrade => _readUpgrade;
    public NegativeEffectData EffectData => _effectData;
    public bool ReadEffect => _readEffect;
}
