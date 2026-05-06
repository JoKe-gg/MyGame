using TMPro;
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
    private ConstUpgradeSO _constUpgradeSO;
    private int _price = 1;

    private void Awake()
    {
        _constUpgradeButton = GetComponent<Button>();
    }
    public void Initialize(ConstUpgradeSO constUpgrade)
    {
        _constUpgradeSO = constUpgrade;
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
    public void AddUpgrade()
    {
        if (ConstUpgradeManager.instance == null) return;
        ConstUpgradeManager.instance.AddConstUpgrade(_constUpgradeSO);
        CoinsManagerMainMenu.instance.SpendCoins(_price);
        Destroy(gameObject);
    }
}
