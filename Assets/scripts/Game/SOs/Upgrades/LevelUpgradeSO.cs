using UnityEngine;
using System.Collections.Generic;

public enum StatType
{
    Damage,
    Health,
    Penetration,
    AttackSpeed,
    MoveSpeed,
    Regeneration,
}
[CreateAssetMenu(fileName = "LevelUpgrade", menuName = "Scriptable Objects/LevelUpgrade")]
public class LevelUpgradeSO : ScriptableObject
{
    [Header ("Level upgrade identification")]
    [SerializeField] private int _id;

    [Header ("Level upgrade data")]
    [SerializeField] private List<LevelUpdateData> _levelUpdateData;

    public int Id => _id;

    public List<LevelUpdateData> LevelUpdateData => _levelUpdateData;
}
