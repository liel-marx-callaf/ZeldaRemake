using System.Collections;
using UnityEngine;
using System;

public class SideRoomManager : MonoBehaviour
{
    [Header("Player Spawn")]
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float fadeDuration = 1f;

    private void Start()
    {
        // 1) Find the Player in the new scene
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null && playerSpawnPoint != null)
        {
            player.transform.position = playerSpawnPoint.position;
        }

        // 2) Fade in
        // fadeAnimator.SetTrigger("Hide");  // "Hide" means fade from black to clear
        StartCoroutine(UnfreezeAfterFade());
    }

    private IEnumerator UnfreezeAfterFade()
    {
        yield return new WaitForSeconds(fadeDuration);
        // if immediate NPC text, skip unfreeze until text is done
        // otherwise, unfreeze now
        MyEvents.TogglePlayerFreeze?.Invoke();
    }
}