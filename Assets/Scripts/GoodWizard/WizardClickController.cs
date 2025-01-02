using UnityEngine;

public class WizardClickController : MonoBehaviour
{
    private WizardAttack wizardAttack;
    private ImageFillGradient cooldownBar; // Reference to the new cooldown bar

    void Start()
    {
        wizardAttack = GetComponent<WizardAttack>();
        cooldownBar = GetComponentInChildren<ImageFillGradient>(); // Assuming the CooldownBar is a child GameObject

        if (wizardAttack == null)
        {
            Debug.LogError("WizardClickController: WizardAttack script not found!");
        }

        if (cooldownBar == null)
        {
            Debug.LogError("WizardClickController: CooldownBar (ImageFillGradient) not found!");
        }
        else
        {
            Debug.Log("WizardClickController: CooldownBar found!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"Clicked on: {hit.collider.gameObject.name}");
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    Debug.Log("-------------Wizard Clicked!-------------");

                    // Check if the cooldown is complete
                    if (cooldownBar != null && cooldownBar.IsCooldownComplete())
                    {
                        if (wizardAttack != null)
                        {
                            switch (wizardAttack.wizardType.ToLower())
                            {
                                case "fire":
                                    wizardAttack.ActivateSpecialPowerFire(); // For Fire wizards
                                    break;

                                case "water":
                                    wizardAttack.ActivateSpecialPowerWater(); // For Water wizards
                                    break;

                                case "earth":
                                    wizardAttack.ActivateSpecialPowerEarth(); // For Earth wizards
                                    break;

                                default:
                                    Debug.LogWarning($"Unknown wizard type: {wizardAttack.wizardType}");
                                    break;
                            }

                            cooldownBar.StartCooldown();

                        }

                        /*    
                        if (wizardAttack != null)
                        {
                            Debug.Log("Special power activated!");
                            wizardAttack.ActivateSpecialPowerWater();
                            cooldownBar.StartCooldown(); // Restart cooldown after activating the power
                        }*/
                    }
                    else
                    {
                        Debug.Log("Special power is not ready yet.");
                    }
                }
            }
        }
    }
}
