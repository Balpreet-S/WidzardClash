using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteAudio : MonoBehaviour
{
    public void MuteToggle(bool muted)
    {
        if (muted)
        {
            AudioListener.volume = 0; // if the toggle is true, sound is off
        }
        else
        {
            AudioListener.volume = 1; // if the toggle is false, sound is on
        }
    }
}
