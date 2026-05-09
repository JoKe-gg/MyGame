using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class ConstUpgradeFiller : MonoBehaviour
{
    [Header("Filler options")]
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _constPrefab;
    private List<ConstUpgradeUIData> _constUpgradeBase;
    private Dictionary<ConstUpgradeType, ConstUpgradeBehaviour> _constUpgradeDictionary = new();

    private void OnEnable()
    {
        _constUpgradeBase = ConstUpgradeManager.instance.ConstUpgradeListForUI;
        Debug.Log($"Const upgrade base amount = {_constUpgradeBase.Count}");
        ClearUI();
        InitializeConstantUpgrades();
    }
    private void ClearUI()
    {
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }

        _constUpgradeDictionary.Clear();
    }
    private void InitializeConstantUpgrades()
    {
        foreach (var constUpgrade in _constUpgradeBase)
        {
            ConstUpgradeSO constUpgradeSO = constUpgrade.UpgradeSO;
            for (int i = 0; i < constUpgradeSO.ConstUpgradeData.Count; i++)
            {
                ConstUpgradeData constUpgradeData = constUpgradeSO.ConstUpgradeData[i];
                if (i == 0)
                {
                    GameObject upgrade = Instantiate(_constPrefab, _content);
                    ConstUpgradeBehaviour constUpgradeBehaviour = upgrade.GetComponent<ConstUpgradeBehaviour>();
                    constUpgradeBehaviour.Initialize(constUpgradeData, constUpgradeSO.ConstUpgradeType, constUpgradeSO.Name, constUpgradeSO.Sprite);
                    _constUpgradeDictionary.Add(constUpgradeSO.ConstUpgradeType, constUpgradeBehaviour);
                }
                else
                {
                    _constUpgradeDictionary[constUpgradeSO.ConstUpgradeType].AddLevel(constUpgradeData);
                }
                if (i == constUpgradeSO.ConstUpgradeData.Count - 1)
                {
                    _constUpgradeDictionary[constUpgradeSO.ConstUpgradeType].EndInitialize(constUpgrade.CurrentLevel);
                }
            }
        }
    }
}
