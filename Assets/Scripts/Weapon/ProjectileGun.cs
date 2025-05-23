using UnityEngine;
using TMPro;
using System;
using UnityEngine.UIElements;

public class ProjectileGun : MonoBehaviour
{
    // Bullet
    [Header("Bullet")]
    [SerializeField] protected GameObject bullet;

    [Header("Bullet Force")]
    // Bullet Force
    [SerializeField] protected float shootForce, upwardForce;

    // Stats
    [Header("Gun Stats")]
    [SerializeField] protected float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    [SerializeField] protected int magazineSize, bulletsPerTap;
    [SerializeField] protected bool allowButtonHold;

    // State
    [Header("State")]
    [SerializeField] protected int bulletsLeft, bulletsShot;

    // Bools
    protected bool shooting, readyToShoot, reloading;

    // Reference
    [Header("References")]
    [SerializeField] protected Transform sourcePoint;
    [SerializeField] protected Transform attackPoint;

    // Graphics
    [Header("Graphics")]
    [SerializeField] protected GameObject muzzleFlash;
    [SerializeField] protected TextMeshProUGUI ammunitionDisplay;

    // Bugfix 
    [Header("Bugfix")]
    [SerializeField] protected bool allowInvoke = true;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        spread = 0;
    }

    private void Update()
    {
        MyInput();

        if (ammunitionDisplay != null)
        {
            if (reloading)
            {
                ammunitionDisplay.SetText("Reloading");

            }
            else
            {
                ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
            }
        }
    }

    private void MyInput() 
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    public void Shoot()
    {
        readyToShoot = false;

        Vector3 source = new Vector3(sourcePoint.position.x, sourcePoint.position.y, 0);
        Vector3 target = new Vector3(attackPoint.position.x, attackPoint.position.y, 0);

        Vector3 direction = (target - source).normalized;

        // Calculate Spread
        float x = UnityEngine.Random.Range(-spread, spread);
        float y = UnityEngine.Random.Range(-spread, spread);

        // Calculate new direction with spread
        Vector3 directionWithSpread = direction + new Vector3(x, y, 0);

        // Instantiate bullet/projectile
        Vector3 position = new Vector3(sourcePoint.position.x, sourcePoint.position.y, 0);
        GameObject currentBullet = Instantiate(bullet, position, Quaternion.identity);
        // Set bullet origin
        currentBullet.GetComponent<Bullet>().SetOriginShooter(gameObject.transform.root.gameObject);

        // Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread * shootForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShot++;

        // Invoke resetShot() to allow shooting again
        if (allowInvoke) Invoke("ResetShot", timeBetweenShooting);

        // If more than one bulletsPerTap make sure to repeat shoot() method
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    public void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    public void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    public void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

}
