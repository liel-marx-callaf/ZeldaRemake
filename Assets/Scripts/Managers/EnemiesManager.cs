using System;
using System.Collections;
// using JetBrains.Annotations;
// using JetBrains.Annotations;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoSingleton<EnemiesManager>
{
    [Header("Enemy Pools")] 
    [SerializeField] private TektitePool tektitePool;

    // [SerializeField] private PeahatPool peahatPool;
    [Header("Spawn Settings")] 
    [SerializeField] private Vector2 topLeftOffset;
    [SerializeField] private Vector2 bottomRightOffset;
    [SerializeField, Range(0f, 5f)] private float spawnEdgeBuffer = 1f;

    [Serializable]
    public class Area
    {
        [SerializeField] public string areaName;
        [SerializeField] public int areaIndex;
        [SerializeField] public Vector2 areaCameraPosition;
        [SerializeField] public EnemyType[] enemyTypes;
    }

    [Header("Area Settings")] 
    [SerializeField] private int startingAreaIndex = 1;

    [SerializeField] private Area[] areas;


    private CinemachineBrain _cinemachineBrain;
    private int _currentAreaIndex;

    private void OnEnable()
    {
        MyEvents.AreaSwitch += AreaChanged;
        MyEvents.EnemyDied += EnemyDied;
        if (Camera.main?.GetComponent<CinemachineBrain>() != null)
            _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        // _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
    }

    private void OnDisable()
    {
        MyEvents.AreaSwitch -= AreaChanged;
        MyEvents.EnemyDied -= EnemyDied;
    }

    private void EnemyDied(EnemyTypeEnum obj, Vector3 position)
    {
        Debug.Log("Enemy died: " + obj);
        foreach (var enemyType in areas[_currentAreaIndex - 1].enemyTypes)
        {
            if (enemyType.enemyType == obj)
            {
                enemyType.spawnAmount--;
            }
        }
    }

    private void AreaChanged(int areaEnterIndex, int areaExitIndex)
    {
        Debug.Log("areaEnterIndex: " + areaEnterIndex + " areaExitIndex: " + areaExitIndex);
        _currentAreaIndex = areaEnterIndex;
        var exitArea = GetAreaFromIndex(areaExitIndex);
        var enterArea = GetAreaFromIndex(areaEnterIndex);
        if (exitArea == null || enterArea == null)
        {
            Debug.Log("Area not found");
            return;
        }
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
                SpawnEnemies(enemyType, area);
            }
        }
    }

    private void SpawnEnemies(EnemyType enemy, Area area)
    {
        var spawnPosition = new Vector3(
            Random.Range(topLeftOffset.x + spawnEdgeBuffer, bottomRightOffset.x - spawnEdgeBuffer) +
            area.areaCameraPosition.x,
            Random.Range(bottomRightOffset.y + spawnEdgeBuffer, topLeftOffset.y - spawnEdgeBuffer) +
            area.areaCameraPosition.y, 0);
        // yield return new WaitForSeconds(spawnDelay);
        if (enemy.enemyType == EnemyTypeEnum.Tektite)
        {
            var enemyObj = tektitePool.Get();
            enemyObj.transform.position = spawnPosition;
            enemyObj.SetAreaIndex(area.areaIndex);
            enemyObj.SetAreaBorders(topLeftOffset + area.areaCameraPosition,
                bottomRightOffset + area.areaCameraPosition);
            enemyObj.SetStartingPosition(spawnPosition);
            enemyObj.gameObject.SetActive(true);
        }
        // enemyObj.transform.position = spawnPosition;
        // yield return null;
    }

    
    private Area GetAreaFromIndex(int index)
    {
        foreach(var area in areas)
        {
            if (area.areaIndex == index)
            {
                return area;
            }
        }
        return null;
    }
}