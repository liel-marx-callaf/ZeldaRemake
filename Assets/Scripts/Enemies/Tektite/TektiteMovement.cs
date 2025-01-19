using System;
using Pool;
using UnityEngine;
using System.Collections;
using UnityEngine.Splines.Interpolators;
using Random = UnityEngine.Random;


public class TektiteMovement : MonoBehaviour , IPoolable
{
    private static readonly int Jump1 = Animator.StringToHash("Jump");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Load = Animator.StringToHash("Load");
    [SerializeField] private Vector2 topLeftBorder;
    [SerializeField] private Vector2 bottomRightBorder;
    
    [SerializeField, Range(0f, 10f)] private float maxJumpInterval = 3f;
    [SerializeField, Range(1f, 5f)] private float jumpRange = 2f;
    private Vector3 _initialPosition;
    private Vector2 _topLeftBorder;
    private Vector2 _bottomRightBorder;
    private float _xDestination;
    private float _yDestination;
    private Rigidbody2D _rb;
    private Animator _animator;
    
    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _initialPosition = transform.position;
        Jump();
    }



    private void Jump()
    {
        _xDestination = Random.Range(-jumpRange, jumpRange) + transform.position.x;
        _yDestination = Random.Range(-jumpRange, jumpRange) + transform.position.y;
        _animator.SetTrigger(Load);
        // _xDestination = Mathf.Clamp(_xDestination, _topLeftBorder.x, _bottomRightBorder.x);
        // _yDestination = Mathf.Clamp(_yDestination, _bottomRightBorder.y, _topLeftBorder.y);
        StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        float waitTime = Random.Range(1.5f, maxJumpInterval);
        yield return new WaitForSeconds(1f);
        _animator.SetTrigger(Jump1);

        float elapsedTime = 0;
        // _animator.SetTrigger(Jump1);
        while (elapsedTime < maxJumpInterval)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Slerp(_initialPosition, new Vector3(_xDestination, _yDestination, 0), elapsedTime / maxJumpInterval);
            yield return null;
        }
        _animator.SetTrigger(Idle);
        _animator.SetTrigger(Load);
        
        _initialPosition = transform.position;
    }

    public void SetTopLeftBorder(Vector2 newTopLeftBorder)
    {
        _topLeftBorder = newTopLeftBorder;
    }
    
    public void SetBottomRightBorder(Vector2 newBottomRightBorder)
    {
        _bottomRightBorder = newBottomRightBorder;
    }
    
    
    public void Reset()
    {
        _rb.linearVelocity = Vector2.zero;
    }
}
