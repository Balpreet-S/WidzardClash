using UnityEngine;
using System.Collections;

public class WizardClickController : MonoBehaviour
{
    private WizardAttack wizardAttack;
    private WizardUpgrade wizardUpgrade;
    private ImageFillGradient cooldownBar;

    [Header("Double-Click Settings")]
    [Tooltip("Time within which a second click must occur to register as a double-click.")]
    public float doubleClickDelay = 0.3f;

    private int clickCount = 0;        // How many clicks have happened
    private bool coroutineRunning = false; 

    void Start()
    {
        // Get references (assuming WizardAttack and WizardUpgrade are on the same GameObject)
        wizardAttack = GetComponent<WizardAttack>();
        wizardUpgrade = GetComponent<WizardUpgrade>();
        cooldownBar  = GetComponentInChildren<ImageFillGradient>();

        if (wizardAttack == null)
            Debug.LogError("WizardClickController: WizardAttack script not found!");

        if (wizardUpgrade == null)
            Debug.LogWarning("WizardClickController: WizardUpgrade script not found. Double-click upgrades won't work.");

        if (cooldownBar == null)
            Debug.LogError("WizardClickController: CooldownBar (ImageFillGradient) not found!");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            // Raycast to see if we actually clicked THIS wizard
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    // We clicked on this wizard; increment click count
                    clickCount++;

                    if (!coroutineRunning)
                    {
                        // Start the coroutine to check if it's single or double click
                        StartCoroutine(ClickRoutine());
                    }
                }
            }
        }
    }

    private IEnumerator ClickRoutine()
    {
        coroutineRunning = true;

        // Wait for the doubleClickDelay to see if a second click happens
        yield return new WaitForSeconds(doubleClickDelay);

        // If after waiting, clickCount is still 1, it's a single click
        if (clickCount == 1)
        {
            HandleSingleClick();
        }
        // If clickCount == 2 (or more), it's a double click
        else if (clickCount >= 2)
        {
            HandleDoubleClick();
        }

        // Reset for next time
        clickCount = 0;
        coroutineRunning = false;
    }

    private void HandleSingleClick()
    {
        Debug.Log("Wizard was SINGLE-clicked!");

        // Existing logic: Try to activate special power if cooldown is ready
        if (cooldownBar != null && cooldownBar.IsCooldownComplete())
        {
            if (wizardAttack != null)
            {
                switch (wizardAttack.wizardType.ToLower())
                {
                    case "fire":
                        wizardAttack.ActivateSpecialPowerFire();
                        break;
                    case "water":
                        wizardAttack.ActivateSpecialPowerWater();
                        break;
                    case "earth":
                        wizardAttack.ActivateSpecialPowerEarth();
                        break;
                    default:
                        Debug.LogWarning($"Unknown wizard type: {wizardAttack.wizardType}");
                        break;
                }

                cooldownBar.StartCooldown();
            }
        }
        else
        {
            Debug.Log("Special power is not ready yet.");
        }
    }

    private void HandleDoubleClick()
    {
        Debug.Log("Wizard was DOUBLE-clicked!");

        // Upgrade damage by 20% if we have a WizardUpgrade reference
        if (wizardUpgrade != null)
        {
            wizardUpgrade.UpgradeWizardDamage(20f);
            Debug.Log("Wizard damage upgraded by +20%!");
        }
        else
        {
            Debug.LogWarning("No WizardUpgrade script found on this Wizard.");
        }
    }
}
