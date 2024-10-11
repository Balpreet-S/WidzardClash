using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower1 : MonoBehaviour
{
    public Transform[] waypoints;  // Array of waypoints for the path
    public float speed = 3f;       // Speed of the enemy's movement
    private int currentWaypointIndex = 0;  // Index of the current waypoint

    void Update()
    {
        MoveAlongPath();
    }

    void MoveAlongPath()
    {
        if (waypoints.Length == 0) return;  // If no waypoints are set, do nothing

        // Get the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Calculate the direction to the waypoint
        Vector3 direction = targetWaypoint.position - transform.position;
        direction.Normalize();  // Normalize the direction vector

        // Move towards the current waypoint
        transform.position += direction * speed * Time.deltaTime;

        // Check if the enemy has reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // Move to the next waypoint
            currentWaypointIndex++;

            // If we have reached the last waypoint, stop or loop back to the first one
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;  // Optional: loop back to the start (remove this if you want to stop at the end)
            }
        }
    }
}
