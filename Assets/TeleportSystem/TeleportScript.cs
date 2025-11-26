using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }

        if (PlayerActionReporter.Instance != null)
        {
            PlayerActionReporter.Instance.ReportTeleporterUsed();
        }

        // Just call the function.
        SceneLoader.LoadNextScene();
    }
}