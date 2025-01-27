using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadNextLevel();
        }
        
    }

    private void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneIndexEnum.MainGame));
    }
    
    private IEnumerator LoadLevel(SceneIndexEnum sceneIndexEnum)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MainGame");
    }
}
