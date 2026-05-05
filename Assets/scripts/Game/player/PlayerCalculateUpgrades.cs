using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCalculateUpgrades : MonoBehaviour
{
    private UpgradeDataBase _upgradeData;
    private TotalUpgradeStorage _totalUpgradeStorage;
    private List<LevelUpdateData> _levelUpgradeSOs;
    public event Action OnUpgradeCalculationFinished;
    void Start()
    {
        _upgradeData = GetComponent<UpgradeDataBase>();
        _totalUpgradeStorage = GetComponent<TotalUpgradeStorage>();
        _levelUpgradeSOs = _upgradeData.GetLevelUpdateData();
        if (_levelUpgradeSOs.Count > 0 )
        {
            _upgradeData.OnUpgradeListChanged += ReCalculate;
            Calculate();
        }
    }
    private void OnDisable()
    {
        if (_upgradeData != null)
        {
            _upgradeData.OnUpgradeListChanged -= ReCalculate;
        }
    }
    public void ReCalculate()
    {
        _levelUpgradeSOs = _upgradeData.GetLevelUpdateData();
        Calculate();
    }
    private void Calculate()
    {
        int currentPlayerLevel = PlayerPrefs.GetInt(PlayerPrefsKeys.PlayerLevel);
        _totalUpgradeStorage.ResetStorage();

        List<LevelUpdateData> levelUpdateDatas = _upgradeData.GetLevelUpdateData();

        foreach (var levelUpdateData in levelUpdateDatas)
        {
            if (levelUpdateData != null && levelUpdateData.Level <= currentPlayerLevel)
            {
                List<int> flatModifiers = new List<int>();
                List<float> multipleModifiers = new List<float>();

                List<StatModifierData> statModifierDatas = levelUpdateData.StatModifiers;
                foreach (var statModifierData in statModifierDatas)
                {
                    if (statModifierData != null)
                    {
                        if (statModifierData.ModifierType == ModifierType.Flat)
                        {
                            flatModifiers.Add((int)statModifierData.Modifier);
                        }
                        else
                        {
                            multipleModifiers.Add(statModifierData.Modifier);
                        }
                    }
                }

                int totalFlat = CalculateFlat(flatModifiers);
                float totalMultiple = CalculateMultiple(multipleModifiers);

                _totalUpgradeStorage.AddNewTotalUpgrade(levelUpdateData.StatType, totalFlat, totalMultiple);

                Debug.Log($"{levelUpdateData.StatType}: Flat: {totalFlat}; Multiple: {totalMultiple}");
            }
        }

        OnUpgradeCalculationFinished?.Invoke();
    }
    private int CalculateFlat(List<int> flatModifiers)
    {
        int totalFlat = 0;
        foreach (int modifier in flatModifiers)
        {
            totalFlat += modifier;
        }
        return totalFlat;
    }
    private float CalculateMultiple(List<float> multipleModifiers)
    {
        float totalMultiple = 1;
        foreach (float modifier in multipleModifiers)
        {
            if (modifier > 0)
            {
                totalMultiple *= modifier;
            }
        }
        return totalMultiple;
    }
}
