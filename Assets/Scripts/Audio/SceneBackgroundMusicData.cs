using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [CreateAssetMenu(fileName = "SceneBackgroundMusicData", menuName = "Game Data/Scene Background Music Data")]
    public class SceneBackgroundMusicData : ScriptableObject
    {
        [Serializable]
        public class SceneBackgroundMusic
        {
            public SceneIndexEnum sceneName;
            public AudioClip backgroundMusic;
        }
        
        public List<SceneBackgroundMusic> sceneBackgroundMusicList = new List<SceneBackgroundMusic>();
        
        
        public bool TryGetBackgroundMusic(SceneIndexEnum currentScene, out AudioClip backgroundMusic)
        {
            foreach (var sceneBackgroundMusic in sceneBackgroundMusicList)
            {
                if (sceneBackgroundMusic.sceneName == currentScene)
                {
                    backgroundMusic = sceneBackgroundMusic.backgroundMusic;
                    return true;
                }
            }
            backgroundMusic = null;
            return false;
        }
        
    }
}