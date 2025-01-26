
using System.Collections;
using Interfaces;
using UnityEngine;

public class HeartPickup : MonoBehaviour, IPickupable
{
    [Header("Heart Settings")]
    [SerializeField] private int healAmount = 1;
    [SerializeField] private float despawnTime = 5f;
    private PoolableHeart _poolableHeart;
    
    [Header("Audio")]
    [SerializeField] private string pickupSoundName = "LOZ_Get_Heart";
    [SerializeField, Range(0f, 1f)] private float pickupSoundVolume = 1f;

    private void OnEnable()
    {
        _poolableHeart = GetComponent<PoolableHeart>();
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
        _poolableHeart.ReturnToPool();
    }

    public void Pickup()
    {
        AudioManager.Instance.PlaySound(transform.position, pickupSoundName, pickupSoundVolume);
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