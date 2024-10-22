using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningScript : MonoBehaviour
{
    // winning game end scipt (should be improved for final game e.g using animation or messages/music)
    void Start()
    {
        Debug.Log("Congrats on beating the game!!");
        Time.timeScale = 0;
    }
}
