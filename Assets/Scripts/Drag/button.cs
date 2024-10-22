using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class button : MonoBehaviour
{
    public GameObject tower;  // select the tower prefab
    public GameObject clone; // stores clone of tower
    public Button fireButton; // select the fire button

    public void Start()
    {
        Button btn = fireButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    public void TaskOnClick()
    {
        clone = Instantiate(tower); // creates clone of tower
        clone.AddComponent<movement2>(); // adds the movement script component to the tower clone
    }
    
    void Update() // deletes clone if clicked anywhere
    {
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(clone);
        }
    }
}
