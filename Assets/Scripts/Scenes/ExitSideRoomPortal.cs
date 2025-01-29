using System.Collections;
using UnityEngine;

public class ExitSideRoomPortal : MonoBehaviour
{
    // [SerializeField] private string mainWorldSceneName = "MainWorld";
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float fadeDuration = 1f;
    // [SerializeField] private string exitAnimationTrigger = "ExitRoom";
    // [SerializeField] private float exitAnimationTime = 1f;

    private bool _isTransitioning;

    private void OnEnable()
    {
        StartCoroutine(DisableTrigger());
        MyEvents.LoadScene += OnLoadScene;
    }
    
    private void OnDisable()
    {
        MyEvents.LoadScene -= OnLoadScene;
    }

    private void OnLoadScene(SceneIndexEnum arg1, SceneIndexEnum arg2)
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
            StartCoroutine(ExitRoutine());
        }
    }

    private IEnumerator ExitRoutine()
    {
        _isTransitioning = true;
        // MyEvents.TogglePlayerFreeze?.Invoke();

        // (Optional) play exit animation in the side room
        // var animControl = player.GetComponent<PlayerAnimationControl>();
        // if (animControl != null)
        // {
        //     player.GetComponent<Animator>().SetTrigger(exitAnimationTrigger);
        // }
        // yield return new WaitForSeconds(exitAnimationTime);

        // fade out
        fadeAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(fadeDuration);
        MyEvents.LoadScene?.Invoke(SceneIndexEnum.MainGame, SceneIndexEnum.StartingSideRoom);

        // load main world
        // SceneManager.LoadScene(mainWorldSceneName);

        _isTransitioning = false;
    }
}