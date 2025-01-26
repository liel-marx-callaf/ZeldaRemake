using UnityEngine;

// using GameManagers;

public class CheatManager : MonoSingleton<CheatManager>
{
    private bool _isAltPressed;
    private bool _is1Pressed;
    private bool _is2Pressed;
    private bool _is3Pressed;
    private bool _is4Pressed;
    private bool _is5Pressed;
    private bool _is6Pressed;
    private bool _is7Pressed;
    private bool _is8Pressed;
    private bool _is9Pressed;
    private bool _is0Pressed;
    private bool _isMPressed;
    private bool _isEscapePressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _is0Pressed = false;
        _is1Pressed = false;
        _is2Pressed = false;
        _is3Pressed = false;
        _is4Pressed = false;
        _is5Pressed = false;
        _is6Pressed = false;
        _is7Pressed = false;
        _is8Pressed = false;
        _is9Pressed = false;
        _isMPressed = false;
        _isAltPressed = false;
        _isEscapePressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckKeyStates();
        ImplementCheats();
    }

    private void CheckKeyStates()
    {
        _isAltPressed = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        _is1Pressed = Input.GetKeyDown(KeyCode.Alpha1);
        _is2Pressed = Input.GetKeyDown(KeyCode.Alpha2);
        _is3Pressed = Input.GetKeyDown(KeyCode.Alpha3);
        _is4Pressed = Input.GetKeyDown(KeyCode.Alpha4);
        _is5Pressed = Input.GetKeyDown(KeyCode.Alpha5);
        _is6Pressed = Input.GetKeyDown(KeyCode.Alpha6);
        _is7Pressed = Input.GetKeyDown(KeyCode.Alpha7);
        _is8Pressed = Input.GetKeyDown(KeyCode.Alpha8);
        _is9Pressed = Input.GetKeyDown(KeyCode.Alpha9);
        _is0Pressed = Input.GetKeyDown(KeyCode.Alpha0);
        _isMPressed = Input.GetKeyDown(KeyCode.M);
        _isEscapePressed = Input.GetKeyDown(KeyCode.Escape);
    }

    private void ImplementCheats()
    {
        if (_isAltPressed && _is1Pressed)
        {
            Debug.Log("Cheat activated: Clear all enemies");
            MyEvents.ClearAllEnemies?.Invoke();
        }
        
        if(_isAltPressed && _is5Pressed)
        {
            Debug.Log("Cheat activated: Force drop rate switch");
            MyEvents.ForceDropSwitch?.Invoke();
        }

        if (_isMPressed)
        {
            Debug.Log("Cheat activated: Mute sounds");
            MyEvents.MuteSounds?.Invoke();
        }
        
        if (_isEscapePressed)
        {
            Debug.Log("Cheat activated: Quit game");
            Application.Quit();
        }
    }
}