using Player;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    // ---------------- HEARTS ----------------
    [Header("Heart Settings")] [SerializeField]
    private Image[] heartContainers;
    // Each element in this array is a single half-heart Image in the top UI
    // So for 4 hearts, you have 8 half-sprites in a row.

    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite halfHeartSprite; // If you want half usage
    [SerializeField] private Sprite emptyHeartSprite;

    // ---------------- SPRITE-BASED DIGITS ----------------
    [Header("Digits Sprites")] [Tooltip("Index 0 to 9 for each digit sprite.")] [SerializeField]
    private Sprite[] digitSprites; // an array of length 10

    [Tooltip("The 'X' sprite for 'X 00' style counters.")] [SerializeField]
    private Sprite spriteX;

    // ---------------- RUPEES COUNTER ----------------
    [Header("Rupees UI")] [SerializeField] private Image rupeeX; // The "X" sprite to the left
    [SerializeField] private Image rupeeTens; // The tens digit
    [SerializeField] private Image rupeeOnes; // The ones digit
    private int currentRupees; // The current rupee count

    // ---------------- KEYS COUNTER ----------------
    [Header("Keys UI")] [SerializeField] private Image keyX;
    [SerializeField] private Image keyTens;
    [SerializeField] private Image keyOnes;
    private int currentKeys;

    // ---------------- BOMBS COUNTER ----------------
    [Header("Bombs UI")] [SerializeField] private Image bombX;
    [SerializeField] private Image bombTens;
    [SerializeField] private Image bombOnes;
    private int currentBombs;

    // ---------------- MINIMAP ----------------
    [Header("Minimap")] [SerializeField] private Image mapBackground;
    [SerializeField] private MinimapData minimapData;
    [SerializeField] private RectTransform playerLocationMarker;
    [SerializeField] private int startingAreaIndex = 1; // The area where the player starts
    // We'll move this marker to show the player's position

    // ---------------- PLAYER STATS ----------------
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
        // MyEvents.PlayerHit += OnPlayerHit;
        // MyEvents.PlayerHeal += OnPlayerHeal;
        MyEvents.AreaSwitch += OnAreaSwitch;
        // etc. (if you have bombs or keys events)
    }

    private void OnDisable()
    {
        // Unsubscribe
        MyEvents.PlayerGainRupees -= OnPlayerGainRupees;
        // MyEvents.PlayerHit -= OnPlayerHit;
        // MyEvents.PlayerHeal -= OnPlayerHeal;
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

    // -------------- EVENT HANDLERS --------------

    private void OnPlayerGainRupees(int amount)
    {
        currentRupees = Mathf.Clamp(currentRupees + amount, 0, 99);
        UpdateRupeesUI();
    }
    
    public void UpdateCurrentHealthUI(int currentHealth)
    {
        _currentHP = currentHealth;
        UpdateHeartsUI();
    }

    // private void OnPlayerHit(int damage)
    // {
    //     _currentHP -= damage;
    //     if (_currentHP < 0) _currentHP = 0;
    //     UpdateHeartsUI();
    // }
    //
    // private void OnPlayerHeal(int amount)
    // {
    //     _currentHP += amount;
    //     if (_currentHP > _maxHP) _currentHP = _maxHP;
    //     UpdateHeartsUI();
    // }

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
        rupeeX.sprite = spriteX; // Always show "X"
        int tens = currentRupees / 10;
        int ones = currentRupees % 10;
        rupeeTens.sprite = digitSprites[tens];
        rupeeOnes.sprite = digitSprites[ones];
    }

    private void UpdateBombUI()
    {
        bombX.sprite = spriteX;
        int tens = currentBombs / 10;
        int ones = currentBombs % 10;
        bombTens.sprite = digitSprites[tens];
        bombOnes.sprite = digitSprites[ones];
    }

    private void UpdateKeysUI()
    {
        keyX.sprite = spriteX;
        int tens = currentKeys / 10;
        int ones = currentKeys % 10;
        keyTens.sprite = digitSprites[tens];
        keyOnes.sprite = digitSprites[ones];
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