using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(ButtonHoverEffect))]
public class ConstUpgradeBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Image _constUpgradeSprite;
    private Button _constUpgradeButton;
    public readonly Dictionary<int, ConstUpgradeSO> _constUpgradeLevelDictionary = new();
    private int _price = 1;
    public int _level { get; private set; } = 1;

    private void Awake()
    {
        _constUpgradeButton = GetComponent<Button>();
    }
    public void Initialize(ConstUpgradeSO constUpgrade)
    {
        _constUpgradeLevelDictionary.Add(constUpgrade.Level, constUpgrade);
        SetCurrentLevel();
        
    }
    private void SetCurrentLevel()
    {
        if (!_constUpgradeLevelDictionary.ContainsKey(_level)) 
        { 
            Destroy(gameObject);
            return;
        }
        ConstUpgradeSO constUpgrade = _constUpgradeLevelDictionary[_level];
        _name.text = constUpgrade.Name;
        _price = constUpgrade.Price;
        _priceText.text = _price.ToString();
        _constUpgradeSprite.sprite = constUpgrade.Sprite; 
        if (CoinsManager.instance != null)
        {
            SetEnable(CoinsManager.instance.Coins);
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
    public void AddLevel(ConstUpgradeSO constUpgrade)
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
        ConstUpgradeSO constUpgrade = _constUpgradeLevelDictionary[_level];
        _level++;
        SetCurrentLevel();
        ConstUpgradeManager.instance.AddConstUpgrade(constUpgrade);
        CoinsManagerMainMenu.instance.SpendCoins(_price);
    }
    public void DebugConstUpgradeList()
    {
        if (_constUpgradeLevelDictionary == null) return;
        if (_constUpgradeLevelDictionary.Values.Count == 0) return;
        string debugText = $"{_constUpgradeLevelDictionary[1].ConstUpgradeType} \n";
        foreach (var constUpgrade in _constUpgradeLevelDictionary.Values)
        {
            debugText += $"\n\n{constUpgrade.Level} : {constUpgrade.StatModifierData.ModifierType} : {constUpgrade.StatModifierData.Modifier}";
        }
        Debug.Log(debugText);
    }
}
