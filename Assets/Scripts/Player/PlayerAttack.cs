using System.Collections;
using System.Collections.Generic;
using Audio;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        private InputPlayerActions _inputPlayerActions;

        // private Animator _animator;

        private PlayerAnimationControl _playerAnimationControl;
        private PlayerMovementController _playerMovementController;
        private PlayerInventory _playerInventory;

        // private RaycastHit2D[] hits;
        [Header("Player Attack Settings")]
        [SerializeField] private LayerMask hittableLayer;
        [Header("Sword Attack Settings")] 
        [SerializeField] private float swordAttackRange = 0.5f;
        [SerializeField] private int swordAttackDamage = 1;
        
        [Header("Bomb Throw Settings")] 
        [SerializeField] private float bombThrowDistance = 2f; // Distance from player
        [SerializeField] private Vector2 upThrowOffset = Vector2.zero;
        [SerializeField] private Vector2 downThrowOffset = Vector2.up;
        [SerializeField] private Vector2 leftThrowOffset = Vector2.up * 0.5f;
        [SerializeField] private Vector2 rightThrowOffset = Vector2.up * 0.5f;

        [Header("Raycast Offsets")] 
        [SerializeField, Range(-1f, 1f)] private float rightRayHeightOffset = 0.4f;
        [SerializeField, Range(-1f, 1f)] private float leftRayHeightOffset = 0.4f;
        [SerializeField, Range(-1f, 1f)] private float downRayHorizontalOffset = -0.4f;
        [SerializeField, Range(-1f, 1f)] private float upRayHorizontalOffset = -0.4f;

        [Header("Audio")] 
        [SerializeField] private string attackSoundName = "LOZ_Sword_Slash";
        [SerializeField, Range(0f, 1f)] private float attackSoundVolume = 0.5f;
        [SerializeField] private string bombThrowSound = "LOZ_Bomb_Throw";
        [SerializeField, Range(0f, 1f)] private float bombThrowVolume = 0.7f;

        private Vector2 _rightRayOrigin;
        private Vector2 _leftRayOrigin;
        private Vector2 _upRayOrigin;
        private Vector2 _downRayOrigin;
        
        private bool _playerFreeze = false;
        private bool _playerHit = false;
        private bool _playerDead = false;
        private float _stunDuration = 0.5f;

        private void Awake()
        {
            // _animator = GetComponent<Animator>();
            _playerAnimationControl = GetComponent<PlayerAnimationControl>();
            _inputPlayerActions = new InputPlayerActions();
            _playerMovementController = GetComponent<PlayerMovementController>();
            _playerInventory = GetComponent<PlayerInventory>();
            _rightRayOrigin = Vector2.right * 0.5f + Vector2.up * rightRayHeightOffset;
            _leftRayOrigin = Vector2.left * 0.5f + Vector2.up * leftRayHeightOffset;
            _upRayOrigin = Vector2.up + Vector2.right * upRayHorizontalOffset;
            _downRayOrigin = Vector2.zero + Vector2.right * downRayHorizontalOffset;
        }

        private void OnEnable()
        {
            _inputPlayerActions.Player.ActionA.performed += OnActionA;
            _inputPlayerActions.Player.ActionA.Enable();
            _inputPlayerActions.Player.ActionB.performed += OnActionB;
            _inputPlayerActions.Player.ActionB.Enable();
            MyEvents.TogglePlayerFreeze += OnTogglePlayerFreeze;
            MyEvents.PlayerHit += OnPlayerHit;
            MyEvents.PlayerDeath += OnPlayerDeath;
        }

        private void OnDisable()
        {
            _inputPlayerActions.Player.ActionA.performed -= OnActionA;
            _inputPlayerActions.Player.ActionA.Disable();
            _inputPlayerActions.Player.ActionB.performed -= OnActionB;
            _inputPlayerActions.Player.ActionB.Disable();
            MyEvents.TogglePlayerFreeze -= OnTogglePlayerFreeze;
            MyEvents.PlayerHit -= OnPlayerHit;
            MyEvents.PlayerDeath -= OnPlayerDeath;
        }

        private void OnPlayerDeath()
        {
            _playerDead = true;
            
        }

        private void OnPlayerHit(int obj)
        {
            if(_playerDead) return;
            StartCoroutine(PlayerHitCoroutine());
        }

        private IEnumerator PlayerHitCoroutine()
        {
            _playerHit = true;
            yield return new WaitForSeconds(_stunDuration);
            _playerHit = false;
        }

        private void OnActionA(InputAction.CallbackContext context)
        {
            if(_playerDead) return;
            if(_playerFreeze) return;
            if(_playerHit) return;
            _playerAnimationControl.SetSwordAttack();
            _playerMovementController.Attacking();
            SwordAttack();
        }

        private void OnActionB(InputAction.CallbackContext context)
        {
            if(_playerDead) return;
            if(_playerFreeze) return;
            if(_playerHit) return;
            if (_playerInventory.GetBombCount() <= 0) return;
            var bomb = BombPool.Instance.Get();
            var lastDir = _playerMovementController.LastFacingDirection;
            var throwOffset = Vector2.zero;
            if(lastDir == Vector2.down) throwOffset = downThrowOffset;
            if(lastDir == Vector2.up) throwOffset = upThrowOffset;
            if(lastDir == Vector2.left) throwOffset = leftThrowOffset;
            if(lastDir == Vector2.right) throwOffset = rightThrowOffset;
            bomb.ThrowBomb(transform.position, _playerMovementController.LastFacingDirection + throwOffset, bombThrowDistance);
            AudioManager.Instance.PlaySound(transform.position, bombThrowSound, bombThrowVolume);
            MyEvents.PlayerUseBomb?.Invoke(1);
        }

        private void OnTogglePlayerFreeze()
        {
            _playerFreeze = !_playerFreeze;
        }

        private void SwordAttack()
        {
            AudioManager.Instance.PlaySound(this.transform.position, attackSoundName, attackSoundVolume);
            _rightRayOrigin = Vector2.right * 0.5f + Vector2.up * rightRayHeightOffset;
            _leftRayOrigin = Vector2.left * 0.5f + Vector2.up * leftRayHeightOffset;
            _upRayOrigin = Vector2.up + Vector2.right * upRayHorizontalOffset;
            _downRayOrigin = Vector2.zero + Vector2.right * downRayHorizontalOffset;
            Vector2 lastDir = _playerMovementController.LastFacingDirection;
            Vector2 playerPos = transform.position;
            Vector2 boxSize = Vector2.one * 0.3f;
            RaycastHit2D[] hits = null;

            if (lastDir == Vector2.down)
            {
                hits = Physics2D.BoxCastAll(_downRayOrigin + playerPos, boxSize, 0, Vector2.down, swordAttackRange,
                    hittableLayer);

                // used for debugging
                Helper.BoxCastDrawer.Draw(hits.Length > 0 ? hits[0] : default, _downRayOrigin + playerPos, boxSize,
                    0f, Vector2.down, swordAttackRange);
                // Debug.Log("Down");
            }
            else if (lastDir == Vector2.up)
            {
                hits = Physics2D.BoxCastAll(_upRayOrigin + playerPos, boxSize, 0, Vector2.up, swordAttackRange,
                    hittableLayer);

                // used for debugging
                Helper.BoxCastDrawer.Draw(hits.Length > 0 ? hits[0] : default, _upRayOrigin + playerPos, boxSize, 0f,
                    Vector2.up, swordAttackRange);
                // Debug.Log("Up");
            }
            else if (lastDir == Vector2.left)
            {
                hits = Physics2D.BoxCastAll(_leftRayOrigin + playerPos, boxSize, 0, Vector2.left, swordAttackRange,
                    hittableLayer);

                // used for debugging
                Helper.BoxCastDrawer.Draw(hits.Length > 0 ? hits[0] : default, _leftRayOrigin + playerPos, boxSize,
                    0f, Vector2.left, swordAttackRange);
                // Debug.Log("Left");
            }
            else if (lastDir == Vector2.right)
            {
                hits = Physics2D.BoxCastAll(_rightRayOrigin + playerPos, boxSize, 0, Vector2.right, swordAttackRange,
                    hittableLayer);

                // used for debugging
                Helper.BoxCastDrawer.Draw(hits.Length > 0 ? hits[0] : default, _rightRayOrigin + playerPos, boxSize,
                    0f, Vector2.right, swordAttackRange);
                // Debug.Log("Right");
            }

            Debug.Log($"Number of hits: {hits?.Length ?? 0}");

            ApplyHit(hits);
        }

        private void ApplyHit(RaycastHit2D[] hits)
        {
            if (hits == null) return;
            var hitEnemies = new HashSet<GameObject>();

            foreach (var hit in hits)
            {
                if (hit.collider == null) continue;
                if (hit.collider.isTrigger && hit.collider.CompareTag("Enemy"))
                {
                    var enemy = hit.collider.gameObject;
                    if (!hitEnemies.Contains(enemy))
                    {
                        Debug.Log("Hit: " + hit.collider.name);
                        hitEnemies.Add(enemy);
                        enemy.GetComponent<EnemyHealth>()?.TakeDamage(swordAttackDamage);
                    }
                }

                if (!hit.collider.CompareTag("Item")) continue;
                var rupee = hit.collider.TryGetComponent(typeof(RupeePickup), out var rupeePickup);
                var heart = hit.collider.TryGetComponent(typeof(HeartPickup), out var heartPickup);
                if (rupee)
                {
                    Debug.Log("Rupee Picked Up");
                    ((RupeePickup)rupeePickup).Pickup();
                }

                if (heart)
                {
                    Debug.Log("Heart Picked Up");
                    ((HeartPickup)heartPickup).Pickup();
                }
            }
            // in case I add more items
        }
    }
}