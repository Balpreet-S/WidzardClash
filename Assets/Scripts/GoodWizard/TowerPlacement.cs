using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
// Tower placement spot script. contains logic for placing the wizards on the tower placement spots.
public class TowerPlacementManager : MonoBehaviour
{
    public GameObject towerPrefab;
    private float towerHeightOffset = 0f;
    private InputAction placeTowerAction;

    // using left mouse button for input in new input system
    private void Awake()
    {
        placeTowerAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        placeTowerAction.Enable();
    }
    // Disable and dispose of the input action when the script is destroyed
    private void OnDestroy()
    {

        placeTowerAction.Disable();
        placeTowerAction.Dispose();
    }

    // check if the mouse is over the tower placement spot and if the left mouse button is pressed to place the tower
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && XPManager.instance.Button != null)
        {
            if (placeTowerAction.WasPerformedThisFrame())
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("TowerSpot"))
                    {
                        if (hit.collider.transform.childCount == 23) 
                        {

                            Vector3 towerPosition = hit.collider.transform.position + new Vector3(0, towerHeightOffset, 0);

                            GameObject towerInstance = Instantiate(XPManager.instance.Button.SkillTowers, towerPosition, Quaternion.identity);

                            towerInstance.transform.SetParent(hit.collider.transform);

                            towerInstance.transform.position = towerPosition;

                            XPManager.instance.PurchaseSkill();

                            Debug.Log("Tower placed!");

                        }
                        else
                        {
                            Debug.Log("Spot already occupied!");

                        }
                    }
                }
            }
        }
    }
}