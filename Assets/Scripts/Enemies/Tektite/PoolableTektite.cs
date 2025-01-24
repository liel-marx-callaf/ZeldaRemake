using UnityEngine;
using Pool;

public class PoolableTektite : MonoBehaviour, IPoolable
{
    private EnemyHealth _tektiteHealth;
    private TektiteMovement _tektiteMovement;
    private Rigidbody2D _rigidbody2D;
    private readonly EnemyTypeEnum _enemyType = EnemyTypeEnum.Tektite;
    [SerializeField] private int sourceAreaIndex;

    private void OnEnable()
    {
        _tektiteHealth = GetComponent<EnemyHealth>();
        _tektiteHealth.SetEnemyType(_enemyType);
        _tektiteMovement = GetComponent<TektiteMovement>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        MyEvents.ReturnEnemiesToPool += ReturnToPoolByAreaIndex;
        MyEvents.ClearAllEnemies += ReturnToPool;
    }

    private void OnDisable()
    {
        MyEvents.ReturnEnemiesToPool -= ReturnToPoolByAreaIndex;
        MyEvents.ClearAllEnemies -= ReturnToPool;
    }

    public void Reset()
    {
        _tektiteHealth.Reset();
        _tektiteMovement.Reset();
        sourceAreaIndex = 0;
    }

    public void SetAreaIndex(int areaIndex)
    {
        sourceAreaIndex = areaIndex;
    }
    
    public void SetAreaBorders(Vector2 topLeftBorder, Vector2 bottomRightBorder)
    {
        _tektiteMovement.SetTopLeftBorder(topLeftBorder);
        _tektiteMovement.SetBottomRightBorder(bottomRightBorder);
    }
    
    public void SetStartingPosition(Vector2 position)
    {
        _tektiteMovement.SetStartingPosition(position);
    }
    
    private void ReturnToPoolByAreaIndex(int areaIndex)
    {
        if (sourceAreaIndex == areaIndex)
        {
            ReturnToPool();
        }
    }
    private void ReturnToPool()
    {
            _rigidbody2D.linearVelocity = Vector2.zero;
            _rigidbody2D.angularVelocity = 0;
            gameObject.SetActive(false);
            TektitePool.Instance.Return(this);
    }
}