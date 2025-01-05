using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Page2 : MonoBehaviour
{
    public void backbutton() // takes you back to the main menu
    {
        SceneManager.LoadScene("How To Play Page 1");
    }
}
