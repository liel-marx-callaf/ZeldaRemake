using Pool;
using UnityEngine;

public class EnemyPool : MonoPool<PoolableEnemy>
{
    public void Initialize(GameObject prefab, int initialSize, Transform parent)
    {
        base.Initialize(prefab, initialSize, parent);
    }
    
    public void ReturnToPool(PoolableEnemy obj)
    {
        base.Return(obj);
    }
    
    public PoolableEnemy GetFromPool()
    {
        return base.Get();
    }
}
