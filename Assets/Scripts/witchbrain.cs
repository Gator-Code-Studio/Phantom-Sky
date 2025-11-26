using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class WitchBrain : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    public Transform player;
    public Transform firePoint;
    public GameObject flameProjectilePrefab;

    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Ranges")]
    [Range(0.5f, 10f)]
    public float attackRange = 1f;

    [Range(0.5f, 15f)]
    public float walkRange = 5f;

    [Header("Combat")]
    public float attackCooldown = 1.5f;
    public float projectileSpeed = 3f;

    private Rigidbody2D rb;
    private float nextAttackTime = 0f;
    private bool shouldMove = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        if (anim == null) anim = GetComponent<Animator>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (firePoint == null) firePoint = transform.Find("FirePoint");
    }

    void Update()
    {
        if (player == null) return;

        FacePlayer();
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= walkRange && distance > attackRange)
        {
            shouldMove = true;
            anim.SetBool("move", true);
        }
        else if (distance <= attackRange)
        {
            shouldMove = false;
            SetAttackState();
        }
        else
        {
            shouldMove = false;
            anim.SetBool("move", false);
        }
    }

    void FixedUpdate()
    {
        if (!shouldMove || player == null) return;

        Vector2 targetPos = new Vector2(player.position.x, player.position.y + 0.2f);
        Vector2 newPos = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    void FacePlayer()
    {
        float scaleX = player.position.x > transform.position.x ? 1f : -1f;
        transform.localScale = new Vector3(scaleX, 1f, 1f);
    }

    void SetAttackState()
    {
        anim.SetBool("move", false);

        if (Time.time >= nextAttackTime)
        {
            anim.SetTrigger("attack");
            nextAttackTime = Time.time + attackCooldown;

            // Delay spawn to sync with animation
            Invoke(nameof(SpawnFlame), 0.4f);
        }
    }

    void SpawnFlame()
    {
        if (flameProjectilePrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(flameProjectilePrefab, firePoint.position, firePoint.rotation);

        // ENABLE THE HITBOX HERE
        EnemyAttackHitbox hitbox = proj.GetComponent<EnemyAttackHitbox>();
        if (hitbox != null)
        {
            hitbox.EnableHitbox();
        }

        // Rigidbody movement
        Rigidbody2D prb = proj.GetComponent<Rigidbody2D>();
        Vector2 dir = (player.position - firePoint.position).normalized;

        float facing = Mathf.Sign(transform.localScale.x);
        proj.transform.localScale = new Vector3(facing, 1f, 1f);

        prb.linearVelocity = dir * projectileSpeed;

        Destroy(proj, 3f);
    }
}
