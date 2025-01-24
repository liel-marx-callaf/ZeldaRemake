
using UnityEngine;
using Pool;

public class HeartPickup : MonoBehaviour, IPoolable
{
    [SerializeField] private int healAmount = 1;
    [SerializeField] private float despawnTime = 5f;
    private float _timeLeft;

    private void OnEnable()
    {
        _timeLeft = despawnTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            MyEvents.PlayerHeal?.Invoke(healAmount);
        }
    }

    public void Reset()
    {
        _timeLeft = despawnTime;
    }
}