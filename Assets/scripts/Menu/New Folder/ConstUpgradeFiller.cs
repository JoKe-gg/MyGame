using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class ConstUpgradeFiller : MonoBehaviour
{
    [Header("Filler options")]
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _constPrefab;
    [SerializeField] private constUpgradeBase _constUpgradeBase;
    [SerializeField] private Dictionary<ConstUpgradeType, ConstUpgradeBehaviour> _constUpgradeDictionary = new();

    private void Start()
    {
        foreach(var constUpgrade in _constUpgradeBase.ConstUpgradeList)
        {
            if (constUpgrade != null)
            {
                if (_constUpgradeDictionary.TryGetValue(constUpgrade.ConstUpgradeType, out ConstUpgradeBehaviour existedConstUpgradeBehaviour))
                {
                    existedConstUpgradeBehaviour.AddLevel(constUpgrade);
                }
                else
                {
                    GameObject upgrade = Instantiate(_constPrefab, _content);
                    string name = constUpgrade.Name + " " + constUpgrade.Level;
                    ConstUpgradeBehaviour constUpgradeBehaviour = upgrade.GetComponent<ConstUpgradeBehaviour>();
                    constUpgradeBehaviour.Initialize(constUpgrade);
                    _constUpgradeDictionary.Add(constUpgrade.ConstUpgradeType, constUpgradeBehaviour);
                }
            }
        }
    }
}
