using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() // Starts the game
    {
        SceneManager.LoadScene("Object Placement");
    }

    public void HowToPlay() // Takes you to another scene that explains how the game works
    {
        SceneManager.LoadScene("How To Play");
    }

    public void QuitGame() // Quits the game
    {
        Application.Quit();
    }
}
