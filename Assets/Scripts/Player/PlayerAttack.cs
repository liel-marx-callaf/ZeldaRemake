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
    }
    
    private void OnActionB(InputAction.CallbackContext context)
    {
        // _animator.SetTrigger(Attack);
    }
    public void OnAttackAnimationEnd()
    {
        _playerMovementController.IsAttacking = false;
    }
    
}
