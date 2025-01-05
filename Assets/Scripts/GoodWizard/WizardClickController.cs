using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


// uiWindow.SetActive(true);

public class WizardClickHandler : MonoBehaviour
{
    public GameObject uiWindowPrefab; // Assign your prefab in the Inspector
    private Camera mainCamera;
    private GameObject activeWindow; // To track the currently active panel


    private WizardAttack wizardAttack;
    private WizardUpgrade wizardUpgrade;
    private ImageFillGradient cooldownBar;

    public TMP_Text LevelText;

    public GameObject button;


    // upgrade level 
    
    private int upgradeLevel = 0;   


    

    void Start()
    {
        mainCamera = Camera.main;

        wizardAttack = GetComponent<WizardAttack>();
        wizardUpgrade = GetComponent<WizardUpgrade>();
        cooldownBar = GetComponentInChildren<ImageFillGradient>();

        // Ensure the LevelText is updated at the start

        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2)){
            Destroy(activeWindow);
            activeWindow = null;
            Debug.Log("UI Window closed.");
            return;
        }

        
        if (activeWindow != null) {
            UpdateLevelText();}


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

        Transform buttonTransform = activeWindow.transform.Find("Level Up"); // Replace "ButtonName" with the actual name
        if (buttonTransform != null)
        {
            button = buttonTransform.gameObject;
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                LevelText = buttonText;
                UpdateLevelText(); // Set the initial text dynamically
            }
            else
            {
                Debug.LogError("TMP_Text component not found in the button!");
            }
        }
        else
        {
            Debug.LogError("Button not found in the activeWindow!");
        }


        Button[] buttons = uiWindow.GetComponentsInChildren<Button>();
        if (buttons.Length >= 2)
        {
            buttons[0].onClick.AddListener(LevelUpWizard);
            buttons[1].onClick.AddListener(ActivateSpecialPower);
            buttons[2].onClick.AddListener(DeleteWizard);
            
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
                if (XPManager.instance.GetSkillPoints() >= 2){
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

    public void DeleteWizard()
    {
        Debug.Log("Delete Wizard!");
        Destroy(gameObject);
        Destroy(activeWindow);
        activeWindow = null;
    }

    private void UpdateLevelText()
    {
        if (LevelText != null)
        {
            if (upgradeLevel+1 <= 2){
                LevelText.text = $"Level up, cost: {upgradeLevel+1}";
            }
            else{
                LevelText.text = "Max level";
            }
            
        }
        else
        {
            Debug.LogError("LevelText reference is missing!");
        }
    }
}
