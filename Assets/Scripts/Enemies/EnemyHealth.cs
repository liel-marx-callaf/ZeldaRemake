using Interfaces;
using UnityEngine;
using Pool;

public class EnemyHealth : MonoBehaviour, IHasHealth, IPoolable
{
    private static readonly int Death = Animator.StringToHash("Death");
    [SerializeField] private int initialHealth = 1;
    private Animator _animator;
    private int _currentHealth;
    private EnemyTypeEnum _enemyType;
    private bool _isInvincible = false;
    private Collider2D[] _colliders;
    private Rigidbody2D _rb;

    private void OnEnable()
    {
        _currentHealth = initialHealth;
        _animator = GetComponent<Animator>();
        _colliders = GetComponents<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        MyEvents.ClearAllEnemies += Die;
    }
    
    private void OnDisable()
    {
        MyEvents.ClearAllEnemies -= Die;
    }

    private void Die()
    {
        MyEvents.EnemyDied?.Invoke(_enemyType, transform.position);
        _animator.SetTrigger(Death);
        foreach (var enemyCollider in _colliders)
        {
            enemyCollider.enabled = false;
        }
        _rb.linearVelocity = Vector2.zero;
    }
    
    public void TakeDamage(int damage)
    {
        if (_isInvincible) return;
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Reset()
    {
        _currentHealth = initialHealth;
        foreach (var enemyCollider in _colliders)
        {
            enemyCollider.enabled = true;
        }
    }

    public void SetEnemyType(EnemyTypeEnum enemyType)
    {
        _enemyType = enemyType;
    }
    
    public void SetInvincibility(bool isInvincible)
    {
        _isInvincible = isInvincible;
    }
}
