using UnityEngine;
using TMPro;

public class WizardUpgrade : MonoBehaviour
{
    private WizardAttack wizardAttack;

    public TextMeshProUGUI LevelText;
    // Call this (e.g., when you pick up an upgrade or complete a level)


    private void Awake()
    {
        wizardAttack = GetComponent<WizardAttack>();
    }

    public void UpgradeWizardDamage(float percentIncrease)
    {
        // e.g., passing in 20 means +20% damage
        
            float increment = percentIncrease / 100f;
            wizardAttack.damageMultiplier += increment;

            if (percentIncrease == 30){
                XPManager.instance.PurchaseUpgrade(1);
                LevelText.text = "LV: 2";
            }
            else if (percentIncrease == 20){
                XPManager.instance.PurchaseUpgrade(2);
                LevelText.text = "LV: 3";
            }
            else{
                XPManager.instance.PurchaseUpgrade(0);
            }
            
        
        
        // If wizardAttack.damageMultiplier was 1.0 and percentIncrease is 20,
        // new multiplier is 1.2 (i.e., +20% damage).
    }
}
