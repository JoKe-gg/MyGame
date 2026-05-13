using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{

    [SerializeField] private GameObject _expGem;
    private BasicStatsEnemySO _basicStatsEnemySO;
    private Enemy _enemy;
    private EnemyMovement _enemyMovement;
    private GemPool _gemPool;
    private int _health;
    private int _maxHealth;
    private int _resistance;
    private bool _isAbleToAttack = true;
    private bool _isDead = false;
    public event Action<int, int> OnHealthChanged;
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        _enemyMovement = GetComponent<EnemyMovement>();
    }
    void Start()
    {
        bool error = false;
        if (_expGem == null)
        {
            Debug.LogError($"Null reference to {nameof(_expGem)} in the script {nameof(EnemyHealth)}");
            error = true;
        }
        if (error)
        {
            Destroy(gameObject);
            return;
        }  
        _gemPool = GameManager.instance.GemPool;
        _basicStatsEnemySO = _enemy.BasicStatsEnemySO;
        _maxHealth = _basicStatsEnemySO.basicStats.BasicMaxHealth;
        _resistance = _basicStatsEnemySO.basicStats.BasicResist;
        _health = _maxHealth;
        _basicStatsEnemySO = _enemy.BasicStatsEnemySO;
        StateManager.instance.OnStateChanged += OnStateChanged;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        _isAbleToAttack = true;
    }
    private void OnStateChanged(RuntimeState state)
    {
        switch (state)
        {
            case RuntimeState.Victory:
                _isDead = true;
                StopAllCoroutines();
                _enemy.ReturnToPool();
                break;
            default:
                break;
        }
    }
    public void TakeDamage(DamageData damageData)
    {
        TakeDamageLogic(damageData);
    }
    public void TakeDamage(DamageData damageData, float knockback)
    {
        TakeDamageLogic(damageData);
        _enemyMovement.KnockBack(knockback);
    }
    private void TakeDamageLogic(DamageData damageData)
    {
        if (_isDead) 
        {
            return; 
        }
        if (!_isAbleToAttack && !damageData.IgnoreInvincibility)
        {
            return;
        }
        int totalDamage = damageData.Amount - _resistance;
        if (totalDamage <= 0)
        {
            return;
        }
        _health -= totalDamage;
        _health = Mathf.Clamp(_health, 0, _maxHealth);
        OnHealthChanged?.Invoke(_health, _maxHealth);

        if (_health == 0)
        {
            Death();
            return;
        }
        if (isActiveAndEnabled)
        {
            if (!damageData.IgnoreInvincibility)
            {
            StartCoroutine(InvincibilityFrames());
            }
        }
    }
    private IEnumerator InvincibilityFrames()
    {
        _isAbleToAttack = false;
        yield return new WaitForSeconds(0.2f);
        _isAbleToAttack = true;
    }
    public void RestoreHP(int healing) 
    {
        _health += healing;
        _health = Math.Clamp(_health, 0, _maxHealth);
        OnHealthChanged?.Invoke(_health, _maxHealth);
    }
    public void RestoreHP(float percent)
    {
        _health += (int)((float)_maxHealth * percent);
        _health = Math.Clamp(_health, 0, _maxHealth); 
        OnHealthChanged?.Invoke(_health, _maxHealth);
    }
    public void UpdateHP()
    {
        OnHealthChanged?.Invoke(_health, _maxHealth);
    }
    public void Death()
    {
        if (_isDead)
            return;

        _isDead = true;
        StopAllCoroutines();
        DropGem();
        AccrueCoins();
        if (TryGetComponent<BossBehaviour>(out BossBehaviour bossBehaviour))
        {
            Debug.Log("Exist component");
            bossBehaviour.OnBossDied(_enemy.ReturnToPool);
        }
        else
        {
            _enemy.ReturnToPool();
        }
    }
    public void ResetHealth()
    {
        _isDead = false;
        _maxHealth = _basicStatsEnemySO.basicStats.BasicMaxHealth;
        RestoreHP(1f);
    }
    private void DropGem()
    {
        if (_basicStatsEnemySO != null)
        {
            Vector2 ExpRange = _basicStatsEnemySO.basicStats.ExpForKillingRange;
            int ExpForKilling = Mathf.RoundToInt(UnityEngine.Random.Range(ExpRange.x, ExpRange.y));
            Gem gem = _gemPool.GetGem();
            if (gem != null)
            {
                gem.Initialize(ExpForKilling, transform.position, _gemPool);
            }
        }
    }
    private void AccrueCoins()
    {
        if (_basicStatsEnemySO != null)
        {
            Vector2 CoinsRange = _basicStatsEnemySO.basicStats.CoinsForKillingRange;
            int CoinsForKilling = Mathf.RoundToInt(UnityEngine.Random.Range(CoinsRange.x, CoinsRange.y));
            CoinsManager.instance.AddCoins(CoinsForKilling);
        }
    }
}
