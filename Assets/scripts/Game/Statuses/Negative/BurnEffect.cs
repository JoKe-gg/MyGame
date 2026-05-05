using System.Collections;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BurnEffect : Status
{
    private IDamageable _damageable;
    private NegativeEffectData _effectData;
    private GameObject _shareColliderHolder;
    public void StartTimerToDestroy(float time) => StartCoroutine(RemoveAfterTime(time));
    private void Start()
    {
        _damageable = GetComponent<IDamageable>();
    }
    private void TakeBurnDamage()
    {
        if (_damageable != null)
        {
            _damageable.TakeDamage(_effectData.DamageData);
        }
    }
    private void OnDestroy()
    {
        Destroy(_shareColliderHolder);
    }
    public override void Initialize(NegativeEffectData negativeEffectPoison)
    {
        GameObject shareColliderHolder = Instantiate(new GameObject(), transform, false);
        CircleCollider2D collider = shareColliderHolder.AddComponent <CircleCollider2D>();
        collider.radius = 0.8f;
        ShareBurn shareBurn = shareColliderHolder.AddComponent<ShareBurn>();
        shareBurn.Initialize(negativeEffectPoison);
        _shareColliderHolder = shareColliderHolder;
        _effectData = negativeEffectPoison;

        StartCoroutine(BurnTick(0, negativeEffectPoison.IntervalBetweenTicks));
        StartCoroutine(RemoveAfterTime(negativeEffectPoison.TimeOfEffect));
    }
    protected IEnumerator BurnTick(float delay, float interval)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            yield return new WaitForSeconds(interval);
            TakeBurnDamage();
        }
    }
}
