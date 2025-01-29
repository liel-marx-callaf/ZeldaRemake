// Purpose: To manage the audio in the game. This includes background music, sound effects, and audio pools.

using System;
using System.Collections;
using System.Collections.Generic;
using Helper;
using Sound;
using Unity.Cinemachine;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [Header("Background Music")]
        [SerializeField] private SceneBackgroundMusicData sceneBackgroundMusicData;
        [SerializeField, Range(0f, 1f)] private float backgroundVolume = 0.5f;
        [SerializeField] private SceneIndexEnum startingSceneIndex;
        private SceneIndexEnum _currentSceneIndex;
        private float _backgroundVolume;
        private AudioSource _backgroundMusicSource;
        // private bool _isBackgroundMusicPlaying = false;
        private bool _shouldBackgroundMusicPlay = true;
    
        [Header("Area Type Data")]
        [SerializeField] private AreaTypeData areaTypeData;
        [SerializeField] private AreaTypeAudioData areaTypeAudioData;
        [SerializeField] private int startingAreaIndex;
        [SerializeField] private AreaTypeEnum startingAreaType;
        
        [Header("Area Switch Sound")]
        [SerializeField] private string areaSwitchSoundName = "LOZ_Stairs";
        [SerializeField, Range(0f, 1f)] private float areaSwitchSoundVolume = 1f;
        private int _currentAreaIndex;
    
        [Header("Sound Effects")]
        [SerializeField] private AudioClip[] audioClips;
        private Dictionary<string, AudioClip> _audioClipDictionary;

    
        [Header("Audio Pool")]
        [SerializeField] private AudioSourcePool audioSourcePool;
    
        private bool _isMuted = false;
        private bool _journalOpen = false;
        private CinemachineBrain _cinemachineBrain;
        private static AudioManager _instance;


        private void Awake()
        {
            InitializeAudioClipDictionary();
            if(_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            MyEvents.PauseUnpauseBackgroundMusic += PauseUnpauseBackgroundMusic;
            MyEvents.MuteSounds += MuteSounds;
            MyEvents.LoadScene += OnLoadScene;
            MyEvents.AreaSwitch += AreaChanged;
            MyEvents.ToggleJournal += ToggleJournal;
            _backgroundMusicSource = gameObject.AddComponent<AudioSource>();
            _backgroundVolume = backgroundVolume;
            _currentAreaIndex = startingAreaIndex;
            _currentSceneIndex = startingSceneIndex;
            if (Camera.main != null) _cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        }

        private void OnDisable()
        {
            MyEvents.PauseUnpauseBackgroundMusic -= PauseUnpauseBackgroundMusic;
            MyEvents.MuteSounds -= MuteSounds;
            MyEvents.LoadScene -= OnLoadScene;
            MyEvents.AreaSwitch -= AreaChanged;
            MyEvents.ToggleJournal -= ToggleJournal;
        }

        private void ToggleJournal()
        {
            if (!_journalOpen)
            {
                _journalOpen = true;
                if(areaTypeData.TryGetAreaType(_currentAreaIndex, out var exitingAreaType))
                {
                    if (areaTypeAudioData.TryGetAreaTypeAudio(exitingAreaType, out var areaTypeAudio))
                    {
                        StopSound(areaTypeAudio.audioClip.name);
                    }
                }
            }
            else
            {
                _journalOpen = false;
                if (areaTypeData.TryGetAreaType(_currentAreaIndex, out var enteringAreaType))
                {
                    if (areaTypeAudioData.TryGetAreaTypeAudio(enteringAreaType, out var areaTypeAudio))
                    {
                        PlaySound(Vector3.zero, areaTypeAudio.audioClip.name, areaTypeAudio.volume, 1, true);
                    }
                }
            }
        }

        private void Start()
        {
            OnLoadScene(startingSceneIndex, startingSceneIndex);
            AreaChanged(startingAreaIndex, startingAreaIndex);
        }

        private void InitializeAudioClipDictionary()
        {
            _audioClipDictionary = new Dictionary<string, AudioClip>();
            foreach (var audioClip in audioClips)
            {
                _audioClipDictionary.TryAdd(audioClip.name, audioClip);
            }
        }


        private void AreaChanged(int enteringAreaIndex, int exitingAreaIndex)
        {
            Debug.Log("Area changed started in AudioManager");
            Debug.Log("Current scene index: " + _currentSceneIndex);
            Debug.Log("Main game index " + (int)SceneIndexEnum.MainGame);
            if (_currentSceneIndex != SceneIndexEnum.MainGame) return;
            if(areaTypeData.TryGetAreaType(_currentAreaIndex, out var exitingAreaType))
            {
                if (areaTypeAudioData.TryGetAreaTypeAudio(exitingAreaType, out var areaTypeAudio))
                {
                    StopSound(areaTypeAudio.audioClip.name);
                }
            }
            _currentAreaIndex = enteringAreaIndex;
            if (areaTypeData.TryGetAreaType(_currentAreaIndex, out var enteringAreaType))
            {
                if (areaTypeAudioData.TryGetAreaTypeAudio(enteringAreaType, out var areaTypeAudio))
                {
                    PlaySound(Vector3.zero, areaTypeAudio.audioClip.name, areaTypeAudio.volume, 1, true);
                }
            }
            // if(enteringAreaIndex == exitingAreaIndex) return;
            Debug.Log("Area changed finished in AudioManager");
            StartCoroutine(PlayAreaSwitchSound());
        }

        private IEnumerator PlayAreaSwitchSound()
        {
            Debug.Log("started playing area switch sound");
            PlaySound(Vector3.zero, areaSwitchSoundName, areaSwitchSoundVolume, 1, true);
            yield return new WaitForSeconds(2f);
            StopSound(areaSwitchSoundName);
            Debug.Log("finished playing area switch sound");
        }


        private void OnLoadScene(SceneIndexEnum enterScene, SceneIndexEnum exitScene)
        {
            _currentSceneIndex = enterScene;
            if (sceneBackgroundMusicData == null) return;
            if (sceneBackgroundMusicData.TryGetBackgroundMusic(enterScene, out var backgroundMusic))
            {
                if (_backgroundMusicSource != null)
                {
                    if(_backgroundMusicSource.clip != backgroundMusic) PlayBackgroundMusic(backgroundMusic);
                }
                else
                {
                    Debug.LogError("Background music source is null");
                }
            }

            if (enterScene == SceneIndexEnum.MainGame && exitScene == SceneIndexEnum.StartingSideRoom)
            {
                StartCoroutine(PlayAreaSwitchSound());
            }
            if (enterScene == SceneIndexEnum.StartingSideRoom && exitScene == SceneIndexEnum.MainGame)
            {
                StartCoroutine(PlayAreaSwitchSound());
            }
        }

        private void MuteSounds()
        {
            _isMuted = !_isMuted;
            if (_isMuted)
            {
                SetBackgroundMusicVolume(0);
            }
            else
            {
                SetBackgroundMusicVolume(_backgroundVolume);
            }
        }


        private void PlayBackgroundMusic(AudioClip backgroundMusic)
        {
            _backgroundMusicSource.Stop();
            _backgroundMusicSource.clip = backgroundMusic;
            _backgroundMusicSource.volume = backgroundVolume;
            _backgroundMusicSource.loop = true;
            _backgroundMusicSource.Play();
            // _isBackgroundMusicPlaying = true;
        }
    
        private void SetBackgroundMusicVolume(float volume)
        {
            _backgroundMusicSource.volume = volume;
        }

        public void PlaySound(Vector3 position, string soundName, float volume = 1f, float pitch = 1f, bool loop = false,
            float spatialBlend = 0f)
        {
            var audioSource = AudioSourcePool.Instance.Get();
            if (audioSource != null)
            {
                if(!GetAudioClip(soundName)) return;
                audioSource.transform.position = position;
                audioSource.SetAudioClip(GetAudioClip(soundName));
                audioSource.SetVolume(volume);
                audioSource.SetPitch(pitch);
                audioSource.SetLoop(loop);
                audioSource.SetSpatialBlend(spatialBlend);
                audioSource.Play();
            }
        }

        public void StopSound(string soundName)
        {
            MyEvents.StopSound?.Invoke(soundName);
        }
        
        
        private AudioClip GetAudioClip(string soundName)
        {
            if (_audioClipDictionary.TryGetValue(soundName, out var audioClip))
            {
                return audioClip;
            }

            Debug.Log("Sound not found: " + soundName);
            return null;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (_backgroundMusicSource == null) return;
            if (hasFocus)
            {
                if (_shouldBackgroundMusicPlay)
                {
                    _backgroundMusicSource.UnPause();
                }
            }
            else
            {
                _backgroundMusicSource.Pause();
            }
        }

        private void PauseUnpauseBackgroundMusic()
        {
            // if (_isBackgroundMusicPlaying)
            // {
                if (_backgroundMusicSource.isPlaying)
                {
                    _backgroundMusicSource.Pause();
                    _shouldBackgroundMusicPlay = false;
                }
                else
                {
                    _backgroundMusicSource.UnPause();
                    _shouldBackgroundMusicPlay = true;
                }
            // }
        }
        public void StopBackgroundMusicPlaying()
        {
            _backgroundMusicSource.Stop();
            // _isBackgroundMusicPlaying = false;
        }

        public void StopAreaSounds()
        {
            if(areaTypeData.TryGetAreaType(_currentAreaIndex, out var currentAreaType))
            {
                if (areaTypeAudioData.TryGetAreaTypeAudio(currentAreaType, out var areaTypeAudio))
                {
                    StopSound(areaTypeAudio.audioClip.name);
                }
            }
        }
    }
}