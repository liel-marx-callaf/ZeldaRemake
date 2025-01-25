using Pool;
using UnityEngine;

public class AudioSourcePool : MonoPool<PooledAudioSource>
{
    public void Initialize(GameObject prefab, int initialSize, Transform parent)
    {
        base.Initialize(prefab, initialSize, parent);
    }
}