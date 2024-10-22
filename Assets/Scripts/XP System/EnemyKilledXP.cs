using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPManager : MonoBehaviour
{
    public SkillsButtons Button { get; private set; }
    public static XPManager instance;  // Singleton instance

    public int playerXP = 0;  // Player's XP

    private int SkillLv; //Player's Skills points aka health bar as well

    private int count = 0; 
    public int div = 1;

    public TextMeshProUGUI LevelText;

    public int SkillLv1 {
        get{
            return SkillLv;
        }
        set{
            this.SkillLv = value;
            this.LevelText.text = "You have: " + value.ToString() + " skills points";
        }
    }

     void Start()
    {
        SkillLv1 = 2;
    }

    void Awake()
    {
        // Ensure there is only one instance of XPManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddXP(int xpAmount)
    {
        playerXP += xpAmount;
        if(playerXP / 50 >= div){
            count = count + 1;
            div = div + 1;
            SkillLv1 = count;
        }
        Debug.Log("Player gained " + xpAmount + " XP. Total XP: " + playerXP);
    }

    public void SkillTowers(SkillsButtons s){
        if(SkillLv1 >= s.Cost){
            this.Button = s;
        }
    }

    public void PurchaseSkill(){
        if(SkillLv1 >= Button.Cost){
            SkillLv1 -= Button.Cost;
        }
        Button = null;
    }
}