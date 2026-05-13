using System.Collections;
using UnityEngine;

public enum WeaponState
{
    Idle,
    Attack,
    Ability
}
public class Melee : Weapon
{
    [SerializeField] private WeaponStatsSO _weaponStatsSO;
    [SerializeField] private PlayerCalculateUpgrades _playerCalculateUpgrades;
    [SerializeField] private WeaponTransform _weaponTransform;
    private TotalUpgrade _damageUpgrade;
    private DamageData _baseDamageData;
    private DamageData _baseAbilityDamageData;
    private DamageData _damageData;
    private DamageData _abilityDamageData;
    [SerializeField]private MeleeAttackBehaviour _meleeAttackBehaviour;
    [SerializeField] private Animator _animator;
    private bool _isAbleToStartAttack = true;
    void Start()
    {
        bool error = false;
        if (_meleeAttackBehaviour == null)
        {
            Debug.LogError($"Null reference to {nameof(_meleeAttackBehaviour)} in the script {nameof(Melee)}");
            error = true;
        }
        if (_weaponTransform == null)
        {
            _weaponTransform = GetComponent<WeaponTransform>();
        }
        if (_playerCalculateUpgrades == null)
        {
            Debug.LogError($"Null reference to {nameof(_playerCalculateUpgrades)} in the script {nameof(Melee)}");
            error = true;
        }
        if (_weaponStatsSO == null)
        {
            Debug.LogError($"Null reference to {nameof(_weaponStatsSO)} in the script {nameof(Melee)}");
            error = true;
        }
        if (_animator == null)
        {
            if (TryGetComponent(out Animator animator))
            {
                _animator = animator;
            }
            else 
            {
                Debug.LogError($"Null reference to {nameof(_animator)} in the script {nameof(Melee)}");
                error = true;
            }

        }
        if (error)
        {
            Destroy(gameObject);
            return;
        }
        _baseDamageData = _weaponStatsSO.DamageData; 
        _baseAbilityDamageData = _weaponStatsSO.AbilityDamageData;
        SetCooldown(_weaponStatsSO.CoolDown);
        _playerCalculateUpgrades.OnUpgradeCalculationFinished += UpdateUpgrade;
        UpdateUpgrade();
    }
    private void OnDestroy()
    {
        _playerCalculateUpgrades.OnUpgradeCalculationFinished -= UpdateUpgrade;
    }
    protected override void Attack()
    {
        _meleeAttackBehaviour.SetDamage(_damageData);
        SeSetAnimatorState(WeaponState.Attack);
        _cooldown = _weaponStatsSO.CoolDown;
        _abilityCooldown = _weaponStatsSO.AbilityCoolDown;
    }
    protected override void UseAbility()
    {
        _meleeAttackBehaviour.SetDamage(_abilityDamageData);
        SeSetAnimatorState(WeaponState.Ability);
    }
    public void EnableAttackCollider() {
        _meleeAttackBehaviour.EnableCollider(true);
    }
    public void DisableAttackCollider(WeaponState weaponState) {
        _meleeAttackBehaviour.EnableCollider(false);
    }
    public void SeSetAnimatorState(WeaponState weaponState)
    {
        _animator.SetInteger("AttackState", (int)weaponState);
    }
    private void UpdateUpgrade()
    {
        _damageUpgrade = _totalUpgradeStorage.GetTotalUpgrade(StatType.Damage);
        if (_damageUpgrade == null)
        {
            Debug.LogWarning("Damage upgrade not ready yet");
            return;
        }
        _damageData = new DamageData(_baseDamageData.Amount, _baseDamageData.DamageType, _playerCalculateUpgrades.gameObject);
        _abilityDamageData = new DamageData(_baseAbilityDamageData.Amount, _baseAbilityDamageData.DamageType, _playerCalculateUpgrades.gameObject);
        _damageData = GetDamage(_damageData);
        _abilityDamageData = GetDamage(_abilityDamageData);


        _meleeAttackBehaviour.SetEffectsList(_effectsData);
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
        damage = new DamageData(Mathf.RoundToInt((amountOfDamage + _damageUpgrade.FlatModifierTotal) * _damageUpgrade.MultipleModifierTotal), damage.DamageType, _playerCalculateUpgrades.gameObject);
        return damage;
    }
}
