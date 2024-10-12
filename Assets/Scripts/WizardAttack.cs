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
        //Debug.Log("WizardAttack: Running Update.");

        // Look for enemies in range if the wizard isn't already attacking
        if (currentTarget == null)
        {
            //Debug.Log("WizardAttack: No current target. Searching for target...");
            FindTarget();
        }

        // Attack the current target if within range
        if (currentTarget != null)
        {
            //Debug.Log("WizardAttack: Target found. Checking if ready to attack...");
            attackTimer -= Time.deltaTime;

            // Check if the wizard can attack again (based on attack cooldown)
            if (attackTimer <= 0f)
            {
                //Debug.Log("WizardAttack: Ready to attack. Attacking target...");
                AttackEnemy();
                attackTimer = attackCooldown;  // Reset attack cooldown
            }
        }
    }

    // Find the closest enemy within attack range
    void FindTarget()
    {
        //Debug.Log("WizardAttack: Finding target...");
        
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
                //Debug.Log("WizardAttack: Found enemy at distance: " + distanceToEnemy);

                // Select the closest enemy
                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        // Set the closest enemy as the target
        /*
        if (closestEnemy != null)
        {
            Debug.Log("WizardAttack: New target acquired.");
        }
        else
        {
            Debug.Log("WizardAttack: No enemies in range.");
        }*/

        currentTarget = closestEnemy;
    }

    // Attack the current target enemy
    void AttackEnemy()
    {
        //Debug.Log("WizardAttack: Attacking enemy.");

        if (currentTarget != null)
        {
            //Debug.Log("WizardAttack: Dealing damage to enemy.");
            // Deal damage directly to the enemy
            currentTarget.TakeDamage(attackDamage);

            // Optional: If using projectiles, shoot a projectile
            
            if (projectilePrefab != null && firePoint != null)
            {
                //Debug.Log("WizardAttack: Firing projectile.");
                ShootProjectile();
            }

            // Check if the target is dead, stop attacking if so
            if (currentTarget.health <= 0)
            {
                //Debug.Log("WizardAttack: Target has been killed.");
                currentTarget = null;  // Reset target if the enemy is killed
            }
        }
        else
        {
            //Debug.Log("WizardAttack: No valid target to attack.");
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
