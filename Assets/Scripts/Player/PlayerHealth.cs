using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int initialHealth = 6;
    private int _currentHealth;
    private Animator _animator;
    
    private void Awake()
    {
        _currentHealth = initialHealth;
        _animator = GetComponent<Animator>();
    }
    
    

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        throw new System.NotImplementedException();
    }
}
