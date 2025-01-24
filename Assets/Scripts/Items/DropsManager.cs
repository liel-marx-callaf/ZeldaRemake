using System.Collections.Generic;
using UnityEngine;
using static ItemsEnum;
using Random = UnityEngine.Random;

public class DropsManager : MonoSingleton<DropsManager>
{
    [SerializeField] private HeartPool heartPool;

    [SerializeField] private RupeePool rupeePool;
    private int _killCount = 0;
    private readonly ItemsEnum[] _groupADrops = { Heart, Rupee, Heart, Rupee, Heart, Rupee, Heart, Heart, Rupee, Rupee };
    private readonly ItemsEnum[] _groupBDrops = { Heart, Rupee, Heart, Rupee, Heart, Rupee, Heart, Heart, Rupee, Rupee };
    private readonly ItemsEnum[] _groupCDrops = { Heart, Rupee, Heart, Rupee, Heart, Rupee, Heart, Heart, Rupee, Rupee };
    private readonly ItemsEnum[] _groupDDrops = { Heart, Heart, Heart, Rupee, Heart, Heart, Heart, Heart, Heart, Rupee };

    [SerializeField] private GroupDropRate[] groupDropRates;
    [SerializeField] private EnemyGroupMappingList[] enemyGroupMappingLists;
    private Dictionary<EnemyTypeEnum, EnemyGroupEnum> _enemyGroupMapping;
    private Dictionary<EnemyGroupEnum, ItemsEnum[]> _groupDropsMapping;
    private Dictionary<EnemyGroupEnum, int> _groupDropRateMapping;
    private bool _forceDrop = false;


    private void Start()
    {
        _enemyGroupMapping = new Dictionary<EnemyTypeEnum, EnemyGroupEnum>();
        foreach (var groupList in enemyGroupMappingLists)
        {
            foreach (var enemyType in groupList.enemyTypes)
            {
                _enemyGroupMapping[enemyType] = groupList.enemyGroup;
            }
        }

        _groupDropsMapping = new Dictionary<EnemyGroupEnum, ItemsEnum[]>
        {
            { EnemyGroupEnum.GroupA, _groupADrops },
            { EnemyGroupEnum.GroupB, _groupBDrops },
            { EnemyGroupEnum.GroupC, _groupCDrops },
            { EnemyGroupEnum.GroupD, _groupDDrops }
        };

        _groupDropRateMapping = new Dictionary<EnemyGroupEnum, int>();
        foreach (var dropRate in groupDropRates)
        {
            _groupDropRateMapping[dropRate.enemyGroup] = dropRate.dropRate;
        }
    }

    private void OnEnable()
    {
        MyEvents.EnemyDied += HandleEnemyKilled;
        MyEvents.ForceDropSwitch += OnForceDropSwitch;
    }

    private void OnDisable()
    {
        MyEvents.EnemyDied -= HandleEnemyKilled;
        MyEvents.ForceDropSwitch -= OnForceDropSwitch;
        
    }

    private void OnForceDropSwitch()
    {
        _forceDrop = !_forceDrop;
    }

    public void HandleEnemyKilled(EnemyTypeEnum enemyType, Vector3 position)
    {
        _killCount++;
        _killCount %= 10;
        if (_enemyGroupMapping.TryGetValue(enemyType, out EnemyGroupEnum group))
        {
            if (_forceDrop)
            {
                ForceDropItem(group, position);
            }
            else
            {
                DropItem(group, position);
            }
        }
    }

    private void DropItem(EnemyGroupEnum group, Vector3 position)
    {
        if (_groupDropRateMapping.TryGetValue(group, out int dropRate))
        {
            if (Random.Range(0, 100) < dropRate)
            {
                if (_groupDropsMapping.TryGetValue(group, out ItemsEnum[] drops))
                {
                    int index = _killCount;
                    switch (drops[index])
                    {
                        case Heart:
                            var heart = heartPool.Get();
                            heart.transform.position = position;
                            break;
                        case Rupee:
                            var rupee = rupeePool.Get();
                            rupee.transform.position = position;
                            break;
                    }
                }
            }
        }
    }

    private void ForceDropItem(EnemyGroupEnum group, Vector3 position)
    {
        if (_groupDropsMapping.TryGetValue(group, out ItemsEnum[] drops))
        {
            int index = _killCount;
            switch (drops[index])
            {
                case Heart:
                    var heart = heartPool.Get();
                    heart.transform.position = position;
                    break;
                case Rupee:
                    var rupee = rupeePool.Get();
                    rupee.transform.position = position;
                    break;
            }
        }
    }
}