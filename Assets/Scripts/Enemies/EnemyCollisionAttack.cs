using System;
using UnityEngine;

public class EnemyCollisionAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float pushBackForce = 5f;


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            // MyEvents.PlayerHit?.Invoke(damage);
            // Apply pushback force to the player's Rigidbody2D
            Rigidbody2D playerRb = other.collider.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Debug.Log("found player rb");
                // Vector2 pushDirection = (other.transform.position - transform.position).normalized;
                MyEvents.PlayerHit?.Invoke(damage);
                // MyEvents.PlayerPushback?.Invoke(pushDirection ,pushBackForce);
                // playerRb.AddForce(pushDirection * pushBackForce, ForceMode2D.Impulse);
            }
        }
    }
}