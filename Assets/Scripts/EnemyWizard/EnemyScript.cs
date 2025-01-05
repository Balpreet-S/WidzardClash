using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/* This script defines the behavior of enemy characters in a Tower Defense game, including movement, 
interactions with the castle, and effects such as slowing and knockback*/


public class EnemyScript : MonoBehaviour
{
    public int health = 100;
    public float movementSpeed = 5f;
    public int damageToCastle = 10;
    public float attackRange = 1f;
    public int xpValue = 5;
    public float knockbackDuration = 0.5f;

    private PathFollower pathFollower;
    private bool hasAttacked = false;

    private float originalSpeed;
    private Coroutine slowEffectCoroutine;

    public event Action OnDeath;

    public bool killedByCastle = false;

    

    void Start()
    {
        originalSpeed = movementSpeed;
    }

    // Set up the path for the enemy to follow
    public void Initialize(Transform[] waypoints)
    {
        pathFollower = GetComponent<PathFollower>();

        if (pathFollower != null)
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
    // Update the enemy state and checks for interactions with the castle
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }

        TryAttackCastle();
    }

    // Reduce enemy health when taking damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    // Handle the enemy's death logic
    public void Die()
    {
        OnDeath?.Invoke();

        if (!killedByCastle)
        {
            XPManager.instance.AddXP(xpValue);
        }

        Destroy(gameObject);
    }

    // Destroy the enemy without awarding XP
    public void DieNoXP()
    {
        Destroy(gameObject);
    }

    // Return the current health of the enemy
    public int GetCurrentHealth()
    {
        return health;
    }

    // Check if the enemy can attack the castle
    void TryAttackCastle()
    {
        if (hasAttacked) return;
        CastleHealth castle = UnityEngine.Object.FindObjectOfType<CastleHealth>();

        if (castle != null)
        {
            float distanceToCastle = Vector3.Distance(transform.position, castle.transform.position);

            if (distanceToCastle <= attackRange)
            {
                ReachCastle();
            }
        }
    }

    // Handle logic for when the enemy reaches the castle
    public void ReachCastle()
    {
        CastleHealth castle = UnityEngine.Object.FindObjectOfType<CastleHealth>();

        if (castle != null)
        {
            castle.TakeDamage(damageToCastle);
        }

        hasAttacked = true;
        killedByCastle = true;

        pathFollower.StopMoving();

        Die();
    }

    // Apply a slowing effect to the enemy
    public void ApplySlow(float slowMultiplier, float duration)
    {
        if (slowEffectCoroutine != null)
        {
            StopCoroutine(slowEffectCoroutine);
            movementSpeed = originalSpeed;
        }

        Debug.Log("Slow effect applied!");
        movementSpeed *= slowMultiplier;
        slowEffectCoroutine = StartCoroutine(RemoveSlowAfterDelay(duration));
    }

    // Remove the slow effect after a delay
    private IEnumerator RemoveSlowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        movementSpeed = originalSpeed;
        slowEffectCoroutine = null;
    }

   // Apply a knockback effect to the enemy
    public void ApplyKnockback(Vector3 firingPosition, float knockbackForce, float knockbackDuration)
    {
        if (pathFollower != null)
        {
            pathFollower.ApplyKnockback(firingPosition, knockbackForce, knockbackDuration);
            Debug.Log("Knockback applied!");
        }
    }

}
