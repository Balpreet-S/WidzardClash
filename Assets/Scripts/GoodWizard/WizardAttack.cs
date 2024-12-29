using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;
    public float rotationSpeed = 5f; // Speed of rotation toward the enemy

    [Header("Wizard Type (e.g., 'Fire', 'Water', 'Earth')")]
    public string wizardType;

    [Header("Fire Wizard")]
    public GameObject fireballPrefab;
    public int fireballPoolSize = 25;

    [Header("Water Wizard")]
    public GameObject waterballPrefab;
    public int waterballPoolSize = 25;

    [Header("Earth Wizard")]
    public GameObject earthballPrefab;
    public int earthballPoolSize = 25;

    [Header("Firing Point")]
    public Transform firePoint;

    [Header("Special Attack")]
    public GameObject specialProjectilePrefab; // Special projectile prefab
    public float specialAttackCooldown = 10f; // Cooldown between special attacks
    private float specialAttackTimer = 0f; // Timer to track cooldown


    [Header("Audio Settings")]
    public AudioClip fireballSound; // Fire wizard sound effect
    public AudioClip waterballSound; // Water wizard sound effect
    public AudioClip earthballSound; // Earth wizard sound effect
    private AudioSource audioSource;

    private float attackTimer = 0f;
    private float soundCooldown = 1f; // Cooldown for the sound (specific to water wizard)
    private float soundCooldownTimer = 0f; // Timer to track cooldown for the sound
    private EnemyScript currentTarget;

    void Start()
    {
        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Initialize the pool for the wizard's actual projectile type
        if (wizardType == "Fire")
        {
            Projectile.InitializePool("Fireball", fireballPoolSize, fireballPrefab);
        }
        else if (wizardType == "Water")
        {

            Projectile.InitializePool("Waterball", waterballPoolSize, waterballPrefab);


        }
        else if (wizardType == "Earth")
        {
            Projectile.InitializePool("Earthball", earthballPoolSize, earthballPrefab);
        }
    }

    void Update()
    {
        // Reduce the sound cooldown timer every frame
        if (soundCooldownTimer > 0)
        {
            soundCooldownTimer -= Time.deltaTime;
        }

        if (specialAttackTimer > 0)
        {
            specialAttackTimer -= Time.deltaTime;
        }

        // If we have a target, check if it's still in range, and rotate if needed
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

        // If we don't have a target, try to find one
        if (currentTarget == null)
        {
            FindTarget();
        }

        // If we have a target, handle attack cooldown
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
            direction.y = 0; // Keep rotation horizontal

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

            // If the enemy died, clear the target
            if (currentTarget.health <= 0)
            {
                currentTarget = null;
            }
        }
    }


    void ShootProjectile()
    {
        GameObject projectilePrefab;

        // Check if the wizard can shoot a special projectile
        if (specialAttackTimer <= 0f && specialProjectilePrefab != null)
        {
            Debug.Log("--------------------Special attack available!--------------------");
            projectilePrefab = specialProjectilePrefab;

            // Instantiate and launch the special projectile
            GameObject specialProjectileInstance = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            SpecialWaterProjectile specialProjectile = specialProjectileInstance.GetComponent<SpecialWaterProjectile>();
            
            if (specialProjectile != null)
            {
                specialProjectile.SetTarget(currentTarget.transform); // Assign the target
                Debug.Log($"Special projectile instantiated at position {firePoint.position} and assigned to target at position {currentTarget.transform.position}.");
            }
            else
            {
                Debug.LogError("Special projectile does not have a SpecialWaterProjectile script attached!");
            }

            specialAttackTimer = specialAttackCooldown; // Reset cooldown
        }
        else
        {
            // Decide which projectile type to shoot
            string projectileType;
            if (wizardType == "Fire")
            {
                projectileType = "Fireball";
                PlaySound(fireballSound);
            }
            else if (wizardType == "Water")
            {
                projectileType = "Waterball";
                if (soundCooldownTimer <= 0f) // Only play sound if cooldown is over
                {
                    PlaySound(waterballSound);
                    soundCooldownTimer = soundCooldown; // Reset cooldown timer
                }
            }
            else if (wizardType == "Earth")
            {
                projectileType = "Earthball";

                if (soundCooldownTimer <= 0f) // Only play sound if cooldown is over
                {
                    PlaySound(earthballSound);
                    soundCooldownTimer = soundCooldown; // Reset cooldown timer
                }
            }
            else
            {
                projectileType = "Fireball"; // Fallback, just in case
            }

            Projectile projectile = Projectile.GetFromPool(projectileType);
            if (projectile != null)
            {
                projectile.transform.position = firePoint.position;
                projectile.transform.rotation = firePoint.rotation;
                projectile.SetTarget(currentTarget.transform);
            }
            else
            {
                Debug.LogError($"Failed to retrieve {projectileType} from pool.");
            }
        }
    }



    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.clip = clip;
            audioSource.pitch = Random.Range(0.8f, 1.1f); // Optional: Randomize pitch
            audioSource.Play();
        }
    }
}




