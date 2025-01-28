using UnityEngine;

public class GameManagerPersistent : MonoBehaviour
{
    private static bool _gameManagerExists = false;

    private void Awake()
    {
        // If a player already exists, destroy this duplicate
        if (_gameManagerExists)
        {
            Destroy(gameObject);
            return;
        }

        // Mark that a player now exists
        _gameManagerExists = true;
        DontDestroyOnLoad(gameObject);
    }
}
