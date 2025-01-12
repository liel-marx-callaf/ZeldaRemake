using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    private Animator _animator;
    private Dictionary<string, int> _animatorParameters;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        CacheAnimatorParameters();
    }

    private void CacheAnimatorParameters()
    {
        _animatorParameters = new Dictionary<string, int>();
        foreach (AnimatorControllerParameter parameter in _animator.parameters)
        {
            _animatorParameters[parameter.name] = Animator.StringToHash(parameter.name);
        }
    }

    public void SetTrigger(string triggerName)
    {
        if (_animatorParameters.TryGetValue(triggerName, out int hash))
        {
            _animator.SetTrigger(hash);
        }
        else
        {
            Debug.LogWarning($"Trigger '{triggerName}' not found in animator parameters.");
        }
    }

    public void ResetTrigger(string triggerName)
    {
        if (_animatorParameters.TryGetValue(triggerName, out int hash))
        {
            _animator.ResetTrigger(hash);
        }
        else
        {
            Debug.LogWarning($"Trigger '{triggerName}' not found in animator parameters.");
        }
    }

    public void SetBool(string boolName, bool value)
    {
        if (_animatorParameters.TryGetValue(boolName, out int hash))
        {
            _animator.SetBool(hash, value);
        }
        else
        {
            Debug.LogWarning($"Bool '{boolName}' not found in animator parameters.");
        }
    }
    public void SetAnimatorSpeed(float speed)
    {
        _animator.speed = speed;
    }
}