using UnityEngine;
using System;
using UnityEngine.InputSystem;

namespace Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [Header("Journal References")] 
        [SerializeField] private GameObject journalCanvas;
        [SerializeField] private JournalController journalController;
        private bool _isJournalOpen;

        [Header("Scene References")]
        [SerializeField] private SceneIndexEnum startingScene;
        private SceneIndexEnum _currentScene;
        
        private InputPlayerActions _inputPlayerActions;
        private InputAction _actionSelect;

        private void OnEnable()
        {
            _inputPlayerActions = new InputPlayerActions();
            _actionSelect = _inputPlayerActions.Player.ActionSelect;
            _actionSelect.Enable();
            _actionSelect.performed += OnActionSelect;
        }

        private void OnDisable()
        {
            _actionSelect.performed -= OnActionSelect;
            _actionSelect.Disable();
        }

        private void Start()
        {
            if (journalCanvas != null) journalCanvas.SetActive(false);
            MyEvents.LoadScene?.Invoke(startingScene);
        }

        private void OnActionSelect(InputAction.CallbackContext context)
        {
            if (!_isJournalOpen)
            {
                OpenJournal();
            }
            else
            {
                CloseJournal();
            }
        }


        private void OpenJournal()
        {
            _isJournalOpen = true;
            Time.timeScale = 0;
            if (journalCanvas != null) journalCanvas.SetActive(true);
            if (journalController != null) journalController.OnOpenJournal();
        }

        private void CloseJournal()
        {
            _isJournalOpen = false;
            Time.timeScale = 1;
            if (journalCanvas != null) journalCanvas.SetActive(false);
            if (journalController != null) journalController.OnCloseJournal();
        }
    }
}