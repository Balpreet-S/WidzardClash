using UnityEngine;

public class CameraController : MonoBehaviour
{
    // camera usage variables
    public float rotationSpeed = 100f;
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Rotate the camera around the Y-axis (slow rotation as fast rotation not needed)
        if (Input.GetMouseButton(1))
        {
            float rotationInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationInput, 0);
        }

        // Zoom in and out on object
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (zoomInput != 0)
        {
            float newZoom = mainCamera.fieldOfView - zoomInput * zoomSpeed;
            mainCamera.fieldOfView = Mathf.Clamp(newZoom, minZoom, maxZoom);
        }
    }
}