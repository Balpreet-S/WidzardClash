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



    private int currentWave = 0;

    void Start()
    {
        // Wave 1: only the base button is interactable
        baseSkillButton.interactable = true;
        fireSkillButton.interactable = false;
        waterSkillButton.interactable = false;
        earthSkillButton.interactable = false;
        ULTButton.interactable = false;
    }

    public void NextWave()
    {
        //adding interactable buttons based on each wave
        currentWave++;
        Debug.Log($"Wave {currentWave} started!");

        switch (currentWave)
        {
            case 2: //unlocks fir skill 
                fireSkillButton.interactable = true;
                break;
            case 5: //unlocks earth skill
                earthSkillButton.interactable = true;
                break;
            case 10: //unlocks water skill and ult skill
                waterSkillButton.interactable = true;
                ULTButton.interactable = true;
                break;
        }
    }
}
