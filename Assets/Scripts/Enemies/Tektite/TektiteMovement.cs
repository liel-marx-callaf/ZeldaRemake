using UnityEngine;
using System.Collections;
using IPoolable = Pool.IPoolable;
using Random = UnityEngine.Random;


public class TektiteMovement : MonoBehaviour, IPoolable
{
    private static readonly int Jump1 = Animator.StringToHash("Jump");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Load = Animator.StringToHash("Load");
    [SerializeField] private Vector2 topLeftBorder;
    [SerializeField] private Vector2 bottomRightBorder;

    [SerializeField, Range(0f, 30f)] private float maxWaitBeforeSpawn = 9f;
    [SerializeField, Range(0f, 30f)] private float maxJumpInterval = 10f;
    [SerializeField, Range(0f, 5f)] private float maxLoadDuration = 3f;
    [SerializeField, Range(0f, 5f)] private float maxIdleDuration = 3f;
    [SerializeField, Range(1f, 12f)] private float jumpRange = 5f;
    [SerializeField, Range(0.01f, 5f)] private float jumpDuration = 1f;
    private Vector3 _initialPosition;
    private Vector3 _currentPosition;
    private Vector2 _topLeftBorder;
    private Vector2 _bottomRightBorder;
    private float _xDestination;
    private float _yDestination;
    private Rigidbody2D _rb;
    private Animator _animator;
    private bool _isDead;
    private bool _isJumping;
    private float _jumpRangeX;
    private float _jumpRangeY;
    private float _elapsedTimeFromLastJump;
    private float _jumpDuration;


    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _initialPosition = transform.position;
        _currentPosition = _initialPosition;
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
            float waitTime = Random.Range(0f, maxJumpInterval);
            while (waitTime > 0)
            {
                // if (_elapsedTimeFromLastJump >= maxJumpInterval)
                // {
                //     _elapsedTimeFromLastJump = 0;
                //     Jump();
                //     yield return null;
                // }
                // Debug.Log(_elapsedTimeFromLastJump);
                waitTime -= Time.deltaTime;
                _elapsedTimeFromLastJump += Time.deltaTime;
                if (!IsInAnimationState("TektiteLoad"))
                {
                    _animator.SetTrigger(Load);
                    // float loadTime = Random.Range(0, maxLoadDuration);
                    // waitTime -= loadTime;
                    // _elapsedTimeFromLastJump += loadTime;
                    // yield return new WaitForSeconds(loadTime);
                }
                float loadTime = Random.Range(0, maxLoadDuration);
                waitTime -= loadTime;
                _elapsedTimeFromLastJump += loadTime;
                yield return new WaitForSeconds(loadTime);
                
                if (Random.value < 0.2f)
                {
                    if (!IsInAnimationState("TektiteIdle"))
                    {
                        _animator.SetTrigger(Idle);
                    }
                    float idleTime = Random.Range(0, maxIdleDuration);
                    waitTime -= idleTime;
                    _elapsedTimeFromLastJump += idleTime;
                    yield return new WaitForSeconds(idleTime);
                }

                // if (Random.value < 0.5f)
                // {
                //     _elapsedTimeFromLastJump = 0;
                //     waitTime = 0;
                //     Jump();
                // }
                
                if (_elapsedTimeFromLastJump >= maxJumpInterval)
                {
                    _elapsedTimeFromLastJump = 0;
                    waitTime = 0;
                    Jump();
                    // yield return null;
                }
                if (_elapsedTimeFromLastJump >= maxJumpInterval)
                {
                    _elapsedTimeFromLastJump = 0;
                    // waitTime = 0;
                    Jump();
                    // yield return null;
                }   
            }
            yield return null;
        }
    }

    private void Jump()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        string currentStateName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        Debug.Log("Jumping and the state is: " + currentStateName);
        // _animator.ResetTrigger(Load);
        _animator.SetTrigger(Jump1);
        if(_isJumping) return;
        // _animator.ResetTrigger(Load);
        _animator.SetTrigger(Jump1);
        _isJumping = true;
        while (Mathf.Abs(_jumpRangeX) < 1) _jumpRangeX = Random.Range(-jumpRange, jumpRange);
        while (Mathf.Abs(_jumpRangeY) < 1) _jumpRangeY = Random.Range(-jumpRange, jumpRange);
        _xDestination = _jumpRangeX + transform.position.x;
        _yDestination = _jumpRangeY + transform.position.y;
        _xDestination = Mathf.Clamp(_xDestination, _topLeftBorder.x- 5, _bottomRightBorder.x + 5);
        _yDestination = Mathf.Clamp(_yDestination, _bottomRightBorder.y - 5, _topLeftBorder.y + 5);
        StartCoroutine(JumpCoroutine());
        _jumpRangeX = 0;
        _jumpRangeY = 0;
    }


    private IEnumerator JumpCoroutine()
{
    float distance = Vector3.Distance(_currentPosition, new Vector3(_xDestination, _yDestination, 0));
    _jumpDuration = distance / Mathf.Sqrt(_jumpRangeX * _jumpRangeX + _jumpRangeY * _jumpRangeY) * jumpDuration;
    Vector2 jumpDirection = new Vector2(_xDestination - _currentPosition.x, _yDestination - _currentPosition.y).normalized;
    float jumpForce = distance / _jumpDuration * _rb.mass;

    // yield return new WaitForSeconds(Random.Range(0f, maxJumpInterval));
    // _animator.SetTrigger(Jump1);

    _rb.gravityScale = 2;
    _rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);

    float elapsedTime = 0;
    while (elapsedTime < _jumpDuration)
    {
        elapsedTime += Time.deltaTime;
        yield return null;
    }

    _rb.linearVelocity = Vector2.zero; // Stop the movement
    _rb.gravityScale = 0;
    _animator.SetTrigger(Idle);
    _currentPosition = transform.position;
    _isJumping = false;
}
    
    
    // // jump coroutine using fixed time and not physics based
    // private IEnumerator JumpCoroutine()
    // {
    //     float distance = Vector3.Distance(_currentPosition, new Vector3(_xDestination, _yDestination, 0));
    //     float adjustedJumpDuration = distance / Mathf.Sqrt(_jumpRangeX * _jumpRangeX + _jumpRangeY * _jumpRangeY) * jumpDuration; // Adjust jump duration based on distance
    //
    //     yield return new WaitForSeconds(Random.Range(0f, maxJumpInterval));
    //     _animator.SetTrigger(Jump1);
    //
    //     float elapsedTime = 0;
    //     while (elapsedTime < adjustedJumpDuration)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         transform.position = Vector3.Slerp(_currentPosition, new Vector3(_xDestination, _yDestination, 0),
    //             elapsedTime / adjustedJumpDuration);
    //         yield return null;
    //     }
    //
    //     _animator.SetTrigger(Idle);
    //     _currentPosition = transform.position;
    //     _isJumping = false;
    // }
    
    
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

    // private IEnumerator FakeLoadCoroutine()
    // {
    //     _animator.SetTrigger(Load);
    //     yield return new WaitForSeconds(Random.Range(0, maxLoadDuration));
    // }
    
    private bool IsInAnimationState(string stateName)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName);
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