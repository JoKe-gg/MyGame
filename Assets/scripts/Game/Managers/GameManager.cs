using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    [SerializeField] private ProjectilePool _pool;
    [SerializeField] private GemPool _gemPool;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private BossBarController _bossBarController;
    public ProjectilePool ProjectilePool => _pool;
    public GemPool GemPool => _gemPool;
    public EnemyPool EnemyPool => _enemyPool;
    public BossBarController BossBarController => _bossBarController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
