using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHP = 5;
    public bool isPlayer = false;           
    public float hurtInvulnTime = 0.3f;     
    public float hp { get; private set; }
    private bool invuln;

    [Header("Death")]
    public GameObject deathVFX;             

    private Animator anim;
    private Collider2D[] cols;
    private Rigidbody2D rb;

    [Header("Enemy Behavior")]
    public bool canChase = false;
    public Transform player;
    public float speed = 3f;

    void Awake()
    {
        hp = maxHP;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cols = GetComponentsInChildren<Collider2D>(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TakeHit(1);

        if (canChase && !isPlayer && hp > 0 && player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    public void TakeHit(int amount)
    {
        if (invuln || hp <= 0) return;

        hp -= Mathf.Max(1, amount);

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

        foreach (var c in cols) c.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script != this) script.enabled = false;
        }

        if (deathVFX) Instantiate(deathVFX, transform.position, Quaternion.identity);

        if (!isPlayer) Destroy(gameObject, 0.75f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canChase = true;
        }
    }
}
