using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;
using IPoolable = Pool.IPoolable;

public class TektiteMovement : MonoBehaviour, IPoolable
{
    private static readonly int Jump1 = Animator.StringToHash("Jump");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Load = Animator.StringToHash("Load");

    [Header("Borders (Room Bounds)")]
    [SerializeField] private Vector2 topLeftBorder;
    [SerializeField] private Vector2 bottomRightBorder;
    [SerializeField] private float edgeBuffer = 1f;

    [Header("Timing")]
    [SerializeField, Range(0f, 30f)] private float maxWaitBeforeSpawn = 9f;
    [SerializeField, Range(0f, 10f)] private float maxLoadDuration = 2f;
    [SerializeField, Range(0f, 10f)] private float maxIdleDuration = 2f;
    [SerializeField, Range(0f, 10f)] private float maxJumpInterval = 4f;

    [Header("Jump Settings")]
    [SerializeField, Range(1f, 12f)] private float jumpRange = 4f;
    [SerializeField, Range(0.01f, 5f)] private float jumpDuration = 1f;
    [Tooltip("Extra vertical arc height for the jump animation.")]
    [SerializeField] private float arcHeight = 1.5f;

    private Animator _animator;
    private bool _isDead;
    private bool _isJumping;
    private Vector3 _startPosition;
    private Vector3 _currentPosition;
    private Collider2D _collider;
    private EnemyHealth _enemyHealth;
    private EnemyCollisionAttack _enemyCollisionAttack;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _enemyHealth = GetComponent<EnemyHealth>();
        _enemyCollisionAttack = GetComponent<EnemyCollisionAttack>();
        _startPosition = transform.position;
        _currentPosition = _startPosition;

        // Optionally freeze the animator until "spawn delay" is over.
        _animator.speed = 0f;

        // Start the behavior loop
        StartCoroutine(MovementCoroutine());
    }

    private IEnumerator MovementCoroutine()
    {
        _collider.enabled = false;
        _enemyHealth.SetInvincibility(true);
        _enemyCollisionAttack.SetSpawned(false);
        // 1) Optional “WaitBeforeSpawn”
        float spawnDelay = Random.Range(0.5f, maxWaitBeforeSpawn);
        yield return new WaitForSeconds(spawnDelay);
        _collider.enabled = true;
        _enemyHealth.SetInvincibility(false);
        _enemyCollisionAttack.SetSpawned(true);

        // Now allow animations to play normally
        _animator.speed = 1f;

        while (!_isDead)
        {
            // --- LOAD Phase ---
            if (!IsInAnimationState("TektiteLoad"))
                _animator.SetTrigger(Load);

            float loadTime = Random.Range(0f, maxLoadDuration);
            yield return new WaitForSeconds(loadTime);

            // --- IDLE Phase (20% chance or your own condition) ---
            if (Random.value < 0.2f)
            {
                if (!IsInAnimationState("TektiteIdle"))
                    _animator.SetTrigger(Idle);

                float idleTime = Random.Range(0f, maxIdleDuration);
                yield return new WaitForSeconds(idleTime);
            }

            // --- JUMP Phase ---
            // Wait random time up to maxJumpInterval before jumping
            float waitBeforeJump = Random.Range(0f, maxJumpInterval);
            yield return new WaitForSeconds(waitBeforeJump);

            // Now actually do the jump
            StartCoroutine(JumpArcCoroutine());
            
            // Wait until the jump finishes before repeating the loop
            while (_isJumping && !_isDead)
            {
                yield return null;
            }
        }
    }
