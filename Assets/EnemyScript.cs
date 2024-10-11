using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;  // Import the namespace for PathFollower

public class EnemyScript : MonoBehaviour
{
    // PathFollower parameters
    public PathCreator pathCreator;    // Reference to the PathCreator component
    public EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Stop;     // End of path instruction

    // Enemy parameters
    public int health = 100;           // Enemy's health
    public float movementSpeed = 5f;   // Movement speed for the enemy
    public int damageToCastle = 10;    // Damage to the castle when the enemy reaches it
    public float attackRange = 1f;     // Range within which the enemy can attack the castle

    private PathFollower pathFollower;  // Reference to the PathFollower component
    private bool hasAttacked = false;   // To ensure the enemy attacks only once

    void Start()
    {
        pathFollower = GetComponent<PathFollower>();

        if (pathFollower != null)
        {
            // Initialize PathFollower with necessary data
            pathFollower.Initialize(pathCreator, endOfPathInstruction, movementSpeed);
        }
        else
        {
            Debug.LogError("PathFollower component not found on the Enemy!");
        }
    }

    void Update()
    {
        // Check if the enemy's health is zero or less
        if (health <= 0)
        {
            Die();  // Destroy the enemy
        }

        // Check if the enemy is close enough to the castle to attack
        TryAttackCastle();
    }

    // Method to take damage
    public void TakeDamage(int damage)
    {
        health -= damage;

        // Check if the enemy's health has reached zero
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to destroy the enemy when health is zero or it reaches the castle
    void Die()
    {
        Destroy(gameObject);  // Destroy the enemy
    }

    // Check if the enemy is close enough to the castle and attack it
    void TryAttackCastle()
    {
        
        if (hasAttacked) return;  // Ensure the enemy only attacks once

        CastleHealth castle = Object.FindAnyObjectByType<CastleHealth>();  // Find the Castle in the scene

        if (castle != null)
        {
            float distanceToCastle = Vector3.Distance(transform.position, castle.transform.position);
            Debug.Log("-----------------  distance to castle. ----------------- " + distanceToCastle);

            // If the enemy is close enough to the castle, attack it
            if (distanceToCastle <= attackRange)
            {
                ReachCastle();
            }
        }
    }

    // Call this when the enemy reaches the castle
    public void ReachCastle()
    {
        CastleHealth castle = Object.FindAnyObjectByType<CastleHealth>();

        if (castle != null)
        {
            castle.TakeDamage(damageToCastle);  // Deal damage to the castle
        }

        hasAttacked = true;  // Mark as attacked to prevent multiple attacks

        // Stop the enemy from moving further
        pathFollower.StopMoving();

        Die();  // Destroy the enemy after attacking the castle
    }
}
