
using System.Collections;
using Interfaces;
using UnityEngine;

public class HeartPickup : MonoBehaviour, IPickupable
{
    [SerializeField] private int healAmount = 1;
    [SerializeField] private float despawnTime = 5f;

    private void OnEnable()
    {
        StartCoroutine(DespawnTimer());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    public void Despawn()
    {
        HeartPool.Instance.Return(this);
    }

    public void Pickup()
    {
        MyEvents.PlayerHeal?.Invoke(healAmount);
        Despawn();
    }

    public void SetDespawnTime(float time)
    {
        despawnTime = time;
    }


    public void Reset()
    {
        StopAllCoroutines();
    }

    private IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(despawnTime);
        Despawn();
    }
}