using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject enemyPrefab4; // Boss zombie prefab

    [Header("Spawn & Waypoints")]
    public Transform spawnPoint;
    public Transform[] waypoints;

    [Header("Wave Settings")]
    public int initialEnemyCount = 5;
    public float spawnInterval = 1f;
    public float initialSpawnDelay = 5f;
    public int waveCount = 20; // 20 waves

    // High Score
    public TextMeshProUGUI CurrentScoreCountText;
    public TextMeshProUGUI HighestScoreCountText;
    public int currentScore;

    private int currentWave = 0;
    private int enemiesToSpawn;
    private int enemiesRemaining;

    void Start()
    {
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
        if (currentWave < waveCount)
        {
            currentWave++; // Increment wave counter

            if (currentWave == waveCount)
            {
                // Boss wave: Only 1 enemy to spawn
                enemiesToSpawn = 1;
            }
            else
            {
                // Regular waves
                enemiesToSpawn = Mathf.FloorToInt(initialEnemyCount + (currentWave - 1) * 6 * (1 + currentWave / 20f));
            }
            enemiesRemaining = enemiesToSpawn;

            Debug.Log($"Starting Wave {currentWave} with {enemiesToSpawn} enemies.");

            StartCoroutine(SpawnEnemies()); // Start spawning enemies for the wave
        }
        else
        {
            Debug.Log("All waves completed!");
        }

        currentScore = currentWave;
        HighScoreUpdate(); // Update the high score UI
    }

    public void HighScoreUpdate()
    {
        if (PlayerPrefs.HasKey("SavedHighScore"))
        {
            if (currentScore > PlayerPrefs.GetInt("SavedHighScore"))
            {
                PlayerPrefs.SetInt("SavedHighScore", currentScore);
            }
        }
        else
        {
            PlayerPrefs.SetInt("SavedHighScore", currentScore);
        }

        CurrentScoreCountText.text = $"Current Wave: {currentWave}";
        HighestScoreCountText.text = $"Highest Wave Achieved: {PlayerPrefs.GetInt("SavedHighScore").ToString()}";
    }

    IEnumerator SpawnEnemies()
    {
        // Get the list of available enemy types for this wave
        List<GameObject> availablePrefabs = GetPrefabsForWave(currentWave);

        // Spawn enemies one at a time
        while (enemiesToSpawn > 0)
        {
            GameObject prefabToSpawn;

            if (currentWave == waveCount)
            {
                // Spawn the boss zombie for wave 20
                prefabToSpawn = enemyPrefab4;
            }
            else
            {
                // Pick one prefab from the available list
                int index = Random.Range(0, availablePrefabs.Count);
                prefabToSpawn = availablePrefabs[index];
            }

            SpawnEnemy(prefabToSpawn);

            enemiesToSpawn--;
            yield return new WaitForSeconds(spawnInterval);
        }

        // Wait for all enemies in the wave to be defeated before starting the next wave
        while (enemiesRemaining > 0)
        {
            yield return null;
        }

        Debug.Log($"Wave {currentWave} complete!");
        StartNextWave();
    }

    private List<GameObject> GetPrefabsForWave(int wave)
    {
        List<GameObject> enemyList = new List<GameObject>();

        // Unlock enemies dynamically based on wave number
        if (wave >= 1) enemyList.Add(enemyPrefab1);
        if (wave >= 5) enemyList.Add(enemyPrefab2); // Introduce GoblinL1 at wave 5
        if (wave >= 10) enemyList.Add(enemyPrefab3); // Introduce GoblinL2 at wave 10

        // Boss zombie spawns exclusively in wave 20
        if (wave == 20)
        {
            enemyList.Clear(); // Clear other enemies to ensure only the boss spawns
            enemyList.Add(enemyPrefab4);
        }

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
            StartNextWave();
        }
    }
}