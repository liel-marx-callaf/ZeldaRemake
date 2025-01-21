using System;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Splines.Interpolators;
using IPoolable = Pool.IPoolable;
using Random = UnityEngine.Random;


public class TektiteMovement : MonoBehaviour , IPoolable
{
    private static readonly int Jump1 = Animator.StringToHash("Jump");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Load = Animator.StringToHash("Load");
    [SerializeField] private Vector2 topLeftBorder;
    [SerializeField] private Vector2 bottomRightBorder;

    [SerializeField, Range(0f, 30f)] private float maxWaitBeforeSpawn = 9f;
    [SerializeField, Range(0f, 30f)] private float maxJumpInterval = 10f;
    [SerializeField, Range(0f, 5f)] private float maxLoadDuration = 3f;
    [SerializeField, Range(1f, 12f)] private float jumpRange = 5f;
    [SerializeField, Range(1f, 5f)] private float jumpDuration = 2f;
    private Vector3 _initialPosition;
    private Vector2 _topLeftBorder;
    private Vector2 _bottomRightBorder;
    private float _xDestination;
    private float _yDestination;
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _isDead = false;
    private float _jumpRange;
    private float elapsedTimeFromLastJump = 0;

    
    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _initialPosition = transform.position;
        _animator.speed = 0;
        _topLeftBorder = topLeftBorder;
        _bottomRightBorder = bottomRightBorder;
        StartCoroutine(MovementCoroutine());
    }

    private IEnumerator MovementCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(0f, maxWaitBeforeSpawn));
        _animator.speed = 1;
        // float elapsedTimeFromLastJump = 0;
        while (!_isDead)
        {
            
            float waitTime = Random.Range(jumpDuration, maxJumpInterval);
            while (waitTime > 0)
            {
                Debug.Log(elapsedTimeFromLastJump);
                // waitTime -= Time.deltaTime;
                // elapsedTimeFromLastJump += Time.deltaTime;
                _animator.SetTrigger(Load);
                float loadTime = Random.Range(0, maxLoadDuration);
                waitTime -= loadTime;
                elapsedTimeFromLastJump += loadTime;
                yield return new WaitForSeconds(loadTime);
            }

            // elapsedTimeFromLastJump += ;
                if (elapsedTimeFromLastJump >= maxJumpInterval)
                {
                    elapsedTimeFromLastJump = 0;
                    Jump();
                }
                yield return null;
            
        }
    }

    private void Jump()
    {
        while(_jumpRange == 0) _jumpRange = Random.Range(-jumpRange, jumpRange);
        // _jumpRange = Random.Range(-jumpRange, jumpRange);
        _xDestination = _jumpRange + transform.position.x;
        _yDestination = _jumpRange + transform.position.y;
        _animator.SetTrigger(Load);
        _xDestination = Mathf.Clamp(_xDestination, _topLeftBorder.x, _bottomRightBorder.x);
        _yDestination = Mathf.Clamp(_yDestination, _bottomRightBorder.y, _topLeftBorder.y);
        StartCoroutine(JumpCoroutine());
        _jumpRange = 0;
    }

    
    private IEnumerator JumpCoroutine()
{
    float distance = Vector3.Distance(_initialPosition, new Vector3(_xDestination, _yDestination, 0));
    float adjustedJumpDuration = distance / jumpRange * jumpDuration; // Adjust jump duration based on distance

    yield return new WaitForSeconds(Random.Range(0f, maxJumpInterval));
    _animator.SetTrigger(Jump1);

    float elapsedTime = 0;
    while (elapsedTime < adjustedJumpDuration)
    {
        elapsedTime += Time.deltaTime;
        transform.position = Vector3.Slerp(_initialPosition, new Vector3(_xDestination, _yDestination, 0), elapsedTime / adjustedJumpDuration);
        yield return null;
    }
    _animator.SetTrigger(Idle);
    _animator.SetTrigger(Load);

    _initialPosition = transform.position;
}
    // private IEnumerator JumpCoroutine()
    // {
    //     float waitTime = Random.Range(0f, maxJumpInterval);
    //     yield return new WaitForSeconds(waitTime);
    //     _animator.SetTrigger(Jump1);
    //
    //     float elapsedTime = 0;
    //     while (elapsedTime < jumpDuration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         transform.position = Vector3.Slerp(_initialPosition, new Vector3(_xDestination, _yDestination, 0), elapsedTime / jumpDuration);
    //         yield return null;
    //     }
    //     _animator.SetTrigger(Idle);
    //     _animator.SetTrigger(Load);
    //     
    //     _initialPosition = transform.position;
    // }

    private IEnumerator FakeLoadCoroutine()
    {
        _animator.SetTrigger(Load);
        yield return new WaitForSeconds(Random.Range(0, maxLoadDuration));
    }
    public void SetTopLeftBorder(Vector2 newTopLeftBorder)
    {
        _topLeftBorder = newTopLeftBorder;
    }
    
    public void SetBottomRightBorder(Vector2 newBottomRightBorder)
    {
        _bottomRightBorder = newBottomRightBorder;
    }

    public void OnDeath()
    {
        _isDead = true;
        _rb.linearVelocity = Vector2.zero;
    }
    
    public void Reset()
    {
        _rb.linearVelocity = Vector2.zero;
    }
}
