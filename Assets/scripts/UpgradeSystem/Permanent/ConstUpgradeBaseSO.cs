using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "constUpgradeBaseSO", menuName = "Scriptable Objects/constUpgradeBaseSO")]
public class constUpgradeBaseSO : ScriptableObject
{
    [SerializeField] private List<ConstUpgradeSO> _constUpgradeList;
    public List<ConstUpgradeSO> ConstUpgradeList => _constUpgradeList;
}
