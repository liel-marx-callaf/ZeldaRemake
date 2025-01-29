using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationControl : MonoBehaviour
    {
        private PlayerMovementController _playerMovementController;
        private Animator _animator;
        private Dictionary<string, int> _animatorParameters;
        private Vector2 _direction = Vector2.down;
        private SpriteRenderer _spriteRenderer;
        private float _stunDuration = 0.5f;
        private bool _isDead;

        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
            _playerMovementController = GetComponent<PlayerMovementController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            CacheAnimatorParameters();
            if(!_isDead)SetAnimatorSpeed(0);
            MyEvents.TogglePlayerFreeze += OnPlayerFreeze;
            MyEvents.PlayerDeath += OnPlayerDeath;
            MyEvents.PlayerHit += OnPlayerHit;
        }
        
        private void OnDisable()
        {
            MyEvents.TogglePlayerFreeze -= OnPlayerFreeze;
            MyEvents.PlayerDeath -= OnPlayerDeath;
            MyEvents.PlayerHit -= OnPlayerHit;
        }

        private void OnPlayerHit(int obj)
        {
            if(_isDead) return;
            StartCoroutine(PlayerHitCoroutine());
        }

        private IEnumerator PlayerHitCoroutine()
        {
            SetAnimatorSpeed(1);
            _animator.SetBool(_animatorParameters["IsHit"], true);
            yield return new WaitForSeconds(_stunDuration);
            _animator.SetBool(_animatorParameters["IsHit"], false);
            SetAnimatorSpeed(0);
        }

        private void OnPlayerDeath()
        {
            _isDead = true;
            // StopCoroutine(PlayerHitCoroutine());
            StopAllCoroutines();
            _animator.SetTrigger(_animatorParameters["Death"]);
            _animator.speed = 1;
        }

        private void OnPlayerFreeze()
        {
            _animator.speed = 1;
        }

        private void CacheAnimatorParameters()
        {
            _animatorParameters = new Dictionary<string, int>();
            foreach (AnimatorControllerParameter parameter in _animator.parameters)
            {
                _animatorParameters[parameter.name] = Animator.StringToHash(parameter.name);
            }
        }

        public void SetAnimatorSpeed(float speed)
        {
            _animator.speed = speed;
        }

        public void SetDirection(DirectionsEnum directionEnum)
        {
            _direction = DirectionVector.GetDirectionToVector(directionEnum);
            if ((_direction.x != 0 && _direction.y != 0) || _direction == Vector2.zero) return;
            _animator.SetFloat(_animatorParameters["HorizontalMovement"], _direction.x);
            _animator.SetFloat(_animatorParameters["VerticalMovement"], _direction.y);
        }
        
        public void SetSwordAttack()
        {
            _animator.SetTrigger(_animatorParameters["Attack"]);
            _animator.speed = 1;
        }
        
        public void OnSwordAttackAnimationEnd()
        {
            _playerMovementController.IsAttacking = false;
            DirectionsEnum directionEnum = DirectionVector.GetVectorToDirection(_playerMovementController.LastFacingDirection);
            SetDirection(directionEnum);
        }
        
        public void TogglePlayerFreeze()
        {
            MyEvents.TogglePlayerFreeze?.Invoke();
        }
        
        public void SetAnimTrigger(string triggerName)
        {
            _animator.SetTrigger(_animatorParameters[triggerName]);
        }
        
        public void SetRenderPriority(int priority)
        {
            _spriteRenderer.sortingOrder = priority;
        }
        
        public void OnDeathAnimationEnd()
        {
            MyEvents.GameOver?.Invoke();
        }
        
    }
}