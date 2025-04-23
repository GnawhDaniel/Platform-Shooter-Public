using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : Projectile
{
    [SerializeField] private float maxDistance = 10f;
    private Vector3 startPos;
    private GameObject originShooter = null;


    private void Start()
    {
        startPos = transform.position;
        damage = 10f;
        //originShooter = null;
    }

    public void SetOriginShooter(GameObject shooter)
    {
        originShooter = shooter;
    }

    public GameObject GetOriginShooter()
    {
        return originShooter;
    }

    private void Update()
    {
        // If the bullet has traveled the max distance, destroy the bullet
        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If bullet hits a box collider, destroy the bullet
        if (collision.gameObject.GetComponent<BoxCollider>())
        {
            Destroy(gameObject);

        }
        else if (collision.gameObject.GetComponent<Bullet>())
        {
            Destroy(gameObject);
        }
    }
}
