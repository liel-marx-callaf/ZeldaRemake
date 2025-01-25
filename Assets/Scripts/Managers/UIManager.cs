using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("Heart Settings")] [SerializeField]
    private Image[] heartContainers;
    // Each element in this array is a single half-heart Image in the top UI
    // So for 4 hearts, you have 8 half-sprites in a row.

    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite halfHeartSprite; // If you want half usage
    [SerializeField] private Sprite emptyHeartSprite;

    [Header("Counters")] [SerializeField] private Text rupeeCounterText; // or a sprite-based approach
    [SerializeField] private Text bombCounterText;
    [SerializeField] private Text keyCounterText;

    [Header("Minimap")] [SerializeField] private Image mapBackground;
    [SerializeField] private MinimapData minimapData;
    [SerializeField] private RectTransform playerLocationMarker;
    [SerializeField] private int startingAreaIndex = 1; // The area where the player starts
    // We'll move this marker to show the player's position

    [Header("Player Stats")] [SerializeField]
    private PlayerHealth playerHealth;

    // We'll track the player's current HP, rupees, bombs, etc.
    private int _currentHP = 8; // default (4 hearts x 2 halves)
    private int _maxHP = 8; // also 4 hearts in halves
    private int _currentRupees = 0;
    private int _currentBombs = 0;
    private int _currentKeys = 0; // Always 0 for now

    private void OnEnable()
    {
        // Subscribe to events
        MyEvents.PlayerGainRupees += OnPlayerGainRupees;
        MyEvents.PlayerHit += OnPlayerHit;
        MyEvents.PlayerHeal += OnPlayerHeal;
        MyEvents.AreaSwitch += OnAreaSwitch;
        // etc. (if you have bombs or keys events)
    }

    private void OnDisable()
    {
        // Unsubscribe
        MyEvents.PlayerGainRupees -= OnPlayerGainRupees;
        MyEvents.PlayerHit -= OnPlayerHit;
        MyEvents.PlayerHeal -= OnPlayerHeal;
        MyEvents.AreaSwitch -= OnAreaSwitch;
    }

    private void Start()
    {
        _currentHP = playerHealth.GetCurrentHealth();
        _maxHP = playerHealth.GetMaxHealth();
        // Initialize UI
        UpdateHeartsUI();
        UpdateRupeesUI();
        UpdateBombUI();
        UpdateKeysUI();
        // Possibly place the location marker at the starting area
        UpdateMapLocation( /*some default index*/ startingAreaIndex);
    }

    private void OnPlayerGainRupees(int amount)
    {
        _currentRupees += amount;
        UpdateRupeesUI();
    }

    private void OnPlayerHit(int damage)
    {
        _currentHP -= damage;
        if (_currentHP < 0) _currentHP = 0;
        UpdateHeartsUI();
    }

    private void OnPlayerHeal(int amount)
    {
        _currentHP += amount;
        if (_currentHP > _maxHP) _currentHP = _maxHP;
        UpdateHeartsUI();
    }

    private void OnAreaSwitch(int areaEnterIndex, int areaExitIndex)
    {
        // Move the green square or do any other map updates
        UpdateMapLocation(areaEnterIndex);
    }

    // -------------- UI UPDATERS --------------

    private void UpdateHeartsUI()
    {
        // E.g. if currentHP = 7, that means 3.5 hearts
        // We have 8 half-hearts total. We'll fill them up from left to right.
        int hpLeft = _currentHP; // e.g. 7
        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (hpLeft >= 2)
            {
                // This half is full
                heartContainers[i].sprite = fullHeartSprite;
                hpLeft -= 2;
            }
            else if (hpLeft == 1)
            {
                heartContainers[i].sprite = halfHeartSprite;
                hpLeft -= 1;
            }
            else
            {
                heartContainers[i].sprite = emptyHeartSprite;
            }
        }
    }

    private void UpdateRupeesUI()
    {
        // For a truly “NES look,” you might use a sprite font or 
        // a pre-made text sprite. 
        // But let's keep it simple with a Text:
        rupeeCounterText.text = _currentRupees.ToString();
    }

    private void UpdateBombUI()
    {
        bombCounterText.text = _currentBombs.ToString();
    }

    private void UpdateKeysUI()
    {
        keyCounterText.text = _currentKeys.ToString();
    }

    private void UpdateMapLocation(int areaIndex)
    {
        if (minimapData == null || playerLocationMarker == null) return;

        if (minimapData.TryGetPosition(areaIndex, out Vector2 newPosition))
        {
            playerLocationMarker.anchoredPosition = newPosition;
        }
        else
        {
            Debug.Log($"Area index {areaIndex} not found in minimap data.");
        }
    }

    // Example map dictionary (You can store in a ScriptableObject or inspector)
    private Dictionary<int, Vector2> mapPositions = new Dictionary<int, Vector2>()
    {
        // areaIndex -> local position on the mini-map
        { 1, new Vector2(0, 0) },
        { 2, new Vector2(16, 0) },
        { 3, new Vector2(32, 0) },
        // etc...
    };

    private bool TryGetMapPosition(int areaIndex, out Vector2 pos)
    {
        return mapPositions.TryGetValue(areaIndex, out pos);
    }

    // -------------- Public Methods for other scripts --------------
    public void AddBomb(int amount)
    {
        _currentBombs += amount;
        UpdateBombUI();
    }

    public void UseBomb()
    {
        if (_currentBombs > 0)
        {
            _currentBombs--;
            UpdateBombUI();
        }
    }
}