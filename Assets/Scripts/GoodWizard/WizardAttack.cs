using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the attack behavior of the wizard
public class WizardAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 5f;
    public float attackCooldown = 2f;
    public float rotationSpeed = 5f; 

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

    [Header("Wizard Upgrade Settings")]
    [Tooltip("Damage multiplier for all projectiles. 1.0 = normal damage, 1.2 = 20% bonus, etc.")]
    public float damageMultiplier = 1.0f;


    [Header("Audio Settings")]
    public AudioClip baseballSound;
    public AudioClip fireballSound; 
    public AudioClip waterballSound;
    public AudioClip earthballSound;
    private AudioSource audioSource;

    private float attackTimer = 0f;
    private float soundCooldown = 1f;
    private float soundCooldownTimer = 0f;
    private EnemyScript currentTarget;

    // set up projectile pools and audio sources
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

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

    // contantly check search for a terget, rotate toeat the target, and fire at the target
    void Update()
    {
        if (soundCooldownTimer > 0)
        {
            soundCooldownTimer -= Time.deltaTime;
        }

        if (specialAttackTimer > 0)
        {
            specialAttackTimer -= Time.deltaTime;
        }

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
    // rotates the wizard to face the target
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
    // finds the closest target in range
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

    // fires at the target using the projectile pool
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

    // fires at the target using the projectile pool assignet to the wizard
    void ShootProjectile()
    {
    
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
            projectileType = "Fireball";
        }

        Projectile projectile = Projectile.GetFromPool(projectileType);
        if (projectile != null)
        {
            projectile.transform.position = firePoint.position;
            projectile.transform.rotation = firePoint.rotation;
            projectile.SetDamageMultiplier(damageMultiplier);
            projectile.SetTarget(currentTarget.transform);
        }
        else
        {
            Debug.LogError($"Failed to retrieve {projectileType} from pool.");
        }
        
    }

    // special powers for the water wizard is activated
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

    // special powers for the fire wizard is activated
    public void ActivateSpecialPowerFire()
    {
        if (specialAttackTimer <= 0f && specialProjectilePrefab != null)
        {
            Debug.Log("Special power activated!");

            GameObject specialFireballInstance = Instantiate(specialProjectilePrefab, firePoint.position, firePoint.rotation);
            SpecialFireballProjectile specialFireball = specialFireballInstance.GetComponent<SpecialFireballProjectile>();

            if (specialFireball != null && currentTarget != null)
            {
                specialFireball.SetTarget(currentTarget.transform);
                Debug.Log($"Special fireball fired at target: {currentTarget.name}");
            }
            else if (specialFireball == null)
            {
                Debug.LogError("Failed to attach SpecialFireballProjectile script to the instantiated special fireball.");
            }

            specialAttackTimer = specialAttackCooldown;
        }
        else
        {
            Debug.Log("Special power is on cooldown or no special projectile prefab assigned.");
        }
    }

    // special powers for the earth wizard is activated
    public void ActivateSpecialPowerEarth()
    {
        if (specialAttackTimer <= 0f && specialProjectilePrefab != null)
        {
            Debug.Log("Special Earth Knockback power activated!");

            Vector3 firingPosition = transform.position + new Vector3(0, 1.5f, 0);

            GameObject specialEarthballInstance = Instantiate(specialProjectilePrefab, firePoint.position, firePoint.rotation);
            KnockbackProjectile knockbackProjectile = specialEarthballInstance.GetComponent<KnockbackProjectile>();
            

            if (knockbackProjectile != null && currentTarget != null)
            {
                knockbackProjectile.SetTarget(currentTarget.transform, firingPosition);
                Debug.Log($"Knockback projectile fired at target: {currentTarget.name}");
            }
            else if (knockbackProjectile == null)
            {
                Debug.LogError("Failed to attach KnockbackProjectile script to the instantiated special projectile.");
            }

            specialAttackTimer = specialAttackCooldown;
        }
        else
        {
            Debug.Log("Special power is on cooldown or no special projectile prefab assigned.");
        }
    }


    // Method to play a sound when a projectile is fired
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




