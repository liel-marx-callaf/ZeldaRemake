using UnityEngine;
using Pool;

public class PoolableEnemyWrapper : MonoBehaviour, IPoolable
{
    public IPoolable PoolableEnemy { get; private set; }

    public void Initialize(IPoolable poolableEnemy)
    {
        PoolableEnemy = poolableEnemy;
    }

    public void Reset()
    {
        PoolableEnemy.Reset();
    }
}