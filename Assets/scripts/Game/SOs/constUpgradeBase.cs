using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "constUpgradeBase", menuName = "Scriptable Objects/constUpgradeBase")]
public class constUpgradeBase : ScriptableObject
{
    [SerializeField] private List<ConstUpgradeSO> _constUpgradeList;
    public List<ConstUpgradeSO> ConstUpgradeList => _constUpgradeList;
}
