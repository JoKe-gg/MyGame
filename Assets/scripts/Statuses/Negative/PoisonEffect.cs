using System.Collections;
using UnityEngine;

public class PoisonEffect : Status
{
    private IDamageable _damageable;
    private NegativeEffectData _effectData;
    private Coroutine _coroutine;
    public void StartTimerToDestroy(float time) => StartCoroutine(RemoveAfterTime(time));
    private void Start()
    {
        _damageable = GetComponent<IDamageable>();
    }
    private void TakePoisonedDamage()
    {
        if (_damageable != null)
        {
            _damageable.TakeDamage(_effectData.DamageData);
        }
    }
    public override void Initialize(NegativeEffectData negativeEffectPoison)
    {
        _effectData = negativeEffectPoison;
        _coroutine = StartCoroutine(PoisonTick(0, negativeEffectPoison.IntervalBetweenTicks));
        StartCoroutine(RemoveAfterTime(negativeEffectPoison.TimeOfEffect));
    }
    private void OnDestroy()
    {
        _coroutine = null;
    }
    protected IEnumerator PoisonTick(float delay, float interval)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            TakePoisonedDamage();
            yield return new WaitForSeconds(interval);
        }
    }
}
