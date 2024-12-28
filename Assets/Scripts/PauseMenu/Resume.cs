using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Resume : MonoBehaviour
{
    public void ResumeButton()
    {
        SceneManager.LoadScene(0); // go back to the game scene
    }
}
