using UnityEngine;
using System.Collections.Generic;
public class ConstUpgradeManager : MonoBehaviour
{
    public static ConstUpgradeManager instance { get; private set; }
    public Dictionary<ConstUpgradeType, ConstUpgradeSO> ConstUpgradeList { get; private set; } = new();
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
    public ConstUpgradeSO GetConstUpgrade(ConstUpgradeType constUpgradeType)
    {
        ConstUpgradeList.TryGetValue(constUpgradeType, out ConstUpgradeSO upgrade);
        return upgrade;
    }
    public void AddConstUpgrade(ConstUpgradeSO constUpgradeSO)
    {
        if (constUpgradeSO == null)
        {
            Debug.LogError("ConstUpgradeSO is null");
            return;
        }
        ConstUpgradeType constUpgradeType = constUpgradeSO.ConstUpgradeType;
        if (ConstUpgradeList.TryGetValue(constUpgradeType, out ConstUpgradeSO currentUpgrade))
        {
            if (currentUpgrade.Level < constUpgradeSO.Level)
            {
                ConstUpgradeList[constUpgradeType] = constUpgradeSO;
            }
        }
        else
        {
            ConstUpgradeList[constUpgradeType] = constUpgradeSO;
        }
        Debug.Log($"New Const added: {constUpgradeSO.Name}\n" +
                  $"{{ {constUpgradeSO.ConstUpgradeType} : " +  
                  $"{constUpgradeSO.StatModifierData.ModifierType} modifier : " +  
                  $"{constUpgradeSO.StatModifierData.Modifier} }}");  
                    
    }
}