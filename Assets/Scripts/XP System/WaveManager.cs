using UnityEngine;
using UnityEngine.UI;

// this code is responsible for the unabling and enabling of wizards playable depending on the wave, 
public class WaveManager : MonoBehaviour
{
    [Header("Skill Buttons")]
    public Button baseSkillButton;
    public Button fireSkillButton;
    public Button waterSkillButton;
    public Button earthSkillButton;
    public Button ULTButton;

    

    private int currentWave = 1;

    void Start()
    {
        // Wave 1: only the base button is interactable
        baseSkillButton.interactable = true;
        fireSkillButton.interactable = true;
        waterSkillButton.interactable = false;
        earthSkillButton.interactable = false;
        ULTButton.interactable = false;
    }

    public void NextWave()
    {
        currentWave++;
        Debug.Log($"Wave {currentWave} started!");

        switch (currentWave)
        {
            case 2:
                fireSkillButton.interactable = true;
                break;
            case 3:
                waterSkillButton.interactable = true;
                break;
            case 4:
                earthSkillButton.interactable = true;
                ULTButton.interactable = true;
                break;
        }
    }
}
