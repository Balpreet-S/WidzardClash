using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void restart() // Starts the game from the beginning
    {
        SceneManager.LoadScene(0);
    }

    public void quit() // quits game
    {
        Application.Quit();
    }
}
