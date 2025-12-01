using UnityEngine;

public class ContractSwayPulse : MonoBehaviour
{
    public float swayAmount = 1f;
    public float swaySpeed = 1f;

    public float pulseAmount = 0.05f;   // 5% bigger/smaller
    public float pulseSpeed = 2f;

    private float startRotation;
    private Vector3 startScale;

    void Start()
    {
        startRotation = transform.localEulerAngles.z;
        startScale = transform.localScale;
    }

    void Update()
    {
        float t = Time.time;

        // sway
        float angle = startRotation + Mathf.Sin(t * swaySpeed) * swayAmount;
        transform.localEulerAngles = new Vector3(0f, 0f, angle);

        // pulse
        float pulse = 1f + Mathf.Sin(t * pulseSpeed) * pulseAmount;
        transform.localScale = startScale * pulse;
    }
}