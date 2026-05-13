using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    Dictionary<StatusEffectType, Status> _activeEffects = new();
    /// <summary>
    /// Накладывает эффект отравления на объект нанося урон типа заданого в <paramref name="damageData"/>.
    /// </summary>
    /// <param name="damageData">Урон за тик</param>
    public void AddStatus(NegativeEffectData effectData)
    {
        if (!gameObject.activeInHierarchy)
            return;
        if (_activeEffects.TryGetValue(effectData.EffectType, out Status existingEffect))
        {
            if (effectData.EffectType == StatusEffectType.Burn)
            {
                return;
            }
            RemoveEffect(effectData.EffectType);
            Destroy(existingEffect);
        }
        

        Status newEffect = CreateEffect(effectData);
        if (newEffect == null)
        {
            return;
        }

        _activeEffects.Add(effectData.EffectType, newEffect);
        newEffect.Initialize(effectData);
    }
    public void ApplyEffect<T>() where T : Status
    {
        if (!gameObject.activeInHierarchy)
            return;
        gameObject.AddComponent<T>();
    }
    public void RemoveEffect(StatusEffectType effectType)
    {
        if (_activeEffects.TryGetValue(effectType, out Status effect))
        {
            _activeEffects.Remove(effectType);
            Destroy(effect);
        }
    }
    private Status CreateEffect(NegativeEffectData effectData)
    {
        return effectData.EffectType switch
        {
            StatusEffectType.Poison => gameObject.AddComponent<PoisonEffect>(),
            StatusEffectType.Burn => gameObject.AddComponent<BurnEffect>(),
            //StatusEffectType.Stun => gameObject.AddComponent<StunEffect>(),
            //StatusEffectType.Slow => gameObject.AddComponent<SlowEffect>(),
            _ => null
        };
    }
    public void ResetEffects()
    {
        foreach (var effect in _activeEffects.Values)
        {
            if (effect != null)
                Destroy(effect);
        }

        _activeEffects.Clear();
    }
}
