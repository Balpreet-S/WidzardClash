using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // enemy spawner variables
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform[] waypoints;
    public int initialEnemyCount = 5;
    public float spawnInterval = 1f;
    public float initialSpawnDelay = 5f;
    public int waveCount = 5;
    private int currentWave = 0;
    private int enemiesToSpawn;
    private int enemiesRemaining;

    void Start()
    {
        // delay for the first wave spawn
        StartCoroutine(StartWaveAfterDelay());
    }

    IEnumerator StartWaveAfterDelay()
    {
        yield return new WaitForSeconds(initialSpawnDelay);
        StartNextWave();
    }

    // enemy spawner for next waves
    void StartNextWave()
    {
        currentWave++;
        if (currentWave <= waveCount)
        {
            enemiesToSpawn = initialEnemyCount + (currentWave - 1) * 6;
            enemiesRemaining = enemiesToSpawn;

            StartCoroutine(SpawnEnemies());
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }

    // interval so that enemies dont spawn all at once
    IEnumerator SpawnEnemies()
    {
        while (enemiesToSpawn > 0)
        {
            SpawnEnemy();
            enemiesToSpawn--;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Method to instantiate (getting enemies from the prefabs) and spawning a new enemy
    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyScript enemyScript = newEnemy.GetComponent<EnemyScript>();

        if (enemyScript != null)
        {
            enemyScript.Initialize(waypoints);

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
            StartNextWave();  // wave starts after all current enemies are dead
        }
    }
}