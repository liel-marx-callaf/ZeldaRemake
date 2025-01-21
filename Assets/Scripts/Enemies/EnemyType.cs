using System;
using Pool;
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
    
    public EnemyTypeEnum enemyType;
    public int spawnAmount;
}


