using System;
using UnityEngine.Audio;
using UnityEngine;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField, Range(0f, 1f)] private float backgroundVolume = 0.5f;
    private AudioSource _backgroundMusicSource;
    private bool _isBackgroundMusicPlaying = false;
    private bool _shouldBackgroundMusicPlay = true;
    private bool _isMuted = false;

    private void Start()
    {
            PlayBackgroundMusic();
    }

    private void OnEnable()
    {
        MyEvents.PauseUnpauseBackgroundMusic += PauseUnpauseBackgroundMusic;
        MyEvents.MuteSounds += MuteSounds;
    }
    
    private void OnDisable()
    {
        if (MyEvents.PauseUnpauseBackgroundMusic != null)
        {
            MyEvents.PauseUnpauseBackgroundMusic -= PauseUnpauseBackgroundMusic;
        }
        if (MyEvents.MuteSounds != null)
        {
            MyEvents.MuteSounds -= MuteSounds;
        }
    }

    private void MuteSounds()
    {
        _isMuted = !_isMuted;
    }


    private void PlayBackgroundMusic()
    {
        _backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        _backgroundMusicSource.clip = GetAudioClip("background_music");
        _backgroundMusicSource.volume = backgroundVolume;
        _backgroundMusicSource.loop = true;
        _backgroundMusicSource.Play();
        _isBackgroundMusicPlaying = true;
    }

    public void PlaySound(Vector3 position, string soundName, float volume = 1f, float pitch = 1f, bool loop = false,
        float spatialBlend = 1f)
    {
        // if (!_isMuted)
        // {
            var audioSource = AudioSourcePool.Instance.Get();
            if (audioSource != null)
            {
                audioSource.transform.position = position;
                audioSource.SetAudioClip(GetAudioClip(soundName));
                audioSource.SetVolume(volume);
                audioSource.SetPitch(pitch);
                audioSource.SetLoop(loop);
                audioSource.SetSpatialBlend(spatialBlend);
                audioSource.Play();
            }
        // }
    }

    private AudioClip GetAudioClip(string soundName)
    {
        foreach (var audioClip in audioClips)
        {
            if (audioClip.name == soundName)
            {
                return audioClip;
            }
        }

        Debug.LogError("Sound not found");
        return null;
    }
    
    private void OnApplicationFocus(bool hasFocus)
    {
        if (_backgroundMusicSource != null)
        {
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
    }
    
    private void PauseUnpauseBackgroundMusic()
    {
        if (_isBackgroundMusicPlaying)
        {
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
        }
    }
}