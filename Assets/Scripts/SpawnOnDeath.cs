using UnityEngine;

/// <summary>
/// Attach this to any GameObject with a Health component (like an enemy).
/// When its health reaches zero, it will spawn a specified prefab.
/// This version uses a public method to be called directly, making it more reliable.
/// </summary>
public class SpawnOnDeath : MonoBehaviour
{
    [Header("Spawning Settings")]
    [Tooltip("The GameObject prefab to spawn when this object 'dies'.")]
    public GameObject prefabToSpawn;

    [Tooltip("(Optional) A specific location for the prefab to spawn. If left empty, it spawns at this object's position.")]
    public Transform spawnPoint;

    private bool hasSpawnedPrefab = false;


    public void SpawnPrefab()
    {
        // Check if we've already spawned something to prevent duplicates
        if (hasSpawnedPrefab) return;

        if (prefabToSpawn == null)
        {
            Debug.LogWarning(gameObject.name + " has nothing assigned to spawn on death.");
            return;
        }

        // Use the specific spawn point's position if assigned, otherwise use this object's position.
        Vector3 spawnPosition = (spawnPoint != null) ? spawnPoint.position : transform.position;

        // Create an instance of the prefab.
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        // Set the flag to true so it cannot be triggered again.
        hasSpawnedPrefab = true;
    }
}