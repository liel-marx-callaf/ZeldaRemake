using Pool;
using UnityEngine;

public class PoolableRupee : MonoBehaviour, IPoolable
{
    private RupeePickup _rupeePickup;
    
    private void OnEnable()
    {
        _rupeePickup = GetComponent<RupeePickup>();
    }
    public void Reset()
    {
        _rupeePickup.Reset();
    }

    public void ReturnToPool()
    {
        RupeePool.Instance.Return(this);
    }
}