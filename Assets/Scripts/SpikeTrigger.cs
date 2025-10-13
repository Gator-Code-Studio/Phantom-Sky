using UnityEngine;

/// <summary>
/// This script deals damage to any object with a Health component that enters its trigger.
/// It's designed to be placed on hazards like spikes, lava, or traps.
/// </summary>
public class SpikeTrigger : MonoBehaviour
{
    // The amount of damage this trigger will deal. Can be changed in the Inspector.
    [Tooltip("The amount of damage to deal to the player.")]
    public int damageAmount = 1;

    /// <summary>
    /// This method is called by Unity automatically when another 2D collider enters this object's trigger.
    /// </summary>
    /// <param name="other">The collider of the object that entered the trigger.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // First, we check if the object that entered the trigger has the "Player" tag.
        if (other.CompareTag("Player"))
        {
            // If it is the player, we try to get the "Health" component from it.
            Health playerHealth = other.GetComponent<Health>();

            // It's good practice to check if the component was actually found.
            if (playerHealth != null)
            {
                // If the Health component exists, call its public TakeHit method
                // and pass in the damage amount.
                playerHealth.TakeHit(damageAmount);
            }
        }
    }
}