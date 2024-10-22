using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyScript : MonoBehaviour
{
    // Enemy parameters
    public int health = 100;
    public float movementSpeed = 5f;
    public int damageToCastle = 10;
    public float attackRange = 1f;
    public int xpValue = 5;

    private PathFollower pathFollower;
    private bool hasAttacked = false;  //check if enemy attacked

    //event for when the enemy dies
    public event Action OnDeath;

    // check if killed by tower or castle
    public bool killedByCastle = false;

    public void Initialize(Transform[] waypoints)
    {
        pathFollower = GetComponent<PathFollower>();

        if (pathFollower != null) //path for enemy wizzards
        {
            pathFollower.SetSpeed(movementSpeed);
            pathFollower.waypoints = waypoints;
            pathFollower.StartMoving();
        }
        else
        {
            Debug.LogError("PathFollower component not found on the Enemy!");
        }
    }

    void Update()
    {
        if (health <= 0) //if enemy health below or = 0, destroy 
        {
            Die();
        }

        TryAttackCastle();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Notify Enemy Spwaner listener that wizzard died
        OnDeath?.Invoke();

        // Only award XP if not killed by the castle
        if (!killedByCastle)
        {
            XPManager.instance.AddXP(xpValue);
        }


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
        killedByCastle = true; //enemy reaches the castle

        pathFollower.StopMoving();

        Die();  // Destroy the enemy wizzard after reaching the castle
    }
}