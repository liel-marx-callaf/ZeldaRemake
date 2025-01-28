using System.Collections;
using Player;
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
        private SceneIndexEnum _lastScene;
        [SerializeField] private Vector2 startingPosition;
        [SerializeField] private Vector2 startingPositionSideRoom;
        [SerializeField] private Vector2 exitPositionSideRoom;

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
            MyEvents.LoadScene += OnLoadScene;
        }

        private void OnDisable()
        {
            _actionSelect.performed -= OnActionSelect;
            _actionSelect.Disable();
            _actionStart.performed -= OnActionStart;
            _actionStart.Disable();
            MyEvents.AreaSwitch -= OnAreaSwitch;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            MyEvents.LoadScene -= OnLoadScene;
        }
        

        private void Start()
        {
            if (journalCanvas != null) journalCanvas.SetActive(false);
            MyEvents.LoadScene?.Invoke(startingScene, startingScene);
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
                MyEvents.LoadScene?.Invoke(SceneIndexEnum.MainGame, SceneIndexEnum.StartMenu);
                // LoadNextScene(_currentScene,SceneIndexEnum.MainGame);
            }
        }
        // private void LoadNextScene(SceneIndexEnum currentSceneIndexEnum, SceneIndexEnum nextSceneIndexEnum)
        // {
        //     if (currentSceneIndexEnum == SceneIndexEnum.StartMenu)
        //     {
        //         StartCoroutine(LoadScene(nextSceneIndexEnum, false));
        //     }
        //     
        // }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Scenes that DO NOT NEED the player:
            if (scene.buildIndex == (int)SceneIndexEnum.StartMenu 
                || scene.buildIndex == (int)SceneIndexEnum.GameOver
                || scene.buildIndex == (int)SceneIndexEnum.Win)
            {
                // If a player already exists, destroy it (or do nothing if you want 
                // them to carry over items between attempts, but usually you'd destroy)
                if (_currentPlayer != null)
                {
                    Destroy(_currentPlayer);
                    _currentPlayer = null;
                }
                return;
            }

            // Scenes that DO NEED the player (MainGame, StartingSideRoom, Shop):
            if (scene.buildIndex == (int)SceneIndexEnum.MainGame 
                || scene.buildIndex == (int)SceneIndexEnum.StartingSideRoom
                || scene.buildIndex == (int)SceneIndexEnum.Shop)
            {
                // If we don’t have a reference, create the player
                if (_currentPlayer == null)
                {
                    _currentPlayer = Instantiate(player);
                    // "DontDestroyOnLoad" is also called in the player's Awake, 
                    // so it won't be destroyed on scene changes
                }
                journalCanvas = GameObject.FindGameObjectWithTag("JournalCanvas");
                // journalCanvas.SetActive(true);
                var tempJournalController = GameObject.FindGameObjectWithTag("JournalController");
                if (tempJournalController != null)
                {
                    journalController = tempJournalController.GetComponent<JournalController>();
                }
                journalCanvas.SetActive(false);
                if (scene.buildIndex == (int)SceneIndexEnum.StartingSideRoom)
                {
                    MyEvents.StartText?.Invoke();
                    _currentPlayer.transform.position = startingPositionSideRoom;
                }
                if(scene.buildIndex == (int)SceneIndexEnum.MainGame && _lastScene == SceneIndexEnum.StartingSideRoom)
                {
                    _currentPlayer.transform.position = exitPositionSideRoom;
                    _currentPlayer.GetComponent<PlayerAnimationControl>().SetAnimTrigger("ExitRoom");
                }
                if(scene.buildIndex == (int)SceneIndexEnum.MainGame && _lastScene == SceneIndexEnum.StartMenu)
                {
                    _currentPlayer.transform.position = startingPosition;
                }
                
            }
        }
        
        private void OnLoadScene(SceneIndexEnum enterSceneIndex, SceneIndexEnum exitSceneIndex)
        {
            Debug.Log("Enter Scene: " + enterSceneIndex + " Exit Scene: " + exitSceneIndex);
            // todo: _currentScene = enterSceneIndex;
            // todo: different logic for different scenes
            _currentScene = enterSceneIndex;
            if(exitSceneIndex != SceneIndexEnum.Journal) _lastScene = exitSceneIndex;
            if (enterSceneIndex == SceneIndexEnum.MainGame && exitSceneIndex == SceneIndexEnum.StartMenu)
            {
                StartCoroutine(LoadScene(enterSceneIndex, startingPosition,true));
                // player.transform.position = startingPosition;
            }
            if(enterSceneIndex == SceneIndexEnum.StartingSideRoom && exitSceneIndex == SceneIndexEnum.MainGame)
            {
                StartCoroutine(LoadScene(enterSceneIndex, startingPositionSideRoom,true));
                // player.transform.position = startingPositionSideRoom;
            }
            if(enterSceneIndex == SceneIndexEnum.MainGame && exitSceneIndex == SceneIndexEnum.StartingSideRoom)
            {
                StartCoroutine(LoadScene(SceneIndexEnum.MainGame,exitPositionSideRoom ,true));
                // player.transform.position = exitPositionSideRoom;
            }
        }

        
    
        private IEnumerator LoadScene(SceneIndexEnum enterSceneIndex ,Vector2 playerPositionAfterLoad ,bool needPlayerFreeze)
        {
            if(needPlayerFreeze) MyEvents.TogglePlayerFreeze?.Invoke();
            transition.SetTrigger(Start1);
            yield return new WaitForSeconds(transitionTime);
            Debug.Log("Loading scene: " + enterSceneIndex.ToString());
            SceneManager.LoadScene(enterSceneIndex.ToString());
            // MyEvents.LoadScene?.Invoke(enterSceneIndex, exitSceneIndex);
            if(needPlayerFreeze) MyEvents.TogglePlayerFreeze?.Invoke();
            yield return new WaitForSeconds(1);
            MyEvents.RefreshUI?.Invoke();
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
            // journalCanvas = GameObject.FindGameObjectWithTag("JournalCanvas");
            // journalController = journalCanvas.transform.GetChild(0).GetComponent<JournalController>();
            if (journalCanvas != null) journalCanvas.SetActive(false);
            if (journalController != null) journalController.OnCloseJournal();
        }
        
        private void ToggleJournal()
        {
            // journalCanvas = GameObject.FindGameObjectWithTag("JournalCanvas");
            // journalController = journalCanvas.transform.GetChild(0).GetComponent<JournalController>();
            if (!_isJournalOpen)
            {
                // _currentScene = SceneIndexEnum.Journal;
                MyEvents.LoadScene?.Invoke(SceneIndexEnum.Journal, _currentScene);
                MyEvents.ToggleJournal?.Invoke();
                OpenJournal();
            }
            else
            {
                // _currentScene = SceneIndexEnum.MainGame;
                MyEvents.LoadScene?.Invoke(_lastScene, _currentScene);
                MyEvents.ToggleJournal?.Invoke();
                CloseJournal();
            }
        }
    }
}