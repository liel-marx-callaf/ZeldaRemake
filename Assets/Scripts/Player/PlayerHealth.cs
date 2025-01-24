using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IHasHealth
    {
        [SerializeField] private int initialHealth = 6;
        private int _currentHealth;
        private Rigidbody2D _rb;
        // private Animator _animator;
    
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentHealth = initialHealth;
            // _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            MyEvents.PlayerHit += TakeDamage;
            MyEvents.PlayerHeal += GainHealth;
        }
    
        private void OnDisable()
        {
            MyEvents.PlayerHit -= TakeDamage;
            MyEvents.PlayerHeal -= GainHealth;
        }


        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            // _rb.AddForce(hitPushback, ForceMode2D.Impulse);
            // Debug.Log($"Player took {damage} damage. Current health: {_currentHealth}");
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    
        private void GainHealth(int health)
        {
            _currentHealth += health;
            Debug.Log($"Player gained {health} health. Current health: {_currentHealth}");
        }

        private void Die()
        {
            // throw new System.NotImplementedException();
        }
    }
}
