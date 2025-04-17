using UnityEngine;

public class Icicle : Projectile
{
    [SerializeField] private float speed = 5f; // Speed of the icicle

    private Transform player; // Reference to the player transform
    private float createdTime;

    private bool isShooting = false; // Flag to check if the icicle is shooting

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            transform.GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
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
        Destroy(gameObject);
    }

}
