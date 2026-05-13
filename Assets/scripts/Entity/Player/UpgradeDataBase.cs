using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class UpgradeDataBase : MonoBehaviour
{
    [SerializeField] private List<LevelUpgradeSO> _levelUpgradeSOs;
    private List<LevelUpdateData> _levelUpdateData;
    public event Action OnUpgradeListChanged;
    public void Awake()
    {
        UpdateLevelUpdateData();
    }
    public List<LevelUpdateData> GetLevelUpdateData()
    {
        return _levelUpdateData;
    }
    private void UpdateLevelUpdateData()
    {
        List<LevelUpdateData> levelUpdateDatas = new List<LevelUpdateData>();
        foreach (var levelUpgradeSO in _levelUpgradeSOs)
        {
            foreach (var LevelUpgradeData in levelUpgradeSO.LevelUpdateData)
            {
                levelUpdateDatas.Add(LevelUpgradeData);
            }
        }
        _levelUpdateData = levelUpdateDatas;
    }
    public void AddNewUpgrade(LevelUpdateData levelUpdateData)
    {
        _levelUpdateData.Add(levelUpdateData);
        OnUpgradeListChanged?.Invoke();
    }
}
