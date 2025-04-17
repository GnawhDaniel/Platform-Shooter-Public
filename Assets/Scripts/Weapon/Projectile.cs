using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float damage;
    public float GetDamage()
    {
        return damage;
    }
}
