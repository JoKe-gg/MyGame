using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(ButtonHoverEffect))]
public class ConstUpgradeBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Image _constUpgradeSprite;
    private Button _constUpgradeButton;
    public readonly Dictionary<int, ConstUpgradeData> _constUpgradeLevelDictionary = new();
    private int _price = 1;
    private string _name = "";
    private Sprite _sprite;
    public ConstUpgradeType Type { get; private set; } = ConstUpgradeType.Defence;
    public int Level { get; private set; } = 1;

    private void Awake()
    {
        _constUpgradeButton = GetComponent<Button>();
    }
    public void Initialize(ConstUpgradeData constUpgrade, ConstUpgradeType constUpgradeType, string name, Sprite sprite)
    {
        Type = constUpgradeType;
        _constUpgradeLevelDictionary.Add(constUpgrade.Level, constUpgrade);
        _name = name;
        _sprite = sprite;
    }
    public void EndInitialize(int startLevel)
    {
        Level = startLevel+1;
        SetCurrentLevel();
    }
    private void SetCurrentLevel()
    {
        if (!_constUpgradeLevelDictionary.ContainsKey(Level)) 
        { 
            Destroy(gameObject);
            return;
        }
        ConstUpgradeData constUpgrade = _constUpgradeLevelDictionary[Level];
        _nameText.text = _name;
        _price = constUpgrade.Price;
        _priceText.text = _price.ToString();
        _constUpgradeSprite.sprite = _sprite;
        if (CoinsManagerMainMenu.instance != null)
        {
            SetEnable(CoinsManagerMainMenu.instance.Coins);
        }
    }
    private void OnEnable()
    {
        if (CoinsManagerMainMenu.instance != null)
            CoinsManagerMainMenu.instance.OnCoinsChanged += SetEnable;
    }
    private void OnDisable()
    {
        if (CoinsManagerMainMenu.instance != null)
            CoinsManagerMainMenu.instance.OnCoinsChanged -= SetEnable;
    }
    private void SetEnable(int amount)
    {
        bool interactable = amount >= _price;
        _constUpgradeButton.interactable = interactable;
        _constUpgradeSprite.color = interactable ? Color.white : Color.darkGray;
    }
    public void AddLevel(ConstUpgradeData constUpgrade)
    {   if (!_constUpgradeLevelDictionary.ContainsKey(constUpgrade.Level)) 
        {
            _constUpgradeLevelDictionary.Add(constUpgrade.Level, constUpgrade);
        }
    }
    public void AddUpgrade()
    {
        if (ConstUpgradeManager.instance == null) return;
        if (CoinsManagerMainMenu.instance == null) { return; }
        if (_constUpgradeLevelDictionary == null) return;
        ConstUpgradeData constUpgrade = _constUpgradeLevelDictionary[Level];
        CoinsManagerMainMenu.instance.SpendCoins(_price);
        ConstUpgradeManager.instance.AddConstUpgrade(constUpgrade, Type);
        Level++;
        SetCurrentLevel();
    }
    public void DebugConstUpgradeList()
    {
        if (_constUpgradeLevelDictionary == null) return;
        if (_constUpgradeLevelDictionary.Values.Count == 0) return;
        string debugText = $"{Type} \n";
        foreach (var constUpgrade in _constUpgradeLevelDictionary.Values)
        {
            debugText += $"\n\n{constUpgrade.Level} : {constUpgrade.StatModifierData.ModifierType} : {constUpgrade.StatModifierData.Modifier}";
        }
        Debug.Log(debugText);
    }
}
