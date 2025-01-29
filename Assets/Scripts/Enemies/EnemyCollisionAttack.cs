using System;
using UnityEngine;

public class EnemyCollisionAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float pushbackForce = 10f;
    private bool _spawned = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!_spawned) return;
        if (other.CompareTag("Player"))
        {
            // Apply pushback force to the player's Rigidbody2D
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 direction = (other.transform.position - transform.position).normalized;
                MyEvents.PlayerHit?.Invoke(damage);
                Vector2 pushDirection = GetCardinalDirection(direction);
                playerRb.AddForce(pushDirection * pushbackForce, ForceMode2D.Impulse);
            }
        }
    }
    private Vector2 GetCardinalDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? Vector2.right : Vector2.left;
        }
        return direction.y > 0 ? Vector2.up : Vector2.down;
        
    }
    public void SetSpawned(bool value)
    {
        _spawned = value;
    }
}