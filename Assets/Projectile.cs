using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;        // Speed of the projectile
    public int damage = 10;          // Damage dealt by the projectile
    private Transform target;        // The enemy the projectile is targeting

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            //Debug.Log("Projectile: Target set to " + target.name);
        }
        else
        {
            Debug.LogError("Projectile: Target is null when SetTarget was called.");
        }
    }


    void Update()
    {
        if (target == null)
        {
            Debug.LogError("Projectile: Target is null, cannot move towards target.");
            Destroy(gameObject);  // Destroy projectile if there is no target
            return;
        }

        // Move the projectile towards the target
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        //Debug.Log("Projectile: Moving towards target. Distance this frame: " + distanceThisFrame);

        // Check if the projectile is close enough to hit the target
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Move the projectile forward
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }


    void HitTarget()
    {
        // Deal damage to the enemy
        EnemyScript enemy = target.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Destroy the projectile after hitting the target
        Destroy(gameObject);
    }

    // Optional: Add collision detection if using physics
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HitTarget();  // Call the hit logic
        }
    }
}
