using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
public class ConstUpgradeUIData
{
    public ConstUpgradeSO UpgradeSO { get; private set; }
    public int CurrentLevel { get; private set; }

    public ConstUpgradeUIData(ConstUpgradeSO upgradeSO, int currentLevel)
    {
        UpgradeSO = upgradeSO;
        CurrentLevel = currentLevel;
    }
}
public class ConstUpgradeManager : Savable
{
    public static ConstUpgradeManager instance { get; private set; }
    [SerializeField] private constUpgradeBase _constUpgradeBase;
    public List<ConstUpgradeUIData> GetConstUpgradeListForUI => GetConstUpgradeListUI();
    public Dictionary<ConstUpgradeType, ConstUpgradeData> ConstUpgradeDictionary { get; private set; } = new();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public override void Load(DataSave dataSave)
    {
        ConstUpgradeDictionary.Clear();
        foreach (var item in dataSave.ConstUpgradeList)
        {
            foreach (var constUpgrade in _constUpgradeBase.ConstUpgradeList)
            {
                if (item.UpgradeType == constUpgrade.ConstUpgradeType)
                {
                    int index = item.Level - 1;

                    if (index >= 0 && index < constUpgrade.ConstUpgradeData.Count)
                    {
                        ConstUpgradeDictionary[constUpgrade.ConstUpgradeType] = constUpgrade.ConstUpgradeData[index];
                    }
                }
            }
        }
    }
    private List<ConstUpgradeUIData> GetConstUpgradeListUI()
    {
        List<ConstUpgradeUIData> NewConstUpgradeDictionary = new();
        foreach (var constUpgrade in _constUpgradeBase.ConstUpgradeList)
        {
            int level = 0;
            if (ConstUpgradeDictionary.TryGetValue(constUpgrade.ConstUpgradeType, out ConstUpgradeData data))
            {
                level = data.Level;
            }
            ConstUpgradeUIData constUpgradeUIData = new(constUpgrade, level);


            NewConstUpgradeDictionary.Add(constUpgradeUIData);
        }
        return NewConstUpgradeDictionary;
    }
    public override void Save(DataSave dataSave)
    {
        dataSave.ConstUpgradeList.Clear();

        foreach (var item in ConstUpgradeDictionary)
        {
            ConstUpgradeDataSave constUpgradeDataSave = new(item.Value.Level, item.Key);
            dataSave.ConstUpgradeList.Add(constUpgradeDataSave);
        }
    }
    public ConstUpgradeData GetConstUpgrade(ConstUpgradeType constUpgradeType)
    {
        ConstUpgradeDictionary.TryGetValue(constUpgradeType, out ConstUpgradeData upgrade);
        return upgrade;
    }
    public void AddConstUpgrade(ConstUpgradeData constUpgradeData, ConstUpgradeType constUpgradeType)
    {
        if (constUpgradeData == null)
        {
            Debug.LogError("ConstUpgradeSO is null");
            return;
        }
        if (ConstUpgradeDictionary.TryGetValue(constUpgradeType, out ConstUpgradeData currentUpgrade))
        {
            if (currentUpgrade.Level < constUpgradeData.Level)
            {
                ConstUpgradeDictionary[constUpgradeType] = constUpgradeData;
            }
        }
        else
        {
            ConstUpgradeDictionary[constUpgradeType] = constUpgradeData;
        }    
    }
}