using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement2 : MonoBehaviour
{
    Camera cam;
    public float hoverOffset = 50.0f; // Adjustable hover offset above the cursor

    // Start is called before the first frame update
    public void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    public void Update()
    {
        // Convert mouse position to world point with offset
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.y += hoverOffset; // Add an upward offset to the Y position

        // Adjust the position using ScreenToWorldPoint with the Z distance of the object
        Vector3 newPos = cam.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 75));
        transform.position = newPos;
    }
}
