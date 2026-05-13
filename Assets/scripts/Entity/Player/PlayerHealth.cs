using UnityEngine;
using System.Collections;
using System;

public class PlayerHealth : MonoBehaviour, IDamageable, IChangingBar
{
    [SerializeField] private BarController _hpBarController;
    [SerializeField] private float _invincibilityTime = 0.2f;
    private PlayerCalculateUpgrades _playerCalculateUpgrades;
    private TotalUpgradeStorage _totalUpgradeStorage;
    private PlayerBehaviour _playerBehaviour;
    private PlayerLevelSystem _playerLevelSystem;
    private SpriteRenderer _spriteRenderer;
    private int _basicHealth = 0;
    private int _health = 0;
    private int _maxHealth = 0;
    private bool _isInvincible = false;
    public event Action OnPlayerDied;
    private void Start()
    {
        if (_hpBarController == null)
        {
            _hpBarController = GameObject.FindGameObjectWithTag("PlayerHPBar").GetComponent<BarController>();
        }
        _playerCalculateUpgrades = GetComponent<PlayerCalculateUpgrades>();
        _totalUpgradeStorage = GetComponent<TotalUpgradeStorage>();
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        _playerLevelSystem = GetComponent<PlayerLevelSystem>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _basicHealth = _playerBehaviour.PlayerBasicStatsSO.PlayerBasicStatsData.HP;
        _maxHealth = _basicHealth;
        _health = _maxHealth;
        _playerCalculateUpgrades.OnUpgradeCalculationFinished += UpdateHP;
    }
    private void OnDestroy()
    {
        _playerCalculateUpgrades.OnUpgradeCalculationFinished -= UpdateHP;
    }
    private void UpdateHP()
    {
        TotalUpgrade totalUpgrade = _totalUpgradeStorage.GetTotalUpgrade(StatType.Health);

        float rawValue = (_basicHealth + totalUpgrade.FlatModifierTotal) * totalUpgrade.MultipleModifierTotal;
        int newMaxHealth = Mathf.RoundToInt(rawValue);

        float ratio = (float)_health / _maxHealth;
        _maxHealth = newMaxHealth;
        _health = _maxHealth;
        _health = Mathf.RoundToInt(_health * ratio);

        _totalUpgradeStorage.Debug();
        ChangeBar();

    }
    public void TakeDamage(DamageData damageData)
    {
        if (_isInvincible)
        {
            return;
        }
        _isInvincible = true;
        StartCoroutine(Invincibility(_invincibilityTime));
        _health -= damageData.Amount;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        ChangeBar();
        if (_health == 0)
        {
            Death();
        }
    }
    public void TakeDamage(DamageData damageData, float knockback)
    {
        if (_isInvincible)
        {
            return;
        }
        _isInvincible = true;
        StartCoroutine(Invincibility(_invincibilityTime));
        _health -= damageData.Amount;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        ChangeBar();
        if (_health == 0)
        {
            Death();
        }
    }
    public void RestoreHP(int hp)
    {
        _health += hp;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        ChangeBar();
    }
    public void RestoreHP(float hp) 
    {
        _health += Mathf.RoundToInt(hp * _maxHealth);
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        ChangeBar();
    }
    private IEnumerator Invincibility(float timeSec)
    {
        yield return new WaitForSeconds(timeSec);
        _isInvincible = false;
    }
    public void ChangeBar()
    {
        _hpBarController.ChangeBar(_health, _maxHealth);
    }
    private void Death()
    {
        OnPlayerDied?.Invoke();
        Destroy(gameObject);
    }
}
