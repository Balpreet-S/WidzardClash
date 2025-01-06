using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; // Reference to the pause menu UI
    public EnemySpawner ScoreCounter; // Reference to the EnemySpawner for high score updates
    public bool check; // Tracks whether the game is paused

    void Start()
    {
        // Ensure the pause menu is hidden at the start
        if (pauseMenu == null)
        {
            Debug.LogError("PauseMenu is not assigned in the Inspector.");
        }
        pauseMenu?.SetActive(false); // Use null conditional operator to avoid errors
    }

    void Update()
    {
        // Toggle pause when the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (check)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        if (pauseMenu == null)
        {
            Debug.LogError("PauseMenu is not assigned in the Inspector. Cannot pause the game.");
            return; // Prevent further execution if the menu is null
        }

        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // Pause the game

        if (ScoreCounter != null)
        {
            try
            {
                ScoreCounter.HighScoreUpdate();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error updating high score: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("ScoreCounter is not assigned in the Inspector. Skipping HighScoreUpdate.");
        }

        check = true;
    }

    public void ResumeGame()
    {
        if (pauseMenu == null)
        {
            Debug.LogError("PauseMenu is not assigned in the Inspector. Cannot resume the game.");
            return; // Prevent further execution if the menu is null
        }

        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        check = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Object Placement"); // Reload the game scene
        Time.timeScale = 1f; // Ensure the game resumes
        AudioListener.volume = 1; // Enable sound
    }

    public void HowToPlay() // Navigate to the "How To Play" scene
    {
        SceneManager.LoadScene("How To Play");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Main Menu"); // Goes back to the main menu
    }
}