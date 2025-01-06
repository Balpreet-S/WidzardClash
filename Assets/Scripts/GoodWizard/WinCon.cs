using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this code is responsable for the win condition
public class WinCon : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Congrats on finishing the game!");
        Time.timeScale = 0;
    }
}
