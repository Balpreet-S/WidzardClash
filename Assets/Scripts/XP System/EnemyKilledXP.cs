using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//xp system for when enemies are killed (includes skill points)
public class XPManager : MonoBehaviour
{
    public static XPManager instance;
    public SkillsButtons Button { get; private set; }

    public int playerXP = 50;
    private int skillPoints;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI XpCounterText;

    public Image xpProgressbar;

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
        UpdateSkillPointsText();
    }
    //add skill points and player xp depending on threshold
    public void AddXP(int xpAmount)
    {
        playerXP += xpAmount;
        xpProgressbar.fillAmount = (float) playerXP / (float) nextXPThreshold;

        if(playerXP >= nextXPThreshold){
            playerXP = playerXP - nextXPThreshold;
            skillPoints++;
        }
        XpCounterText.text = $"{playerXP} xp / {nextXPThreshold} xp";

    }

    //buying wizard
    public void SkillTowers(SkillsButtons s)
    {
        if (skillPoints >= s.Cost)
        {
            Button = s;        
        }
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

            // Loads the game won scene
            SceneManager.LoadScene("Game Won");
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

    public int PurchaseUpgrade(int cost)
    {
        if (skillPoints >= cost)
        {
            skillPoints -= cost;
            Debug.Log("Purchased Upgrade point! inside ");
            return 1;
        }

        return 0;
    }

    public int GetSkillPoints()
    {
        return skillPoints;
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