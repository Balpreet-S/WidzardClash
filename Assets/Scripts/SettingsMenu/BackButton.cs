using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void previous_page()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // loads in the previous page
    }
}
