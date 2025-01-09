
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputPlayerActions _inputPlayerActions;
    
    private InputAction _moveAction;
    
    private void Awake()
    {
        _inputPlayerActions = new InputPlayerActions();
    }
    
    private void OnEnable()
    {
        _moveAction = _inputPlayerActions.Player.Move;
        _moveAction.Enable();
        _moveAction.performed += OnMovePerformed;
        // _moveAction.canceled += OnMoveCanceled;

        _inputPlayerActions.Player.ActionA.performed += OnActionA;
        _inputPlayerActions.Player.ActionA.Enable();
        _inputPlayerActions.Player.ActionB.performed += OnActionB;
        _inputPlayerActions.Player.ActionB.Enable();
        _inputPlayerActions.Player.ActionStart.performed += OnActionStart;
        _inputPlayerActions.Player.ActionStart.Enable();
        _inputPlayerActions.Player.ActionSelect.performed += OnActionSelect;
        _inputPlayerActions.Player.ActionSelect.Enable();
    }


    private void OnDisable()
    {
        _moveAction.performed -= OnMovePerformed;
        // _moveAction.canceled -= OnMoveCanceled;
        _moveAction.Disable();
        _inputPlayerActions.Player.ActionA.performed -= OnActionA;
        _inputPlayerActions.Player.ActionA.Disable();
        _inputPlayerActions.Player.ActionB.performed -= OnActionB;
        _inputPlayerActions.Player.ActionB.Disable();
        _inputPlayerActions.Player.ActionStart.performed -= OnActionStart;
        _inputPlayerActions.Player.ActionStart.Disable();
        _inputPlayerActions.Player.ActionSelect.performed -= OnActionSelect;
        _inputPlayerActions.Player.ActionSelect.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Debug.Log(moveInput);
    }
    
    
    private void OnActionA(InputAction.CallbackContext context)
    {
        Debug.Log("Action A");
    }
    
    
    private void OnActionB(InputAction.CallbackContext context)
    {
        Debug.Log("Action B");
    }
    
    private void OnActionStart(InputAction.CallbackContext context)
    {
        Debug.Log("Action Start");
    }


    private void OnActionSelect(InputAction.CallbackContext context)
    {
        Debug.Log("Action Select");
    }
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        if (moveInput.y > 0)
        {
            Debug.Log("Move Up performed");
        }
        else if (moveInput.y < 0)
        {
            Debug.Log("Move Down performed");
        }

        if (moveInput.x > 0)
        {
            Debug.Log("Move Right performed");
        }
        else if (moveInput.x < 0)
        {
            Debug.Log("Move Left performed");
        }
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        if (moveInput.y == 0)
        {
            Debug.Log("Move Up/Down canceled");
        }

        if (moveInput.x == 0)
        {
            Debug.Log("Move Left/Right canceled");
        }
    }
}
