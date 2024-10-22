using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//tower behaviour
public class WizardAttack : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;
    public GameObject projectilePrefab;
    public Transform firePoint;

    private float attackTimer = 0f;
    private EnemyScript currentTarget;

    void Update()
    {
        if (currentTarget != null)
        {
            // Check if the target is still within attack range
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distanceToTarget > attackRange)
            {
                currentTarget = null;
                return;
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
                attackTimer = attackCooldown;
            }
        }
    }

    // Find the closest enemy within attack range
    void FindTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);

        float closestDistance = Mathf.Infinity;
        EnemyScript closestEnemy = null;

        foreach (Collider collider in hitColliders)
        {
            EnemyScript enemy = collider.GetComponent<EnemyScript>();
            if (enemy != null)
            {

                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                // Select the closest enemy
                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distanceToEnemy;
                }
            }
        }
        currentTarget = closestEnemy;
    }

    // Attack the current target enemy
    void AttackEnemy()
    {
        if (currentTarget != null)
        {

            currentTarget.TakeDamage(attackDamage);

            if (projectilePrefab != null && firePoint != null)
            {
                ShootProjectile();
            }

            if (currentTarget.health <= 0)
            {
                currentTarget = null;
            }
        }
    }

    // Shoot a projectile towards the enemy
    void ShootProjectile()
    {

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);


        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetTarget(currentTarget.transform);
        }
        else
        {
            Debug.LogError("Projectile script is missing on the projectile prefab.");
        }
    }
}