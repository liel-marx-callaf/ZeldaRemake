using Interfaces;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IHasHealth
    {
        [Header("Health Settings")]
        [SerializeField] private int initialHealth = 8;
        [SerializeField, Range(0, 4)] private int lowHealthThreshold = 2;
        private int _currentHealth;
        private int _maxHealth;

        [Header("Audio")]
        [SerializeField] private string hitSoundName = "LOZ_Link_Hurt";
        [SerializeField, Range(0f, 1f)] private float hitSoundVolume = 1f;
        [SerializeField] private string lowHealthSoundName = "LOZ_LowHealth";
        [SerializeField, Range(0f, 1f)] private float lowHealthSoundVolume = 1f;
        private bool _isLowHealthPlaying = false;
        [SerializeField] private string deathSoundName = "LOZ_Link_Die";
        [SerializeField, Range(0f, 1f)] private float deathSoundVolume = 1f;
    
        private void Awake()
        {
            _currentHealth = initialHealth;
            _maxHealth = initialHealth;
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
            Vector3 position = transform.position;
            AudioManager.Instance.PlaySound(position,hitSoundName, hitSoundVolume);
            _currentHealth -= damage;
            CheckLowHealthSound();
            UIManager.Instance.UpdateCurrentHealthUI(_currentHealth);
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void CheckLowHealthSound()
        {
            if (_currentHealth <= lowHealthThreshold && !_isLowHealthPlaying)
            {
                AudioManager.Instance.PlaySound(transform.position, lowHealthSoundName, lowHealthSoundVolume, 1f, true);
                _isLowHealthPlaying = true;
            }
            else if (_currentHealth > lowHealthThreshold && _isLowHealthPlaying)
            {
                AudioManager.Instance.StopSound(lowHealthSoundName);
                _isLowHealthPlaying = false;
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
            CheckLowHealthSound();
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
            AudioManager.Instance.PlaySound(transform.position, deathSoundName, deathSoundVolume);
        }
    }
}
