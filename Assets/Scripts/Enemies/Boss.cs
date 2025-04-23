using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    // Bullet
    [SerializeField] private GameObject bullet;
    [SerializeField] private float shootForce, upwardForce;

    // Stats
    [SerializeField] private float timeBetweenShooting, spread;
    private bool readyToShoot;

    [SerializeField] private Slider healthUI;
    private float health;
    private float maxHealth = 2000f;

    // References
    [SerializeField] private Transform[] weapons;
    [SerializeField] private Enemy underling;
    [SerializeField] private Icicle iciclePrefab;

    // Stages
    private enum Stages
    {
        STAGE1, 
        STAGE2, 
        STAGE3 
    }
    private enum Attacks
    {
        BulletWheelAttack,
        IcicleAttack,
    }

    private Stages currentStage = Stages.STAGE1;

    // Attack
    private Attacks[] attacks = {Attacks.BulletWheelAttack, Attacks.IcicleAttack};
    private Attacks currentAttack;
    private float attackTimer;
    private float attackDuration = 25f;

    private bool spawnedIce = false;

    void Start()
    {
        readyToShoot = true;
        attackTimer = Time.time + attackDuration;
        currentAttack = Attacks.IcicleAttack;
        health = maxHealth;

    }

    void Update()
    {
        if (Time.time > attackTimer)
        {
            // Randomly choose an attackddddddddddddddd
            currentAttack = attacks[Random.Range(0, attacks.Length)];
            //currentAttack = Attacks.BulletWheelAttack;
            attackTimer = Time.time + attackDuration;
        }


        UpdateHealthBar();
    }
    private void FixedUpdate()
    {
        // Randomly spawn underlings
        if (UnityEngine.Random.Range(0, 1000) < 2)
        {
            SpawnUnderling();
        }

        if (currentStage == Stages.STAGE1)
        {
            switch (currentAttack)
            {
                case Attacks.BulletWheelAttack:
                    BulletWheelAttack();
                    break;
                case Attacks.IcicleAttack:
                    IcicleAttack();
                    break;
            }
        }
        else if (currentStage == Stages.STAGE2)
        {

        }
        else if (currentStage == Stages.STAGE3)
        {

        }
    }

    private void UpdateHealthBar()
    {
        // Update the health bar UI
        if (healthUI != null)
        {
            healthUI.value = health / maxHealth;
        }
    }

    private void SpawnUnderling()
    {
        // Choose random position in game world
        Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(1, 5), 0);

        // Spawn
        Enemy currentUnderling = Instantiate(underling, spawnPosition, Quaternion.identity);
        currentUnderling.SetTargetPlayer();
    }

    private void BulletWheelAttack()
    {
        if (readyToShoot)
        {
            readyToShoot = false;
            foreach (Transform weapon in weapons)
            {

                // Create a bullet
                GameObject currentBullet = Instantiate(bullet, weapon.position, Quaternion.identity);
                currentBullet.GetComponent<Rigidbody>().AddForce(weapon.forward * shootForce, ForceMode.Impulse);
                currentBullet.GetComponent<Bullet>().SetOriginShooter(gameObject.transform.root.gameObject);


            }
            Invoke("ResetShot", timeBetweenShooting);
        }

    }

    private void IcicleAttack()
    {
        if (spawnedIce) return;
        
        for (int i = 0; i < 3; i++)
        {
            Vector3 newPosition = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-4.0f, 2f), 0);
            GameObject icicleObject = Instantiate(iciclePrefab.gameObject, transform.position + newPosition, Quaternion.identity);
        }

        spawnedIce = true;

        Invoke("ResetIceAttack", 3);
    }

    private void ResetIceAttack()
    {
        spawnedIce = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the bullet hits an object with a rigidbody, destroy the bullet
        if (collision.gameObject.tag == "Projectile")
        {

            // If gameobject is Bullet
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            if (bullet != null)
            {
                // Add damage to boss
                health -= projectile.GetDamage();
                if (health <= 0)
                {
                    Destroy(healthUI.gameObject);
                    Destroy(gameObject);
                }
            }

            Destroy(collision.gameObject);

        }
    }
}