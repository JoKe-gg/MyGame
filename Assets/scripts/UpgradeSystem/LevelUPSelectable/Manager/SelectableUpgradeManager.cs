using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class SelectableUpgradeManager : MonoBehaviour
{
    [SerializeField] private List<SelectableUpgradeSO> _originUpgrades;
    private List<SelectableUpgradeSO> _upgrades;
    private void Awake()
    {
        _upgrades = new List<SelectableUpgradeSO>(_originUpgrades);
    }
    public List<SelectableUpgradeSO> GetChoices(int requiredAmount = 2)
    {
        if (_upgrades == null || _upgrades.Count == 0)
        {
            return new List<SelectableUpgradeSO>();
        }
        List<SelectableUpgradeSO> availableSelectableUpgrades = GetAvailableChoices();
        List<SelectableUpgradeSO> choices = new();

        if (availableSelectableUpgrades.Count <= requiredAmount)
        {
            return availableSelectableUpgrades;
        }

        for (int i = 0; i < requiredAmount; i++)
        {
            int randIndex = Random.Range(0, availableSelectableUpgrades.Count);
            SelectableUpgradeSO newSelect = availableSelectableUpgrades[randIndex];
            choices.Add(newSelect);
            availableSelectableUpgrades.RemoveAt(randIndex);
        }
        return choices;
    }
    private List<SelectableUpgradeSO> GetAvailableChoices()
    {
        Dictionary<UpgradeChoiceType, SelectableUpgradeSO> availableChoices = new Dictionary<UpgradeChoiceType, SelectableUpgradeSO>();
        foreach (var upgrade in _upgrades)
        {
            if (availableChoices.TryGetValue(upgrade.UpgradeChoiceType, out SelectableUpgradeSO value)) 
            {
                if(upgrade.ReadUpgrade)
                {
                    if (upgrade.Upgrade.Level < value.Upgrade.Level)
                    {
                        availableChoices[upgrade.UpgradeChoiceType] = upgrade;
                    }
                }
                else
                {
                    if (upgrade.EffectData.Level < value.EffectData.Level)
                    {
                        availableChoices[upgrade.UpgradeChoiceType] = upgrade;
                    }
                }
            }
            else
            {
                availableChoices[upgrade.UpgradeChoiceType] = upgrade;
            }
        }
        List<SelectableUpgradeSO> selectableUpgrades = availableChoices.Values.ToList();
        return selectableUpgrades;
    }
    public void RemoveChoice(SelectableUpgradeSO selectableUpgradeSO)
    {
        _upgrades.RemoveAll(u => u == selectableUpgradeSO);
    }
}
