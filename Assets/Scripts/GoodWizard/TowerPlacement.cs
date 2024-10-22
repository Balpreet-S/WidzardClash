using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacementManager : MonoBehaviour
{
    public GameObject towerPrefab;
    private float towerHeightOffset = 1.15f; // Adjust this for correct height

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && XPManager.instance.Button != null)
        {
            if (Input.GetMouseButtonDown(0)) // Detect left mouse click
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Check if the player is pointing in the correct spot
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("TowerSpot")) // Checks if the place the player clicked is a placable tower spot
                    {
                        if (hit.collider.transform.childCount == 0) // If spot is empty
                        {
                            // Calculate the exact position to place the tower above the placement spot
                            Vector3 towerPosition = hit.collider.transform.position + new Vector3(0, towerHeightOffset, 0);

                            // Instantiate the tower at the calculated position
                            GameObject towerInstance = Instantiate(XPManager.instance.Button.SkillTowers, towerPosition, Quaternion.identity);

                            // Parent the tower to the spot so childCount increases
                            towerInstance.transform.SetParent(hit.collider.transform);

                            // Force the position to be exactly at the calculated position, to avoid any other influences
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