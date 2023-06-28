using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    public int bulletsPerMagazine;  // Total bullets per magazine
    public int totalMagazines;      // Total number of magazines
    public float fireRate;          // Fire rate in rounds per second
    public Transform bulletSpawnPoint;  // Point where bullets are instantiated
    public GameObject bulletPrefab;  // Prefab of the bullet object

    protected int bulletsRemainingInMagazine;  // Bullets remaining in the current magazine
    protected bool isReloading;  // Flag to indicate if the weapon is currently reloading
    protected float fireTimer;   // Timer to control the fire rate

    public float Damage;  // Damage caused by the bullet

    protected virtual void Start()
    {
        bulletsRemainingInMagazine = bulletsPerMagazine;
        isReloading = false;

        // Assign the "Weapon" tag to the game object
        gameObject.tag = "Weapon";
    }

    protected virtual void Update()
    {
        // Decrement the fire timer
        if (fireTimer > 0f)
        {
            fireTimer -= Time.deltaTime;
        }
    }

    public virtual void Fire()
    {
        // If the weapon is reloading, don't allow shooting
        if (isReloading)
        {
            return;
        }

        // Check if there are bullets remaining in the magazine
        if (bulletsRemainingInMagazine > 0)
        {
            // Instantiate a bullet at the bullet spawn point
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            // Get the PlayerBullet script component from the bullet object
            PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();
            if (bulletScript != null)
            {
                // Set the bullet damage in the PlayerBullet script
                bulletScript.DamageDealt = Damage;
            }

            // Decrement the bullets remaining in the magazine
            bulletsRemainingInMagazine--;

            // Reset the fire timer
            fireTimer = 1f / fireRate;
        }
        else
        {
            // Out of bullets, need to reload
            Reload();
        }
    }

    protected virtual void Reload()
    {
        // Check if there are additional magazines available
        if (totalMagazines > 0)
        {
            isReloading = true;

            // Perform the reloading animation or any other actions

            // Delay the reloading process
            float reloadTime = 2f;  // You can adjust the reload time based on your requirements
            Invoke("FinishReloading", reloadTime);
        }
        else
        {
            // No more magazines, weapon is empty
            Debug.Log("Weapon is empty!");
        }
    }

    protected virtual void FinishReloading()
    {
        // Refill the magazine with bullets
        bulletsRemainingInMagazine = bulletsPerMagazine;

        // Decrement the total number of magazines
        totalMagazines--;

        isReloading = false;
    }
}

