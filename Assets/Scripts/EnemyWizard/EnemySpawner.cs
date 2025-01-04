using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject enemyPrefab4;

    [Header("Spawn & Waypoints")]
    public Transform spawnPoint;
    public Transform[] waypoints;

    [Header("Wave Settings")]
    public int initialEnemyCount = 5;
    public float spawnInterval = 1f;
    public float initialSpawnDelay = 5f;
    public int waveCount = 5;
    
    private int currentWave = 0;
    private int enemiesToSpawn;
    private int enemiesRemaining;

    private WaveManager waveManager;

    void Start()
    {
        waveManager = GetComponent<WaveManager>();
        Debug.DrawRay(spawnPoint.position, spawnPoint.forward * 2, Color.magenta, 2f);
        StartCoroutine(StartWaveAfterDelay());
    }

    IEnumerator StartWaveAfterDelay()
    {
        yield return new WaitForSeconds(initialSpawnDelay);
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWave++;
        if (currentWave <= waveCount)
        {
            // Increase enemy count per wave
            enemiesToSpawn = initialEnemyCount + (currentWave - 1) * 6;
            enemiesRemaining = enemiesToSpawn;

            StartCoroutine(SpawnEnemies());
        }
        else
        {
            Debug.Log("All waves completed!");
        }
    }

    IEnumerator SpawnEnemies()
    {
        // 1) Get the list of available enemy types for this wave
        List<GameObject> availablePrefabs = GetPrefabsForWave(currentWave);

        // 2) Spawn enemies one at a time
        while (enemiesToSpawn > 0)
        {
            // 3) Pick one prefab from the list to spawn
            int index = Random.Range(0, availablePrefabs.Count);
            GameObject prefabToSpawn = availablePrefabs[index];
            
            SpawnEnemy(prefabToSpawn);

            enemiesToSpawn--;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /// <summary>
    /// Returns a list of enemy prefabs allowed for the given wave.
    /// Example logic:
    ///   Wave 1: Enemy 1
    ///   Wave 2: Enemy 1, 2
    ///   Wave 3: Enemy 1, 2, 3
    ///   Wave 4+: Enemy 1, 2, 3, 4
    /// </summary>
    private List<GameObject> GetPrefabsForWave(int wave)
    {
        List<GameObject> enemyList = new List<GameObject>();

        // Wave 1
        if (wave >= 1) enemyList.Add(enemyPrefab1);

        // Wave 2
        if (wave >= 2) enemyList.Add(enemyPrefab2);

        // Wave 3
        if (wave >= 3) enemyList.Add(enemyPrefab3);

        // Wave 4+
        if (wave >= 4) enemyList.Add(enemyPrefab4);

        return enemyList;
    }

    
    void SpawnEnemy(GameObject prefab)
    {
        GameObject newEnemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
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

    void OnEnemyDeath()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            Debug.Log("Wave " + currentWave + " complete.");
            waveManager.NextWave();
            StartNextWave();
        }
    }
}
