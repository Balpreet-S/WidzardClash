using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFireballProjectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the fireball
    public int damage = 20; // Damage dealt to each enemy in the area
    public float explosionRadius = 5f; // Radius of the explosion
    public GameObject explosionEffect; // Optional explosion visual effect

    private Transform target; // Target for the fireball
    private bool hasHit = false; // Prevent multiple hits

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        hasHit = false; // Reset state when reused
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Destroy the projectile if the target no longer exists
            return;
        }

        // Move toward the target
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        if (hasHit) return; // Prevent multiple hits
        hasHit = true;

        // Play explosion effect if assigned
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Deal damage to all enemies in the explosion radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        List<string> enemyNames = new List<string>();
        foreach (Collider collider in hitColliders)
        {
            EnemyScript enemy = collider.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                enemyNames.Add(enemy.gameObject.name);
            }
        }

        if (enemyNames.Count > 0)
        {
            Debug.Log($"Explosion hit {enemyNames.Count} enemies: {string.Join(", ", enemyNames)}");
        }
        else
        {
            Debug.Log("Explosion hit no enemies.");
        }

        Destroy(gameObject); // Destroy the fireball after the explosion
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !hasHit)
        {
            HitTarget();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
