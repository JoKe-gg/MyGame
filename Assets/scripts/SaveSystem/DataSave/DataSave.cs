using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class DataSave
{
    public int Coins;
    public List<ConstUpgradeDataSave> ConstUpgradeList = new();
    public void SetConstUpgradeList(List<ConstUpgradeDataSave> constUpgradeDataSaves)
    {
        foreach (var constUpgrade in constUpgradeDataSaves)
        {
            if (constUpgrade != null) 
            {
                bool doesExist = false;
                bool toReplace = false;
                ConstUpgradeDataSave constUpgradeDataSaveToRemove = null;
                foreach(var item in ConstUpgradeList)
                {
                    if (item.UpgradeType == constUpgrade.UpgradeType)
                    {   
                        doesExist = true;
                        if (item.Level < constUpgrade.Level)
                        {
                            toReplace = true;
                            constUpgradeDataSaveToRemove = item;
                        }
                    }
                }
                if (doesExist)
                {
                    if (toReplace)
                    {
                        ConstUpgradeList.Remove(constUpgradeDataSaveToRemove);
                        ConstUpgradeList.Add(constUpgrade);
                    }
                }
                else
                {
                    ConstUpgradeList.Add(constUpgrade);
                }
            }
        }
    }
    public void SetCoins(int coins)
    {
        Coins = coins;
    }
}
[Serializable]
public class ConstUpgradeDataSave
{
    public int Level = 0;
    public ConstUpgradeType UpgradeType;
    public ConstUpgradeDataSave(int level, ConstUpgradeType upgradeType)
    {
        Level = level;
        UpgradeType = upgradeType;
    }
}
