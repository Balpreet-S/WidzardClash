using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Array of waypoints the enemy will follow
    public Transform[] waypoints;

    // Index to track the current waypoint
    private int currentWaypointIndex = 0;

    // Speed of the enemy movement
    public float speed = 3f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Freeze rotation to prevent falling over
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY; // Optionally freeze Y position if needed
    }

    void FixedUpdate() // Use FixedUpdate for physics-based movement
    {
        // Move towards the current waypoint if there are still waypoints left
        if (currentWaypointIndex < waypoints.Length)
        {
            MoveTowardsWaypoint();
        }
    }

    void MoveTowardsWaypoint()
    {
        // If there are no waypoints assigned, do nothing
        if (waypoints.Length == 0)
            return;

        // Get the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Calculate the direction to the waypoint
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Calculate the target position
        Vector3 targetPosition = transform.position + direction * speed * Time.fixedDeltaTime;

        // Use Rigidbody to move the capsule smoothly towards the waypoint
        rb.MovePosition(Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.fixedDeltaTime));

        // Check if the capsule is close enough to the waypoint to consider it "reached"
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            Debug.Log("Reached waypoint: " + currentWaypointIndex);
            // Move to the next waypoint
            currentWaypointIndex++;

            // If all waypoints are reached, stop moving or perform an action
            if (currentWaypointIndex >= waypoints.Length)
            {
                StartAttacking();
            }
        }
    }

    void StartAttacking()
    {
        // Implement attacking logic
        Debug.Log("The wizard has reached the castle and is attacking!");
    }
}
