using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public Transform[] waypoints;
    private float speed;
    private int currentWaypointIndex = 0;
    private bool shouldMove = true;

    void Update()
    {
        if (shouldMove)
        {
            MoveAlongPath();
        }
    }

    //function for wizzards to move along the set waypoints
    void MoveAlongPath()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        Vector3 direction = targetWaypoint.position - transform.position;
        direction.Normalize();

        transform.position += direction * speed * Time.deltaTime;
        //move to next waypoint after reaching the first
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

    // Function to set a new speed for the enemy helpful for changing speeds for special enemy wizzards in final game
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}