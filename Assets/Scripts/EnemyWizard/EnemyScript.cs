using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Enemy parameters
    public int health = 100;           // Enemy's health
    public float movementSpeed = 5f;   // Movement speed for the enemy
    public int damageToCastle = 10;    // Damage to the castle when the enemy reaches it
    public float attackRange = 1f;     // Range within which the enemy can attack the castle
    public int xpValue = 5;           // XP given when the enemy wizard is killed

    private PathFollower pathFollower;  // Reference to the PathFollower component
    private bool hasAttacked = false;   // To ensure the enemy attacks only once

    public void Initialize(Transform[] waypoints)
    {
        pathFollower = GetComponent<PathFollower>();

        if (pathFollower != null)
        {
            pathFollower.SetSpeed(movementSpeed);
            pathFollower.waypoints = waypoints;  // Set the waypoints for the enemy to follow
            pathFollower.StartMoving();  // Ensure movement starts
        }
        else
        {
            Debug.LogError("PathFollower component not found on the Enemy!");
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            Die();  // Destroy the enemy and give XP
        }

        TryAttackCastle();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();  // Destroy the enemy and give XP
        }
    }

    void Die()
    {
        // Award XP to the player
        XPManager.instance.AddXP(xpValue);

        // Destroy the enemy
        Destroy(gameObject);
    }

    public int getCurrentHealth()
    {
        return health;
    }

    void TryAttackCastle()
    {
        if (hasAttacked) return;

        CastleHealth castle = Object.FindAnyObjectByType<CastleHealth>();

        if (castle != null)
        {
            float distanceToCastle = Vector3.Distance(transform.position, castle.transform.position);

            if (distanceToCastle <= attackRange)
            {
                Debug.Log(" ----------------------------- Attacking Castle ----------------------------- ");
                ReachCastle();
            }
        }
    }

    public void ReachCastle()
    {
        CastleHealth castle = Object.FindAnyObjectByType<CastleHealth>();

        if (castle != null)
        {
            castle.TakeDamage(damageToCastle);
        }

        hasAttacked = true;

        pathFollower.StopMoving();

        Die();  // Destroy the enemy after attacking the castle
    }
}