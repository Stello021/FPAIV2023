using UnityEngine;

public class AssaultRifle : WeaponLogic
{
    protected override void Start()
    {
        bulletsPerMagazine = 5;
        totalMagazines = 8;
        fireRate = 10f;  // Adjust as needed
        bulletSpawnPoint = transform;  // Use the gun object's transform as the spawn point
        bulletPrefab = Resources.Load<GameObject>("Bullet");  // Load the bullet prefab (make sure it exists in your project)
        Damage = 20;  // Set the damage for the assault rifle

        // Call the base Start() function
        base.Start();
    }
}