using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Needed for the Action delegate

public class EnemyScript : MonoBehaviour
{
    // Enemy parameters
    public int health = 100;           // Enemy's health
    public float movementSpeed = 5f;   // Movement speed for the enemy
    public int damageToCastle = 10;    // Damage to the castle when the enemy reaches it
    public float attackRange = 1f;     // Range within which the enemy can attack the castle
    public int xpValue = 5;            // XP given when the enemy wizard is killed

    private PathFollower pathFollower;  // Reference to the PathFollower component
    private bool hasAttacked = false;   // To ensure the enemy attacks only once

    // Declare an event that will be triggered when the enemy dies
    public event Action OnDeath;

    // Flag to check if the enemy was killed by the castle or by the player
    public bool killedByCastle = false;

    public void Initialize(Transform[] waypoints)
    {
        pathFollower = GetComponent<PathFollower>();

        if (pathFollower != null)
        {
            pathFollower.SetSpeed(movementSpeed);
            pathFollower.waypoints = waypoints;  // Set the waypoints for the enemy to follow
            pathFollower.StartMoving();          // Ensure movement starts
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
        // Notify any listeners (e.g., EnemySpawner) that this enemy has died
        OnDeath?.Invoke();

        // Only award XP if not killed by the castle
        if (!killedByCastle)
        {
            XPManager.instance.AddXP(xpValue);
        }

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
        CastleHealth castle = UnityEngine.Object.FindObjectOfType<CastleHealth>();

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
        CastleHealth castle = UnityEngine.Object.FindObjectOfType<CastleHealth>();

        if (castle != null)
        {
            castle.TakeDamage(damageToCastle);
        }

        hasAttacked = true;
        killedByCastle = true; // Set the flag when the enemy reaches the castle

        pathFollower.StopMoving();

        Die();  // Destroy the enemy after attacking the castle
    }
}