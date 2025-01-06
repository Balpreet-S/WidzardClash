using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
// code for the enemy spawner object that is responsable for waves and types of enemies
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject enemyPrefab1; //basic goblin prefab
    public GameObject enemyPrefab2; // speedy goblin prefab
    public GameObject enemyPrefab3; //heavy armor goblin prefab
    public GameObject enemyPrefab4; // Boss goblin prefab

    [Header("Spawn & Waypoints")]
    public Transform spawnPoint;
    public Transform[] waypoints;

    [Header("Wave Settings")]
    public int initialEnemyCount = 5;
    public float spawnInterval = 1f;
    public float initialSpawnDelay = 5f;
    public int waveCount = 20;

    // High Score
    public TextMeshProUGUI CurrentScoreCountText;
    public TextMeshProUGUI HighestScoreCountText;
    public int currentScore;

    private int currentWave = 0;
    private int enemiesToSpawn;
    private int enemiesRemaining;
    private WaveManager waveManager;


    void Start()
    {
        waveManager = GetComponent<WaveManager>();
        StartCoroutine(StartWaveAfterDelay()); //start wave after timer
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
            waveManager.NextWave(); //check which wizard buttons are available
            currentWave++; // Increment wave counter

            // Define custom logic for boss waves
            if (currentWave == 10)
            {
                // Wave 10: 1 boss only
                enemiesToSpawn = 1;
            }
            else if (currentWave == 15)
            {
                // Wave 15: 2 bosses only
                enemiesToSpawn = 2;
            }
            else if (currentWave == 20)
            {
                // Wave 20: 4 bosses only
                enemiesToSpawn = 4;
            }
            else
            {
                // Regular waves (no bosses)
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

    public void HighScoreUpdate() //updating the high score based on the current score scored by the user
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

            // Check for boss waves
            if (currentWave == 10 || currentWave == 15 || currentWave == 20)
            {
                prefabToSpawn = enemyPrefab4; // Spawn only the boss prefab
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
    //fetching prefabs for the different prefab types
    private List<GameObject> GetPrefabsForWave(int wave)
    {
        List<GameObject> enemyList = new List<GameObject>();

        // Boss waves
        if (wave == 10 || wave == 15 || wave == 20)
        {
            enemyList.Clear(); // Clear other enemies to ensure only the boss spawns
            enemyList.Add(enemyPrefab4);
            return enemyList;
        }

        // Regular waves
        if (wave >= 1) enemyList.Add(enemyPrefab1);
        if (wave >= 5) enemyList.Add(enemyPrefab2); // Introduce GoblinL1 at wave 5
        if (wave >= 10) enemyList.Add(enemyPrefab3); // Introduce GoblinL2 at wave 10

        return enemyList;
    }
    //spawn enemy by instentiation and add waypoints for enemy to follow
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
    //on death funtion to decrease the enemy when killed
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