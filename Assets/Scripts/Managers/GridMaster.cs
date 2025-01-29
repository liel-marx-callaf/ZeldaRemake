using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class GridMaster : MonoBehaviour
{
    [Header("Tilemaps To Fade")]
    [SerializeField] private Tilemap levelTilemapPlayerEdges;
    [SerializeField] private Tilemap levelEntrances;
    [SerializeField] private Tilemap levelTilemapBackground;
    
    [Header("Death Animation")]
    [SerializeField] private float deathAnimationDuration = 3f;
    
    
    private void OnEnable()
    {
        MyEvents.PlayerDeath += OnPlayerDeath;
    }
    
    private void OnDisable()
    {
        MyEvents.PlayerDeath -= OnPlayerDeath;
    }
    
    private void OnPlayerDeath()
    {
        StartCoroutine(BackgroundFadeRoutine());
    }
    
    private IEnumerator BackgroundFadeRoutine()
    {
        // 3-phase fade: normal -> red -> dark -> black
        // We'll do a simple example with linear interpolation
        float duration = deathAnimationDuration; 
        float elapsed = 0f;

        Color startColor = Color.white;  // tilemap's original color
        Color midColor = Color.red;      // halfway color
        Color endColor = Color.black;    // final color

        // Phase 1: white -> red (first third of time)
        while (elapsed < duration / 3f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (duration / 3f);
            Color current = Color.Lerp(startColor, midColor, t);

            levelTilemapPlayerEdges.color = current;
            levelEntrances.color = current;
            levelTilemapBackground.color = current;

            yield return null;
        }

        // Phase 2: red -> black (second third of time)
        float secondPhaseTime = 0f;
        while (secondPhaseTime < duration / 3f)
        {
            secondPhaseTime += Time.deltaTime;
            float t = secondPhaseTime / (duration / 3f);
            Color current = Color.Lerp(midColor, endColor, t);

            levelTilemapPlayerEdges.color = current;
            levelEntrances.color = current;
            levelTilemapBackground.color = current;

            yield return null;
        }
        float thirdPhaseTime = 0f;
        while (thirdPhaseTime < duration / 3f)
        {
            thirdPhaseTime += Time.deltaTime;


            yield return null;
        }
    }
}
