// CollectableItem.cs
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public static int itemsCollected = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }

        itemsCollected = itemsCollected + 1;
        Debug.Log("Item collected! Total items: " + itemsCollected);

        if (PlayerActionReporter.Instance != null)
        {
            PlayerActionReporter.Instance.ReportCollectiblePicked();
        }

        Destroy(gameObject);
    }

    public static void ResetItemCount()
    {
        itemsCollected = 0;
    }
}