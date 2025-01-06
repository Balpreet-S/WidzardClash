using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// this code is responsable for the castle health and animation throughout the damage process
public class CastleHealth : MonoBehaviour
{
    [SerializeField] private CastleHealthBar _healthbar;

    public int currentHealth = 100; // Castle starts with 100 health
    public float animationSpeed = 1.0f; // Speed of the animation playback

    public List<GameObject> towerStates; // List of preloaded tower GameObjects for different health tiers

    public AudioClip transitionSound;
    private AudioSource audioSource;

    private int currentTier = -1; // Tracks the current health tier

    void Start()
    {
        // Ensure health starts at 100
        currentHealth = 100;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on CastleHealth object.");
        }

        // Initialize tower states
        UpdateTowerVisibility();

        _healthbar.UpdateHealthBar(100, currentHealth); // updates health bar
    }

    // Method to apply damage to the castle
    public void TakeDamage(int damage)
    {
        Debug.Log($"TakeDamage called: Initial health = {currentHealth}, Damage = {damage}");
        currentHealth -= damage;
        Debug.Log("The castle has been attacked! The new health is " + currentHealth);

        // Destroy the castle if health reaches 0 or below
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Clamp health to 0
            CastleDestroyed();
        }
        else
        {
            _healthbar.UpdateHealthBar(100, currentHealth); // updates health bar
            // Update the tower prefab based on current health
            UpdateTowerVisibility();
        }
    }

    // Update tower visibility
    void UpdateTowerVisibility()
    {
        int newTier;

        // Determine the appropriate tier based on health
        if (currentHealth > 75) newTier = 0; // Full health
        else if (currentHealth > 50) newTier = 1;
        else newTier = 2; // Damaged

        if (newTier != currentTier)
        {
            Debug.Log($"Switching from tier {currentTier} to tier {newTier}");

            // Deactivate all towers
            for (int i = 0; i < towerStates.Count; i++)
            {
                if (towerStates[i] != null)
                {
                    towerStates[i].SetActive(false);
                }
            }

            // Activate the new tier's tower
            if (newTier < towerStates.Count)
            {
                towerStates[newTier].SetActive(true);
                Debug.Log($"Activating tower: {towerStates[newTier].name}");
            }

            if (newTier > 0)
            {
                PlayTransitionSound();
            }

            currentTier = newTier;
        }
    }
    // playing the transition soound from one stage to another after being destroyed based on hp.
    void PlayTransitionSound()
    {
        if (audioSource != null && transitionSound != null)
        {
            audioSource.PlayOneShot(transitionSound);
        }
        else
        {
            Debug.Log("Transition sound or audio source is missing");
        }
    }

    // Destroy the castle method that causes the loss.
    void CastleDestroyed()
    {
        Debug.Log("The castle has been destroyed!");
        Debug.Log("You Lose!!");

        // Find all enemies in the scene and destroy them
        EnemyScript[] allEnemies = FindObjectsOfType<EnemyScript>();

        foreach (EnemyScript enemy in allEnemies)
        {
            enemy.Die(); // Call the Die method to destroy the enemy
        }

        // Pause the game
        Time.timeScale = 0;

        // Loads Game Over Scene
        SceneManager.LoadScene("Game Over");
    }

    // Get the current health of the castle
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
