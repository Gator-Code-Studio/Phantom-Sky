// JumpPad.cs
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float bounce = 20f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) { return; }

        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (rb.linearVelocity.y < 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            }
            rb.AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
        }

        if (PlayerActionReporter.Instance != null)
        {
            PlayerActionReporter.Instance.ReportJumpPadUsed();
        }
    }
}