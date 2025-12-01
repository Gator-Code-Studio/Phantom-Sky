using UnityEngine;

public class ForceTopLayerAndHealthBar : MonoBehaviour
{
    public GameObject healthBar;

    void Start()
    {
        Canvas c = GetComponent<Canvas>();
        c.overrideSorting = true;
        c.sortingOrder = 9999;

        if (healthBar != null)
        {
            healthBar.SetActive(true);
        }
    }
}