using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = .8f;
    public float attackRange = .01f;
    private Animator animator;
    public EnemyAttackHitbox attackHitbox;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;


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

    public void EnableHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.EnableHitbox();
    }
    public void DisableHitbox()
    {
        if (attackHitbox != null)
            attackHitbox.DisableHitbox();
    }
}