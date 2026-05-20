using UnityEngine;

[CreateAssetMenu(fileName = "BasicStatsEnemySO", menuName = "Scriptable Objects/NewBasicStatsEnemySO")]
public class BasicStatsEnemySO : ScriptableObject
{
    public BasicStatsEnemy basicStats;
    private void OnValidate()
    {
        basicStats.OnValidate();
    }
}
