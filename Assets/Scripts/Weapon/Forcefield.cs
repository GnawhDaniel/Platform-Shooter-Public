using UnityEngine;
using UnityEngine.UI;

public class Forcefield : MonoBehaviour
{
    private float energy = 100f;
    private float maxEnergy = 100f;
    private float energyRegen = 5f; // Per second

    private float cooldownTime = 0f; // Time to wait before being able to use the forcefield again
    private float timeLastUsed;

    private bool availableToUse = true;
    private bool isActive = false;

    private Renderer forcefieldRenderer;
    private Collider forcefieldCollider;

    [SerializeField] private Slider energyUI;
    private Image sliderImage;


    private void Awake()
    {
        timeLastUsed = -cooldownTime;
        forcefieldRenderer = this.GetComponent<Renderer>();
        forcefieldCollider = this.GetComponent<Collider>();
        sliderImage = energyUI.fillRect.GetComponent<Image>();
        // Initialize the forcefield to be disabled
        DisableForcefield();
    }

    private void FixedUpdate()
    {
        // Regenerate energy over time
        if (energy < maxEnergy && !isActive)
        {
            energy += energyRegen * Time.deltaTime;
            if (energy > maxEnergy)
            {
                energy = maxEnergy;
            }
        }
        else if (isActive)
        {
            // Deplete energy when active
            energy -= energyRegen * Time.deltaTime;
            if (energy <= 0)
            {
                energy = 0;
                availableToUse = false;
                DisableForcefield();
            }
        }

            UpdateEnergyBar();
    }

    private void UpdateEnergyBar()
    {
        if (energyUI != null)
        {
            energyUI.value = energy / maxEnergy;
            sliderImage.color = IsAvailable() ? new Color(25f / 255f, 31f / 255f, 214f / 255f) : new Color(105f / 255f, 109f / 255f, 255f / 255f);
        }

       
    }

    public void DeductEnergy(float amount)
    {
        energy -= amount;
        if (energy < 0)
        {
            energy = 0;
            availableToUse = false;
        }
    }

    public void DisableForcefield()
    {
        // Disable visual parts
        forcefieldRenderer.enabled = false;

        // Disable collider
        forcefieldCollider.enabled = false;

        isActive = false;
    }

    public void EnableForcefield()
    {
        // Enable visual parts
        forcefieldRenderer.enabled = true;
        // Enable collider
        forcefieldCollider.enabled = true;

        isActive = true;
    }

    public bool IsAvailable()
    {
        // Cooldown
        if (Time.time < timeLastUsed + cooldownTime)
        {
            return false;
        }

        if (availableToUse)
        {
            return true;
        }
        else
        {
            // Check if health regenerated to 100
            if (energy == maxEnergy)
            {
                availableToUse = true;
            }
            return availableToUse;
        }
    }
    
    public void SetLastUsed(float time)
    {
        timeLastUsed = time;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {

            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                // Ignore Collision from within forcefield
                Player player = bullet.GetOriginShooter().GetComponent<Player>();
                if (player != null)
                {
                    return;
                }

            }
      
            // Reduce energy when hit by projectiles
            energy -= 10f;
            if (energy <= 0)
            {
                energy = 0;
                availableToUse = false;
            }

            Destroy(collision.gameObject);
        }
    }
}
