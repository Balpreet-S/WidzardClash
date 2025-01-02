using UnityEngine;
using TMPro;

//xp system for when enemies are killed (includes skill points)
public class XPManager : MonoBehaviour
{
    public static XPManager instance;
    public SkillsButtons Button { get; private set; }

    public int playerXP = 50;
    private int skillPoints;
    public TextMeshProUGUI LevelText;

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
        playerXP = 50;
        nextXPThreshold = 100;
    }
    //add skill points and player xp depending on threshold
    public void AddXP(int xpAmount)
    {
        playerXP += xpAmount;

        while (playerXP >= nextXPThreshold)
        {
            SkillPoints += 1;
            nextXPThreshold += 50;
        }

        //Debug.Log($"Player gained {xpAmount} XP. Total XP: {playerXP}");
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
            LevelText.text = $"You have: {SkillPoints} skill points";
        }
    }
}