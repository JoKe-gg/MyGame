using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemySpawnSO", menuName = "Scriptable Objects/EnemySpawnSO")]
public class EnemySpawnSO : ScriptableObject
{
    [SerializeField] private string _arena = "first";
    [SerializeField] private List<EnemySpawnData> _enemySpawnDatas;

    public string Arena => _arena;
    public List<EnemySpawnData> EnemySpawnDatas => _enemySpawnDatas;
}
