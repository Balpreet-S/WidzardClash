using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialWaterProjectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public int damage = 5; // Damage dealt by the projectile
    public float slowEffectDuration = 3f; // Duration of the slow effect
    public float slowEffectMultiplier = 0.5f; // Percentage to slow the enemy (e.g., 0.5 for 50%)
    public GameObject impactEffect; // Optional impact visual effect

    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
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
        if (target != null)
        {
            // Apply damage and slow effect to the enemy
            EnemyScript enemy = target.GetComponent<EnemyScript>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                enemy.ApplySlow(slowEffectMultiplier, slowEffectDuration); // Call a method on EnemyScript to apply the slow effect
                Debug.Log($"Special Projectile: Applied slow effect to enemy. Slow Multiplier: {slowEffectMultiplier}, Duration: {slowEffectDuration}");
            }
            else{
                Debug.LogError("Special Projectile: EnemyScript not found on target.");
            }
        }

        // Play impact effect if assigned
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Destroy the projectile after it hits the target
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HitTarget();
        }
    }
}
