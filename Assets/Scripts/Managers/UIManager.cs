using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
[Header("Heart Settings")]
    [SerializeField] private Image[] heartHalves; 
    // Each element in this array is a single half-heart Image in the top UI
    // So for 4 hearts, you have 8 half-sprites in a row.

    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite halfHeartSprite; // If you want half usage
    [SerializeField] private Sprite emptyHeartSprite;

    [Header("Counters")]
    [SerializeField] private Text rupeeCounterText; // or a sprite-based approach
    [SerializeField] private Text bombCounterText;
    [SerializeField] private Text keyCounterText;

    [Header("Minimap")]
    [SerializeField] private Image mapBackground;
    [SerializeField] private RectTransform playerLocationMarker;
    [SerializeField] private int startingAreaIndex = 1; // The area where the player starts
    // We'll move this marker to show the player's position

    // We'll track the player's current HP, rupees, bombs, etc.
    private int currentHP = 8; // default (4 hearts x 2 halves)
    private int maxHP = 8;     // also 4 hearts in halves
    private int currentRupees = 0;
    private int currentBombs = 0;
    private int currentKeys = 0; // Always 0 for now

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
        currentRupees += amount;
        UpdateRupeesUI();
    }

    private void OnPlayerHit(int damage)
    {
        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;
        UpdateHeartsUI();
    }

    private void OnPlayerHeal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
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
        int hpLeft = currentHP; // e.g. 7
        for (int i = 0; i < heartHalves.Length; i++)
        {
            if (hpLeft >= 1)
            {
                // This half is full
                heartHalves[i].sprite = fullHeartSprite;
                hpLeft -= 1;
            }
            else
            {
                heartHalves[i].sprite = emptyHeartSprite;
            }
        }
    }

    private void UpdateRupeesUI()
    {
        // For a truly “NES look,” you might use a sprite font or 
        // a pre-made text sprite. 
        // But let's keep it simple with a Text:
        rupeeCounterText.text = currentRupees.ToString();
    }

    private void UpdateBombUI()
    {
        bombCounterText.text = currentBombs.ToString();
    }

    private void UpdateKeysUI()
    {
        keyCounterText.text = currentKeys.ToString();
    }

    private void UpdateMapLocation(int areaIndex)
    {
        // The original Zelda had a small green square on a blocky map 
        // that highlights your current room.
        // Easiest approach: store a dictionary of "AreaIndex -> localPosition".
        // Then just set `playerLocationMarker.anchoredPosition = thatPosition;`

        // For example:
        Vector2 pos;
        if (TryGetMapPosition(areaIndex, out pos))
        {
            playerLocationMarker.anchoredPosition = pos;
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
        currentBombs += amount;
        UpdateBombUI();
    }

    public void UseBomb()
    {
        if (currentBombs > 0)
        {
            currentBombs--;
            UpdateBombUI();
        }
    }
    
}
