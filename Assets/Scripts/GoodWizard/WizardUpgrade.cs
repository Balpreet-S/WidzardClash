using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WizardUpgrade : MonoBehaviour
{
    private int currentLevel = 0;
    private WizardAttack wizardAttack;

    public TextMeshProUGUI WizardLevel;



    void Awake()
    {
        // Get the WizardAttack component
        wizardAttack = GetComponent<WizardAttack>();
    }

    void Update()
    {
        UpdateLevelText();
    }   

    // Upgrade wizard's damage
    public void UpgradeWizardDamage(float percentIncrease)
    {
        float increment = percentIncrease / 100f;
        wizardAttack.damageMultiplier += increment;

        currentLevel += 1;
        // Optional: Deduct resources or XP for the upgrade
        XPManager.instance.UpgradeTowers(50);

        // Upgrade the spot to the next level
        //UpgradeSpot();

        Debug.Log($"Wizard damage upgraded by {percentIncrease}%! New damage multiplier: {wizardAttack.damageMultiplier}");
    }

    
    private void UpdateLevelText()
    {
        if (WizardLevel != null)
        {
            WizardLevel.text = $"LV: {currentLevel}";
        }
    }
/*
    private void UpdateLevelText()
    {
        if (WizardLevel != null)
        {
            WizardLevel.text = $"Level: {currentLevel}";
            WizardLevel.color = Color.white; // Set text color
            WizardLevel.fontStyle = FontStyle.Bold; // Bold style for visibility
        }

        // Optional: Add an outline for better readability
        /*
        if (WizardLevel.gameObject.GetComponent<Outline>() == null)
        {
            var outline = WizardLevel.gameObject.AddComponent<Outline>();
            outline.effectColor = Color.black;
            outline.effectDistance = new Vector2(2, -2);
        }
    }*/
}










/*



    // Method to upgrade the spot to the next level
    public void UpgradeSpot()
    {
        // Check if there's a higher-level spot available
        Debug.Log($"spotStates.Count: {spotStates.Count}");
        if (currentLevel < spotStates.Count - 1)
        {
            int newLevel = currentLevel + 1;
            UpdateSpotVisibility(newLevel);
            Debug.Log($"Spot upgraded to level {newLevel + 1}");
        }
        else
        {
            Debug.Log("Spot is already at max level!");
        }
    }

    // Initialize the spots and ensure only the first one is active
    private void InitializeSpots()
    {
        for (int i = 0; i < spotStates.Count; i++)
        {
            if (spotStates[i] != null)
            {
                Debug.Log($"activating initial spot {spotStates[i]}!");
                spotStates[i].SetActive(i == 0); // Activate the first spot only
            }
        }
    }

    // Updates the visibility of the spot based on the level
    void UpdateSpotVisibility(int newLevel)
    {
        if (newLevel != currentLevel)
        {
            Debug.Log($"Switching from spot {currentLevel} to {newLevel}");

            // Deactivate all spots
            for (int i = 0; i < spotStates.Count; i++)
            {
                if (spotStates[i] != null)
                {
                    Debug.Log($"Deactivating spot: {spotStates[i].name}");
                    spotStates[i].SetActive(false);
                }
            }

            // Activate the new spot
            if (newLevel < spotStates.Count && spotStates[newLevel] != null)
            {
                Debug.Log($"Activating spot: {spotStates[newLevel].name}");
                spotStates[newLevel].SetActive(true);
            }
            else
            {
                Debug.LogWarning($"Spot for level {newLevel} is null or missing.");
            }

            // Update the current level
            currentLevel = newLevel;

            if (spotStates[currentLevel].activeSelf)
            {
                Debug.Log($"Spot {spotStates[currentLevel].name} is correctly activated.");
            }
            else
            {
                Debug.LogError($"Spot {spotStates[currentLevel].name} failed to activate!");
            }

        }
        else
        {
            Debug.Log("The new level is the same as the current level. No action taken.");
        }
    }




























using UnityEngine;

public class WizardUpgrade : MonoBehaviour
{
    private WizardAttack wizardAttack;
    public GameObject[] spotVariations; // Array of spot prefabs for each level
    private int currentLevel = 0; // Current level of the tower spot

    private void Awake()
    {
        wizardAttack = GetComponent<WizardAttack>();
        spotVariations[0].SetActive(true);
    }

    // Upgrade wizard's damage
    public void UpgradeWizardDamage(float percentIncrease)
    {
        float increment = percentIncrease / 100f;
        wizardAttack.damageMultiplier += increment;

        // Optional: Deduct resources or XP for the upgrade
        XPManager.instance.UpgradeTowers(50);
        //UpgradeSpot(spotPrefabs[currentLevel].transform);
        UpgradeSpot();
        //Debug.Log($"Wizard damage upgraded by {percentIncrease}%! New damage multiplier: {wizardAttack.damageMultiplier}");
    }

    // Upgrade the spot
    public void UpgradeSpot()
    {
        if (currentLevel < spotVariations.Length - 1)
        {
            Debug.Log($"Spot upgraded to level {currentLevel + 1}!");
            // Deactivate the current spot
            spotVariations[currentLevel].SetActive(false);

            // Increment the level
            currentLevel++;

            // Activate the upgraded spot
            spotVariations[currentLevel].SetActive(true);

            Debug.Log($"Spot upgraded to level {currentLevel + 1}!");
        }
        else
        {
            Debug.Log("Spot is already at max level!");
        }
    }
    */
    /*

    // Upgrade the spot prefab
    public void UpgradeSpot(Transform currentSpot)
    {
        // Check if there's a higher-level spot prefab available
        if (currentLevel < spotPrefabs.Length - 1)
        {
            Debug.Log($"Spot is getting upgraded to level {currentLevel + 1}!");
            // Increase the level
            currentLevel++;

            // Replace the current spot with the new prefab
            ReplaceSpotPrefab(currentSpot);
        }
        else
        {
            Debug.Log("Spot is already at max level!");
        }
    }

    private void ReplaceSpotPrefab(Transform currentSpot)
    {
        Debug.Log($"Spot upgraded to level {currentLevel + 1}!");
        // Save the position and rotation of the current spot
        Vector3 position = currentSpot.position;
        Quaternion rotation = currentSpot.rotation;

        // Destroy the current spot
        Destroy(currentSpot.gameObject);

        // Instantiate the new spot prefab at the same position and rotation
        GameObject newSpot = Instantiate(spotPrefabs[currentLevel], position, rotation);
        newSpot.transform.SetParent(currentSpot.parent); // Set the parent to maintain hierarchy

        Debug.Log($"Spot upgraded to level {currentLevel + 1}!");
    }

    // Combined method to upgrade both wizard and spot
    public void UpgradeWizardAndSpot(float percentIncrease, Transform currentSpot)
    {
        UpgradeWizardDamage(percentIncrease); // Upgrade wizard
        UpgradeSpot(currentSpot); // Upgrade spot
    }
}
*/


/*using UnityEngine;
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
}*/
