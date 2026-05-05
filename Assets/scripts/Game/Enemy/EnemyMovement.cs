using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : Movable
{
    [SerializeField] private float _stunTime;
    private Enemy _enemy;
    private SpriteRenderer _spriteRenderer;
    private BasicStatsEnemySO _basicStatsEnemySO;
    private GameObject _player;
    private Vector3 _target;
    private float _movementSpeed = 1.0f;
    private Vector2 _lastFramePosition;
    private bool _isABleToMove = true;
    private bool _toKnockBack = false;
    private float _knockBackSpeed = 8f;
    private Coroutine _stunCoroutine;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null || _rigidBody == null)
        {
            enabled = false;
        }
        _enemy = GetComponent<Enemy>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _basicStatsEnemySO = _enemy.BasicStatsEnemySO;
        _movementSpeed = _basicStatsEnemySO.basicStats.BasicMovementSpeed;
        _lastFramePosition = transform.position;
    }
    public void Stun(float stunTime)
    {
        if (_stunCoroutine != null)
        {
            StopCoroutine(_stunCoroutine);
        }

        _isABleToMove = false;
        _stunCoroutine = StartCoroutine(Stunning(stunTime));
    }
    private IEnumerator Stunning(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        _isABleToMove = true;
    }
    public void KnockBack(float KnockBackSpeed) 
    {
        _toKnockBack = true;
        _knockBackSpeed = KnockBackSpeed;
    }
    private void FixedUpdate()
    {
        if (!_isABleToMove)
        {
            return;
        }
        MoveToPlayer();
    }
    private void Update()
    {
        if (!_isABleToMove)
        {
            return;
        }
        Flip();

    }
    void OnEnable()
    {
        InvokeRepeating(nameof(UrgentTarget), 0.1f, 0.2f);
    }
    void OnDisable()
    {
        CancelInvoke(nameof(UrgentTarget));
    }
    void UrgentTarget()
    {
        if (_player != null)
        {
            _target = _player.transform.position;
        }
        else return;
    }
    private void MoveToPlayer()
    {
        if (_player == null)
        {
            _rigidBody.linearVelocity = Vector3.zero;
            return;
        }
        Vector2 targetDir = -(transform.position - _target).normalized;
        if (_toKnockBack)
        {
            targetDir *= -1; 
            Stun(_stunTime);
            _rigidBody.linearVelocity = targetDir * _knockBackSpeed;
            _toKnockBack = false;
            return;
        }
        _rigidBody.linearVelocity = targetDir * _movementSpeed;
    }
    private void Flip()
    {
        if (_lastFramePosition.x == transform.position.x) { return; }
        _spriteRenderer.flipX = transform.position.x < _lastFramePosition.x;
        _lastFramePosition = transform.position;
    }
    public void ResetMovement()
    {
        _toKnockBack = false;
        _isABleToMove = true;
        _knockBackSpeed = 0;
        _stunCoroutine = null;
    }
}