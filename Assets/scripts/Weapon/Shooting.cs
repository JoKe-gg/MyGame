using NUnit.Framework;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Shooting : Weapon
{
    [SerializeField] private WeaponStatsSO _weaponStatsSO;
    [SerializeField] private PlayerCalculateUpgrades _playerCalculateUpgrades;
    [SerializeField] private Transform _anchoredPosition;
    [SerializeField] private WeaponTransform _weaponTransform;
    private ProjectilePool _projectilePool;
    private TotalUpgrade _damageUpgrade;
    private int _flatModifier = 0;
    private float _multipleModifier = 1;
    private void Start()
    {
        bool error = false;
        _projectilePool = GameManager.instance.ProjectilePool;
        if (_projectilePool == null)
        {
            Debug.LogError($"Null reference to {nameof(_projectilePool)} in the script {nameof(Shooting)}");
            error = true;
        }
        if (_weaponTransform == null)
        {
            _weaponTransform = GetComponent<WeaponTransform>();
        }
        if (_playerCalculateUpgrades == null) 
        {
            Debug.LogError($"Null reference to {nameof(_playerCalculateUpgrades)} in the script {nameof(Shooting)}");
            error = true;
        }
        if (_totalUpgradeStorage == null)
        {
            Debug.LogError($"Null reference to {nameof(_totalUpgradeStorage)} in the script {nameof(Shooting)}");
            error = true;
        }
        if (_weaponStatsSO == null)
        {
            Debug.LogError($"Null reference to {nameof(_weaponStatsSO)} in the script {nameof(Shooting)}");
            error = true;
        }
        if (_anchoredPosition == null)
        {
            Debug.LogError($"Null reference to {nameof(_anchoredPosition)} in the script {nameof(Shooting)}");
            error = true;
        }
        if (error)
        {
            Destroy(gameObject);
            return;
        }
        SetCooldown(_weaponStatsSO.CoolDown);
        SetAbilityCooldown(_weaponStatsSO.AbilityCoolDown);
        _playerCalculateUpgrades.OnUpgradeCalculationFinished += UpdateUpgrade;
        UpdateUpgrade();
    }
    private void OnDestroy()
    {
        _playerCalculateUpgrades.OnUpgradeCalculationFinished -= UpdateUpgrade;
    }
    protected override void Attack()
    {
        DamageData _basicDamageData = _weaponStatsSO.DamageData;
        DamageData damage = GetDamage(_basicDamageData);
        float speed = _weaponStatsSO.Speed * 2f * (_weaponTransform.IsFlipped() ? -1 : 1);
        BulletBehaviour bulletBehaviour = _projectilePool.GetProjectile();
        bulletBehaviour.transform.localRotation = transform.localRotation;
        bulletBehaviour.Initialize(_effectsData, gameObject, _anchoredPosition.position, transform, damage, _weaponStatsSO.Penetration, speed, _projectilePool, 2f);
    }
    protected override void UseAbility()
    {
        DamageData _basicDamageData = _weaponStatsSO.AbilityDamageData;
        DamageData damage = GetDamage(_basicDamageData);
        float speed = _weaponStatsSO.AbilitySpeed * 2f * (_weaponTransform.IsFlipped() ? -1 : 1);
        BulletBehaviour bulletBehaviour = _projectilePool.GetProjectile();
        bulletBehaviour.transform.localRotation = transform.localRotation;
        bulletBehaviour.Initialize(_effectsData, gameObject, _anchoredPosition.position, transform, damage, _weaponStatsSO.AbilityPenetration, speed, _projectilePool, 5f);
    }
    private void UpdateUpgrade()
    {
        _damageUpgrade = _totalUpgradeStorage.GetTotalUpgrade(StatType.Damage);
        if (_damageUpgrade == null)
        {
            Debug.LogWarning("Damage upgrade not ready yet");
            return;
        }

        _flatModifier = _damageUpgrade.FlatModifierTotal;
        _multipleModifier = _damageUpgrade.MultipleModifierTotal;
    }
    private DamageData GetDamage(DamageData damage)
    {
        int amountOfDamage = damage.Amount;
        if (_constUpgradeSO != null)
        {
            var modifierData = _constUpgradeSO.StatModifierData;

            amountOfDamage = Mathf.RoundToInt(modifierData.ModifierType == ModifierType.Multiple
                ? amountOfDamage * modifierData.Modifier
                : amountOfDamage + modifierData.Modifier);
        }
        damage = new DamageData(Mathf.RoundToInt((amountOfDamage + _flatModifier) * _multipleModifier), damage.DamageType, _playerCalculateUpgrades.gameObject);
        return damage;
    }
}
