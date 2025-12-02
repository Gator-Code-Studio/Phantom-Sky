using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen Instance { get; private set; }
    
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeInDuration = 0.5f;
    
    private bool isShowing;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
        EnsureEventSystem();
    }

    private void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }

    public void Show()
    {
        if (isShowing) return;
        isShowing = true;
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        
        if (canvasGroup != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0f;
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = elapsed / fadeInDuration;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
