using UnityEngine;

public enum ConstUpgradeType
{
    Defence,
    Damage,
    MovementSPD,
    MaxHP,
    EXPBonus,
    RegenerationHP,
    Shield
}

[CreateAssetMenu(fileName = "ConstUpgradeSO", menuName = "Scriptable Objects/ConstUpgradeSO")]
public class ConstUpgradeSO : ScriptableObject
{
    [Header("ID")]
    [SerializeField] private string _name = "someUpgradeName";
    [SerializeField] private Sprite _sprite = null;
    [Header("Const Upgrade stats")]
    [SerializeField, Min(1)] private int _level;
    [SerializeField] private ConstUpgradeType _constUpgradeType;
    [SerializeField] private StatModifierData _statModifierData;
    [Header("Price")]
    [SerializeField, Min(1)] private int _price;
    public string Name => _name;
    public Sprite Sprite => _sprite;
    public int Level => _level;
    public ConstUpgradeType ConstUpgradeType => _constUpgradeType;
    public StatModifierData StatModifierData => _statModifierData;
    public int Price => _price;

}
