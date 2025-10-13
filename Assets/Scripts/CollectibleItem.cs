using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public static int itemsCollected = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            itemsCollected += 1;

            Debug.Log("Item collected! Total items: " + itemsCollected);
            

            Destroy(gameObject);
        }
    }
    
    public static void ResetItemCount()
    {
        itemsCollected = 0;
    }
}