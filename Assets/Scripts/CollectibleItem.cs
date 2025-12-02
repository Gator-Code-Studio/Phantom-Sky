// CollectableItem.cs
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public static int itemsCollected = 0;

    [SerializeField] private GameObject objectToDestroy;
    [SerializeField] private int keysNeeded = 0;

    void Awake()
    {
        if (keysNeeded <= 0)
        {
            CollectableItem[] allKeys = FindObjectsOfType<CollectableItem>();
            keysNeeded = allKeys.Length;
            Debug.Log("Keys needed (auto): " + keysNeeded);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }

        itemsCollected = itemsCollected + 1;
        Debug.Log("Item collected! Total items: " + itemsCollected + " / " + keysNeeded);

        if (PlayerActionReporter.Instance != null)
        {
            PlayerActionReporter.Instance.ReportCollectiblePicked();
        }

        if (objectToDestroy != null && itemsCollected >= keysNeeded)
        {
            Debug.Log("All keys collected. Destroying: " + objectToDestroy.name);
            Destroy(objectToDestroy);
        }

        Destroy(gameObject);
    }

    public static void ResetItemCount()
    {
        itemsCollected = 0;
    }
}