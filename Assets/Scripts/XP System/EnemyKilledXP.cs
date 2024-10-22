using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//class for managing xp from enemy kills
public class XPManager : MonoBehaviour
{
    public SkillsButtons Button { get; private set; }
    public static XPManager instance;

    public int playerXP = 0;

    private int SkillLv;

    private int count = 0;
    public int div = 1;

    public TextMeshProUGUI LevelText;
    //getting skill level
    public int SkillLv1
    {
        get
        {
            return SkillLv;
        }
        set
        {
            this.SkillLv = value;
            this.LevelText.text = "You have: " + value.ToString() + " skills points";
        }
    }

    //start with 2 skills points to place 2 towers
    void Start()
    {
        SkillLv1 = 2;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // add xp for every kill and add skill points for every 50 xp
    public void AddXP(int xpAmount)
    {
        playerXP += xpAmount;
        if (playerXP / 50 >= div)
        {
            count = count + 1;
            div = div + 1;
            SkillLv1 = count;
        }
        Debug.Log("Player gained " + xpAmount + " XP. Total XP: " + playerXP);
    }
    //skill tower function
    public void SkillTowers(SkillsButtons s)
    {
        if (SkillLv1 >= s.Cost)
        {
            this.Button = s;
        }
    }

    public void WinningCondition(SkillsButtons s)
    {
        if (SkillLv1 >= s.Cost)
        {
            this.Button = s;
            SkillLv1 -= s.Cost;
            Time.timeScale = 0;
            EnemyScript[] allEnemies = FindObjectsOfType<EnemyScript>();

            // kill all remaigning enemies
            foreach (EnemyScript enemy in allEnemies)
            {
                enemy.Die();
            }
            Debug.Log("Congrats on Winning the game!!");
        }
    }
    //buy the skill 
    public void PurchaseSkill()
    {
        if (SkillLv1 >= Button.Cost)
        {
            SkillLv1 -= Button.Cost;
        }
        Button = null;
    }
}