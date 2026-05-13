using UnityEngine;
using UnityEngine.Events;

public class BossBehaviour : MonoBehaviour
{
    private EnemyHealth _enemyHealth;
    private void Awake()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
    }
    public void OnBossDied(UnityAction unityAction)
    {
        StateManager.instance.SetState(RuntimeState.Victory);
        unityAction();
    }
}
