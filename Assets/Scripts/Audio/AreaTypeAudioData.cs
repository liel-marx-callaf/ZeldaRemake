using System;
using System.Collections.Generic;
using Helper;
using UnityEngine;
// using Audio;

namespace Audio
{
    [CreateAssetMenu(fileName = "AreaTypeAudioData", menuName = "Game Data/Area Type Audio Data")]
    public class AreaTypeAudioData : ScriptableObject
    {
        [Serializable]
        public class AreaTypeAudio
        {
            public AreaTypeEnum areaType;
            public bool hasAudio;
            public AudioClip audioClip;
            [Range(0f,1f)] public float volume = 1f;
        }
        
        public List<AreaTypeAudio> areaTypeAudios = new List<AreaTypeAudio>();
        
        public bool TryGetAreaTypeAudio(AreaTypeEnum areaType, out AreaTypeAudio audioSource)
        {
            foreach (var area in areaTypeAudios)
            {
                if (area.areaType == areaType)
                {
                    if (!area.hasAudio)
                    {
                        audioSource = null;
                        return false;
                    }
                    audioSource = area;
                    return true;
                }
            }
            audioSource = null;
            return false;
        }
    }
}
