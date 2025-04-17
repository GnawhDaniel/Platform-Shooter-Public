using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Serialized Fields (Visible in Inspector)
    [Header("Player Settings")]
    [SerializeField] private float sprintSpeed = 7; // Speed when sprinting
    [SerializeField] private float walkSpeed = 4;  // Speed when walking
    [SerializeField] private Transform targetTransform; // Target for aiming
    [SerializeField] private Slider healthDisplay; // UI element for health display

    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckTransform; // Transform to check if player is grounded
    [SerializeField] private LayerMask playerMask; // Layer mask for player collision
    [SerializeField] private LayerMask mouseAimMask; // Layer mask for mouse aiming

    // Internal Variables (Not visible in Inspector)
    private float health; // Player's health
    [SerializeField] private float maxHealth; // Maximum health
    private bool jumpKeyWasPressed = false; // Tracks if jump key was pressed
    private bool sprintKeyHold = false; // Tracks if sprint key is held
    private float horizontalInput; // Horizontal movement input
    private int jumpCounter = 2; // Tracks remaining jumps

    // Cached Components
    private Camera mainCamera; // Reference to the main camera
    private Rigidbody rigidbodyComponent; // Reference to the Rigidbody component
    private Transform hand; // Reference to the player's hand transform

    void Start()
    {
        rigidbodyComponent = GetComponent<Rigidbody>();
        hand = transform.Find("Hand");
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle aiming
        mainCamera = Camera.main;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            targetTransform.position = hit.point;
            hand.LookAt(hit.point);
        }

        // Handle jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyWasPressed = true;
            --jumpCounter;
        }

        // Handle sprint input
        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprintKeyHold = true;
        }

        // Capture horizontal movement input
        horizontalInput = Input.GetAxis("Horizontal");

        // Update health display
        healthDisplay.value = health / maxHealth;
    }

    // FixedUpdate is called once per physics update
    private void FixedUpdate()
    {
        // Handle horizontal movement (sprint or walk)
        if (sprintKeyHold)
        {
            rigidbodyComponent.linearVelocity = new Vector3(horizontalInput * sprintSpeed, rigidbodyComponent.linearVelocity.y, 0);
            sprintKeyHold = false;
        }
        else
        {
            rigidbodyComponent.linearVelocity = new Vector3(horizontalInput * walkSpeed, rigidbodyComponent.linearVelocity.y, 0);
        }

        // Handle player facing direction
        float direction = Mathf.Sign(targetTransform.position.x - transform.position.x);
        float yRotation = direction >= 1 ? 180 : 0;
        rigidbodyComponent.MoveRotation(Quaternion.Euler(new Vector3(0, yRotation, 0)));

        // Limit jumps if not grounded
        if (jumpCounter <= 0 && Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }

        // Reset jump counter when grounded
        if (Physics.OverlapSphere(groundCheckTransform.position, 0.1f, playerMask).Length >= 1)
        {
            jumpCounter = 2;
        }

        // Apply jump force
        if (jumpKeyWasPressed)
        {
            rigidbodyComponent.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
            jumpKeyWasPressed = false;
        }
    }

    // Method to handle damage taken by the player
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDeath();
        }
    }

    // Method to adjust player's health
    public void AdjustHP(float hp)
    {
        health += hp;
        if (health > 100)
        {
            health = 100;
        }
    }

    // Method called when the player dies
    private void OnDeath()
    {
        healthDisplay.value = 0;
        Destroy(gameObject);
        Destroy(healthDisplay.gameObject);
    }

    // Handle collision with projectiles
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            if (projectile != null)
            {
                TakeDamage(projectile.GetDamage());
            }
            Destroy(collision.gameObject);
        }
    }
}
