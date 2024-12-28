using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer; // Import Alembic namespace

public class CastleHealth : MonoBehaviour
{
    public int currentHealth = 100; // Castle starts with 100 health
    public float animationSpeed = 1.0f; // Speed of the animation playback

    public List<GameObject> towerStates; // List of preloaded tower GameObjects for different health tiers
    public List<AlembicStreamPlayer> alembicStreamPlayers; // List of Alembic Stream Players for each tower

    public AudioClip transitionSound;
    private AudioSource audioSource;

    private int currentTier = -1; // Tracks the current health tier
    private float animationTime = 0f; // Tracks the current time in the Alembic animation

    void Start()
    {
        // Ensure health starts at 100
        currentHealth = 100;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on CastleHealth object.");
        }

        // Initialize tower states and Alembic players
        UpdateTowerVisibility();


        // Reset the animations for all Alembic players
        foreach (AlembicStreamPlayer player in alembicStreamPlayers)
        {
            if (player != null)
            {
                player.CurrentTime = 0;
            }
        }
    }

    void Update()
    {
        // Continuously play the Alembic animation for the active player
        if (currentTier >= 0 && currentTier < alembicStreamPlayers.Count && alembicStreamPlayers[currentTier] != null)
        {
            AlembicStreamPlayer activePlayer = alembicStreamPlayers[currentTier];
            animationTime += Time.deltaTime * animationSpeed;

            // Loop the animation when it reaches the end
            if (animationTime > activePlayer.Duration)
            {
                animationTime = 0f; // Reset to the beginning
            }

            // Update the AlembicStreamPlayer's playback time
            activePlayer.CurrentTime = animationTime;
        }
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
            // Update the tower prefab based on current health
            UpdateTowerVisibility();
        }
    }

    // Update tower visibility and manage Alembic Stream Players

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

            // Deactivate all towers and stop their animations
            for (int i = 0; i < towerStates.Count; i++)
            {
                if (towerStates[i] != null)
                {
                    towerStates[i].SetActive(false);
                }

                if (i < alembicStreamPlayers.Count && alembicStreamPlayers[i] != null)
                {
                    alembicStreamPlayers[i].CurrentTime = 0; // Reset animation
                }
            }

            // Activate the new tier's tower and its animation
            if (newTier < towerStates.Count)
            {
                towerStates[newTier].SetActive(true);
                Debug.Log($"Activating tower: {towerStates[newTier].name}");

                if (newTier < alembicStreamPlayers.Count && alembicStreamPlayers[newTier] != null)
                {
                    alembicStreamPlayers[newTier].CurrentTime = 0; // Reset animation for the new tier
                }
            }

            if (newTier > 0)
            {
                PlayTransitionSound();
            }
            

            currentTier = newTier;
            animationTime = 0f; // Reset animation time for the new tier
        }
    }

    

    void PlayTransitionSound()
    {
        if (audioSource != null && transitionSound != null)
        {
            audioSource.PlayOneShot(transitionSound);
        }
        else{
            Debug.Log("Transition sound or audio source is missing");
        }
    }

    // Destroy the castle method
    void CastleDestroyed()
    {
        Debug.Log("The castle has been destroyed!");
        Debug.Log("You Lose!!");

        // Stop the animation for the current tier
        if (currentTier >= 0 && currentTier < alembicStreamPlayers.Count && alembicStreamPlayers[currentTier] != null)
        {
            alembicStreamPlayers[currentTier].CurrentTime = alembicStreamPlayers[currentTier].Duration; // Set to end of animation
        }

        // Find all enemies in the scene and destroy them
        EnemyScript[] allEnemies = FindObjectsOfType<EnemyScript>();

        foreach (EnemyScript enemy in allEnemies)
        {
            enemy.Die(); // Call the Die method to destroy the enemy
        }

        // Pause the game
        Time.timeScale = 0;
    }

    // Get the current health of the castle
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
