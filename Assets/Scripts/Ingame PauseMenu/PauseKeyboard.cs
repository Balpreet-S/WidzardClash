using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseKeyboard : MonoBehaviour
{
    public GameObject pausemenu;
    private Button pausebutton;
    // Start is called before the first frame update
    void Start()
    {
        pausebutton = pausemenu.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKey(KeyCode.Escape))){
            pausebutton.onClick.Invoke();
        }
    }

    public void PauseApp(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
