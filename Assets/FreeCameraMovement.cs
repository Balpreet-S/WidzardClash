using UnityEngine;
using Cinemachine;

public class FreeLookCameraController : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;  // Reference to your FreeLook camera
    public float zoomSpeed = 10f;               // Speed of zoom
    public float minZoom = 5f;                  // Minimum zoom distance
    public float maxZoom = 50f;                 // Maximum zoom distance
    private Camera mainCamera;

    void Start()
    {
        if (freeLookCamera == null)
        {
            freeLookCamera = GetComponent<CinemachineFreeLook>();
        }

        // Reference to the main camera
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Check if the left mouse button is pressed for panning
        if (Input.GetMouseButton(0))  // Left Mouse Button
        {
            // Enable X and Y axis input
            freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
            freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";
        }
        else
        {
            // Disable X and Y axis input when the button is not pressed
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
        }

        // Handle zoom based on scroll wheel input
        HandleZoom();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            // Perform a raycast from the mouse position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Get the point where the ray hit an object
                Vector3 hitPoint = hit.point;

                // Calculate the zoom amount based on the scroll input
                float zoomAmount = scroll * zoomSpeed;

                // Adjust the orbits (rigs) of the Cinemachine FreeLook camera based on hit point
                AdjustCameraZoom(zoomAmount, hitPoint);
            }
        }
    }

    void AdjustCameraZoom(float zoomAmount, Vector3 hitPoint)
    {
        // Get the current camera position and the direction to the hit point
        Vector3 cameraPosition = freeLookCamera.transform.position;
        Vector3 direction = hitPoint - cameraPosition;

        // Calculate new zoom distance based on scroll input
        float currentDistance = direction.magnitude;
        float newDistance = Mathf.Clamp(currentDistance - zoomAmount, minZoom, maxZoom);

        // Update camera position to zoom toward the hit point
        freeLookCamera.transform.position = hitPoint - direction.normalized * newDistance;

        // Adjust Cinemachine FreeLook orbit radii for smoother zoom effect
        float middleRigRadius = freeLookCamera.m_Orbits[1].m_Radius;
        float newRadius = Mathf.Clamp(middleRigRadius - zoomAmount, minZoom, maxZoom);

        freeLookCamera.m_Orbits[0].m_Radius = newRadius; // Top rig
        freeLookCamera.m_Orbits[1].m_Radius = newRadius; // Middle rig
        freeLookCamera.m_Orbits[2].m_Radius = newRadius; // Bottom rig
    }
}
