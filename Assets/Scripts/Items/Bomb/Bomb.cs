using System.Collections;
using Audio;
using UnityEngine;
using Pool;

public class Bomb : MonoBehaviour, IPoolable
{
    [Header("Bomb Settings")]
    // [SerializeField] private float throwDistance = 2f;  // Distance from player
    [SerializeField] private float explosionDelay = 1.5f;
    [SerializeField, Range(1, 10)] private int explosionDamage = 1;

    [Header("Audio")]
    // [SerializeField] private string bombThrowSound = "LOZ_Bomb_Throw";
    // [SerializeField] private float bombThrowVolume = 0.7f;
    [SerializeField] private string bombExplosionSound = "LOZ_Bomb_Explosion";
    [SerializeField] private float bombExplosionVolume = 0.8f;

    [Header("Explosion Prefabs")]
    [Tooltip("Drag a prefab or references to smoke/explosion objects if each smoke is a separate entity. Alternatively, you can do an animated sprite here.")]
    // [SerializeField] private GameObject smokePrefab; // If each smoke is an individual object
    
    // Predefined offsets for the hex-like shape around the bomb
    private static readonly Vector2[] ExplosionOffsets = new Vector2[]
    {
        new Vector2(-0.5f, 1f), 
        new Vector2(0.5f, 1f),
        new Vector2(-1f, 0f),
        new Vector2(0f, 0f),
        new Vector2(1f, 0f),
        new Vector2(-0.5f, -1f),
        new Vector2(0.5f, -1f),
    };

    private Vector3 _startPosition;
    private bool _hasExploded;

    public void Reset()
    {
        // Called when object is reused from the pool
        _hasExploded = false;
        // reset bomb sprite or any other states
    }

    private void OnEnable()
    {
        // Optionally, start the explosion countdown here if you want
    }

    /// <summary>
    /// Called by the Player when ActionB is triggered, to place or "throw" the bomb.
    /// </summary>
    /// <param name="playerPosition"></param>
    /// <param name="playerFacing"></param>
    public void ThrowBomb(Vector3 playerPosition, Vector2 playerFacing, float throwDistance)
    {
        // Set bomb position in front of the player
        Vector3 offset = new Vector3(playerFacing.x, playerFacing.y, 0f) * throwDistance;
        transform.position = playerPosition + offset;
        _startPosition = transform.position;

        // Play throw sound
        // AudioManager.Instance.PlaySound(transform.position, bombThrowSound, bombThrowVolume);

        // Start the fuse countdown
        StartCoroutine(ExplosionCountdown());
    }

    private IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(explosionDelay);

        // Explosion logic
        Explode();
    }

    private void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;

        // 1) Screen Flash
        ScreenFlash.Instance.FlashScreen();

        // 2) Play explosion sound
        AudioManager.Instance.PlaySound(transform.position, bombExplosionSound, bombExplosionVolume);

        // 3) Spawn smoke objects in the shape around the bomb
        Vector3 center = transform.position;
        foreach (var offset in ExplosionOffsets)
        {
            Vector3 smokePos = center + new Vector3(offset.x, offset.y, 0f);
            var smoke = SmokePool.Instance.Get();
            smoke.transform.position = smokePos;
            // // Instantiate or pull from pool
            // // e.g.: var smokeObj = Instantiate(smokePrefab, smokePos, Quaternion.identity);
            // // or if you have a smoke pool: SmokePool.Instance.Get().Init(smokePos);
            // Instantiate(smokePrefab, smokePos, Quaternion.identity);
        }

        // 4) Damage enemies in that shape
        DamageEnemiesInExplosion(center);

        // 5) Return this bomb to the pool
        BombPool.Instance.Return(this);
    }

    private void DamageEnemiesInExplosion(Vector3 center)
    {
        // Typically, you'd do an overlap circle or multiple small overlap checks
        float explosionRadius = 1.2f; // or define a suitable radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var enemyHealth = hit.GetComponent<EnemyHealth>();
                enemyHealth?.TakeDamage(explosionDamage);
            }
        }
        DrawDebugCircle(center, explosionRadius, Color.red);
    }
    private void DrawDebugCircle(Vector3 center, float radius, Color color)
    {
        int segments = 36;
        float angle = 0f;
        for (int i = 0; i < segments; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 start = new Vector3(x, y, 0) + center;
            angle += 2 * Mathf.PI / segments;
            x = Mathf.Cos(angle) * radius;
            y = Mathf.Sin(angle) * radius;
            Vector3 end = new Vector3(x, y, 0) + center;
            Debug.DrawLine(start, end, color, 2f); // Duration of 2 seconds
        }
    }
}
