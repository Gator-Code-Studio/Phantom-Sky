using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsLoader : MonoBehaviour
{
    public float autoDelay = 14.1f;

    void Start()
    {
        StartCoroutine(LoadAfterDelayCoroutine(autoDelay));
    }

    public static void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("Already at the last scene. Cannot load next scene.");
        }
    }

    public IEnumerator LoadAfterDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadNextScene();
    }
}