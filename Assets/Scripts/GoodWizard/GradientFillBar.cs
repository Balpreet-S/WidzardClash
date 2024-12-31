using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[ExecuteInEditMode]
public class ImageFillGradient : MonoBehaviour
{
    [SerializeField] private Gradient _gradient = null; // Gradient for color transition
    [SerializeField] private float cooldownDuration = 10f; // Total duration of the cooldown
    private Image _image; // Reference to the Image component
    private float cooldownTimer = 0f; // Remaining cooldown time
    private bool isCooldownActive = false; // Whether the cooldown is currently active

    private void Awake()
    {
        _image = GetComponent<Image>();

        if (_gradient == null)
        {
            Debug.LogError("ImageFillGradient: Gradient is not assigned!");
        }
    }

    private void Update()
    {
        if (isCooldownActive)
        {
            // Reduce the cooldown timer
            cooldownTimer -= Time.deltaTime;

            // Calculate the progress (0 = empty, 1 = full)
            float progress = Mathf.Clamp01(1 - (cooldownTimer / cooldownDuration));

            // Update the fill amount and color
            _image.fillAmount = progress;
            _image.color = _gradient.Evaluate(progress);

            // Stop the cooldown if it is complete
            if (cooldownTimer <= 0f)
            {
                isCooldownActive = false;
                Debug.Log("Cooldown complete!");
            }
        }
    }

    /// <summary>
    /// Starts the cooldown process.
    /// </summary>
    public void StartCooldown()
    {
        cooldownTimer = cooldownDuration;
        isCooldownActive = true;

        // Reset the fill amount to empty
        _image.fillAmount = 0f;
        Debug.Log("Cooldown started.");
    }

    /// <summary>
    /// Checks if the cooldown is complete.
    /// </summary>
    public bool IsCooldownComplete()
    {
        return !isCooldownActive;
    }
}
