using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHP = 5;
    public bool isPlayer = false;
    public float hurtInvulnTime = 0.3f;
    private int hp;
    private bool invuln;

    [Header("Death")]
    public GameObject deathVFX;

    private Animator anim;
    private Collider2D[] cols;
    private Rigidbody2D rb;

    void Awake()
    {
        hp = maxHP;
        anim = GetComponent<Animator>();
        cols = GetComponentsInChildren<Collider2D>(true);
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeHit(int amount)
    {
        if (invuln || hp <= 0) return;

        hp -= Mathf.Max(1, amount);

        if (isPlayer) Debug.Log($"PLAYER HIT! HP now {hp}");
        else Debug.Log($"{name} hit! HP now {hp}");

        if (hp > 0)
        {
            if (anim) anim.SetTrigger("Hurt");
            if (isPlayer) StartInvuln();
        }
        else
        {
            hp = 0;
            Die();
        }
    }

    private void StartInvuln()
    {
        if (!gameObject.activeInHierarchy) return;
        invuln = true;
        Invoke(nameof(EndInvuln), hurtInvulnTime);
    }

    private void EndInvuln() => invuln = false;

    private void Die()
    {
        if (anim) anim.SetBool("Dead", true);

        foreach (var c in GetComponentsInChildren<Collider2D>(true))
            c.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;  
        }

        if (isPlayer)
        {
            var move = GetComponent<PlayerMovement>();
            if (move) move.enabled = false;

            var attack = GetComponent<PlayerAttack>();
            if (attack) attack.enabled = false;
        }

        if (deathVFX) Instantiate(deathVFX, transform.position, Quaternion.identity);

    }

    public void Despawn()
    {
        if (!isPlayer)
        {
            Destroy(gameObject);
        }
    }
}
