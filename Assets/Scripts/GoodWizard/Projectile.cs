using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public GameObject impactEffect; // Explosion effect prefab (optional)
    public float explosionRadius = 2f; // Area of effect radius (optional)

    private Transform target;
    private Animator animator; // Animator for animations
    private bool hasHit = false; // To prevent multiple triggers
    private static Dictionary<string, ObjectPool<Projectile>> projectilePools; // Separate pools for each projectile type

    public string type; // Type of projectile (e.g., "Fireball", "Waterfall")

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

    public static Projectile GetFromPool(string type)
    {
        if (projectilePools != null && projectilePools.ContainsKey(type))
        {
            return projectilePools[type].GetObject();
        }

        Debug.LogError($"No pool initialized for projectile type: {type}");
        return null;
    }

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

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        hasHit = false; // Reset state when reused
    }

    void OnEnable()
    {
        if (animator == null) animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.Play("LaunchAnimation"); // Play the projectile animation
        }
    }

    void Update()
    {
        if (target == null)
        {
            ReturnToPool(this);
            return;
        }

        // Rotate to face the target
        Vector3 direction = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame && !hasHit)
        {
            HitTarget();
            return;
        }

        // Move the projectile
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        if (hasHit) return; // Prevent multiple hits
        hasHit = true;

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in hitColliders)
        {
            EnemyScript enemy = collider.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        if (animator != null)
        {
            animator.Play("ImpactAnimation");
            StartCoroutine(ReturnToPoolAfterDelay(0.5f)); // Wait for impact animation to finish
        }
        else
        {
            ReturnToPool(this);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !hasHit)
        {
            HitTarget();
        }
    }

    IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool(this);
    }
}
