using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//projectiles fired from towers
public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        //uncomment lines to chck for errors
        if (target != null)
        {
            //Debug.Log("Projectile: Target set to " + target.name);
        }
        else
        {
            //Debug.LogError("Projectile: Target is null when SetTarget was called.");
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
