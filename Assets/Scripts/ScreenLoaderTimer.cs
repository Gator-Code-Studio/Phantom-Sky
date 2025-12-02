using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoaderTimer
{
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

    // Automatically start loading next scene after 14.3 seconds
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AutoUse()
    {
        SceneLoaderRunner runner = new GameObject("SceneLoaderRunner").AddComponent<SceneLoaderRunner>();
        runner.StartCoroutine(runner.LoadAfterDelayCoroutine(14.1f));
    }
}

public class SceneLoaderRunner : MonoBehaviour
{
    public System.Collections.IEnumerator LoadAfterDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneLoader.LoadNextScene();
        Destroy(gameObject);
    }
}