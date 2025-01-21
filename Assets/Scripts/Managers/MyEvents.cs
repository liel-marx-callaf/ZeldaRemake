using System;
using Unity.VisualScripting;
using UnityEngine;

public class MyEvents : MonoBehaviour
{
    public static Action GameOver;
    public static Action ClearAllEnemies;
    public static Action<int, int> AreaSwitch;
}
