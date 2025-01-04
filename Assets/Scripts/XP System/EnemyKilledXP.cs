using UnityEngine;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Image = UnityEngine.UI.Image;
using UnityEngine.UIElements;

//xp system for when enemies are killed (includes skill points)
public class XPManager : MonoBehaviour
{
    public static XPManager instance;
    public SkillsButtons Button { get; private set; }

    public int playerXP = 50;
    private int skillPoints;
    public TextMeshProUGUI LevelText; //Skill Points the player currently has
    public TextMeshProUGUI TillNextLevelText;
    public int currentXP = 0;
    public int fullXP;
    public int nextRank = 50;

    //AnimationCurve experienceCurve;
    public Image experienceFill;

    private int nextXPThreshold;

    public int SkillPoints
    {
        get { return skillPoints; }
        set
        {
            skillPoints = value;
            UpdateSkillPointsText();
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SkillPoints = 2;
        playerXP = 25;
        nextXPThreshold = 50;
    }

    //add skill points and player xp depending on threshold
    public void AddXP(int xpAmount)
    {
        // playerXP += xpAmount;
        // currentXP += xpAmount;

        // while (playerXP >= nextXPThreshold)
        // {
        //     SkillPoints += 1;
        //     nextXPThreshold += 50;
        // }
        // TillNextLevelText.text = $"{playerXP} xp / {nextRank} xp";
        // XpFillBar();
        // Debug.Log($"Player gained {xpAmount} XP. Total XP: {playerXP}");

        TillNextLevelText.text = $"{playerXP} xp / {nextXPThreshold} xp";
        playerXP += xpAmount;

        if(playerXP >= nextXPThreshold){
            playerXP = playerXP - nextXPThreshold;
            SkillPoints += 1;
            //nextXPThreshold += 50; //Only enables when the game is fully functioning (Adding difficulty to game)
        }
    }

    void Update(){
        XpFillBar();
    }

    //buying fire tower
    public void SkillTowers(SkillsButtons s)
    {
        if (SkillPoints >= s.Cost)
        {
            Button = s;
        }
    }
    //for buying final skill

    public void WinningCondition(SkillsButtons s)
    {
        if (SkillPoints >= s.Cost)
        {
            Button = s;
            SkillPoints -= s.Cost;
            Time.timeScale = 0;
            EnemyScript[] allEnemies = FindObjectsOfType<EnemyScript>();
            foreach (EnemyScript enemy in allEnemies)
            {
                enemy.Die();
            }

            Debug.Log("Congrats on winning the game!!");
        }
    }

    //decrease skillpoints when used by a button 
    public void PurchaseSkill()
    {
        if (SkillPoints >= Button.Cost)
        {
            SkillPoints -= Button.Cost;
            Button = null;
        }
    }


    // update the uk to show current skill points

    private void UpdateSkillPointsText()
    {
        if (LevelText != null)
        {
            LevelText.text = $"{SkillPoints}";
        }
    }

    //Fill out xp Bar
    void XpFillBar(){
        experienceFill.fillAmount = (float) playerXP/ (float) nextXPThreshold;
    }
}