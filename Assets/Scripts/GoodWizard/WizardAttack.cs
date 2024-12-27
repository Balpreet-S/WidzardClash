using System.Collections.Generic;
using UnityEngine;

public class WizardAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;
    public float rotationSpeed = 5f;  // Speed of rotation toward the enemy

    [Header("Wizard Type (e.g., 'Fire', 'Water', 'Earth')")]
    public string wizardType;

    [Header("Fire Wizard")]
    public GameObject fireballPrefab;
    public int fireballPoolSize = 25;

    [Header("Water Wizard")]
    public GameObject waterballPrefab;
    public int waterballPoolSize = 25;

    [Header("Earth Wizard")]
    public GameObject earthballPrefab;
    public int earthballPoolSize = 25;

    [Header("Firing Point")]
    public Transform firePoint;

    private float attackTimer = 0f;
    private EnemyScript currentTarget;

    void Start()
    {
        // Only initialize the pool for the wizard's actual projectile type
        if (wizardType == "Fire")
        {
            Projectile.InitializePool("Fireball", fireballPoolSize, fireballPrefab);
        }
        else if (wizardType == "Water")
        {
            Projectile.InitializePool("Waterball", waterballPoolSize, waterballPrefab);
        }
        else if (wizardType == "Earth")
        {
            Projectile.InitializePool("Earthball", earthballPoolSize, earthballPrefab);
        }
    }

    void Update()
    {
        // If we have a target, check if it's still in range, and rotate if needed
        if (currentTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);

            if (distanceToTarget > attackRange)
            {
                currentTarget = null;
                return;
            }

            RotateTowardsTarget();
        }

        // If we don't have a target, try to find one
        if (currentTarget == null)
        {
            FindTarget();
        }

        // If we have a target, handle attack cooldown
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

    void RotateTowardsTarget()
    {
        if (currentTarget != null)
        {
            Vector3 direction = currentTarget.transform.position - transform.position;
            direction.y = 0;  // Keep rotation horizontal

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

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
                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distanceToEnemy;
                }
            }
        }

        currentTarget = closestEnemy;
    }

    void AttackEnemy()
    {
        if (currentTarget != null)
        {
            currentTarget.TakeDamage(attackDamage);

            if (firePoint != null)
            {
                ShootProjectile();
            }

            // If the enemy died, clear the target
            if (currentTarget.health <= 0)
            {
                currentTarget = null;
            }
        }
    }

    void ShootProjectile()
    {
        // Decide which projectile type to shoot
        string projectileType;
        if (wizardType == "Fire")
            projectileType = "Fireball";
        else if (wizardType == "Water")
            projectileType = "Waterball";
        else if (wizardType == "Earth")
            projectileType = "Earthball";
        else
            projectileType = "Fireball"; // Fallback, just in case

        Projectile projectile = Projectile.GetFromPool(projectileType);
        if (projectile != null)
        {
            projectile.transform.position = firePoint.position;
            projectile.transform.rotation = firePoint.rotation;
            projectile.SetTarget(currentTarget.transform);

            // Optional: Adjust projectile damage or other properties here
            // projectile.damage = attackDamage;

            Debug.Log($"{projectileType} instantiated at: {projectile.transform.position}");
        }
        else
        {
            Debug.LogError($"Failed to retrieve {projectileType} from pool.");
        }
    }
}
