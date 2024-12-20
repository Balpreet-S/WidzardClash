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

        //uncomment lines to check for run time errors
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
            // uncoment line to check errors with the projectile
            //Debug.LogError("Projectile: Target is null, cannot move towards target.");
            Destroy(gameObject);
            return;
        }

        // Move the projectile towards the target
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;


        // Check if the projectile is close enough to hit the target
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }


    // Deal damage to the enemy
    void HitTarget()
    {
        EnemyScript enemy = target.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    // collision for physics
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HitTarget();
        }
    }
}
