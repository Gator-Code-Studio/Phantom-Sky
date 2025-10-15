using UnityEngine;

public class SimpleDragonFollowExplode : MonoBehaviour
{
    [Header("References")]
    public Transform target;     
    public Animator animator;     

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private AudioManager audioManager;

    [Header("Detection Settings")]
    public float followRange = 2f;      // Distance to start following the player

    [Header("Movement Settings")]
    public float speed = 2.5f;          // Movement speed
    public float accel = 8f;            // Smooth acceleration
    public float hoverAmplitude = 0.15f;
    public float hoverFrequency = 2.0f;

    [Header("Explosion Settings")]
    public float explosionDistance = 0.8f;  
    public float explosionDuration = 0.6f;  

    private bool isFollowing = false;
    private bool hasExploded = false;
    private float hoverTime = 0f;
    private float explosionTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p) target = p.transform;
    }

    void Update()
    {
        if (hasExploded)
        {
            explosionTimer += Time.deltaTime;
            if (explosionTimer >= explosionDuration)
                Destroy(gameObject);
            return;
        }

        if (!target || !animator) return;

        float distance = Vector2.Distance(transform.position, target.position);

        // Start following when the player is close enough
        if (!isFollowing && distance <= followRange)
        {
            isFollowing = true;
            animator.SetTrigger("StartFollow");
            audioManager.PlaySFX(audioManager.followSFX); 
        }

        if (!isFollowing) return;

        if (sr)
            sr.flipX = target.position.x < transform.position.x;

        // Trigger explosion when close enough
        if (distance <= explosionDistance)
        {
            TriggerExplosion();
            return;
        }

        // Hover motion
        hoverTime += Time.deltaTime * hoverFrequency;
        float hoverOffset = Mathf.Sin(hoverTime) * hoverAmplitude;

        Vector2 direction = (target.position - transform.position).normalized;
        Vector2 desiredVel = new Vector2(
            direction.x * speed,
            direction.y * speed * 0.6f + hoverOffset
        );

        rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, desiredVel, accel * Time.deltaTime);
    }

    private void TriggerExplosion()
    {
        hasExploded = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Explode");
        audioManager.PlaySFX(audioManager.explodeSFX); 
        }

    public void OnExplosionFinished()
    {
        Destroy(gameObject);
    }
}
