using UnityEngine;

public class DefaultGun : WeaponLogic
{
    protected override void Start()
    {
        bulletsPerMagazine = 12;
        totalMagazines = int.MaxValue;  // Unlimited magazines
        fireRate = 5f;  // Adjust as needed
        bulletSpawnPoint = transform;  // Use the gun object's transform as the spawn point
        bulletPrefab = Resources.Load<GameObject>("Bullet");  // Load the bullet prefab (make sure it exists in your project)
        Damage = 10;  // Set the damage for the default gun

        // Call the base Start() function
        base.Start();
    }
}