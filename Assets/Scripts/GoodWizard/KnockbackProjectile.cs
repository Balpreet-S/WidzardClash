using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float knockbackForce = 5f;
    public float knockbackRadius = 3f;
    public GameObject impactEffect;

    public Vector3 firingPosition;

    private Transform target;
    private bool hasHit = false;

    public void SetTarget(Transform newTarget, Vector3 firingPosition)
    {
        target = newTarget;
        firingPosition = firingPosition;
        hasHit = false; 
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
        if (hasHit) return; // Prevent multiple hits
        hasHit = true;

        // Play explosion effect if assigned
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, knockbackRadius);
        foreach (Collider collider in hitColliders)
        {
            EnemyScript enemy = collider.GetComponent<EnemyScript>();
            if (enemy != null)
            { 
                enemy.TakeDamage(damage); 
                Vector3 knockbackDirection = (enemy.transform.position - transform.position).normalized;
                float knockbackDuration = knockbackDirection.magnitude / knockbackForce;
                enemy.ApplyKnockback(firingPosition, knockbackForce, knockbackDuration);
            }
        }
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !hasHit)
        {
            HitTarget();
        }
    }
}
