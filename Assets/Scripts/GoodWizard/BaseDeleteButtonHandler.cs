using UnityEngine;
using UnityEngine.UI;
using System.Collections;



// uiWindow.SetActive(true);

public class DeleteButtonHandler : MonoBehaviour
{
    public GameObject uiWindowPrefab; // Assign your prefab in the Inspector
  
    private GameObject activeWindow; // To track the currently active panel
    private Camera mainCamera;


    private WizardAttack wizardAttack;

    void Start()
    {
        mainCamera = Camera.main;

        wizardAttack = GetComponent<WizardAttack>();
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
        buttons[0].onClick.AddListener(DeleteWizard);

        Debug.Log("UI Window opened and positioned.");
    }
    public void DeleteWizard()
    {
        Debug.Log("Delete Wizard!");
        Destroy(gameObject);
        Destroy(activeWindow);
        activeWindow = null;
    }
}
