using UnityEngine;

public class PersistentMusic : MonoBehaviour
{
    private static PersistentMusic instance;
    private AudioSource source;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        if (source != null && !source.isPlaying)
        {
            source.Play();
        }
    }
}