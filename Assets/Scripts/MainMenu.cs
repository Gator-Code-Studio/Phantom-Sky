using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("ArenaL1");
    }

    public void QuitGame()
    {
        Application.Quit(); // steve: This will only quit when a real process is started.
    }
}
