using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    public AudioSource source;
    public float startTime = 14.3f; // seconds

    void Start()
    {
        source.time = startTime;
        source.Play();
    }
}