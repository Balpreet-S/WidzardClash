using UnityEngine;
using UnityEngine.UI;

public class CooldownCounter : MonoBehaviour
{
    public Slider cooldownBar; // Reference to the slider
    public float cooldownDuration = 10f; // Duration for the cooldown
    private float cooldownTimer = 0f;
    private bool isCooldownActive = false;

    void Update()
    {
        if (isCooldownActive)
        {
            cooldownTimer -= Time.deltaTime;

            // Update the slider value (starts empty, fills up)
            float progress = Mathf.Clamp01(1 - (cooldownTimer / cooldownDuration));
            cooldownBar.value = progress;

            if (cooldownTimer <= 0f)
            {
                isCooldownActive = false;
                Debug.Log("Special power is ready!");
            }
        }
    }

    public void StartCooldown()
    {
        cooldownTimer = cooldownDuration;
        isCooldownActive = true;

        // Reset bar to empty at the start of the cooldown
        cooldownBar.value = 0f;
        Debug.Log("Cooldown started, bar is empty.");
    }

    public bool IsCooldownComplete()
    {
        // The power is ready when the bar is completely filled
        return !isCooldownActive;
    }
}
