using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Page1 : MonoBehaviour
{
    public void backbutton() // takes you back to the main menu
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void nextButton() // takes you to the next how to play page
    {
        SceneManager.LoadScene("How To Play Page 2");
    }
}
