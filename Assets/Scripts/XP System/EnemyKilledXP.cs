using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.Interactions;
using UnityEditor.Build.Reporting;
using UnityEngine.UI;



//xp system for when enemies are killed (includes skill points)
public class XPManager : MonoBehaviour
{
    public static XPManager instance;
    public SkillsButtons Button { get; private set; }
    public int playerXP;
    public int skillPoints;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI XpCounterText;
    public TextMeshProUGUI RankLevelCounterText;

    public Image XpProgressBar;

    private int nextXPThreshold = 50;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        playerXP = 0;
        skillPoints = 2;
        UpdateSkillPointsText();
    }

    private void Update()
    {
        //Debug.Log($"Current XP: {playerXP}");
        UpdateSkillPointsText();
    }
    //add skill points and player xp depending on threshold
    public void AddXP(int xpAmount)
    {
        playerXP += xpAmount;
        XpCounterText.text = $"{playerXP} xp / {nextXPThreshold} xp";
        XpProgressBar.fillAmount = (float) playerXP / (float) nextXPThreshold;
        if(playerXP >= nextXPThreshold){
            playerXP = playerXP - nextXPThreshold;
            skillPoints++;
        }
    }

    //buying wizard
    public void SkillTowers(SkillsButtons s)
    {
        if (skillPoints >= s.Cost)
        {
            Button = s;
        }
    }

    public void UpgradeTowers(int cost)
    {
        if (playerXP >= cost)
        {
            playerXP -= cost;
            Debug.Log("Tower upgraded!, in the if statement");
        }
        Debug.Log($"Tower upgraded!, current XP: {playerXP}");
    }

    
    //for buying final skill

    public void WinningCondition(SkillsButtons s)
    {
        if (skillPoints >= s.Cost)
        {
            Button = s;
            skillPoints -= s.Cost;
            Time.timeScale = 0;
            EnemyScript[] allEnemies = FindObjectsOfType<EnemyScript>();
            foreach (EnemyScript enemy in allEnemies)
            {
                enemy.DieNoXP();
            }

            // Destory insted of die 

            Debug.Log("Congrats on winning the game!!");
        }
    }

    //decrease skillpoints when used by a button 
    public void PurchaseSkill()
    {
        if (skillPoints >= Button.Cost)
        {
            skillPoints -= Button.Cost;
            Button = null;
        }
    }

    public void PurchaseUpgrade()
    {
        if (playerXP >= Button.Cost)
        {
            playerXP -= Button.Cost;
            Button = null;
        }
    }


    // update the uk to show current skill points

    private void UpdateSkillPointsText()
    {
        if (LevelText != null)
        {
           LevelText.text = $"{skillPoints}";
        }
    }
}