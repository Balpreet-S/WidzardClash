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
        baseSkillButton.interactable = true;
        fireSkillButton.interactable = true;
        waterSkillButton.interactable = true;
        earthSkillButton.interactable = false;
        ULTButton.interactable = false;
    }

    public void NextWave()
    {
        currentWave++;

        switch (currentWave)
        {
            case 2:
                earthSkillButton.interactable = true;
                break;
            case 3:
                waterSkillButton.interactable = true;
                break;
            case 4:
                fireSkillButton.interactable = true;
                ULTButton.interactable = true;
                break;
        }
    }
}
