using UnityEngine;
using System.Collections;

public class ContractSquash : MonoBehaviour
{
    public float squashScaleY = 0.7f;   // how flat it gets (Y smaller)
    public float squashScaleX = 1.1f;   // how wide it gets (X bigger)
    public float squashTime = 0.08f;    // time to go down
    public float recoverTime = 0.12f;   // time to go back up

    private Vector3 originalScale;
    private bool isSquashing;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isSquashing)
        {
            return;
        }

        if (collision.collider.CompareTag("Player"))
        {
            StartCoroutine(Squash());
        }
    }

    IEnumerator Squash()
    {
        isSquashing = true;

        Vector3 squashed = new Vector3(
            originalScale.x * squashScaleX,
            originalScale.y * squashScaleY,
            originalScale.z
        );

        float t = 0f;
        while (t < squashTime)
        {
            t += Time.deltaTime;
            float lerp = t / squashTime;
            transform.localScale = Vector3.Lerp(originalScale, squashed, lerp);
            yield return null;
        }

        t = 0f;
        while (t < recoverTime)
        {
            t += Time.deltaTime;
            float lerp = t / recoverTime;
            transform.localScale = Vector3.Lerp(squashed, originalScale, lerp);
            yield return null;
        }

        transform.localScale = originalScale;
        isSquashing = false;
    }
}