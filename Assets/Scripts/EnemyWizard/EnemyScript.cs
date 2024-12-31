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
    public float knockbackDuration = 0.5f;

    private PathFollower pathFollower;
    private bool hasAttacked = false;

    private float originalSpeed;
    private Coroutine slowEffectCoroutine; // Corrected typo here

    public event Action OnDeath;

    public bool killedByCastle = false;

    

    void Start()
    {
        originalSpeed = movementSpeed;
    }

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

    void Update()
    {
        if (health <= 0)
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

    public void Die()
    {
        // Notify listeners that the enemy died, for wave starting/ending logic
        OnDeath?.Invoke();

        // Only award XP if not killed by the castle
        if (!killedByCastle)
        {
            XPManager.instance.AddXP(xpValue);
        }

        Destroy(gameObject);
    }

    public int GetCurrentHealth()
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
        killedByCastle = true;

        pathFollower.StopMoving();

        Die(); // Destroy the enemy after reaching the castle
    }

    public void ApplySlow(float slowMultiplier, float duration)
    {
        // Cancel any existing slow effect
        if (slowEffectCoroutine != null)
        {
            StopCoroutine(slowEffectCoroutine);
            movementSpeed = originalSpeed; // Reset to original speed before applying a new slow effect
        }

        Debug.Log($"enemy: Applying slow effect, Original speed {originalSpeed}, New speed {movementSpeed * slowMultiplier}, Duration {duration}");
        movementSpeed *= slowMultiplier; // Apply the slow effect
        slowEffectCoroutine = StartCoroutine(RemoveSlowAfterDelay(duration));
    }

    private IEnumerator RemoveSlowAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        movementSpeed = originalSpeed; // Restore the original speed
        Debug.Log($"Enemey: Slow effect expired, New speed {originalSpeed}");
        slowEffectCoroutine = null; // Clear the coroutine reference
    }

   
    public void ApplyKnockback(Vector3 firingPosition, float knockbackForce, float knockbackDuration)
    {
        // Temporarily stop path-following
        if (pathFollower != null)
        {
            //pathFollower.StopMoving();
            pathFollower.ApplyKnockback(firingPosition, knockbackForce, knockbackDuration);
        }
    }

}
