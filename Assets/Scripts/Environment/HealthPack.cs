using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private float toHeal = 15.0f;

    private void Start()
    {
        // Set the health pack to be destroyed after 10 seconds
        Destroy(gameObject, 10f);
        transform.rotation = Quaternion.Euler(90, 0, 180);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Heal the player
            Player player = other.gameObject.GetComponent<Player>();
           
            if (player != null)
            {
                player.AdjustHP(toHeal);
            }

            // Destroy the health pack
            Destroy(gameObject);
        }
    }
}
