using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlay : MonoBehaviour
{
    public void backbutton() // takes you back to the main menu
    {
        SceneManager.LoadScene("Main Menu");
    }
}
