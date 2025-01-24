using System.Collections;
using Interfaces;
using UnityEngine;

public class RupeePickup : MonoBehaviour, IPickupable
{
    [Header("Rupee Settings")]
    [SerializeField] private int value = 1;
    [SerializeField] private float despawnTime = 5f;
    private PoolableRupee _poolableRupee;
    private void OnEnable()
    {
        _poolableRupee = GetComponent<PoolableRupee>();
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
        _poolableRupee.ReturnToPool();
    }

    public void Pickup()
    {
        MyEvents.PlayerGainRupees?.Invoke(value);
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
