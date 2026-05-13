using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class TotalUpgrade
{
    public StatType StatTypeTotal { get; private set; } = StatType.Damage;
    public int FlatModifierTotal { get; private set; } = 0;
    public float MultipleModifierTotal { get; private set; } = 1;

    public TotalUpgrade(StatType statType, int flatModifier = 0, float multipleModifier = 1)
    {
        StatTypeTotal = statType;
        FlatModifierTotal = flatModifier;
        MultipleModifierTotal = multipleModifier;
    }
    public void UpdateTotalUpgrade (int flatModifier, float multipleModifier)
    {
        FlatModifierTotal += flatModifier;
        MultipleModifierTotal *= multipleModifier;
    }
}
public class Effect
{
    public readonly NegativeEffectData EffectData;

    public Effect(NegativeEffectData effectData)
    {
        EffectData = effectData;
    }
}
public class TotalUpgradeStorage : MonoBehaviour
{
    private PlayerCalculateUpgrades _playerCalculateUpgrades;
    private Dictionary<StatType ,TotalUpgrade> _totalUpgrades;
    private Dictionary<StatusEffectType, Effect> _effectList;
    public event Action<List<Effect>> OnEffectListChanged;
    private void Awake()
    {
        _playerCalculateUpgrades = GetComponent<PlayerCalculateUpgrades>();
        _totalUpgrades = new Dictionary<StatType, TotalUpgrade>();
        _effectList = new Dictionary<StatusEffectType, Effect>();
    }

    private void OnEnable()
    {
        if (_playerCalculateUpgrades != null)
        {
            _playerCalculateUpgrades.OnUpgradeCalculationFinished += OnUpgradeCalculationFinished;
        }
    }

    private void OnDisable()
    {
        if (_playerCalculateUpgrades != null)
        {
            _playerCalculateUpgrades.OnUpgradeCalculationFinished -= OnUpgradeCalculationFinished;
        }
    }
    public void ResetStorage()
    {
        _totalUpgrades.Clear();
    }
    public void AddNewTotalUpgrade(StatType statType, int flatModifierTotal, float multipleModifierTotal)
    {
        if (_totalUpgrades.ContainsKey(statType))
        {
            _totalUpgrades[statType].UpdateTotalUpgrade(flatModifierTotal, multipleModifierTotal);
            return;
        }
        TotalUpgrade totalUpgrade = new TotalUpgrade(statType, flatModifierTotal, multipleModifierTotal);

        _totalUpgrades.Add(statType, totalUpgrade);
    }
    public void AddNewEffect(NegativeEffectData negativeEffectData)
    {
        if (negativeEffectData == null)
        {
            return;
        }
        if (_effectList.ContainsKey(negativeEffectData.EffectType))
        {
            Effect effect = new Effect(negativeEffectData);
            _effectList[negativeEffectData.EffectType] = effect;
        }
        else 
        {
            Effect effect = new Effect(negativeEffectData);
            _effectList.Add(negativeEffectData.EffectType, effect);
        }
        OnEffectListChanged?.Invoke(GetEffects());
    }
    public TotalUpgrade GetTotalUpgrade(StatType statType)
    {
        if (_totalUpgrades.ContainsKey(statType))
        {
            return _totalUpgrades[statType];
        }
        TotalUpgrade emptyUpgrade = new TotalUpgrade(statType);
        return emptyUpgrade;
    }
    public List<Effect> GetEffects()
    {
        return _effectList.Values.ToList();
    }
    public Effect GetEffect(StatusEffectType statusEffectType)
    {
        return _effectList[statusEffectType];
    }
    public void DeleteEffect(StatusEffectType statusEffectType)
    {
        _effectList.Remove(statusEffectType);
        OnEffectListChanged?.Invoke(GetEffects());
    }
    private void OnUpgradeCalculationFinished()
    {
        Debug();
    }
    public void Debug()
    {
        foreach (TotalUpgrade totalUpgrade in _totalUpgrades.Values)
        {
            UnityEngine.Debug.Log(
                $"Total Upgrade Data of {totalUpgrade.StatTypeTotal}." +
                $"\n Flat modifier: {totalUpgrade.FlatModifierTotal};" +
                $"\n Multiple modifier: {totalUpgrade.MultipleModifierTotal}\n\n");
        }
    }
}
