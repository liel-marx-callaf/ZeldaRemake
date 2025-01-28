using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private static readonly int Start1 = Animator.StringToHash("Start");
        [Header("Player")]
        [SerializeField] private GameObject player;
        private GameObject _currentPlayer;
        
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
        public bool PlayerFreeze { get; private set; }
        private CinemachineBrain _cinemachineBrain;

        private void OnEnable()
        {
            _inputPlayerActions = new InputPlayerActions();
            _actionSelect = _inputPlayerActions.Player.ActionSelect;
            _actionSelect.Enable();
            _actionSelect.performed += OnActionSelect;
            _actionStart = _inputPlayerActions.Player.ActionStart;
            _actionStart.Enable();
            _actionStart.performed += OnActionStart;
            MyEvents.AreaSwitch += OnAreaSwitch;
            _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            _actionSelect.performed -= OnActionSelect;
            _actionSelect.Disable();
            _actionStart.performed -= OnActionStart;
            _actionStart.Disable();
            MyEvents.AreaSwitch -= OnAreaSwitch;
            SceneManager.sceneLoaded -= OnSceneLoaded;
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
                LoadNextScene(_currentScene,SceneIndexEnum.MainGame);
            }
        }
        private void LoadNextScene(SceneIndexEnum currentSceneIndexEnum, SceneIndexEnum nextSceneIndexEnum)
        {
            if (currentSceneIndexEnum == SceneIndexEnum.StartMenu)
            {
                StartCoroutine(LoadScene(nextSceneIndexEnum, false));
            }
            
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.buildIndex == (int)SceneIndexEnum.StartMenu || scene.buildIndex == (int)SceneIndexEnum.GameOver || scene.buildIndex == (int)SceneIndexEnum.Win)
            {
                if (_currentPlayer != null)
                {
                    Destroy(_currentPlayer);
                    _currentPlayer = null;
                }
            }
            if(scene.buildIndex == (int)SceneIndexEnum.MainGame || scene.buildIndex == (int)SceneIndexEnum.StartingSideRoom || scene.buildIndex == (int)SceneIndexEnum.Shop)
            {
                if (_currentPlayer == null)
                {
                    _currentPlayer = Instantiate(player);
                    DontDestroyOnLoad(_currentPlayer);
                }
            }
        }

    
        private IEnumerator LoadScene(SceneIndexEnum sceneIndexEnum, bool needPlayerFreeze)
        {
            if(needPlayerFreeze) MyEvents.TogglePlayerFreeze?.Invoke();
            transition.SetTrigger(Start1);
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(sceneIndexEnum.ToString());
            MyEvents.LoadScene?.Invoke(sceneIndexEnum);
            if(needPlayerFreeze) MyEvents.TogglePlayerFreeze?.Invoke();
        }

        private void OnAreaSwitch(int enteringAreaIndex, int exitingAreaIndex)
        { 
            MyEvents.TogglePlayerFreeze?.Invoke();
            StartCoroutine(AreaSwitchTimer());
        }

        private IEnumerator AreaSwitchTimer()
        {
            yield return new WaitForSeconds(_cinemachineBrain.DefaultBlend.Time); 
            MyEvents.TogglePlayerFreeze?.Invoke();
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