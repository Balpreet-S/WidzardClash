using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{

    private int Xp;
    private int SkillLv;

    public TextMeshProUGUI LevelText;

    public int SkillLv1 {
        get{
            return SkillLv;
        }
        set{
            this.SkillLv = value;
            this.LevelText.text = "Current Skill Level is: " + value.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SkillLv1 = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
