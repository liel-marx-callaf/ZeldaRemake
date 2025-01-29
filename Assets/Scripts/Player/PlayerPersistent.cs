using System;
using UnityEngine;

public class PlayerPersistent : MonoBehaviour
{
    private static bool _playerExists = false;

    private void Awake()
    {
        // If a player already exists, destroy this duplicate
        if (_playerExists)
        {
            Destroy(gameObject);
            return;
        }

        // Mark that a player now exists
        _playerExists = true;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        MyEvents.PlayerDeath += OnPlayerDeath;
        MyEvents.GameWon += OnGameWon;
    }
    
    private void OnDisable()
    {
        MyEvents.PlayerDeath -= OnPlayerDeath;
        MyEvents.GameWon -= OnGameWon;
    }

    private void OnGameWon()
    {
        _playerExists = false;
    }

    private void OnPlayerDeath()
    {
        _playerExists = false;
    }
}
