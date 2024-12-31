using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void restart() // Starts the game from the beginning
    {
        SceneManager.LoadScene("Object Placement"); // Reloads the game
        Time.timeScale = 1f; // resumes the game
        AudioListener.volume = 1; // turns sound on
    }

    public void quit() // quits game
    {
        Application.Quit();
    }
}
