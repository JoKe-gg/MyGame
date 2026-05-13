using TMPro;
using UnityEngine;

public class ChoiceFilling
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public ChoiceFilling(string name, string description) 
    {
        Name = name;
        Description = description;
    }
}
public class UpgradeChoiceBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _description;

    public void SetSelect(SelectableUpgradeSO selectableUpgradeSO)
    {
        _name.text = CreateName(selectableUpgradeSO);
        _description.text = CreateDescription(selectableUpgradeSO);
    }
    private string CreateName(SelectableUpgradeSO selectableUpgradeSO)
    {
        return selectableUpgradeSO.name + " " + (selectableUpgradeSO.ReadUpgrade ? selectableUpgradeSO.Upgrade.Level : selectableUpgradeSO.EffectData.Level);
    }
    private string CreateDescription(SelectableUpgradeSO selectableUpgradeSO)
    {
        string description = "Description of effect";
        if (selectableUpgradeSO.ReadUpgrade)
        {
            description = $"{selectableUpgradeSO.Upgrade.StatType.ToString()}\n";
            foreach(var modifier in selectableUpgradeSO.Upgrade.StatModifiers)
            {
                if (modifier.ModifierType == ModifierType.Multiple)
                {
                    description += $" {modifier.Modifier * 100}%\n";
                }
                else
                {
                    description += $"+ {modifier.Modifier}\n";
                }
            }
        }
        else if (selectableUpgradeSO.ReadEffect)
        {
            description = $"{selectableUpgradeSO.EffectData.EffectType.ToString()}\n";
            description += $"Deals {selectableUpgradeSO.EffectData.DamageData.Amount} of {selectableUpgradeSO.EffectData.DamageData.DamageType} " +
                           $"damage every {selectableUpgradeSO.EffectData.IntervalBetweenTicks} second for {selectableUpgradeSO.EffectData.TimeOfEffect} second.";
        }
        return description;
    }
}
