using System;
using UnityEngine;

namespace Audio
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
        [SerializeField] private AudioSourceCategoryEnum audioSourceCategoryEnum;
    }
}