using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoSingleton<EnemiesManager>
{
    [SerializeField] private TektitePool tektitePool;
    // [SerializeField] private PeahatPool peahatPool;

    // [Serializable]
    // public class Offset
    // {
    //     public float x;
    //     public float y;
    // }

    [SerializeField] private Vector2 topLeftOffset;
    [SerializeField] private Vector2 bottomRightOffset;
    // [SerializeField, Range(0f, 3f)] private float maxSpawnDelay = 2f;
    // [SerializeField, Range(0f, 3f)] private float minSpawnDelay = 0f;

    [Serializable]
    public class Area
    {
        [SerializeField] public string areaName;
        [SerializeField] public int areaIndex;
        [SerializeField] public Vector2 areaCameraPosition;
        [SerializeField] public EnemyType[] enemyTypes;
    }

    [SerializeField] private Area[] areas;
    [SerializeField] private int startingAreaIndex;
    
    private CinemachineBrain _cinemachineBrain;
    private int _currentAreaIndex;

    private void OnEnable()
    {
        MyEvents.AreaSwitch += AreaChanged;
        MyEvents.EnemyDied += EnemyDied;
        if(Camera.main.GetComponent<CinemachineBrain>() != null)
            _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        // _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void OnDisable()
    {
        MyEvents.AreaSwitch -= AreaChanged;
        MyEvents.EnemyDied -= EnemyDied;
    }

    private void EnemyDied(EnemyTypeEnum obj)
    {
        Debug.Log("Enemy died: " + obj);
        foreach (var enemytype in areas[_currentAreaIndex - 1].enemyTypes)
        {
            if (enemytype.enemyType == obj)
            {
                enemytype.spawnAmount--;
            }
        }
    }

    private void AreaChanged(int areaEnterIndex, int areaExitIndex)
    {
        Debug.Log("areaEnterIndex: " + areaEnterIndex + " areaExitIndex: " + areaExitIndex);
        _currentAreaIndex = areaEnterIndex;
        var exitArea = areas[areaExitIndex - 1];
        var enterArea = areas[areaEnterIndex - 1];
        DespawnEnemies(exitArea);
        StartCoroutine(SpawnEnemies(enterArea));
        
    }

    private void DespawnEnemies(Area exitArea)
    {
        // Debug.Log("Exited area: " + exitArea.areaName);
        MyEvents.ReturnEnemiesToPool?.Invoke(exitArea.areaIndex);
    }


    private IEnumerator SpawnEnemies(Area area)
    {
        yield return new WaitForSeconds(_cinemachineBrain.DefaultBlend.Time);
        foreach (var enemyType in area.enemyTypes)
        {
            for (int i = 0; i < enemyType.spawnAmount; i++)
            {
                // var spawnDelay = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);
                SpawnEnemiesCoroutine(enemyType, area);
            }
        }
    }

    private void SpawnEnemiesCoroutine(EnemyType enemy, Area area)
    {
        var spawnPosition = new Vector3(Random.Range(topLeftOffset.x, bottomRightOffset.x) + area.areaCameraPosition.x, Random.Range(topLeftOffset.y, bottomRightOffset.y)+ area.areaCameraPosition.y, 0);
        // yield return new WaitForSeconds(spawnDelay);
        if (enemy.enemyType == EnemyTypeEnum.Tektite)
        {
            var enemyObj = tektitePool.Get();
            enemyObj.transform.position = spawnPosition;
            enemyObj.SetAreaIndex(area.areaIndex);
            enemyObj.SetAreaBorders(topLeftOffset+area.areaCameraPosition, bottomRightOffset+area.areaCameraPosition);
            enemyObj.gameObject.SetActive(true);
        }
        // enemyObj.transform.position = spawnPosition;
        // yield return null;
    }
}
