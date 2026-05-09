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
public class ConstUpgradeManager : MonoBehaviour, ISavable
{
    public static ConstUpgradeManager instance { get; private set; }
    [SerializeField] private constUpgradeBase _constUpgradeBase;
    public List<ConstUpgradeUIData> ConstUpgradeListForUI { get; private set; } = new();
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
    public void Load(DataSave dataSave)
    {
        Debug.Log($"CONST IS BEING LOADED");
        ConstUpgradeListForUI.Clear();
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
        foreach (var constUpgrade in _constUpgradeBase.ConstUpgradeList)
        {
            int level = 0;
            if (ConstUpgradeDictionary.TryGetValue(constUpgrade.ConstUpgradeType, out ConstUpgradeData data))
            {
                level = data.Level;
            }
            ConstUpgradeUIData constUpgradeUIData = new(constUpgrade, level);
            

            ConstUpgradeListForUI.Add(constUpgradeUIData);
        }
    }
    public void Save(DataSave dataSave)
    {
        dataSave.ConstUpgradeList.Clear();

        foreach (var item in ConstUpgradeDictionary)
        {
            ConstUpgradeDataSave constUpgradeDataSave = new(item.Value.Level, item.Key);
            dataSave.ConstUpgradeList.Add(constUpgradeDataSave);
        }
    }
    private void OnEnable()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.Register(this);
        }
        else
        {
            Debug.Log($"SAVE MANAGER IS NOT READY");
            StartCoroutine(WaitToRegister());
        }
    }
    private void OnDisable()
    {
        if (SaveManager.instance != null)
        {
            SaveManager.instance.Unregister(this);
        }
    }
    private IEnumerator WaitToRegister()
    {
        yield return new WaitForEndOfFrame();
        if (SaveManager.instance != null)
        {
            SaveManager.instance.Register(this);
        }
        else
        {
            Debug.Log($"SAVE MANAGER IS NOT READY IN COROUTINE");
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