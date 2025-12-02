using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicSource;
    public AudioSource SFXSource;

    [Header("Clips")]
    public AudioClip background;
    public AudioClip punch;
    public AudioClip sword;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip explodeSFX;
    public AudioClip followSFX;
    public AudioClip batSFX;
    public AudioClip enemySFX;
    public AudioClip Witch;

    [Header("Volume")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Awake()
    {
        // Enforce singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (background != null && musicSource != null)
        {
            musicSource.clip = background;
            musicSource.loop = true;
            musicSource.volume = musicVolume;   
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && SFXSource != null)
        {
            SFXSource.pitch = Random.Range(0.707f, 1.414f);
            SFXSource.PlayOneShot(clip, sfxVolume);  
            StartCoroutine(ResetPitchAfterDelay(clip.length));
        }
    }

    private IEnumerator ResetPitchAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SFXSource.pitch = 1f;
    }
}
