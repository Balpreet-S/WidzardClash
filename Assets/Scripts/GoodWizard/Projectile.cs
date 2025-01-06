using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base projectile for all projectiles to build on and basic wizard. (this is used as an abstract)
public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Speed at which the projectile moves.
    public int damage = 10; // Base damage dealt by the projectile.
    public GameObject impactEffect; // Visual effect displayed upon impact.
    public float explosionRadius = 2f; // Radius of the explosion effect.
    private float damageMultiplier = 1.0f; // Multiplier to adjust damage.

    private Transform target; // The target the projectile is moving towards.
    private Animator animator; // Animator component for animations.
    private bool hasHit = false; // Tracks if the projectile has already hit a target.
    private static Dictionary<string, ObjectPool<Projectile>> projectilePools; // Pools for different projectile types.

    public string type; // Type of the projectile
    public float maxLifetime = 5f; // Maximum lifetime of the projectile before it is returned to the pool.
    private float lifetimeTimer; // Tracks the remaining lifetime of the projectile.

    // Initializes a projectile pool of a specified size for a given type.
    public static void InitializePool(string type, int size, GameObject prefab)
    {
        if (projectilePools == null)
        {
            projectilePools = new Dictionary<string, ObjectPool<Projectile>>();
        }

        if (!projectilePools.ContainsKey(type))
        {
            projectilePools[type] = new ObjectPool<Projectile>(size, prefab);
        }
    }

    // Retrieves a projectile from the pool of the specified type.
    public static Projectile GetFromPool(string type)
    {
        if (projectilePools != null && projectilePools.ContainsKey(type))
        {
            return projectilePools[type].GetObject();
        }

        Debug.LogError($"No pool initialized for projectile type: {type}");
        return null;
    }

    // Returns a projectile to its respective pool.
    public static void ReturnToPool(Projectile projectile)
    {
        if (projectilePools != null && projectilePools.ContainsKey(projectile.type))
        {
            projectilePools[projectile.type].ReturnObject(projectile);
        }
        else
        {
            Destroy(projectile.gameObject);
        }
    }

    // Sets the target for the projectile.
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        hasHit = false;
    }

    // Sets the damage multiplier for the projectile.
    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    // Called when the projectile is enabled, initializes its state.
    void OnEnable()
    {
        if (animator == null) animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.Play("LaunchAnimation");
        }

        lifetimeTimer = maxLifetime; // Reset the lifetime timer.
    }

    // Updates the projectile's position and checks for collisions or timeout.
    void Update()
    {
        if (target == null)
        {
            ReturnToPool(this); // Return to pool if no target.
            return;
        }

        lifetimeTimer -= Time.deltaTime; // Decrease lifetime timer.
        if (lifetimeTimer <= 0f && !hasHit)
        {
            ReturnToPool(this); // Return to pool if lifetime expires.
            return;
        }

        Vector3 direction = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame && !hasHit)
        {
            HitTarget(); // Hit the target if within distance.
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    // Handles logic for when the projectile hits its target.
    void HitTarget()
    {
        if (hasHit) return;
        hasHit = true;

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }

        float finalDamage = damage * damageMultiplier;

        // Apply damage to all enemies within the explosion radius.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in hitColliders)
        {
            EnemyScript enemy = collider.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(Mathf.RoundToInt(finalDamage));
            }
        }

        if (animator != null)
        {
            animator.Play("ImpactAnimation");
            StartCoroutine(ReturnToPoolAfterDelay(0.5f)); // Wait before returning to pool.
        }
        else
        {
            ReturnToPool(this);
        }
    }

    // Handles logic for when the projectile collides with an object.
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !hasHit)
        {
            HitTarget(); // Hit the target if it is an enemy.
        }
    }

    // Waits for a delay before returning the projectile to the pool.
    IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool(this);
    }
}