using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleHealth : MonoBehaviour
{
    public int currentHealth = 100; // Castle starts with 100 health

    void Start()
    {
        // Ensure health starts at 100
        currentHealth = 100;
    }

    // Method to apply damage to the castle
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("The castle has been attacked! The new health is " + currentHealth);

        // Destroy the castle if health reaches 0 or below
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Clamp health to 0
            CastleDestroyed();
        }
    }

    // Destroy the castle method
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
    }

    // Get the current health of the castle
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}