using System.Collections;
using Player;
using UnityEngine;


public class SideRoomPortal : MonoBehaviour
{
    // [Header("Scene to Load")] [SerializeField]
    // private string sideRoomSceneName = "StartingSideRoom";

    [Header("Fade and Animation")] [SerializeField]
    private Animator fadeAnimator; // fade to black

    [SerializeField] private float fadeDuration = 1f; // how long the fade is
    [SerializeField] private string enterAnimationTrigger = "EnterRoom";
    [SerializeField] private float enterAnimationTime = 1f; // how long the anim is

    private static bool _isTransitioning;

    private void OnEnable()
    {
        MyEvents.LoadScene += OnLoadScene;
        StartCoroutine(DisableTrigger());
    }

    private void OnDisable()
    {
        MyEvents.LoadScene -= OnLoadScene;
    }

    private void OnLoadScene(SceneIndexEnum enterSceneIndex, SceneIndexEnum exitSceneIndex)
    {
        StartCoroutine(DisableTrigger());
    }

    private IEnumerator DisableTrigger()
    {
        _isTransitioning = true;
        yield return new WaitForSeconds(1);
        _isTransitioning = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTransitioning) return;
        if (other.CompareTag("Player"))
        {
            // Start the enter side room routine
            StartCoroutine(EnterSideRoomRoutine(other.gameObject));
        }
    }

    private IEnumerator EnterSideRoomRoutine(GameObject player)
    {
        _isTransitioning = true;

        // 1) Freeze player input
        MyEvents.TogglePlayerFreeze?.Invoke();
        player.GetComponent<PlayerMovementController>().Teleport(transform.position - Vector3.up * 0.5f);

        // 2) Play "enter" animation on the player (in the main world)
        var animControl = player.GetComponent<PlayerAnimationControl>();
        if (animControl != null)
        {
            player.GetComponent<Animator>().SetTrigger(enterAnimationTrigger);
        }

        // Wait for that animation to finish
        yield return new WaitForSeconds(enterAnimationTime);
        MyEvents.TogglePlayerFreeze?.Invoke();
        // 3) Fade out
        fadeAnimator.SetTrigger("Start"); // "Start" is the fade-out trigger
        yield return new WaitForSeconds(fadeDuration);
        MyEvents.LoadScene?.Invoke(SceneIndexEnum.StartingSideRoom, SceneIndexEnum.MainGame);

        // 4) Load the side room scene
        // SceneManager.LoadScene(sideRoomSceneName);
        // The side room scene should handle spawning the player inside

        _isTransitioning = false;
    }
}