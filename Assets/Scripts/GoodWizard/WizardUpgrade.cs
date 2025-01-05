using UnityEngine;
using UnityEngine.UI;

public class WizardUpgrade : MonoBehaviour
{
    private WizardAttack wizardAttack;
    // Call this (e.g., when you pick up an upgrade or complete a level)


    private void Awake()
    {
        wizardAttack = GetComponent<WizardAttack>();
    }
    public void UpgradeWizardDamage(float percentIncrease)
    {

        float increment = percentIncrease / 100f;
        wizardAttack.damageMultiplier += increment;
        XPManager.instance.UpgradeTowers(50);
    }
}
