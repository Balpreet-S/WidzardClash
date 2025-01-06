using UnityEngine;
using TMPro;

//this file is used to upgrade the wizard

public class WizardUpgrade : MonoBehaviour
{
    private WizardAttack wizardAttack;

    public TextMeshProUGUI LevelText;

    // get the wizard attack component when the game starts
    private void Awake()
    {
        wizardAttack = GetComponent<WizardAttack>();
    }


    // method to upgrade the wizard damage
    public void UpgradeWizardDamage(float percentIncrease)
    {
        
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
    }
}
