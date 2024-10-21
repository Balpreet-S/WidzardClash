using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CastleHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the castle
    private int currentHealth;  // Current health of the castle

    void Start()
    {
        // Initialize the castle's health to the maximum health value
        currentHealth = XPManager.instance.playerXP;
    }

    // This method will be called to decrease the castle's health
    public void TakeDamage(int damage)
    {
        // Decrease the current health by the damage amount
        currentHealth -= damage;
        Debug.Log("The castle has been attacked!, the new health is " + currentHealth);

        // Check if the castle's health has reached zero or below
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            CastleDestroyed(); // Call the castle destroyed method
        }
    }

    // Method to handle what happens when the castle is destroyed
    void CastleDestroyed()
    {
        Debug.Log("The castle has been destroyed!");
        Debug.Log("You Lose!!");
        //Application.Quit();
        Time.timeScale = 0;
        // Add any additional logic like ending the game, playing an animation, etc.
    }

    // Optional: Method to get the current health of the castle for display purposes
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}