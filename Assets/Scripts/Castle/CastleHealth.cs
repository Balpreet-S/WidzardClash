using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CastleHealth : MonoBehaviour
{

    public void TakeDamage(int damage)
    {

        XPManager.instance.playerXP -= damage;
        Debug.Log("The castle has been attacked!, the new health is " + XPManager.instance.playerXP);

        // destroy the castle if the health is 0 or below
        if (XPManager.instance.playerXP <= 0)
        {
            XPManager.instance.playerXP = 0;
            CastleDestroyed();
        }
    }

    // Destroy the castle method
    void CastleDestroyed()
    {
        Debug.Log("The castle has been destroyed!");
        Debug.Log("You Lose!!");
        EnemyScript[] allEnemies = FindObjectsOfType<EnemyScript>();

        // Loop through each enemy and kill them
        foreach (EnemyScript enemy in allEnemies)
        {
            enemy.Die(); // Call the Die method to destroy the enemy
        }

        Time.timeScale = 0;
    }

    // Get current health method
    public int GetCurrentHealth()
    {
        return XPManager.instance.playerXP;
    }
}