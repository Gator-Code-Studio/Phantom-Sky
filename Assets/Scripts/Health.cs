using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] int enemyMaxHP = 5;  // used for enemies only
    private int maxHP;                    // actual max HP used by logic

    public bool isPlayer = false;
    public float hurtInvulnTime = 0.3f;
    public float hp { get; private set; }
    private bool invuln;

    [Header("Death")] public GameObject deathVFX;
    private Animator anim;
    private Collider2D[] cols;
    private Rigidbody2D rb;

    [Header("Enemy Behavior")] public bool canChase = false;
    public Transform player;
    public float speed = 3f;
    private bool reportedKill;

    void Awake()
    {
        if (isPlayer)
        {
            maxHP = 5;           // player fixed at 5
        }
        else
        {
            maxHP = enemyMaxHP;  // enemies use serialized value
        }

        hp = maxHP;
        anim = GetComponent<Animator>();
        cols = GetComponentsInChildren<Collider2D>(true);
        rb = GetComponent<Rigidbody2D>();
        cols = GetComponentsInChildren<Collider2D>(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) TakeHit(1);
        if (canChase && !isPlayer && hp > 0 && player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    public void TakeHit(int amount)
    {
        if (invuln || hp <= 0) return;
        hp -= Mathf.Max(1, amount);
        if (isPlayer) Debug.Log("PLAYER HIT! HP now " + hp);
        else Debug.Log(name + " hit! HP now " + hp);

        // Wake bat-type enemies when they take damage
        if (!isPlayer)
        {
            BatBrain bat = GetComponent<BatBrain>();
            if (bat == null) bat = GetComponentInChildren<BatBrain>();
            if (bat == null) bat = GetComponentInParent<BatBrain>();
            if (bat != null)
            {
                bat.ForceWake();
            }
        }

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
        SpawnOnDeath spawner = GetComponent<SpawnOnDeath>();
        if (spawner != null)
        {
            spawner.SpawnPrefab();
        }

        if (anim) anim.SetBool("Dead", true);
        foreach (var c in GetComponentsInChildren<Collider2D>(true)) c.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script != this) script.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        if (isPlayer)
        {
            var move = GetComponent<PlayerMovement>();
            if (move) move.enabled = false;
            var attack = GetComponent<PlayerAttack>();
            if (attack) attack.enabled = false;

            StartCoroutine(ShowGameOverDelayed());
        }

        if (!isPlayer && !reportedKill)
        {
            reportedKill = true;
            if (PlayerActionReporter.Instance != null) PlayerActionReporter.Instance.ReportEnemyKilled();

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                Health playerHealth = playerObj.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.AddHealth(1f);
                }
            }
        }

        if (deathVFX) Instantiate(deathVFX, transform.position, Quaternion.identity);

        var respawn = GetComponent<EnemyRespawn>();
        if (respawn != null && respawn.enemyPrefab != null)
        {
            respawn.Respawn();
            Destroy(gameObject, respawn.respawnTime + 0.1f);
        }
        else
        {
            if (!isPlayer)
            {
                Destroy(gameObject, 0.75f);
            }
        }

        foreach (var c in GetComponentsInChildren<Collider2D>(true)) c.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canChase = true;
        }
    }

    void OnEnable()
    {
        hp = maxHP;
        invuln = false;
        reportedKill = false;

        if (anim == null) { anim = GetComponent<Animator>(); }
        if (rb == null) { rb = GetComponent<Rigidbody2D>(); }
        cols = GetComponentsInChildren<Collider2D>(true);

        foreach (var c in cols) { c.enabled = true; }
        if (rb != null)
        {
            rb.simulated = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
        }
        if (anim != null)
        {
            anim.enabled = true;
            anim.SetBool("Dead", false);
            anim.Rebind();
            anim.Update(0f);
        }
    }

    public void HealToFull()
    {
        hp = maxHP;
        Debug.Log("Player healed to full health!");
    }

    public void AddHealth(float amount)
    {
        hp += amount;
        if (hp > maxHP) hp = maxHP;
        if (isPlayer) Debug.Log("Player gained health, now " + hp);
    }

    private IEnumerator ShowGameOverDelayed()
    {
        yield return new WaitForSeconds(1f);

        if (GameOverScreen.Instance != null)
        {
            GameOverScreen.Instance.Show();
        }
    }
}
