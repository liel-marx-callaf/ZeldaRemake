using Pool;
using UnityEngine;

public class PoolableHeart : MonoBehaviour, IPoolable
{
    private HeartPickup _heartPickup;
    
    private void OnEnable()
    {
        _heartPickup = GetComponent<HeartPickup>();
    }
    public void Reset()
    {
        _heartPickup.Reset();
    }
    
    public void ReturnToPool()
    {
        HeartPool.Instance.Return(this);
    }
}