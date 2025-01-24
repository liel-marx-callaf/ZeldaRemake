using System;
using Unity.VisualScripting;
using UnityEngine;

public class MyEvents : MonoBehaviour
{
    public static Action GameOver;
    public static Action ClearAllEnemies;
    public static Action<int, int> AreaSwitch;
    public static Action<int> PlayerHit;
    public static Action<int> PlayerHeal;
    public static Action<int> PlayerGainRupees;
    // public static Action<float> PlayerPushback;
    public static Action<int> ReturnEnemiesToPool;
    public static Action<EnemyTypeEnum, Vector3> EnemyDied;
    public static Action ClearAreaFromEnemies;
    public static Action ForceDropSwitch;
}
