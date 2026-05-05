using NUnit.Framework;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(Rigidbody2D))]
public class BulletBehaviour : MonoBehaviour, IPoolable
{
    [SerializeField] private LayerMask _layerMask;
    private Rigidbody2D _rb;
    private int _penetration = 0;
    private float _speed;
    private DamageData _damageData;
    private ProjectilePool _projectilePool;
    private bool _isAbleToReturn = false;
    private List<NegativeEffectData> _effectsList;
    private Vector2 _startScale;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startScale = transform.localScale;
    }
    public void Initialize(List<NegativeEffectData> list, GameObject shooter, Vector2 pos, Transform playerTransform, DamageData damageData, int penetration, float speed, ProjectilePool pool, float scaleMultiplier = 1)
    {
        _effectsList = list;
        transform.position = pos;
        _penetration = penetration;
        _speed = speed;
        _damageData = damageData;
        _projectilePool = pool;
        float angleDeg = playerTransform.rotation.eulerAngles.z;
        float rad = (angleDeg) * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        _rb.linearVelocity = dir * _speed;
        transform.localScale = new Vector2(_startScale.x * scaleMultiplier, _startScale.y * scaleMultiplier);
        StopAllCoroutines();
        StartCoroutine(TimerToReturnToPool());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _layerMask) == 0 || collision.isTrigger == true)
        {
            return;
        }
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(_damageData);
            if (_effectsList != null && _effectsList.Count > 0)
            {
                if (collision.TryGetComponent(out EffectController effectController))
                {
                    foreach (var effect in _effectsList)
                    {
                        effectController.AddStatus(effect);
                    }
                }
            }
            if (_penetration <= 0){
                StopAllCoroutines();
                ReturnToPool();
            }
            else
                _penetration--;
        }
    }
    public void OnSpawnFromPool()
    {
        _isAbleToReturn = true;
        _rb.linearVelocity = Vector2.zero;
    }
    public void OnReturnToPool()
    {
        StopAllCoroutines();
        transform.localScale = _startScale;
        _rb.linearVelocity = Vector2.zero;
    }
    private void ReturnToPool()
    {
        if (_isAbleToReturn){
            _isAbleToReturn = false;
            _projectilePool.ReturnProjectile(this);
        }
    }
    private IEnumerator TimerToReturnToPool()
    {
        yield return new WaitForSeconds(2);
        ReturnToPool();
    }
}
