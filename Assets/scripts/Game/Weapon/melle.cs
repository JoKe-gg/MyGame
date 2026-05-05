using System.Collections;
using UnityEngine;

public enum WeaponState
{
    Idle,
    Attack
}
public class melee : Weapon
{
    [SerializeField] private WeaponStatsSO _weaponStatsSO;
    [SerializeField] private PlayerCalculateUpgrades _playerCalculateUpgrades;
    [SerializeField] private WeaponTransform _weaponTransform;
    private TotalUpgrade _damageUpgrade;
    private DamageData _damageData;
    [SerializeField]private MeleeAttackBehaviour _meleeAttackBehaviour;
    [SerializeField] private Animator _animator;
    private bool _isAbleToStartAttack = true;
    void Start()
    {
        bool error = false;
        if (_meleeAttackBehaviour == null)
        {
            Debug.LogError($"Null reference to {nameof(_meleeAttackBehaviour)} in the script {nameof(melee)}");
            error = true;
        }
        if (_weaponTransform == null)
        {
            _weaponTransform = GetComponent<WeaponTransform>();
        }
        if (_playerCalculateUpgrades == null)
        {
            Debug.LogError($"Null reference to {nameof(_playerCalculateUpgrades)} in the script {nameof(melee)}");
            error = true;
        }
        if (_weaponStatsSO == null)
        {
            Debug.LogError($"Null reference to {nameof(_weaponStatsSO)} in the script {nameof(melee)}");
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
                Debug.LogError($"Null reference to {nameof(_animator)} in the script {nameof(melee)}");
                error = true;
            }

        }
        if (error)
        {
            Destroy(gameObject);
            return;
        }
        _damageData = _weaponStatsSO.DamageData;
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
        if (!_isAbleToStartAttack)
        {
            return;
        }
        _isAbleToStartAttack = false;
        SeSetAnimatorState(WeaponState.Attack);
    }
    protected override void UseAbility()
    {
        throw new System.NotImplementedException();
    }
    public void EnableAttackCollider() {
        _meleeAttackBehaviour.EnableCollider(true);
    }
    public void DisableAttackCollider() {
        _meleeAttackBehaviour.EnableCollider(false);
        StartCoroutine(Cooldown(_weaponStatsSO.CoolDown));
    }
    private IEnumerator Cooldown(float sec)
    {
        yield return new WaitForSeconds(sec);
        _isAbleToStartAttack = true;
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
        int damage = Mathf.RoundToInt((_damageData.Amount + _damageUpgrade.FlatModifierTotal) * _damageUpgrade.MultipleModifierTotal);
        DamageData damageData = new DamageData(damage, _damageData.DamageType, _playerCalculateUpgrades.gameObject);


        _meleeAttackBehaviour.SetDamage(damageData);
        _meleeAttackBehaviour.SetEffectsList(_effectsData);
    }
}
