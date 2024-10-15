using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;      // The enemy prefab to be spawned
    public Transform spawnPoint;        // The point where enemies will spawn
    public Transform[] waypoints;       // The waypoints for the path the enemies will follow
    public int initialEnemyCount = 5;   // Initial number of enemies to spawn
    public float spawnInterval = 1f;    // Time delay between each spawn
    public float initialSpawnDelay = 5f;// Time delay before the first enemy spawns
    public int waveCount = 5;           // Total number of waves
    private int currentWave = 0;        // Tracks the current wave
    private int enemiesToSpawn;         // Number of enemies to spawn in current wave
    private int enemiesRemaining;       // Number of enemies remaining in current wave

    void Start()
    {
        // Start the first wave with an initial delay
        StartCoroutine(StartWaveAfterDelay());
    }

    // Coroutine to wait before starting the first wave
    IEnumerator StartWaveAfterDelay()
    {
        yield return new WaitForSeconds(initialSpawnDelay);
        StartNextWave();
    }

    // Start spawning enemies for the next wave
    void StartNextWave()
    {
        currentWave++;
        if (currentWave <= waveCount)
        {
            enemiesToSpawn = initialEnemyCount + (currentWave - 1) * 7; // Increase enemies by 5 each wave
            enemiesRemaining = enemiesToSpawn;

            StartCoroutine(SpawnEnemies());
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }

    // Coroutine to spawn enemies at intervals
    IEnumerator SpawnEnemies()
    {
        while (enemiesToSpawn > 0)
        {
            SpawnEnemy();  // Spawn a new enemy
            enemiesToSpawn--;
            yield return new WaitForSeconds(spawnInterval);  // Wait before spawning the next one
        }
    }

    // Method to instantiate and spawn a new enemy
    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyScript enemyScript = newEnemy.GetComponent<EnemyScript>();

        if (enemyScript != null)
        {
            enemyScript.Initialize(waypoints);

            // Subscribe to the OnDeath event to track when the enemy dies
            enemyScript.OnDeath += OnEnemyDeath;
        }
        else
        {
            Debug.LogError("No EnemyScript found on the spawned enemy!");
        }
    }

    // Called when an enemy dies
    void OnEnemyDeath()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            Debug.Log("Wave " + currentWave + " complete.");
            StartNextWave();  // Start the next wave when all enemies are dead
        }
    }
}