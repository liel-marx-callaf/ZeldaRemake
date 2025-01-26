using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        private InputPlayerActions _inputPlayerActions;

        // private Animator _animator;

        private PlayerAnimationControl _playerAnimationControl;
        private PlayerMovementController _playerMovementController;

        // private RaycastHit2D[] hits;
        [Header("Attack Settings")]
        [SerializeField] private float attackRange = 0.5f;
        [SerializeField] private int attackDamage = 1;
        [SerializeField] private LayerMask hittableLayer;
        
        [Header("Raycast Offsets")]
        [SerializeField, Range(-1f, 1f)] private float rightRayHeightOffset = 0.4f;
        [SerializeField, Range(-1f, 1f)] private float leftRayHeightOffset = 0.4f;
        [SerializeField, Range(-1f, 1f)] private float downRayHorizontalOffset = -0.4f;
        [SerializeField, Range(-1f, 1f)] private float upRayHorizontalOffset = -0.4f;
        
        [Header("Audio")]
        [SerializeField] private string attackSoundName = "LOZ_Sword_Slash";
        [SerializeField, Range(0f, 1f)] private float attackSoundVolume = 0.5f;

        private Vector2 _rightRayOrigin;
        private Vector2 _leftRayOrigin;
        private Vector2 _upRayOrigin;
        private Vector2 _downRayOrigin;

        private void Awake()
        {
            // _animator = GetComponent<Animator>();
            _playerAnimationControl = GetComponent<PlayerAnimationControl>();
            _inputPlayerActions = new InputPlayerActions();
            _playerMovementController = GetComponent<PlayerMovementController>();
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
        }

        private void OnDisable()
        {
            _inputPlayerActions.Player.ActionA.performed -= OnActionA;
            _inputPlayerActions.Player.ActionA.Disable();
            _inputPlayerActions.Player.ActionB.performed -= OnActionB;
            _inputPlayerActions.Player.ActionB.Disable();
        }

        private void OnActionA(InputAction.CallbackContext context)
        {
            _playerAnimationControl.SetSwordAttack();
            _playerMovementController.Attacking();
            SwordAttack();
        }

        private void OnActionB(InputAction.CallbackContext context)
        {
            // todo: implement bomb attack?
            // _animator.SetTrigger(Attack);
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
                hits = Physics2D.BoxCastAll(_downRayOrigin + playerPos, boxSize, 0, Vector2.down, attackRange,
                    hittableLayer);

                // used for debugging
                Helper.BoxCastDrawer.Draw(hits.Length > 0 ? hits[0] : default, _downRayOrigin + playerPos, boxSize,
                    0f, Vector2.down, attackRange);
                // Debug.Log("Down");
            }
            else if (lastDir == Vector2.up)
            {
                hits = Physics2D.BoxCastAll(_upRayOrigin + playerPos, boxSize, 0, Vector2.up, attackRange,
                    hittableLayer);

                // used for debugging
                Helper.BoxCastDrawer.Draw(hits.Length > 0 ? hits[0] : default, _upRayOrigin + playerPos, boxSize, 0f,
                    Vector2.up, attackRange);
                // Debug.Log("Up");
            }
            else if (lastDir == Vector2.left)
            {
                hits = Physics2D.BoxCastAll(_leftRayOrigin + playerPos, boxSize, 0, Vector2.left, attackRange,
                    hittableLayer);

                // used for debugging
                Helper.BoxCastDrawer.Draw(hits.Length > 0 ? hits[0] : default, _leftRayOrigin + playerPos, boxSize,
                    0f, Vector2.left, attackRange);
                // Debug.Log("Left");
            }
            else if (lastDir == Vector2.right)
            {
                hits = Physics2D.BoxCastAll(_rightRayOrigin + playerPos, boxSize, 0, Vector2.right, attackRange,
                    hittableLayer);

                // used for debugging
                Helper.BoxCastDrawer.Draw(hits.Length > 0 ? hits[0] : default, _rightRayOrigin + playerPos, boxSize,
                    0f, Vector2.right, attackRange);
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
                    if(!hitEnemies.Contains(enemy))
                    {
                        Debug.Log("Hit: " + hit.collider.name);
                        hitEnemies.Add(enemy);
                        enemy.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
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