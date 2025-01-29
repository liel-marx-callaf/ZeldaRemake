using System;

[Serializable] 
public enum EnemyTypeEnum
{
    Tektite,
    Peahat
}

[Serializable]
public enum EnemyGroupEnum
{
    GroupA,
    GroupB,
    GroupC,
    GroupD
}

[Serializable]
public class EnemyType
{
    public string enemyName;
    public EnemyTypeEnum enemyType;
    // private string Name => enemyType.ToString();
    public int spawnAmount;
    public int originalSpawnAmount;
    
    public EnemyType()
    {
        enemyName = enemyType.ToString();
    }
}


