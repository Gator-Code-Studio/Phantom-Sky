using UnityEngine;

public class GlowPulse : MonoBehaviour
{
    public float glowStrength = 0.3f;    // how bright it gets
    public float glowSpeed = 2f;         // how fast it pulses

    private SpriteRenderer sr;
    private Color baseColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * glowSpeed) + 1f) * 0.5f; 
        float glow = 1f + t * glowStrength; 
        sr.color = baseColor * glow;
    }
}