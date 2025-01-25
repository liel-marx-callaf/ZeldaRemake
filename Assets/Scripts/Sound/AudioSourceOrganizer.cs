using System;
using UnityEngine;

namespace Sound
{
    public enum AudioSourceCategoryEnum
    {
        BackgroundMusic,
        SFX,
        Voice
    }
    [Serializable]
    public class AudioSourceCategory
    {
        [SerializeField] private AudioSourceCategoryEnum _audioSourceCategoryEnum;
        
    }
}