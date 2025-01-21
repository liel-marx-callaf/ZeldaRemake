using System;
using System.Collections;
using UnityEngine;

public class EnemiesManager : MonoSingleton<EnemiesManager>
{
    [SerializeField] private TektitePool tektitePool;

    [Serializable]
    public class Offset
    {
        public float x;
        public float y;
    }

    [SerializeField] private Offset topLeftOffset;
    [SerializeField] private Offset bottomRightOffset;
    [SerializeField, Range(0f, 3f)] private float maxSpawnDelay = 2f;
    [SerializeField, Range(0f, 3f)] private float minSpawnDelay = 0f;

    [Serializable]
    public class Area
    {
        [SerializeField] public string areaName;
        [SerializeField] public int areaIndex;
        [SerializeField] public Vector2 areaCameraPosition;
        [SerializeField] public EnemyType[] enemyTypes;
    }

    [SerializeField] private Area[] areas;

    private void OnEnable()
    {
        MyEvents.AreaSwitch += AreaChanged;
    }

    private void OnDisable()
    {
        MyEvents.AreaSwitch -= AreaChanged;
    }

    private void AreaChanged(int areaEnterIndex, int areaExitIndex)
    {
        var exitArea = areas[areaExitIndex];
        var enterArea = areas[areaEnterIndex];
        DespawnEnemies(exitArea);
        SpawnEnemies(enterArea);
        
    }

    private void DespawnEnemies(Area exitArea)
    {
        Debug.Log("Exited area: " + exitArea.areaName);
        // throw new NotImplementedException();
    }


    private void SpawnEnemies(Area area)
    {
        foreach (var enemyType in area.enemyTypes)
        {
            for (int i = 0; i < enemyType.spawnAmount; i++)
            {
                var spawnDelay = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);
                StartCoroutine(SpawnEnemiesCoroutine(enemyType, spawnDelay));
            }
        }
    }

    private IEnumerator SpawnEnemiesCoroutine(EnemyType enemy, float spawnDelay)
    {
        var spawnPosition = new Vector3(UnityEngine.Random.Range(topLeftOffset.x, bottomRightOffset.x),
            UnityEngine.Random.Range(topLeftOffset.y, bottomRightOffset.y), 0);
        yield return new WaitForSeconds(spawnDelay);
        var enemyObj = tektitePool.Get();
        enemyObj.transform.position = spawnPosition;
    }
}
