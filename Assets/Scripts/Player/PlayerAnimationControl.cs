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

        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
            _playerMovementController = GetComponent<PlayerMovementController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            CacheAnimatorParameters();
            SetAnimatorSpeed(0);
            MyEvents.TogglePlayerFreeze += OnPlayerFreeze;
            MyEvents.PlayerDeath += OnPlayerDeath;
        }
        
        private void OnDisable()
        {
            MyEvents.TogglePlayerFreeze -= OnPlayerFreeze;
            MyEvents.PlayerDeath -= OnPlayerDeath;
        }

        private void OnPlayerDeath()
        {
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