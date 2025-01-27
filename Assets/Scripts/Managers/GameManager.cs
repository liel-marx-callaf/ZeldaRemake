using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private static readonly int Start1 = Animator.StringToHash("Start");

        [Header("Journal References")] [SerializeField]
        private GameObject journalCanvas;

        [SerializeField] private JournalController journalController;
        private bool _isJournalOpen;

        [Header("Transition Settings")]
        [SerializeField] private Animator transition;
        [SerializeField] private float transitionTime = 1f;
        
        [Header("Scene References")] 
        [SerializeField] private SceneIndexEnum startingScene;
        private SceneIndexEnum _currentScene;

        [Header("Area References")] [SerializeField]
        private AreaTypeData areaTypeData;

        [SerializeField] private int startingAreaIndex;

        private InputPlayerActions _inputPlayerActions;
        private InputAction _actionSelect;
        private InputAction _actionStart;

        private void OnEnable()
        {
            _inputPlayerActions = new InputPlayerActions();
            _actionSelect = _inputPlayerActions.Player.ActionSelect;
            _actionSelect.Enable();
            _actionSelect.performed += OnActionSelect;
            _actionStart = _inputPlayerActions.Player.ActionStart;
            _actionStart.Enable();
            _actionStart.performed += OnActionStart;
        }

        private void OnDisable()
        {
            _actionSelect.performed -= OnActionSelect;
            _actionSelect.Disable();
            _actionStart.performed -= OnActionStart;
            _actionStart.Disable();
        }

        private void Start()
        {
            if (journalCanvas != null) journalCanvas.SetActive(false);
            MyEvents.LoadScene?.Invoke(startingScene);
            _currentScene = startingScene;
        }

        private void OnActionSelect(InputAction.CallbackContext context)
        {
            if (_currentScene is SceneIndexEnum.MainGame or SceneIndexEnum.Journal)
            {
                ToggleJournal();
            }
        }
        
        private void OnActionStart(InputAction.CallbackContext context)
        {
            if (_currentScene is SceneIndexEnum.StartMenu)
            {
                LoadNextScene(SceneIndexEnum.MainGame);
            }
        }
        private void LoadNextScene(SceneIndexEnum sceneIndexEnum)
        {
            StartCoroutine(LoadScene(sceneIndexEnum));
        }
    
        private IEnumerator LoadScene(SceneIndexEnum sceneIndexEnum)
        {
            transition.SetTrigger(Start1);
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(sceneIndexEnum.ToString());
            MyEvents.LoadScene?.Invoke(sceneIndexEnum);
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
        
        private void ToggleJournal()
        {
            if (!_isJournalOpen)
            {
                _currentScene = SceneIndexEnum.Journal;
                MyEvents.LoadScene?.Invoke(_currentScene);
                MyEvents.ToggleJournal?.Invoke();
                OpenJournal();
            }
            else
            {
                _currentScene = SceneIndexEnum.MainGame;
                MyEvents.LoadScene?.Invoke(_currentScene);
                MyEvents.ToggleJournal?.Invoke();
                CloseJournal();
            }
        }
    }
}