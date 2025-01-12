using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    private InputPlayerActions _inputPlayerActions;
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _inputPlayerActions = new InputPlayerActions();
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
        _animator.SetTrigger(Attack);
    }
    
    private void OnActionB(InputAction.CallbackContext context)
    {
        // _animator.SetTrigger(Attack);
    }
    
}
