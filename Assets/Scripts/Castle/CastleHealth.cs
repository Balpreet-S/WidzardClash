using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CastleHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = XPManager.instance.playerXP;
    }

    // castle taking damage function
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
        Time.timeScale = 0;
        // for final game animation or message on screen can be added 
    }

    // get health method to display current health if needed --also helpful for final game
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}