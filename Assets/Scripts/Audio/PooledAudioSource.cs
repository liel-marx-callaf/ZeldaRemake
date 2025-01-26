using System;
using System.Collections;
using Pool;
using UnityEngine;

public class PooledAudioSource : MonoBehaviour, IPoolable
{
    private AudioSource _audioSource;
    private static bool _isMuted = false;
    private float _originalVolume;


    private void OnEnable()
    {
        _audioSource = GetComponent<AudioSource>();
        // _originalVolume = _audioSource.volume;
        MyEvents.MuteSounds += MuteSounds;
    }

    private void OnDisable()
    {
        MyEvents.MuteSounds -= MuteSounds;
    }

    private void MuteSounds()
    {
        _isMuted = !_isMuted;
        if (!_isMuted) SetVolume(0);
        else SetVolume(_originalVolume);
    }

    public void SetAudioClip(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
    }

    public void SetPitch(float pitch)
    {
        _audioSource.pitch = pitch;
    }

    public void SetVolume(float volume)
    {
        _originalVolume = volume;
        _audioSource.volume = volume;
    }

    public void SetLoop(bool loop)
    {
        _audioSource.loop = loop;
    }

    public void SetSpatialBlend(float spatialBlend)
    {
        _audioSource.spatialBlend = spatialBlend;
    }

    public float GetClipLength()
    {
        return _audioSource.clip.length;
    }


    public void Play()
    {
        Debug.Log("Playing audio");
        _audioSource.Play();
        StartCoroutine(ReturnToPoolWhenFinished());
    }

    public void Stop()
    {
        _audioSource.Stop();
        AudioSourcePool.Instance.Return(this);
    }

    private IEnumerator ReturnToPoolWhenFinished()
    {
        float clipLength = GetClipLength();
        if (clipLength < 1) yield return new WaitForSeconds(1f);
        else yield return new WaitWhile(() => _audioSource.isPlaying);
        AudioSourcePool.Instance.Return(this);
    }

    public void Reset()
    {
        _audioSource.Stop();
    }
}