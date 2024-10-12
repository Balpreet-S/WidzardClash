using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // The enemy prefab to be spawned
    public Transform spawnPoint;    // The point where enemies will spawn
    public Transform[] waypoints;   // The waypoints for the path the enemies will follow
    public int numberOfEnemies = 5; // Number of enemies to spawn
    public float spawnInterval = 1f; // Time delay between each spawn
    public float initialSpawnDelay = 5f; // Time delay before the first enemy spawns

    private int enemiesSpawned = 0; // Count of how many enemies have been spawned

    void Start()
    {
        // Start spawning enemies with an initial delay
        StartCoroutine(StartSpawningAfterDelay());
    }

    // Coroutine to wait for a delay before starting to spawn enemies
    IEnumerator StartSpawningAfterDelay()
    {
        // Wait for the initial delay (5 seconds)
        for (float countdown = initialSpawnDelay; countdown > 0; countdown--)
        {
            Debug.Log("Waiting for " + countdown + " seconds before starting enemy spawn...");
            yield return new WaitForSeconds(1f);  // Wait for 1 second between each countdown tick
        }

        // Start spawning enemies after the delay
        Debug.Log("Starting enemy spawn...");
        StartCoroutine(SpawnEnemies());
    }

    // Coroutine to spawn enemies at intervals
    IEnumerator SpawnEnemies()
    {
        while (enemiesSpawned < numberOfEnemies)
        {
            SpawnEnemy();  // Spawn a new enemy
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnInterval);  // Wait before spawning the next one
        }
    }

    // Method to instantiate and spawn a new enemy
    void SpawnEnemy()
    {
        // Instantiate the enemy prefab at the spawn point's position and rotation
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Get the EnemyScript component from the new enemy
        EnemyScript enemyScript = newEnemy.GetComponent<EnemyScript>();

        if (enemyScript != null)
        {
            // Initialize the enemy by passing the waypoints
            enemyScript.Initialize(waypoints);
        }
        else
        {
            Debug.LogError("No EnemyScript found on the spawned enemy!");
        }
    }
}
