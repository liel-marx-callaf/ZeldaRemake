using System.Collections;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float stunDuration = 0.2f;

    private bool _playerHit;
    private Rigidbody2D _rb;
    private InputPlayerActions _inputPlayerActions;
    private InputAction _moveAction;

    private Vector2 _moveDirection;
    
    private PlayerAnimationControl _playerAnimationControl;

    internal bool IsAttacking { get; set; }
    public Vector2 LastFacingDirection { get; private set; }
    private bool _playerFreeze = false;


    private void Awake()
    {
        _inputPlayerActions = new InputPlayerActions();
        LastFacingDirection = Vector2.down;
    }


    private void OnEnable()
    {
        _playerAnimationControl = GetComponent<PlayerAnimationControl>();
        _rb = GetComponent<Rigidbody2D>();
        _moveAction = _inputPlayerActions.Player.Move;
        _moveAction.Enable();
        _moveAction.performed += OnMovePerformed;
        _moveAction.canceled += OnMoveCanceled;
        
        // _playerAnimationControl.SetDirection(DirectionsEnum.Down);
        
        MyEvents.PlayerHit += OnPlayerHit;
        MyEvents.AreaSwitch += OnAreaSwitch;
        MyEvents.TogglePlayerFreeze += OnTogglePlayerFreeze;
    }


    private void OnDisable()
    {
        _moveAction.performed -= OnMovePerformed;
        _moveAction.canceled -= OnMoveCanceled;
        _moveAction.Disable();
        MyEvents.PlayerHit -= OnPlayerHit;
        MyEvents.AreaSwitch -= OnAreaSwitch;
        MyEvents.TogglePlayerFreeze -= OnTogglePlayerFreeze;
    }

    private void OnTogglePlayerFreeze()
    {
        _playerFreeze = !_playerFreeze;
    }

    private void OnAreaSwitch(int arg1, int arg2)
    {
        _rb.linearVelocity = Vector2.zero;
    }

    private void OnPlayerHit(int obj)
    {
        StartCoroutine(PlayerHitCoroutine());
    }

    private IEnumerator PlayerHitCoroutine()
    {
        _playerHit = true;
        yield return new WaitForSeconds(stunDuration);
        _playerHit = false;
    }

    private void FixedUpdate()
    {
        if(_playerFreeze) return;
        if (_playerHit) return;
        if (!IsAttacking)
        {
            Vector2? moveInput = _moveAction.ReadValue<Vector2>();
            PlayerMovement(moveInput);
        }
        // else
        // {
        //     _rb.linearVelocity = Vector2.zero;
        // }
    }

    private void PlayerMovement(Vector2? moveInput)
    {
        if (moveInput == null) return;
        if (moveInput != Vector2.zero)
        {
            _playerAnimationControl.SetAnimatorSpeed(1);
        }

        if (!IsAttacking)
        {
            _rb.linearVelocity = _moveDirection * speed;
            Debug.Log("linar velocity: " + _rb.linearVelocity);
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if(_playerFreeze) return;
        // if(_playerHit) return;
        // if(IsAttacking) return;
        Vector2 moveInput = context.ReadValue<Vector2>();
        Debug.Log("Move input: " + moveInput);
        //
        // _animator.ResetTrigger(MoveHorizontal);
        // _animator.ResetTrigger(MoveUp);
        // _animator.ResetTrigger(MoveDown);
        if (moveInput.y > 0)
        {
            if (!IsAttacking)
            {
                _playerAnimationControl.SetDirection(DirectionsEnum.Up);
                _playerAnimationControl.SetAnimatorSpeed(1);
            }

            _moveDirection = Vector2.up;
            // _spriteRenderer.flipX = false;
            // _animator.SetTrigger(MoveUp);
            // _animator.speed = 1;
            LastFacingDirection = _moveDirection;
        }
        else if (moveInput.y < 0)
        {
            if (!IsAttacking)
            {
                _playerAnimationControl.SetDirection(DirectionsEnum.Down);
                _playerAnimationControl.SetAnimatorSpeed(1);
            }

            _moveDirection = Vector2.down;
            LastFacingDirection = _moveDirection;
        }
        else if (moveInput.x > 0)
        {
            if (!IsAttacking)
            {
                _playerAnimationControl.SetDirection(DirectionsEnum.Right);
                _playerAnimationControl.SetAnimatorSpeed(1);
            }

            _moveDirection = Vector2.right;
            // _spriteRenderer.flipX = false;
            // _animator.SetTrigger(MoveHorizontal);
            // _animator.speed = 1;
            LastFacingDirection = _moveDirection;
        }
        else if (moveInput.x < 0)
        {
            if (!IsAttacking)
            {
                _playerAnimationControl.SetDirection(DirectionsEnum.Left);
                _playerAnimationControl.SetAnimatorSpeed(1);
            }

            _moveDirection = Vector2.left;
            // _spriteRenderer.flipX = true;
            // _animator.SetTrigger(MoveHorizontal);
            // _animator.speed = 1;
            LastFacingDirection = _moveDirection;
        }
        else
        {
            // _moveDirection = Vector2.zero;
            _playerAnimationControl.SetAnimatorSpeed(0);
            // _animator.speed = 0;
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // if(_playerHit) return;
        if (!IsAttacking)
        {
            // Vector2 moveInput = context.ReadValue<Vector2>();
            _moveDirection = Vector2.zero;
            _playerAnimationControl.SetAnimatorSpeed(0);
            // _animator.speed = 0;
        }
        else
        {
            _moveDirection = Vector2.zero;
        }
    }

    public void Attacking()
    {
        IsAttacking = true;
        _rb.linearVelocity = Vector2.zero;
    }
}