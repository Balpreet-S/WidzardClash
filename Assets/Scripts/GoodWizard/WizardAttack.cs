using System.Collections.Generic;
using UnityEngine;

public class WizardAttack : MonoBehaviour
{
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;
    public Transform firePoint;
    public float rotationSpeed = 5f; // Speed of rotation toward the enemy

    public GameObject fireballPrefab;
    public GameObject waterfallPrefab;

    public int fireballPoolSize = 25;
    public int waterfallPoolSize = 25;

    public string wizardType; // Type of wizard ("Fire", "Water", etc.)

    private float attackTimer = 0f;
    private EnemyScript currentTarget;

    void Start()
    {
        // Initialize pools for each type of projectile
        Projectile.InitializePool("Fireball", fireballPoolSize, fireballPrefab);
        Projectile.InitializePool("Waterfall", waterfallPoolSize, waterfallPrefab);
    }

    void Update()
    {
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

        if (currentTarget == null)
        {
            FindTarget();
        }

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
            direction.y = 0;

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

            if (currentTarget.health <= 0)
            {
                currentTarget = null;
            }
        }
    }

    void ShootProjectile()
    {
        string projectileType = wizardType == "Fire" ? "Fireball" : "Waterfall";

        Projectile projectile = Projectile.GetFromPool(projectileType);
        if (projectile != null)
        {
            projectile.transform.position = firePoint.position;
            projectile.transform.rotation = firePoint.rotation;

            projectile.SetTarget(currentTarget.transform);

            Debug.Log($"{projectileType} instantiated at: {projectile.transform.position}");
        }
        else
        {
            Debug.LogError($"Failed to retrieve {projectileType} from pool.");
        }
    }
}
