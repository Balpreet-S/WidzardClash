using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


// This script handles the opening and closing of the UI window for each wizard, and well as the fuctionality of each button in the window

public class WizardClickHandler : MonoBehaviour
{
    public GameObject uiWindowPrefab;
    private Camera mainCamera;
    private GameObject activeWindow;


    private WizardAttack wizardAttack;
    private WizardUpgrade wizardUpgrade;
    private ImageFillGradient cooldownBar;

    public TMP_Text LevelText;

    public GameObject button;

    private int upgradeLevel = 0;   


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        wizardAttack = GetComponent<WizardAttack>();
        wizardUpgrade = GetComponent<WizardUpgrade>();
        cooldownBar = GetComponentInChildren<ImageFillGradient>();
        
    }

    // contantly check if middle mouse button is pressed, and if so, close the window, and update the level text
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

    // Handles opening and positioning the UI window when the wizard is clicked, and also updates the level text
    void OnMouseDown()
    {
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

        GameObject uiWindow = Instantiate(uiWindowPrefab);
        uiWindow.transform.SetParent(canvas.transform, false);

        uiWindow.SetActive(true);

        Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.worldCamera,
            out Vector2 localPosition
        );

        RectTransform rectTransform = uiWindow.GetComponent<RectTransform>();
        localPosition += new Vector2(0, -55);
        rectTransform.localPosition = localPosition;

        activeWindow = uiWindow;

        Transform buttonTransform = activeWindow.transform.Find("Level Up"); 
        if (buttonTransform != null)
        {
            button = buttonTransform.gameObject;
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                LevelText = buttonText;
                UpdateLevelText();
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



    // Handles the activation of the special power
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
    // Handles the level up
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
    // Handles the deletion of the wizard
    public void DeleteWizard()
    {
        Debug.Log("Delete Wizard!");
        Destroy(gameObject);
        Destroy(activeWindow);
        activeWindow = null;
    }

    // Updates the level text update based on the upgrade level
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
