using UnityEngine;

public class Enemy : ProjectileGun
{

    [Header("Enemy Stats")]
    [SerializeField] private HealthPack healthPackPrefab;
    [SerializeField] private double maxHealth = 50f;
    
    private double health;
    private Transform hand;
    private LayerMask ignoreLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform player = GameObject.Find("Player").transform;
        health = maxHealth;

        hand = transform.Find("Hand");
        readyToShoot = true;
        bulletsLeft = magazineSize;
        reloading = false;

        int layerToIgnore = LayerMask.NameToLayer("Forcefield");
        ignoreLayer = ~(1 << layerToIgnore);
    }

    void FixedUpdate()
    {
        if (attackPoint == null)
        {
            return;
        }

        LookAt();
        RaycastHit hit;
        Vector3 direction = (attackPoint.position - transform.position).normalized;
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, ignoreLayer))
        {
            if (hit.transform.name == "Player")
            {
                shooting = true;
            }
            else
            {
                shooting = false;
            }
        }
        EnemyInput();
    }

    private void EnemyInput()
    {
        // Reloading
        if (readyToShoot && !reloading && bulletsLeft <= 0) Reload();

        // Shooting 
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void LookAt()
    {

        if (attackPoint != null)
        {
            Vector3 target = attackPoint.position;
            if (hand != null && target != null)
            {
                hand.LookAt(target);
            }
        }
    }

    public void TakeDamage(float damage)
    {

        health -= damage;
        if (health <= 0)
        {
            // Spawn Health pack
            HealthPack currentHealthPack = Instantiate(healthPackPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    public void SetTargetPlayer()
    {
        Transform player = GameObject.Find("Player").transform;
        attackPoint = player;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Add damage to player
            Projectile projectile = collision.gameObject.GetComponent<Bullet>();
            if (projectile != null)
            {
                // Add damage to enemy
                TakeDamage(projectile.GetDamage());
            }
            Destroy(collision.gameObject);
        }
    }
}
