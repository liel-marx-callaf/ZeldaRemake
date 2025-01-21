using UnityEngine;
using Pool;

public class PoolableTektite: MonoBehaviour , IPoolable
{
    private EnemyHealth _tektiteHealth;
    private TektiteMovement _tektiteMovement;
    
    private void OnEnable()
    {
        _tektiteHealth = GetComponent<EnemyHealth>();
        _tektiteMovement = GetComponent<TektiteMovement>();
    }
    public void Reset()
    {
        _tektiteHealth.Reset();
        _tektiteMovement.Reset();
    }
    
    private void ReturnToPool()
    {
        TektitePool.Instance.Return(this);
    }
}
