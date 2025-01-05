using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlay : MonoBehaviour
{
    public void backbutton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // loads previous page
    }

    public void nextButton() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // loads next page
    }
}
