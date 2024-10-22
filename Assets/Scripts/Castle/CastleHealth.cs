using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CastleHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        // Initialize the castle's health to the maximum health value
        currentHealth = XPManager.instance.playerXP;
    }

    // Decrease health when the enemy wizard reaches tower
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("The castle has been attacked!, the new health is " + currentHealth);

        // destroy the castle if the health is 0 or below
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            CastleDestroyed();
        }
    }

    // Destroy the castle method
    void CastleDestroyed()
    {
        Debug.Log("The castle has been destroyed!");
        Debug.Log("You Lose!!");
        //Application.Quit();
        Time.timeScale = 0;
        // Add any additional logic like ending the game, playing an animation, etc.
    }

    // get health method to display current health if needed
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}