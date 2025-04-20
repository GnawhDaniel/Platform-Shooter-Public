using UnityEngine;

public class Icicle : Projectile
{
    [SerializeField] private float speed = 3.5f; // Speed of the icicle
    [SerializeField] private float maxHealth = 10f; // Maximum health of the icicle
    private float health; // Health of the icicle
    private Transform player; // Reference to the player transform
    private float createdTime;

    private bool isShooting = false; // Flag to check if the icicle is shooting

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth; // Initialize health
        GameObject _player = GameObject.Find("Player");
        damage = 25f;

        if (_player != null)
        {
            player = GameObject.Find("Player").transform;
        }
        createdTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If the icicle is older than 5 seconds, shoot in forward direction
        if (!isShooting && (Time.time - createdTime > 2))
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();

            //rigidbody.isKinematic = false; // Set the Rigidbody to be non-kinematic
            rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
            isShooting = true;
        }
        // If the icicle is older than 10 seconds, destroy it
        if (isShooting && (Time.time - createdTime > 10f))
        {
            Destroy(gameObject);
        }

        if (!isShooting)
        {
            LookAt();
        };
    }

    private void LookAt()
    {
        // If player is not found (dead)
        if (!player)
        {
            return;
        }
        Vector3 target = player.position;
        if (target != null)
        {
            transform.LookAt(target);
        }
    }

    private void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Breakable"))
        {
            BreakableEnv breakable = collision.gameObject.GetComponent<BreakableEnv>();
            if (breakable != null)
            {
                breakable.TakeDamage(damage);
            }

        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("Projectile"))
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            TakeDamage(projectile.GetDamage());
            return;
        }
        Destroy(gameObject);

    }

}
