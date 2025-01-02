using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// COMMENTS COMPLETED
public class WizardAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public float rotationSpeed = 5f; // Speed of rotation toward the enemy

    [Header("Wizard Type (e.g., 'Fire', 'Water', 'Earth')")]
    public string wizardType;

    [Header("Base Wizard")]
    public GameObject BasePrefab;
    public int BasePoolSize = 25;

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
    public GameObject specialProjectilePrefab; 
    public float specialAttackCooldown = 10f; 
    private float specialAttackTimer = 0f; 


    [Header("Audio Settings")]
    public AudioClip baseballSound;
    public AudioClip fireballSound; 
    public AudioClip waterballSound;
    public AudioClip earthballSound;
    private AudioSource audioSource;

    private float attackTimer = 0f;
    private float soundCooldown = 1f; //  used only for the water wizard
    private float soundCooldownTimer = 0f;
    private EnemyScript currentTarget;

    void Start()
    {
        // Initialize the audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Initialize the pool for the wizard's actual projectile type
        if (wizardType == "Base")
        {
            Projectile.InitializePool("BaseBall", BasePoolSize, BasePrefab); 
        }
        else if (wizardType == "Fire")
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

        // reduce the special attack timer
        if (specialAttackTimer > 0)
        {
            specialAttackTimer -= Time.deltaTime;
        }

        // If there is a target, check if it's still in range, and rotate if needed
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
            direction.y = 0; 
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

            if (firePoint != null)
            {
                ShootProjectile();
            }

            if (currentTarget.health <= 0)
            {
                currentTarget = null;
            }
        }
    }


    void ShootProjectile()
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
            if (soundCooldownTimer <= 0f)
            {
                PlaySound(waterballSound);
                soundCooldownTimer = soundCooldown;
            }
        }
        else if (wizardType == "Earth")
        {
            projectileType = "Earthball";

            if (soundCooldownTimer <= 0f)
            {
                PlaySound(earthballSound);
                soundCooldownTimer = soundCooldown;
            }
        }
        else if (wizardType == "Base")
        {
            projectileType = "BaseBall";
            PlaySound(baseballSound);
        }
        else
        {
            projectileType = "Fireball"; // backup projectile type
        }

        // retrieve a projectile from the pool and fire it
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

    public void ActivateSpecialPowerWater()
    {
        if (specialAttackTimer <= 0f && specialProjectilePrefab != null)
        {
            Debug.Log("Special power activated!");

            GameObject specialProjectileInstance = Instantiate(specialProjectilePrefab, firePoint.position, firePoint.rotation);
            SpecialWaterProjectile specialProjectile = specialProjectileInstance.GetComponent<SpecialWaterProjectile>();

            if (specialProjectile != null && currentTarget != null)
            {
                specialProjectile.SetTarget(currentTarget.transform);
                Debug.Log($"Special projectile fired at target: {currentTarget.name}");
            }
            else if (specialProjectile == null)
            {
                Debug.LogError("Failed to attach SpecialWaterProjectile script to the instantiated special projectile.");
            }

            specialAttackTimer = specialAttackCooldown;
        }
        else
        {
            Debug.Log("Special power is on cooldown or no special projectile prefab assigned.");
        }
    }
    public void ActivateSpecialPowerFire()
    {
        if (specialAttackTimer <= 0f && specialProjectilePrefab != null)
        {
            Debug.Log("Special power activated!");

            // Instantiate the special fireball
            GameObject specialFireballInstance = Instantiate(specialProjectilePrefab, firePoint.position, firePoint.rotation);
            SpecialFireballProjectile specialFireball = specialFireballInstance.GetComponent<SpecialFireballProjectile>();

            if (specialFireball != null && currentTarget != null)
            {
                specialFireball.SetTarget(currentTarget.transform); // Assign the target
                Debug.Log($"Special fireball fired at target: {currentTarget.name}");
            }
            else if (specialFireball == null)
            {
                Debug.LogError("Failed to attach SpecialFireballProjectile script to the instantiated special fireball.");
            }

            // Reset the cooldown for the special attack
            specialAttackTimer = specialAttackCooldown;
        }
        else
        {
            Debug.Log("Special power is on cooldown or no special projectile prefab assigned.");
        }
    }

    // Special power for Earth wizards
    public void ActivateSpecialPowerEarth()
    {
        if (specialAttackTimer <= 0f && specialProjectilePrefab != null)
        {
            Debug.Log("Special Earth Knockback power activated!");

            // Instantiate the knockback projectile
            Vector3 firingPosition = transform.position + new Vector3(0, 1.5f, 0);

            GameObject specialEarthballInstance = Instantiate(specialProjectilePrefab, firePoint.position, firePoint.rotation);
            KnockbackProjectile knockbackProjectile = specialEarthballInstance.GetComponent<KnockbackProjectile>();
            

            if (knockbackProjectile != null && currentTarget != null)
            {
                knockbackProjectile.SetTarget(currentTarget.transform, firingPosition); // Assign the target
                Debug.Log($"Knockback projectile fired at target: {currentTarget.name}");
            }
            else if (knockbackProjectile == null)
            {
                Debug.LogError("Failed to attach KnockbackProjectile script to the instantiated special projectile.");
            }

            // Reset the cooldown for the special attack
            specialAttackTimer = specialAttackCooldown;
        }
        else
        {
            Debug.Log("Special power is on cooldown or no special projectile prefab assigned.");
        }
    }



    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {

            audioSource.clip = clip;
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            audioSource.Play();
        }
    }
}




