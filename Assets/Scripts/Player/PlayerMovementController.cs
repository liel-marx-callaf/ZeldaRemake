using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using PDirection = PlayerDirectionEnum;

public class PlayerMovementController : MonoBehaviour
{
    // private static readonly int MoveHorizontal = Animator.StringToHash("MoveHorizontal");
    // private static readonly int MoveUp = Animator.StringToHash("MoveUp");
    // private static readonly int MoveDown = Animator.StringToHash("MoveDown");
    [SerializeField] private float speed = 4f;
    [SerializeField] private float stunDuration = 0.2f;

    private bool _playerHit;
    private Rigidbody2D _rb;
    private InputPlayerActions _inputPlayerActions;
    private InputAction _moveAction;
    private Vector2 _moveDirection;
    // private SpriteRenderer _spriteRenderer;
    // private Animator _animator;
    private PlayerAnimationControl _playerAnimationControl;
    
    internal bool IsAttacking {get; set;}
    public Vector2 LastFacingDirection {get; private set;}
    
    
    private void Awake()
    {
        _inputPlayerActions = new InputPlayerActions();
        LastFacingDirection = Vector2.down;
    }
    
    
    private void OnEnable()
    {
        // _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimationControl = GetComponent<PlayerAnimationControl>();
        // _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        // _playerAnimationControl.SetAnimatorSpeed(0);
        // _animator.speed = 0;
        _moveAction = _inputPlayerActions.Player.Move;
        _moveAction.Enable();
        _moveAction.performed += OnMovePerformed;
        _moveAction.canceled += OnMoveCanceled;
        MyEvents.PlayerHit += OnPlayerHit;
    }


    private void OnDisable()
    {
        _moveAction.performed -= OnMovePerformed;
        _moveAction.canceled -= OnMoveCanceled;
        _moveAction.Disable();
        MyEvents.PlayerHit -= OnPlayerHit;
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
        if(_playerHit) return;
        if (!IsAttacking)
        {
            Vector2? moveInput = _moveAction.ReadValue<Vector2>();
            PlayerMovement(moveInput);
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
        }
    }
    
    private void PlayerMovement(Vector2? moveInput)
    {
        if(moveInput == null) return;
        if(moveInput != Vector2.zero)
        {
            _playerAnimationControl.SetAnimatorSpeed(1);
        }
        _rb.linearVelocity = _moveDirection * speed;

    }
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        // if(_playerHit) return;
        // if(IsAttacking) return;
        Vector2 moveInput = context.ReadValue<Vector2>();
        //
        // _animator.ResetTrigger(MoveHorizontal);
        // _animator.ResetTrigger(MoveUp);
        // _animator.ResetTrigger(MoveDown);
        if (moveInput.y > 0)
        {
            if (!IsAttacking)
            {
                _playerAnimationControl.SetDirection(PDirection.Up);
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
                _playerAnimationControl.SetDirection(PDirection.Down);
                _playerAnimationControl.SetAnimatorSpeed(1);
            }
            _moveDirection = Vector2.down;
            // _playerAnimationControl.SetDirection(PDirection.Down);
            // _playerAnimationControl.SetAnimatorSpeed(1);
            // _spriteRenderer.flipX = false;
            // _animator.SetTrigger(MoveDown);
            // _animator.speed = 1;
            LastFacingDirection = _moveDirection;
        }
        else if (moveInput.x > 0)
        {
            if (!IsAttacking)
            {
                _playerAnimationControl.SetDirection(PDirection.Right);
                _playerAnimationControl.SetAnimatorSpeed(1);
            }
            // _playerAnimationControl.SetDirection(PDirection.Right);
            // _playerAnimationControl.SetAnimatorSpeed(1);
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
                _playerAnimationControl.SetDirection(PDirection.Left);
                _playerAnimationControl.SetAnimatorSpeed(1);
            }
            // _playerAnimationControl.SetDirection(PDirection.Left);
            // _playerAnimationControl.SetAnimatorSpeed(1);
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
    // private void Pushback(Vector2 direction, float force)
    // {
    //     _rb.AddForce(direction * force, ForceMode2D.Impulse);
    // }
}
