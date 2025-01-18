using UnityEngine;
using Pool;

public class PoolableEnemy : MonoBehaviour , IPoolable
{
    private EnemyHealth _enemyHealth;
    
    private void OnEnable()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
    }
    public void Reset()
    {
     _enemyHealth.Reset();   
    }
}
