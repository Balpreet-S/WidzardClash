using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Reference to the enemy (or any target to follow)
    public Transform target;

    // Offset between the camera and the enemy
    public Vector3 offset = new Vector3(0, 10, -10);

    // Speed of following the enemy
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the target position for the camera
            Vector3 targetPosition = target.position + offset;

            // Smoothly move the camera towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            // Optional: Make the camera look at the enemy
            transform.LookAt(target);
        }
    }
}
