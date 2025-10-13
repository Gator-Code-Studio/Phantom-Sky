using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggles the pause menu on/off
            if (container.activeSelf)
            {
                ResumeButton();
            }
            else
            {
                container.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void ResumeButton()
    {
        container.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}