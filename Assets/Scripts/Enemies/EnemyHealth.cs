using Interfaces;
using UnityEngine;
using Pool;

public class EnemyHealth : MonoBehaviour, IHasHealth, IPoolable
{
    private static readonly int Death = Animator.StringToHash("Death");
    [SerializeField] private int initialHealth = 1;
    private Animator _animator;
    private int _currentHealth;

    private void OnEnable()
    {
        _currentHealth = initialHealth;
        _animator = GetComponent<Animator>();
        MyEvents.ClearAllEnemies += Die;
    }
    
    private void OnDisable()
    {
        MyEvents.ClearAllEnemies -= Die;
    }

    public void Die()
    {
        _animator.SetTrigger(Death);
    }
    
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Reset()
    {
        _currentHealth = initialHealth;
    }
}
