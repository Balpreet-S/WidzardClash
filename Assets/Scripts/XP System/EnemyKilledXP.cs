using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager instance;  // Singleton instance

    public int playerXP = 0;  // Player's XP

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
        Debug.Log("Player gained " + xpAmount + " XP. Total XP: " + playerXP);
    }
}