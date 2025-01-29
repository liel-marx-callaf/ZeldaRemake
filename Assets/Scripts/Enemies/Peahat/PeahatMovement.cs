using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PeahatMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    // [SerializeField, Range(0.1f, 5f)] private float moveSpeed = 1f;
    [SerializeField, Range(3f, 10f)] private float minMoveDuration = 5f;
    [SerializeField, Range(5f, 20f)] private float maxMoveDuration = 15f;
    private float _moveDuration;

    [Header("Idle Settings")]
    [SerializeField, Range(0.1f, 5f)] private float minIdleDuration = 1f;
    [SerializeField, Range(1f, 8f)] private float maxIdleDuration = 3f;
    private float _idleDuration;
    
    [Header("Transitions")]
    // [SerializeField, Range(0f, 5f)] private float transitionSpeed = 1f;
    [SerializeField] private float edgeBuffer = 1f;
    
    
    private Vector2 _initialPosition;
    private Vector2 _topLeftBorder;
    private Vector2 _bottomRightBorder;
    private Vector2 _movementDirection;
    private DirectionsEnum _currentDirectionEnum;
    private Rigidbody2D _rb;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
        _currentDirectionEnum = DirectionsEnum.Up;
        _initialPosition = transform.position;
        _movementDirection = DirectionVector.GetDirectionToVector(_currentDirectionEnum);
    }
    
    private IEnumerator MovementCoroutine()
    {
        while (true)
        {
            _moveDuration = GetRandomMoveDuration();
            _idleDuration = GetRandomIdleDuration();
            SetMovementDirection(ChooseRandomMovementDirection());
            yield return Move();
            yield return Idle();
        }
    }

    private object Idle()
    {
        throw new NotImplementedException();
    }

    private object Move()
    {
        throw new NotImplementedException();
    }


    private DirectionsEnum ChooseRandomMovementDirection()
    {
        return (DirectionsEnum) Random.Range(1, 8);
    }

    private void SetMovementDirection(DirectionsEnum direction)
    {
        _movementDirection = DirectionVector.GetDirectionToVector(direction);
    }
    
    public void SetTopLeftBorder(Vector2 topLeftBorder)
    {
        _topLeftBorder = topLeftBorder;
    }
    
    public void SetBottomRightBorder(Vector2 bottomRightBorder)
    {
        _bottomRightBorder = bottomRightBorder;
    }
    
    private float GetRandomMoveDuration()
    {
        return Random.Range(minMoveDuration, maxMoveDuration);
    }
    
    private float GetRandomIdleDuration()
    {
        return Random.Range(minIdleDuration, maxIdleDuration);
    }

    private bool GoingToPassBorder()
    {
        Vector2 nextPosition = (Vector2) transform.position + _movementDirection;
        return nextPosition.x < _topLeftBorder.x + edgeBuffer || nextPosition.x > _bottomRightBorder.x - edgeBuffer ||
               nextPosition.y > _topLeftBorder.y - edgeBuffer || nextPosition.y < _bottomRightBorder.y + edgeBuffer;
    }
    
}
