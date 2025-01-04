using UnityEngine;

public class PopupMenuController : MonoBehaviour
{
    public GameObject popupMenu; // Assign the popup menu in the Inspector

    private void OnMouseDown()
    {
        if (popupMenu != null)
        {
            // Toggle the popup menu's visibility
            bool isActive = popupMenu.activeSelf;
            popupMenu.SetActive(!isActive);
        }
    }
}
