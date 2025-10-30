using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private HashSet<GameObject> portalObjects = new HashSet<GameObject>();
    [SerializeField] private Transform destination;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss")) { return; }
        bool isPlayer = collision.CompareTag("Player");

        if (portalObjects.Contains(collision.gameObject)) { return; }

        if (destination != null)
        {
            Portal destinationPortal;
            if (destination.TryGetComponent(out destinationPortal))
            {
                destinationPortal.portalObjects.Add(collision.gameObject);
            }

            collision.transform.position = destination.position;

            if (isPlayer && PlayerActionReporter.Instance != null)
            {
                PlayerActionReporter.Instance.ReportPortalUsed();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        portalObjects.Remove(collision.gameObject);
    }
}