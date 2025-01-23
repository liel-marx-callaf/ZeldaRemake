using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    private Animator _animator;
    private Dictionary<string, int> _animatorParameters;
    private Vector2 _direction = Vector2.down;

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

    public void SetDirection(DirectionsEnum directionEnum)
    {
        _direction = DirectionVector.GetDirectionVector(directionEnum);
        if ((_direction.x != 0 && _direction.y != 0) || (_direction.x == 0 && _direction.y == 0)) return;
        _animator.SetFloat(_animatorParameters["HorizontalMovement"], _direction.x);
        _animator.SetFloat(_animatorParameters["VerticalMovement"], _direction.y);
    }

    
    private Vector2? GetDirectionVector(DirectionsEnum directionEnum)   // made obsolete by DirectionVector.GetDirectionVector

    {
        switch (directionEnum)
        {
            case DirectionsEnum.Up:
                return Vector2.up;
            case DirectionsEnum.Down:
                return Vector2.down;
            case DirectionsEnum.Left:
                return Vector2.left;
            case DirectionsEnum.Right:
                return Vector2.right;
        }

        return null;
    }
}