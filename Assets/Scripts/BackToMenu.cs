using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoReturnToMenu : MonoBehaviour
{
    public float totalDelay = 30f;
    public float fadeDuration = 2f;

    void Start()
    {
        StartCoroutine(ReturnRoutine());
    }

    IEnumerator ReturnRoutine()
    {
        float waitTime = totalDelay - fadeDuration;
        if (waitTime < 0f)
        {
            waitTime = 0f;
        }

        // Wait before starting fade
        yield return new WaitForSeconds(waitTime);

        // Try to fade PersistentMusic if it exists
        PersistentMusic music = FindObjectOfType<PersistentMusic>();
        AudioSource source = null;
        float startVolume = 0f;

        if (music != null)
        {
            source = music.GetComponent<AudioSource>();
            if (source != null)
            {
                startVolume = source.volume;

                float t = 0f;
                while (t < fadeDuration)
                {
                    t += Time.deltaTime;
                    float k = t / fadeDuration;
                    source.volume = Mathf.Lerp(startVolume, 0f, k);
                    yield return null;
                }

                source.volume = 0f;
                Destroy(music.gameObject);
            }
            else
            {
                // No AudioSource found, still wait fadeDuration to keep ~totalDelay
                yield return new WaitForSeconds(fadeDuration);
            }
        }
        else
        {
            // No PersistentMusic at all, just wait fadeDuration to keep ~totalDelay
            yield return new WaitForSeconds(fadeDuration);
        }

        SceneManager.LoadScene(0);
    }
}