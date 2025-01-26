using System.Collections;
using Interfaces;
using UnityEngine;

public class RupeePickup : MonoBehaviour, IPickupable
{
    [Header("Rupee Settings")]
    [SerializeField] private int value = 1;
    [SerializeField] private float despawnTime = 5f;
    private PoolableRupee _poolableRupee;
    
    [Header("Audio")]
    [SerializeField] private string pickupSoundName = "LOZ_Get_Rupee";
    [SerializeField, Range(0f, 1f)] private float pickupSoundVolume = 1f;
    
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
        AudioManager.Instance.PlaySound(transform.position, pickupSoundName, pickupSoundVolume);
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
