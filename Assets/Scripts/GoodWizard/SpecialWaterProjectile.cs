using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// COMMENTS COMPLETED

public class SpecialWaterProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 5;
    public float slowEffectDuration = 3f;
    public float slowEffectMultiplier = 0.5f;
    public GameObject impactEffect;

    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
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
                enemy.ApplySlow(slowEffectMultiplier, slowEffectDuration);
                Debug.Log($"Special Projectile: Applied slow effect to enemy. Slow Multiplier: {slowEffectMultiplier}, Duration: {slowEffectDuration}");
            }
            else{
                Debug.LogError("Special Projectile: EnemyScript not found on target.");
            }
        }

        
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HitTarget();
        }
    }
}
