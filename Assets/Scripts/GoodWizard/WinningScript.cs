using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Winning game end script (when alt is pressed)
public class WinningScript : MonoBehaviour
{
    
    public void WinGame()
    {
        Debug.Log("Congrats on beating the game!!");
        
        // Find all enemies in the scene
        EnemyScript[] allEnemies = FindObjectsOfType<EnemyScript>();

        // kill all remaigning enemies
        foreach (EnemyScript enemy in allEnemies)
        {
            enemy.Die();
        }

        Time.timeScale = 0;
    }
}
