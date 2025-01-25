using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IHasHealth
    {
        [SerializeField] private int initialHealth = 8;
        private int _currentHealth;
        private int _maxHealth;
        private Rigidbody2D _rb;
        // private Animator _animator;
    
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentHealth = initialHealth;
            _maxHealth = initialHealth;
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
            UIManager.Instance.UpdateCurrentHealthUI(_currentHealth);
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
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            UIManager.Instance.UpdateCurrentHealthUI(_currentHealth);
            Debug.Log($"Player gained {health} health. Current health: {_currentHealth}");
        }

        public int GetCurrentHealth()
        {
            return _currentHealth;
        }
        
        public int GetMaxHealth()
        {
            return _maxHealth;
        }
        private void Die()
        {
            // throw new System.NotImplementedException();
        }
    }
}
