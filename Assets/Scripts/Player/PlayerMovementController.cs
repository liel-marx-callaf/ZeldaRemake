using UnityEngine;
using UnityEngine.InputSystem;

// todo: 4) public pushback function
// todo: 6) can change collider size depending on direction

public class PlayerMovementController : MonoBehaviour
{
    private static readonly int MoveHorizontal = Animator.StringToHash("MoveHorizontal");
    private static readonly int MoveUp = Animator.StringToHash("MoveUp");
    private static readonly int MoveDown = Animator.StringToHash("MoveDown");
    [SerializeField] private float speed = 5f;

    private Rigidbody2D _rb;
    private InputPlayerActions _inputPlayerActions;
    private InputAction _moveAction;
    private Vector2 _moveDirection;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    internal bool IsAttacking {get; set;}
    public Vector2 LastFacingDirection {get; private set;}
    
    
    private void Awake()
    {
        _inputPlayerActions = new InputPlayerActions();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        LastFacingDirection = Vector2.down;
    }
    
    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        _moveAction = _inputPlayerActions.Player.Move;
        _moveAction.Enable();
        _moveAction.performed += OnMovePerformed;
        _moveAction.canceled += OnMoveCanceled;
    }


    private void OnDisable()
    {
        _moveAction.performed -= OnMovePerformed;
        _moveAction.canceled -= OnMoveCanceled;
        _moveAction.Disable();
    }

    private void FixedUpdate()
    {
        if (!IsAttacking)
        {
            Vector2 moveInput = _moveAction.ReadValue<Vector2>();
            PlayerMovement(moveInput);
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
        }
    }
    
    private void PlayerMovement(Vector2 moveInput)
    {
        if(moveInput != Vector2.zero)
        {
            _animator.speed = 1;
        }
        _rb.linearVelocity = _moveDirection * speed;

    }
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        
        _animator.ResetTrigger(MoveHorizontal);
        _animator.ResetTrigger(MoveUp);
        _animator.ResetTrigger(MoveDown);
        if (moveInput.y > 0)
        {
            _moveDirection = Vector2.up;
            _spriteRenderer.flipX = false;
            _animator.SetTrigger(MoveUp);
            _animator.speed = 1;
            LastFacingDirection = _moveDirection;
        }
        else if (moveInput.y < 0)
        {
            _moveDirection = Vector2.down;
            _spriteRenderer.flipX = false;
            _animator.SetTrigger(MoveDown);
            _animator.speed = 1;
            LastFacingDirection = _moveDirection;
        }
        else if (moveInput.x > 0)
        {
            _moveDirection = Vector2.right;
            _spriteRenderer.flipX = false;
            _animator.SetTrigger(MoveHorizontal);
            _animator.speed = 1;
            LastFacingDirection = _moveDirection;
        }
        else if (moveInput.x < 0)
        {
            _moveDirection = Vector2.left;
            _spriteRenderer.flipX = true;
            _animator.SetTrigger(MoveHorizontal);
            _animator.speed = 1;
            LastFacingDirection = _moveDirection;
        }
        else
        {
            _moveDirection = Vector2.zero;
            _animator.speed = 0;
        }
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        if (!IsAttacking)
        {
            // Vector2 moveInput = context.ReadValue<Vector2>();
            _moveDirection = Vector2.zero;
            _animator.speed = 0;
        }
        else
        {
            _moveDirection = Vector2.zero;
        }
    }
    // public
}
