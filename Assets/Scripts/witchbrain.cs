using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class WitchBrain : MonoBehaviour
{
    // references
    [Header("References")]
    public Animator anim;
    public Transform player;
    public Transform firePoint;
    public GameObject flameProjectilePrefab;

    // movement
    [Header("Movement")]
    public float moveSpeed = 1f;
    [Space]
    public float hoverHeight = 0.2f;       
    public float hoverHeightVariance = 0.05f;  
    public float bobFrequency = 1.8f;
    [Space]
    public float predictionTime = 0.25f;      
    public float smoothTime = 0.12f;          
    // combat
    [Header("Combat")]
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    public float projectileSpeed = 1f;

    // debug
    [Header("Debug")]
    public bool drawGizmos = true;

    // private
    private Rigidbody2D rb;
    private float nextAttackTime = 0f;
    private float bobTimer = 0f;
    private Vector2 velocity;  // For SmoothDamp

    // unity
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

        float distance = Vector2.Distance(transform.position, player.position);
        FacePlayer();

        if (distance <= attackRange)
        {
            SetAttackState();
        }
        else
        {
            SetMoveState();
        }
    }

    // movement
    void FacePlayer()
    {
        float scaleX = player.position.x > transform.position.x ? 1f : -1f;
        transform.localScale = new Vector3(scaleX, 1f, 1f);
    }

    void SetMoveState()
    {
        anim.SetBool("move", true);
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        // Predict player position
        Vector2 predictedPos = (Vector2)player.position;
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            predictedPos += playerRb.velocity * predictionTime;
        }

        // Target: always above player with smooth sine bob
        bobTimer += Time.deltaTime * bobFrequency;
        float bobOffset = Mathf.Sin(bobTimer + Mathf.PI * 0.25f) * hoverHeightVariance * 0.7f;
        Vector2 targetPos = new Vector2(predictedPos.x, predictedPos.y + hoverHeight + bobOffset);

        Vector2 smoothPos = Vector2.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
        rb.MovePosition(smoothPos);
    }

    void SetAttackState()
    {
        anim.SetBool("move", false);
        if (Time.time >= nextAttackTime)
        {
            anim.SetTrigger("attack");
            nextAttackTime = Time.time + attackCooldown;
            Invoke(nameof(SpawnFlame), 0.4f);
        }
    }

    void SpawnFlame()
    {
        if (flameProjectilePrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(flameProjectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D prb = proj.GetComponent<Rigidbody2D>();
        Vector2 dir = (player.position - firePoint.position).normalized;
        prb.velocity = dir * projectileSpeed;
        Destroy(proj, 3f);
    }

    // gizmos
    void OnDrawGizmosSelected()
    {
        if (!drawGizmos || player == null) return;

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Hover target (visual debug)
        bobTimer += Time.deltaTime * bobFrequency;
        float bobOffset = Mathf.Sin(bobTimer + Mathf.PI * 0.25f) * hoverHeightVariance * 0.7f;
        Vector2 predictedPos = (Vector2)player.position;
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null) predictedPos += playerRb.velocity * predictionTime;

        Vector2 targetPos = new Vector2(predictedPos.x, predictedPos.y + hoverHeight + bobOffset);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(targetPos, 0.3f);
        Gizmos.DrawLine(transform.position, targetPos);
    }
}