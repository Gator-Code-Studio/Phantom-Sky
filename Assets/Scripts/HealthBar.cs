using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalhealthBar;
    [SerializeField] private Image currenthealthBar;

    private void Start()
    {
        totalhealthBar.fillAmount = playerHealth.hp / 10;
    }

    private void Update()
    {
        currenthealthBar.fillAmount = playerHealth.hp / 10;
    }
}
