using UnityEngine;
using UnityEngine.SceneManagement;

// This is a 'static' class. It cannot be attached to an object.
// It just holds functions that you can call from anywhere.
public static class SceneLoader
{
    // This is a 'static' function.
    // Call it from any script by writing: SceneLoader.LoadNextScene();
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
            // Optional: Log a warning if at the end of the scene list
            Debug.LogWarning("Already at the last scene. Cannot load next scene.");
            
            // Or you could loop back to the main menu (scene 0)
            // SceneManager.LoadScene(0);
        }
    }
}