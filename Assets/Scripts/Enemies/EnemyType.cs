using System;
using Pool;
using Unity.VisualScripting;
using UnityEngine;

[Serializable] 
public enum EnemyTypeEnum
{
    Tektite,
    Peahat
}

[Serializable]
public class EnemyType
{
    public string enemyName;
    public EnemyTypeEnum enemyType;
    // private string Name => enemyType.ToString();
    public int spawnAmount;
    
    public EnemyType()
    {
        enemyName = enemyType.ToString();
    }
}


