using UnityEngine;

public class SpawnArrow : MonoBehaviour
{
    [Header("Prefab to Spawn")]
    public GameObject prefabToSpawn;

    [Header("Spawn Location (leave empty to use this object's position)")]
    public Transform spawnLocation;

    // Call this method to spawn the prefab
    public void SpawnPrefab()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Prefab to spawn is not assigned!");
            return;
        }

        Vector3 spawnPos = spawnLocation != null ? spawnLocation.position : transform.position;

        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}