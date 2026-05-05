using System;
using UnityEngine;
using System.Collections;
public class StatusBasicData
{
    public string StatusName { get; private set; }

    public StatusBasicData (string statusName)
    {
        StatusName = statusName;
    }
} 

public abstract class Status : MonoBehaviour
{
    [SerializeField] private StatusBasicData _negativeStatusBasicData;

    protected IEnumerator RemoveAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this);
    }
    public abstract void Initialize(NegativeEffectData negativeEffectData);
}
