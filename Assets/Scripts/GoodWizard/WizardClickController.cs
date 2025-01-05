/*using UnityEngine;

public class WizardClickHandler : MonoBehaviour
{
    public GameObject uiWindowPrefab; // Assign your prefab in the Inspector
    private GameObject activeWindow; // To track the currently active panel

    void OnMouseDown()
    {
        // If a panel is already active for this wizard, destroy it (close the panel)
        if (activeWindow != null)
        {
            Destroy(activeWindow);
            activeWindow = null; // Reset the reference
            Debug.Log("UI Window closed.");
            return;
        }

        if (uiWindowPrefab == null)
        {
            Debug.LogError("UI Window Prefab is not assigned!");
            return;
        }

        // Instantiate the UI window
        GameObject uiWindow = Instantiate(uiWindowPrefab);

        // Parent the window to this wizard (so it follows the wizard)
        uiWindow.transform.SetParent(transform, false);

        // Position the pop-up slightly above the wizard
        Vector3 offset = new Vector3(0, 2, 0); // Adjust this offset as needed
        uiWindow.transform.localPosition = offset;

        // Store reference to the active window
        activeWindow = uiWindow;

        Debug.Log($"UI Window opened at position: {uiWindow.transform.position}");
    }

    void Update()
    {
        // Ensure the UI always faces the camera
        if (activeWindow != null)
        {
            activeWindow.transform.LookAt(Camera.main.transform);
            activeWindow.transform.Rotate(0, 180, 0); // Flip to face the camera properly
        }
    }
}

private WizardAttack wizardAttack;
    private WizardUpgrade wizardUpgrade;
    private ImageFillGradient cooldownBar;

*/











using UnityEngine;
using UnityEngine.UI;
using System.Collections;


// uiWindow.SetActive(true);

public class WizardClickHandler : MonoBehaviour
{
    public GameObject uiWindowPrefab; // Assign your prefab in the Inspector
    private Camera mainCamera;
    private GameObject activeWindow; // To track the currently active panel


    private WizardAttack wizardAttack;
    private WizardUpgrade wizardUpgrade;
    private ImageFillGradient cooldownBar;


    // upgrade level 
    
    private int upgradeLevel = 0;   


    

    void Start()
    {
        mainCamera = Camera.main;

        wizardAttack = GetComponent<WizardAttack>();
        wizardUpgrade = GetComponent<WizardUpgrade>();
        cooldownBar = GetComponentInChildren<ImageFillGradient>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2)){
            Destroy(activeWindow);
            activeWindow = null;
            Debug.Log("UI Window closed.");
            return;
        }
    }

    void OnMouseDown()
    {
        // If a panel is already active for this wizard, destroy it (close the panel)
        if (activeWindow != null)
        {
            Destroy(activeWindow);
            activeWindow = null;
            Debug.Log("UI Window closed.");
            return;
        }

        if (uiWindowPrefab == null)
        {
            Debug.LogError("UI Window Prefab is not assigned!");
            return;
        }

        // Find the WizardCanvas
        GameObject canvasObject = GameObject.Find("WizardCanvas");
        if (canvasObject == null)
        {
            Debug.LogError("WizardCanvas not found in the scene!");
            return;
        }

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("WizardCanvas does not have a Canvas component!");
            return;
        }

        // Instantiate the UI window
        GameObject uiWindow = Instantiate(uiWindowPrefab);
        uiWindow.transform.SetParent(canvas.transform, false);

        // Ensure the panel is active
        uiWindow.SetActive(true);

        // Position the panel at the wizard's screen position
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position);

        // Convert screen position to local position in the Canvas space
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.worldCamera,
            out Vector2 localPosition
        );

        // Set the panel's position
        RectTransform rectTransform = uiWindow.GetComponent<RectTransform>();
        localPosition += new Vector2(0, -55);
        rectTransform.localPosition = localPosition;

        // Store reference to the active window
        activeWindow = uiWindow;

        Button[] buttons = uiWindow.GetComponentsInChildren<Button>();
        if (buttons.Length >= 2)
        {
            buttons[0].onClick.AddListener(LevelUpWizard);
            buttons[1].onClick.AddListener(ActivateSpecialPower);
        }

        Debug.Log("UI Window opened and positioned.");
    }



    public void ActivateSpecialPower()
    {
        Debug.Log("Activate Special Power!");

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

    public void LevelUpWizard()
    {
        Debug.Log("Level Up Wizard!");

        if (wizardUpgrade == null)
        {
            Debug.LogWarning("No WizardUpgrade script found on this Wizard.");
            return;
        }


        switch (upgradeLevel)
        {
            case 0:
                if (XPManager.instance.GetSkillPoints() >= 1){
                    wizardUpgrade.UpgradeWizardDamage(30f);
                    Debug.Log("Wizard damage upgraded by +30%!");
                    upgradeLevel++;
                }
                break;
            case 1:
                if (XPManager.instance.GetSkillPoints() >= 1){
                    wizardUpgrade.UpgradeWizardDamage(20f);
                    Debug.Log("Wizard damage upgraded by +20%!");
                    upgradeLevel++;
                }
                break;
            default:
                Debug.LogWarning($"Invalid upgrade level: {upgradeLevel}");
                break;
        }

    }
}
