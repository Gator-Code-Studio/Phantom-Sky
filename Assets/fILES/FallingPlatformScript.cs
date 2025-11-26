using System;
using System.Collections;
using UnityEngine;

public class FallingPlatformScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float fallTime;

    private Coroutine fallCoroutine;









    private void OnCollisionEnter2D(Collision2D collision)
    {
       fallCoroutine= StartCoroutine(PlatformFall());
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        StopCoroutine(fallCoroutine);
        fallCoroutine = null;
    }

    IEnumerator PlatformFall()
    {

        yield return new WaitForSeconds(fallTime);
        rb.bodyType = RigidbodyType2D.Dynamic;

        Destroy(this.gameObject, 3f);



    }
}