using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation
    public float zoomSpeed = 10f;       // Speed of zoom
    public float minZoom = 5f;          // Minimum zoom value
    public float maxZoom = 20f;         // Maximum zoom value

    private Camera mainCamera; // Renamed from 'camera' to 'mainCamera'

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    void Update()
    {
        // Rotate the camera around the Y-axis
        if (Input.GetMouseButton(1)) // Right mouse button to rotate
        {
            float rotationInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationInput, 0);
        }

        // Zoom in and out
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (zoomInput != 0)
        {
            float newZoom = mainCamera.fieldOfView - zoomInput * zoomSpeed;
            mainCamera.fieldOfView = Mathf.Clamp(newZoom, minZoom, maxZoom);
        }
    }
}