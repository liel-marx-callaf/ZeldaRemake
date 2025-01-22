using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    private Animator _animator;
    private Dictionary<string, int> _animatorParameters;
    private Vector2? _direction = Vector2.down;

    private void OnEnable()
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

    public void SetAnimatorSpeed(float speed)
    {
        _animator.speed = speed;
    }

    public void SetDirection(PlayerDirectionEnum direction)
    {
        _direction = GetDirectionVector(direction);
        if (_direction.HasValue)
        {
            _animator.SetFloat(_animatorParameters["HorizontalMovement"], _direction.Value.x);
            _animator.SetFloat(_animatorParameters["VerticalMovement"], _direction.Value.y);
        }
    }

    private Vector2? GetDirectionVector(PlayerDirectionEnum direction)
    {
        switch (direction)
        {
            case PlayerDirectionEnum.Up:
                return Vector2.up;
            case PlayerDirectionEnum.Down:
                return Vector2.down;
            case PlayerDirectionEnum.Left:
                return Vector2.left;
            case PlayerDirectionEnum.Right:
                return Vector2.right;
        }

        return null;
    }
}