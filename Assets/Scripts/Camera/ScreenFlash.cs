using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlash : MonoSingleton<ScreenFlash>
{
    [SerializeField] private SpriteRenderer flashImage;
    [SerializeField, Range(0f,2f)] private float flashDuration = 0.2f;

    public void FlashScreen()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        // Fade to white
        flashImage.color = new Color(1, 1, 1, 1);
        yield return new WaitForSecondsRealtime(flashDuration);
        // Fade back to clear
        flashImage.color = new Color(1, 1, 1, 0);
    }
}