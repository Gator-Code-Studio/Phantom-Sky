using UnityEngine;

public class BatBrain : MonoBehaviour
{
    [Header("Refs")]
    public Animator anim;                     
    public EnemyAttackHitbox attackHitbox;
    private AudioManager audioManager;

    [Header("Targeting")]
    public Transform target;                  
    public float wakeRange = 4f;
    public float chaseRange = 8f;
    public float attackRange = 1.1f;
    public float attackCooldown = 1.0f;

    [Header("Flight")]
    public float moveSpeed = 3.0f;
    public float accel = 10f;
    public float hoverAmplitude = 0.15f;
    public float hoverFrequency = 3.0f;

    // [Header("Attack Lunge")]
    // public float attackLungeSpeed = 5f;       // optional: small dash on attack

    [Header("Visual Facing")]
    public bool spriteFacesRight = true;      
    private SpriteRenderer sr;

    private Rigidbody2D rb;
    private Vector2 desiredVel;
    private float cooldown;
    private bool isAwake = false;
    private Vector3 perchPos;
    private float hoverTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!anim) anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        perchPos = transform.position;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {

        if (!target)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) target = p.transform;
        }

        float dt = Time.deltaTime;


        if (!isAwake)
        {
            if (target)
            {
                float d = Vector2.Distance(transform.position, target.position);
                if (d <= wakeRange)
                {
                    anim.SetTrigger("Wake");
                }
            }
            return;
        }


        cooldown -= dt;

        Vector2 toPlayer = target ? (Vector2)(target.position - transform.position) : Vector2.zero;
        float dist = target ? toPlayer.magnitude : Mathf.Infinity;


        if (target && sr != null)
        {
            bool playerIsToRight = (target.position.x - transform.position.x) >= 0f;

            sr.flipX = spriteFacesRight ? !playerIsToRight : playerIsToRight;
        }

        if (target && dist <= attackRange && cooldown <= 0f)
        {
            anim.SetTrigger("Attack");
            audioManager.PlaySFX(audioManager.batSFX);

            cooldown = attackCooldown;
            desiredVel = Vector2.zero; 
        }
        else if (target && dist <= chaseRange)
        {
            Vector2 dir = toPlayer.normalized;
            desiredVel = new Vector2(dir.x * moveSpeed, dir.y * moveSpeed * 0.6f);
        }
        else
        {
            Vector2 toPerch = (Vector2)(perchPos - transform.position);
            desiredVel = toPerch * 0.8f;
        }

        hoverTime += dt * hoverFrequency;
        float hoverOffset = Mathf.Sin(hoverTime) * hoverAmplitude;
        desiredVel.y += hoverOffset;


        Vector2 v = rb.linearVelocity;
        v = Vector2.MoveTowards(v, desiredVel, accel * dt);
        rb.linearVelocity = v;


        anim.SetFloat("Speed", v.magnitude);
    }

    // === Animation Events ===

    public void OnWakeFinished() { isAwake = true; }


    public void EnableHitbox() { if (attackHitbox) attackHitbox.EnableHitbox(); }
    public void DisableHitbox() { if (attackHitbox) attackHitbox.DisableHitbox(); }

    // optional: call once near the start of Attack.anim
    /* public void AttackLunge()
    {
        if (!target) return;
        Vector2 dir = ((Vector2)(target.position - transform.position)).normalized;
        rb.linearVelocity = dir * attackLungeSpeed;
    }
    */
}
