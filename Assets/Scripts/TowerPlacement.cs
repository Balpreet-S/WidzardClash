using UnityEngine;

public class TowerPlacementManager : MonoBehaviour
{
    public GameObject towerPrefab;
    // Set this to a value above the cube's height
    private float towerHeightOffset = 1.15f; // Adjust this as neede

    void Update()
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
                        // Calculate the position to place the tower above the placement spot
                        Vector3 towerPosition = hit.collider.transform.position + new Vector3(0, towerHeightOffset, 0);
                        
                        // Instantiate a clone of the tower and place it at the new position
                        Instantiate(towerPrefab, towerPosition, Quaternion.identity, hit.collider.transform); 
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