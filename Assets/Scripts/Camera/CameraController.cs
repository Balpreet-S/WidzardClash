using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Camera Movement and Placement. Horizontal movement using wasd keys. Rotation using middle mouse button. Zoom in and out using scroll wheel.
public class CameraController : MonoBehaviour
{
    private CameraControlActions inputActions;
    private InputAction moveAction;
    private Transform cameraTransform;

    // Horizontal movement of camera
    [SerializeField]
    private float maxMoveSpeed = 5f;
    private float currentSpeed;
    [SerializeField]
    private float accelerationRate = 10f;
    [SerializeField]
    private float dampingRate = 15f;

    // Zooming of camera vertically using scroll wheel
    [SerializeField]
    private float zoomStep = 2f;
    [SerializeField]
    private float zoomDampening = 7.5f;
    [SerializeField]
    private float minZoomHeight = 5f;
    [SerializeField]
    private float maxZoomHeight = 50f;
    [SerializeField]
    private float zoomSpeed = 2f;

    // Camera rotation by clicking middle mouse button and moving the mouse
    [SerializeField]
    private float rotationSpeed = 1f;

    // Position and velocity tracking for speed and camera position
    private Vector3 targetMovePosition;
    private float currentZoomHeight;
    private Vector3 horizontalVelocity;
    private Vector3 lastFramePosition;

    // Initializes input actions and sets up camera references.
    private void Awake()
    {
        inputActions = new CameraControlActions();
        cameraTransform = GetComponentInChildren<Camera>().transform;
    }
    // Enables input actions and initializes movement and zoom settings.
    private void OnEnable()
    {
        currentZoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(transform);
        lastFramePosition = transform.position;

        moveAction = inputActions.Camera.Movement;
        inputActions.Camera.RotateCamera.performed += HandleCameraRotation;
        inputActions.Camera.ZoomCamera.performed += HandleCameraZoom;
        inputActions.Camera.Enable();
    }
    // Disables input actions and unsubscribes from events.
    private void OnDisable()
    {
        inputActions.Camera.RotateCamera.performed -= HandleCameraRotation;
        inputActions.Camera.ZoomCamera.performed -= HandleCameraZoom;
        inputActions.Camera.Disable();
    }

    // Handles frame-based updates for movement, position, and velocity.
    private void Update()
    {
        HandleMovementInput();
        UpdateCameraPosition();
        UpdateHorizontalVelocity();
        UpdateObjectPosition();
    }

    // Updates the camera's horizontal velocity.
    private void UpdateHorizontalVelocity()
    {
        horizontalVelocity = (transform.position - lastFramePosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastFramePosition = transform.position;
    }
    // Handles player input for horizontal camera movement.
    private void HandleMovementInput()
    {
        if (moveAction != null && moveAction.enabled)
        {
            Vector3 inputDirection = moveAction.ReadValue<Vector2>().x * GetCameraRight() +
                                     moveAction.ReadValue<Vector2>().y * GetCameraForward();
            inputDirection = inputDirection.normalized;

            if (inputDirection.sqrMagnitude > 0.1f && !float.IsNaN(inputDirection.x) && !float.IsNaN(inputDirection.z))
            {
                targetMovePosition += inputDirection;
            }
        }
    }
    // Gets the camera's right direction, ignoring vertical tilt.
    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0;
        return right.normalized;
    }
    // Gets the camera's forward direction, ignoring vertical tilt.
    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        return forward.normalized;
    }
    // Updates the position of the object the camera is following.
    private void UpdateObjectPosition()
    {
        Vector3 newPosition;

        if (targetMovePosition.sqrMagnitude > 0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxMoveSpeed, Time.deltaTime * accelerationRate);
            newPosition = transform.position + targetMovePosition * currentSpeed * Time.deltaTime;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * dampingRate);
            newPosition = transform.position + horizontalVelocity * Time.deltaTime;
        }

        // Check for NaN and assign position
        if (!float.IsNaN(newPosition.x) && !float.IsNaN(newPosition.z))
        {
            transform.position = newPosition;
        }

        targetMovePosition = Vector3.zero;
    }
    // Handles player input for rotating the camera around the target.
    private void HandleCameraRotation(InputAction.CallbackContext context)
    {
        if (!Mouse.current.middleButton.isPressed)
            return;

        float rotationInput = context.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0f, rotationInput * rotationSpeed + transform.rotation.eulerAngles.y, 0f);
    }
    // Handles player input for zooming the camera in and out.
    private void HandleCameraZoom(InputAction.CallbackContext context)
    {
        float zoomInput = -context.ReadValue<Vector2>().y / 100f;
        if (Mathf.Abs(zoomInput) > 0.1f)
        {
            currentZoomHeight = Mathf.Clamp(cameraTransform.localPosition.y + zoomInput * zoomStep, minZoomHeight, maxZoomHeight);
        }
    }
    // Smoothly updates the camera's position based on zoom and damping settings.
    private void UpdateCameraPosition()
    {
        Vector3 zoomTargetPosition = new Vector3(cameraTransform.localPosition.x, currentZoomHeight, cameraTransform.localPosition.z);
        zoomTargetPosition -= zoomSpeed * (currentZoomHeight - cameraTransform.localPosition.y) * Vector3.forward;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTargetPosition, Time.deltaTime * zoomDampening);
        cameraTransform.LookAt(transform);
    }
    // Resets camera movement and zoom variables when the game ends.
    public void OnGameEnd()
    {
        // Reset variables to avoid NaN errors
        targetMovePosition = Vector3.zero;
        horizontalVelocity = Vector3.zero;
        currentSpeed = 0f;
        currentZoomHeight = Mathf.Clamp(cameraTransform.localPosition.y, minZoomHeight, maxZoomHeight);
    }
}