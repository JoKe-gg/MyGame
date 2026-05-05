using Unity.VisualScripting;
using UnityEngine;

public class ShareBurn : MonoBehaviour
{
    private CircleCollider2D _circleCollider;
    private NegativeEffectData _negativeEffectData;
    private void Awake()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
    }
    public void Initialize(NegativeEffectData negativeEffectData)
    {
        _negativeEffectData = negativeEffectData;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_negativeEffectData == null || _negativeEffectData.EffectType != StatusEffectType.Burn)
        { 
            return;
        }
        if (collision.TryGetComponent(out IDamageable damageable))
        {
            if (collision.TryGetComponent(out EffectController effectController))
            {
                if (Random.Range(0, 1) == 0)
                {
                    effectController.AddStatus(_negativeEffectData);
                }
            }
        }
    }
}
