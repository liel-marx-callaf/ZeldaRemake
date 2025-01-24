using System;
using UnityEngine;

[Serializable]
public class GroupDropRate
{
    public EnemyGroupEnum enemyGroup;
    [Range(0, 100)]
    public int dropRate;
}