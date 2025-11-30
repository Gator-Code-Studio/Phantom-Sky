using UnityEngine;
using System.Collections;
using TMPro;

public class TheCorruptedGroveTransition : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip transitionSound;

    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.2f;

    public TextMeshProUGUI runText;
    public float runTextDuration = 1.0f;
    public float runTextShakeMagnitude = 10f;

    private Vector3 originalPos;

    void Start()
    {
        originalPos = Camera.main.transform.localPosition;
        PlayTransition();
    }

    public void PlayTransition()
    {
        if (audioSource != null && transitionSound != null)
        {
            audioSource.PlayOneShot(transitionSound);
        }

        if (runText != null)
        {
            runText.text = "Run!!!";
            StartCoroutine(ShakeRunText());
        }

        StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeMagnitude, shakeMagnitude);
            float y = Random.Range(-shakeMagnitude, shakeMagnitude);

            Camera.main.transform.localPosition =
                new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }

    private IEnumerator ShakeRunText()
    {
        RectTransform rt = runText.rectTransform;
        Vector3 originalRunPos = rt.localPosition;
        float elapsed = 0f;

        runText.alpha = 1f;

        while (elapsed < runTextDuration)
        {
            float x = Random.Range(-runTextShakeMagnitude, runTextShakeMagnitude);
            float y = Random.Range(-runTextShakeMagnitude, runTextShakeMagnitude);

            rt.localPosition = new Vector3(originalRunPos.x + x, originalRunPos.y + y, originalRunPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rt.localPosition = originalRunPos;
        runText.alpha = 0f;
    }
}
