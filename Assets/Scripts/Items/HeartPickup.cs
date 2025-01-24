using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 1;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                
            }
        }
}
