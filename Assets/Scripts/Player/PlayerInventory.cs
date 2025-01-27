using System;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [Header("Currency")] 
        [SerializeField] private int rupees = 0;
        [SerializeField] private int keys = 0;

        [Header("Items")] 
        [SerializeField] private int bombs = 0;

        private void OnEnable()
        {
            MyEvents.PlayerGainRupees += AddRupee;
            MyEvents.PlayerGainKey += AddKey;
            MyEvents.PlayerGainBomb += AddBomb;
            MyEvents.PlayerUseBomb += UseBomb;
            MyEvents.PlayerUseKey += UseKey;
            MyEvents.PlayerUseRupee += UseRupee;
        }

        private void OnDisable()
        {
            MyEvents.PlayerGainRupees -= AddRupee;
            MyEvents.PlayerGainKey -= AddKey;
            MyEvents.PlayerGainBomb -= AddBomb;
            MyEvents.PlayerUseBomb -= UseBomb;
            MyEvents.PlayerUseKey -= UseKey;
            MyEvents.PlayerUseRupee -= UseRupee;
        }
        
        private void Start()
        {
            UIManager.Instance.UpdateCurrentRupeesUI(rupees);
            UIManager.Instance.UpdateCurrentKeysUI(keys);
            UIManager.Instance.UpdateCurrentBombsUI(bombs);
        }

        private void AddBomb(int amount)
        {
            bombs += amount;
            UIManager.Instance.UpdateCurrentBombsUI(bombs);
        }

        private void AddKey(int amount)
        {
            keys += amount;
            UIManager.Instance.UpdateCurrentKeysUI(keys);
        }

        private void AddRupee(int amount)
        {
            rupees += amount;
            UIManager.Instance.UpdateCurrentRupeesUI(rupees);
        }

        private void UseBomb(int amount)
        {
            bombs -= amount;
            UIManager.Instance.UpdateCurrentBombsUI(bombs);
        }

        private void UseKey(int amount)
        {
            keys -= amount;
            UIManager.Instance.UpdateCurrentKeysUI(keys);
        }

        private void UseRupee(int amount)
        {
            rupees -= amount;
            UIManager.Instance.UpdateCurrentRupeesUI(rupees);
        }
        
        public int GetBombCount()
        {
            return bombs;
        }
        
        public int GetKeyCount()
        {
            return keys;
        }
        
        public int GetRupeeCount()
        {
            return rupees;
        }
    }
}