using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 0.8f;
    public float attackRange = 0.01f;
    public EnemyAttackHitbox attackHitbox;

    [Header("Trigger Settings")]
    public Collider2D chaseTrigger;

    private Animator animator;
    private Health health;
    private Rigidbody2D rb;
    private bool canChase = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();

        if (chaseTrigger != null)
        {
            chaseTrigger.isTrigger = true;

            // Add a trigger relay if the trigger is a different object
            TriggerRelay relay = chaseTrigger.gameObject.AddComponent<TriggerRelay>();
            relay.enemyBrain = this;
        }
    }

    void Update()
    {
        if (player == null || health == null) return;
        if (health.hp <= 0 || rb == null || !rb.simulated || !canChase) return;

        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        float direction = player.position.x - transform.position.x;
        if (direction != 0)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(direction) * Mathf.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z
            );
        }

        float distanceToPlayer = Mathf.Abs(player.position.x - transform.position.x);
        if (distanceToPlayer <= attackRange && animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void ActivateChase()
    {
        canChase = true;
    }

    public void EnableHitbox()
    {
        if (attackHitbox != null) attackHitbox.EnableHitbox();
    }

    public void DisableHitbox()
    {
        if (attackHitbox != null) attackHitbox.DisableHitbox();
    }
}

public class TriggerRelay : MonoBehaviour
{
    public EnemyBrain enemyBrain;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyBrain.ActivateChase();
        }
    }
}
