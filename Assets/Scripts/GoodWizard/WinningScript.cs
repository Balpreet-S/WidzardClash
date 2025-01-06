using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Winning game end script (when alt is pressed)
public class WinningScript : MonoBehaviour
{
    public GameObject gameWon;

    // kill all enemeis and end the game when called
    public void WinGame()
    {
        Debug.Log("Congrats on beating the game!!");
        
        EnemyScript[] allEnemies = FindObjectsOfType<EnemyScript>();

        foreach (EnemyScript enemy in allEnemies)
        {
            enemy.Die();
        }

        Time.timeScale = 0;

        gameWon.SetActive(true);

    }
}
