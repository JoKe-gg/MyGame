using UnityEngine;
using System.Collections.Generic;
public class ConstUpgradeFiller : MonoBehaviour
{
    [Header("Filler options")]
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _constPrefab;
    [SerializeField] private List<ConstUpgradeSO> _constUpgradeSO;

    private void Start()
    {
        foreach(var constUpgrade in _constUpgradeSO)
        {
            if (constUpgrade != null)
            {
                GameObject upgrade = Instantiate(_constPrefab, _content);
                string name = constUpgrade.Name + " " + constUpgrade.Level;
                upgrade.GetComponent<ConstUpgradeBehaviour>().Initialize(constUpgrade);
            }
        }
    }
}
