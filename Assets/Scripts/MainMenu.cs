using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneLoader.LoadNextScene();
    }

    public void QuitGame()
    {
        Application.Quit(); // steve: This will only quit when a real process is started.
    }
}
