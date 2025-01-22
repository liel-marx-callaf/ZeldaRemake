using System;
using Unity.VisualScripting;
using UnityEngine;

public class MyEvents : MonoBehaviour
{
    public static Action GameOver;
    public static Action ClearAllEnemies;
    public static Action<int, int> AreaSwitch;
    public static Action<int> PlayerHit;
    public static Action<Vector2, float> PlayerPushback;
    public static Action<int> ReturnEnemiesToPool;
}
