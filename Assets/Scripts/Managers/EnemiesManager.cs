using System;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class EnemiesManager : MonoSingleton<EnemiesManager>
{
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
    private EnemyPool _enemyPool;

    private void Awake()
    {
        foreach (var area in areas)
        {
            foreach (var enemyType in area.enemyTypes)
            {
                _enemyPool.Initialize(enemyType.prefab, enemyType.spawnAmount, transform);
            }
        }
    }
}