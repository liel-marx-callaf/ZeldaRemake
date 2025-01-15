using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    // private static readonly int Attack = Animator.StringToHash("Attack");
    private InputPlayerActions _inputPlayerActions;

    // private Animator _animator;
    private PlayerAnimationControl _playerAnimationControl;
    private PlayerMovementController _playerMovementController;
    private RaycastHit2D _hit;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    private Vector2 _downRayOrigin = Vector2.zero;
    private Vector2 _upRayOrigin = Vector2.up;
    private Vector2 _leftRayOrigin = Vector2.left * 0.5f + Vector2.up * 0.5f;
    private Vector2 _rightRayOrigin = Vector2.right * 0.5f + Vector2.up * 0.5f;


    private void Awake()
    {
        // _animator = GetComponent<Animator>();
        _playerAnimationControl = GetComponent<PlayerAnimationControl>();
        _inputPlayerActions = new InputPlayerActions();
        _playerMovementController = GetComponent<PlayerMovementController>();
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
        _playerAnimationControl.SetTrigger("Attack");
        _playerAnimationControl.SetAnimatorSpeed(1f);
        _playerMovementController.IsAttacking = true;
        SwordAttack();
    }

    private void OnActionB(InputAction.CallbackContext context)
    {
        // _animator.SetTrigger(Attack);
    }

    public void OnAttackAnimationEnd()
    {
        _playerMovementController.IsAttacking = false;
    }

    private void SwordAttack()
    {
        Vector2 lastDir = _playerMovementController.LastFacingDirection;
        Vector2 playerPos = transform.position;
        if (lastDir == Vector2.down)
        {
            _hit = Physics2D.BoxCast(_downRayOrigin + playerPos, Vector2.one*0.3f,  0, Vector2.down, attackRange, enemyLayer);
            Debug.DrawLine(_downRayOrigin + playerPos, _downRayOrigin + playerPos + Vector2.down * attackRange, Color.red, 2f);
            // Debug.DrawRay(_downRayOrigin+playerPos, Vector2.down*attackRange, Color.red, 2f);
            Debug.Log("Down");
        }
        else if (lastDir == Vector2.up)
        {
            _hit = Physics2D.BoxCast(_upRayOrigin + playerPos, Vector2.one*0.3f, 0, Vector2.up, attackRange, enemyLayer);
            // _hit = Physics2D.Raycast(_upRayOrigin + playerPos, Vector2.up, attackRange, enemyLayer);
            Debug.DrawLine(_upRayOrigin + playerPos, _upRayOrigin + playerPos + Vector2.up * attackRange, Color.red, 2f);
            // Debug.DrawRay(_upRayOrigin+playerPos, Vector2.up*attackRange, Color.red, 2f);
            Debug.Log("Up");
        }
        else if (lastDir == Vector2.left)
        {
            _hit = Physics2D.BoxCast(_leftRayOrigin + playerPos, Vector2.one*0.3f, 0, Vector2.left, attackRange, enemyLayer);
            // _hit = Physics2D.Raycast(_leftRayOrigin+playerPos, Vector2.left, attackRange, enemyLayer);
            Debug.DrawLine(_leftRayOrigin + playerPos, _leftRayOrigin + playerPos + Vector2.left * attackRange, Color.red, 2f);
            // Debug.DrawRay(_leftRayOrigin+playerPos, Vector2.left*attackRange, Color.red, 2f);
            Debug.Log("Left");
        }
        else if (lastDir == Vector2.right)
        {
            _hit = Physics2D.BoxCast(_rightRayOrigin + playerPos, Vector2.one*0.3f, 0, Vector2.right, attackRange, enemyLayer);
            // _hit = Physics2D.Raycast(_rightRayOrigin+playerPos, Vector2.right, attackRange, enemyLayer);
            Debug.DrawLine(_rightRayOrigin + playerPos, _rightRayOrigin + playerPos + Vector2.right * attackRange, Color.red, 2f);
            // Debug.DrawRay(_rightRayOrigin+playerPos, Vector2.right*attackRange, Color.red, 2f);
            Debug.Log("Right");
        }

        if (_hit.collider != null)
        {
            Debug.Log("Hit: " + _hit.collider.name);
            // Debug.DrawRay();
            // _hit.collider.GetComponent<EnemyHealth>().TakeDamage(1);
        }
    }
}