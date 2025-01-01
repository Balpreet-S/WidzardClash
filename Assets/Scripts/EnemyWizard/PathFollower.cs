using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Transform[] waypoints;
    private float speed; // This can still be set via SetSpeed if needed.
    private int currentWaypointIndex = 0;
    private bool shouldMove = true;

    private EnemyScript enemyScript; // Reference to EnemyScript to fetch speed dynamically
    private Quaternion modelRotationOffset = Quaternion.Euler(0, 0, 0);

    private Coroutine knockbackCoroutine;

    void Start()
    {
        // Cache the EnemyScript component
        enemyScript = GetComponent<EnemyScript>();


        if (enemyScript == null)
        {
            Debug.LogError("PathFollower: EnemyScript not found on this GameObject!");
        }
    }

    void Update()
    {

        Vector3 lookAtPosition = transform.position + transform.forward * 10; // Extend forward direction to visualize

        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue, 3f);

        if (shouldMove)
        {
            MoveAlongPath();
        }
    }

    // Function for enemies to move along the set waypoints
    void MoveAlongPath()
    {
        if (waypoints.Length == 0) return;

        // Fetch speed from EnemyScript if available
        if (enemyScript != null)
        {
            speed = enemyScript.movementSpeed;
        }

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        Vector3 direction = targetWaypoint.position - transform.position;
        direction.Normalize();

        // Rotate to face the next waypoint
        if (direction != Vector3.zero) // Ensure direction is not zero to avoid errors
        {
            //Debug.Log("Direction should be updates, AND IT IS: " + direction);
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            Quaternion adjustedRotation = targetRotation * modelRotationOffset;

            Quaternion rotationOffset = Quaternion.Euler(90, -90, 0); 

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);

        }

        transform.position += direction * speed * Time.deltaTime;

        // Move to the next waypoint after reaching the current one
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;

            // Loop back to the start or stop moving if it's the last waypoint
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0; // Or set `shouldMove = false;` if no looping
            }
        }
    }



    // Method to stop the enemy from moving
    public void StopMoving()
    {
        shouldMove = false;
    }

    // Method to start moving the enemy again
    public void StartMoving()
    {
        shouldMove = true;
    }

    // Function to reset the enemy back to the first waypoint
    public void ResetToStart()
    {
        currentWaypointIndex = 0;
        transform.position = waypoints[currentWaypointIndex].position;
    }

    // Function to set a new speed for the enemy (still useful for initialization or special cases)
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Function to move to the next waypoint
    public void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        // Increment the current waypoint index
        currentWaypointIndex++;

        // Loop back to the start if at the last waypoint
        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = 0;
        }

        // Move to the next waypoint's position instantly
        transform.position = waypoints[currentWaypointIndex].position;

        Debug.Log($"Moved to next waypoint: {currentWaypointIndex} at position {waypoints[currentWaypointIndex].position}");
    }

    // New: Apply knockback effect
    // Apply knockback effect with emulated force-based logic
    public void ApplyKnockback(Vector3 firingPosition, float force, float duration)
    {
        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine); // Stop any existing knockback
        }

        knockbackCoroutine = StartCoroutine(KnockbackCoroutine(firingPosition, force, duration));
    }

private IEnumerator KnockbackCoroutine(Vector3 firingPosition, float force, float duration)
{
    StopMoving(); // Temporarily disable path following

    // Main knockback direction (away from the projectile)
    Vector3 knockbackDirection = (transform.position - firingPosition).normalized;

    // Calculate lateral direction (perpendicular to knockback direction and "up")
    Vector3 lateralDirection = Vector3.Cross(Vector3.up, knockbackDirection).normalized;

    Debug.Log($" Lateral Direction: {lateralDirection}");
    // Determine whether to push left or right based on the position of the projectile
    //float side = Vector3.Dot(projectilePosition - transform.position, transform.right);
    
    //lateralDirection *= Mathf.Sign(side); // Adjust lateral direction to match the side

    Vector3 firingToEnemy = transform.position - firingPosition; // Direction from firing position to enemy
    float side = Vector3.Dot(firingToEnemy, transform.right); // Positive = right, Negative = left
    Debug.Log($"Side: {side}");
    lateralDirection *= Mathf.Sign(side); 

    // Combine knockback direction with lateral movement
    Vector3 finalKnockback = (knockbackDirection + lateralDirection).normalized * (force / duration);

    Debug.Log($"Knockback Direction: {knockbackDirection}, Lateral Direction: {lateralDirection}, Final Knockback: {finalKnockback}");

    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
        elapsedTime += Time.deltaTime;

        // Apply knockback movement
        transform.position += finalKnockback * Time.deltaTime;

        yield return null; // Wait for the next frame
    }

    StartMoving(); // Re-enable path following after knockback
    knockbackCoroutine = null; // Clear the coroutine reference
}



}
