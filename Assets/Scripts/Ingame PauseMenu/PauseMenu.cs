using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public EnemySpawner ScoreCounter;

    public bool check;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        
    }

    public void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(check){
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // pauses the game
        ScoreCounter.HighScoreUpdate();
        check = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // resumes the game
        check = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Object Placement"); // Reloads the game
        Time.timeScale = 1f; // resumes the game
        AudioListener.volume = 1; // turns sound on
    }
    public void QuitGame()
    {
        Application.Quit(); // quits game
    }
}