// private IEnumerator JumpArcCoroutine()
// {
//     _isJumping = true;
//     _animator.SetTrigger(Jump1);
//
//     // How long the jump lasts
//     float totalTime = jumpDuration;
//     float elapsed = 0f;
//
//     // 1) Determine random horizontal offset, which we’ll treat as velocity
//     // over the duration
//     float randX = Random.Range(-jumpRange, jumpRange);
//     // Horizontal velocity = total horizontal displacement / total time
//     float horizontalVelocity = randX / totalTime;
//
//     // 2) Determine how high (or low) we want to move vertically in total
//     float randY = Random.Range(-jumpRange, jumpRange);
//     float startY = _currentPosition.y;
//     float endY = _currentPosition.y + randY;
//
//     // Save the initial x, so we can do smaller steps each frame
//     float currentX = _currentPosition.x;
//
//     while (elapsed < totalTime)
//     {
//         // Increment time
//         elapsed += Time.deltaTime;
//         float t = Mathf.Clamp01(elapsed / totalTime);
//
//         // --- HORIZONTAL MOVEMENT (with bounce) ---
//         float nextX = currentX + (horizontalVelocity * Time.deltaTime);
//
//         // Check if we hit left or right boundary
//         if (nextX < (topLeftBorder.x + edgeBuffer))
//         {
//             nextX = topLeftBorder.x + edgeBuffer; 
//             horizontalVelocity = -horizontalVelocity; 
//         }
//         else if (nextX > (bottomRightBorder.x - edgeBuffer))
//         {
//             nextX = bottomRightBorder.x - edgeBuffer;
//             horizontalVelocity = -horizontalVelocity;
//         }
//
//         // Update currentX
//         currentX = nextX;
//
//         // --- VERTICAL MOVEMENT (parabolic) ---
//         // We linearly interpolate from startY to endY,
//         // then add a parabolic arc offset in the middle.
//         float baseY = Mathf.Lerp(startY, endY, t);
//         float arcOffset = (1f - Mathf.Pow(2f * t - 1f, 2f)) * arcHeight;
//         float finalY = baseY + arcOffset;
//
//         // Update position
//         transform.position = new Vector3(currentX, finalY, 0f);
//
//         yield return null; 
//     }
//
//     // Land exactly at the final position
//     float clampedX = Mathf.Clamp(currentX, topLeftBorder.x + edgeBuffer, bottomRightBorder.x - edgeBuffer);
//     float clampedY = Mathf.Clamp(endY, bottomRightBorder.y + edgeBuffer, topLeftBorder.y - edgeBuffer);
//     transform.position = new Vector3(clampedX, clampedY, 0f);
//     _currentPosition = transform.position;
//
//     // Switch to Idle after landing
//     if (!_isDead)
//         _animator.SetTrigger(Idle);
//
//     _isJumping = false;
// }

    private IEnumerator JumpArcCoroutine()
    {
        _isJumping = true;
    
        // Trigger the Jump animation
        _animator.SetTrigger(Jump1);
    
        // 1) Calculate random target within jumpRange
        float randX = Random.Range(-jumpRange, jumpRange);
        float randY = Random.Range(-jumpRange, jumpRange);
    
        Vector3 targetPos = _currentPosition + new Vector3(randX, randY, 0f);
    
        // 2) Clamp target to stay in room
        float clampedX = Mathf.Clamp(targetPos.x, topLeftBorder.x + edgeBuffer, bottomRightBorder.x - edgeBuffer);
        float clampedY = Mathf.Clamp(targetPos.y, bottomRightBorder.y + edgeBuffer, topLeftBorder.y - edgeBuffer);
        targetPos = new Vector3(clampedX, clampedY, 0f);
    
        // 3) Interpolate from _currentPosition to targetPos with a parabolic arc
        float elapsed = 0f;
        Vector3 startPos = _currentPosition;
    
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / jumpDuration);
    
            // Horizontal interpolation
            float x = Mathf.Lerp(startPos.x, targetPos.x, t);
    
            // Simple parabolic arc for y:
            //   y = lerp(startY, targetY, t) + arc offset
            float baseY = Mathf.Lerp(startPos.y, targetPos.y, t);
    
            // Create a "peak" in the middle
            // e.g. a parabola offset = (1 - (2t - 1)^2) = 4t(1 - t), scaled by arcHeight
            float arcOffset = (1f - Mathf.Pow(2f*t - 1f, 2f)) * arcHeight;
            float y = baseY + arcOffset;
    
            // Update position
            transform.position = new Vector3(x, y, 0f);
    
            yield return null;
        }
    
        // At the end of the jump, land exactly at target
        transform.position = targetPos;
        _currentPosition = targetPos;
    
        // Switch to Idle after landing
        if (!_isDead)
            _animator.SetTrigger(Idle);
    
        _isJumping = false;
    }

    private bool IsInAnimationState(string stateName)
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(stateName);
    }

    // Called externally if the Tektite dies
    public void OnDeath()
    {
        _isDead = true;
        // Stop any jump coroutines if you like
        StopAllCoroutines();
        // Possibly switch to a Death animation or disable movement
    }

    // IPoolable requirement
    public void Reset()
    {
        _isDead = false;
        _isJumping = false;
        // Potentially reset the animator or position
    }

    public void SetTopLeftBorder(Vector2 newTopLeftBorder)
    {
        topLeftBorder = newTopLeftBorder;
    }

    public void SetBottomRightBorder(Vector2 newBottomRightBorder)
    {
        bottomRightBorder = newBottomRightBorder;
    }
    
    public void SetStartingPosition(Vector3 startingPosition)
    {
        _startPosition = startingPosition;
        _currentPosition = _startPosition;
    }
}
