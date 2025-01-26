using System;
using Unity.VisualScripting;
using UnityEngine;

public class MyEvents : MonoBehaviour
{
    // Game events
    public static Action GameOver;
    public static Action ClearAllEnemies;
    public static Action<int, int> AreaSwitch;
    
    // Player events
    public static Action<int> PlayerHit;
    public static Action<int> PlayerHeal;
    
    // Item events
    public static Action<int> PlayerGainRupees;
    public static Action<int> PlayerGainKey;
    public static Action<int> PlayerGainBomb;
    public static Action<int> PlayerUseBomb;
    public static Action<int> PlayerUseKey;
    public static Action<int> PlayerUseRupee;
    
    // Enemy events
    public static Action<int> ReturnEnemiesToPool;
    public static Action<EnemyTypeEnum, Vector3> EnemyDied;
    public static Action ClearAreaFromEnemies;
    public static Action ForceDropSwitch;
    
    // Audio events
    public static Action PauseUnpauseBackgroundMusic;
    public static Action MuteSounds;
    public static Action<string> StopSound;
    
    // UI events
    public static Action ToggleJournal;

    public static Action<SceneIndexEnum> LoadScene;



}
