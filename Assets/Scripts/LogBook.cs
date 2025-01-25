using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LogBook : MonoBehaviour
{
    [SerializeField] private GameObject _logBook;
    
    // private InputPlayerActions _inputPlayerActions;
    // private InputAction _logBookAction;
    private bool _isLogBookOpen;
    
    private void Start()
    {
        // _inputPlayerActions = new InputPlayerActions();
        _logBook.SetActive(false);
    }

    private void OnEnable()
    {
        // _logBookAction = _inputPlayerActions.Player.ActionSelect;
        // _logBookAction.Enable();
        // _logBookAction.performed += OnSelectPerformed;
    }
    
    private void OnDisable()
    {
        // _logBookAction.performed -= OnSelectPerformed;
        // _logBookAction.Disable();
    }
    
    private void OnSelectPerformed(InputAction.CallbackContext obj)
    {
        if (_isLogBookOpen)
        {
            EnterLogBook();
        }
        else
        {
            ExitLogBook();
        }
    }

    public void EnterLogBook()
    {
        _logBook.SetActive(true);
        Time.timeScale = 0;
    }

    public void ExitLogBook()
    {
        _logBook.SetActive(false);
        Time.timeScale = 1;
    }

}
