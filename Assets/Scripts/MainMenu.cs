using UnityEngine;
using UnityEngine.SceneManagement;

<<<<<<< HEAD
public class NewMonoBehaviourScript : MonoBehaviour
=======
public class MainMenu : MonoBehaviour
>>>>>>> main
{
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit(); // steve: This will only quit when a real process is started.
    }
}
