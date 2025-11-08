// EnemyRespawn.cs
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] public float respawnTime = 3f;

    public void Respawn()
    {
        Invoke(nameof(Spawn), respawnTime);
    }

    void Spawn()
    {
        if (enemyPrefab == null) return;

        GameObject clone = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // Ensure physics and colliders are live
        foreach (var c in clone.GetComponentsInChildren<Collider2D>(true)) c.enabled = true;

        var rb = clone.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
        }

        var anim = clone.GetComponent<Animator>();
        if (anim != null)
        {
            anim.enabled = true;
            anim.Rebind();
            anim.Update(0f);
            anim.SetBool("Dead", false);
        }
    }
}