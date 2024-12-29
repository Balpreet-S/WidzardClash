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

        transform.position += direction * speed * Time.deltaTime;

        // Move to the next waypoint after reaching the current one
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
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
}
