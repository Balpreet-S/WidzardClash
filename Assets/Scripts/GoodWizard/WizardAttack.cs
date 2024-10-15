using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAttack : MonoBehaviour
{
    public float attackRange = 5f;        // Range within which the wizard can attack enemies
    public float attackCooldown = 2f;     // Time between attacks
    public int attackDamage = 10;         // Damage dealt to enemies
    public GameObject projectilePrefab;   // Optional: Projectile prefab to shoot at enemies
    public Transform firePoint;           // Optional: The point from which the wizard fires projectiles

    private float attackTimer = 0f;       // Timer to track when the wizard can attack again
    private EnemyScript currentTarget;    // The enemy currently being targeted

    void Update()
    {
        // Check if there is a current target
        if (currentTarget != null)
        {
            // Check if the target is still within attack range
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distanceToTarget > attackRange)
            {
                // Target is out of range, stop attacking
                currentTarget = null;
                return; // Exit early to avoid attacking an out-of-range target
            }
        }

        // If there is no current target, search for a new one
        if (currentTarget == null)
        {
            FindTarget();
        }

        // If there is a target and attack cooldown is ready, attack the enemy
        if (currentTarget != null)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
            {
                AttackEnemy();
                attackTimer = attackCooldown;  // Reset cooldown after attacking
            }
        }
    }

    // Find the closest enemy within attack range
    void FindTarget()
    {
        // Find all colliders within the wizard's attack range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        float closestDistance = Mathf.Infinity;
        EnemyScript closestEnemy = null;

        foreach (Collider collider in hitColliders)
        {
            EnemyScript enemy = collider.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                // Calculate distance to the enemy
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                // Select the closest enemy
                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        // Set the closest enemy as the target
        currentTarget = closestEnemy;
    }

    // Attack the current target enemy
    void AttackEnemy()
    {
        if (currentTarget != null)
        {
            // Deal damage directly to the enemy
            currentTarget.TakeDamage(attackDamage);

            // Optional: If using projectiles, shoot a projectile
            if (projectilePrefab != null && firePoint != null)
            {
                ShootProjectile();
            }

            // Check if the target is dead, stop attacking if so
            if (currentTarget.health <= 0)
            {
                currentTarget = null;  // Reset target if the enemy is killed
            }
        }
    }

    // Optional: Shoot a projectile towards the enemy
    void ShootProjectile()
    {
        // Instantiate the projectile at the fire point's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Set the projectile's target to be the current enemy
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetTarget(currentTarget.transform);  // Make sure currentTarget is assigned properly
        }
        else
        {
            Debug.LogError("Projectile script is missing on the projectile prefab.");
        }
    }

    // Optional: Visualize the attack range in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}