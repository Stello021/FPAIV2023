using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    public int bulletsPerMagazine;
    public int totalMagazines;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    protected int bulletsRemainingInMagazine;
    protected bool isReloading;

    public float damage;

    protected virtual void Start()
    {
        bulletsRemainingInMagazine = bulletsPerMagazine;
        isReloading = false;

        gameObject.tag = "Weapon";
    }

    public virtual void Fire(Vector3 dir)
    {
        if (isReloading)
        {
            return;
        }

        if (bulletsRemainingInMagazine > 0)
        {
            GameObject bullet = WeaponManager.Instance.GetPlayerBullet();
            bullet.transform.position = bulletSpawnPoint.position;

            PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();
            if (bulletScript != null)
            {
                bulletScript.DamageDealt = damage;
                bulletScript.dir = dir;
            }

            bulletsRemainingInMagazine--;
        }
        else
        {
            Reload();
        }
    }

    protected virtual void Reload()
    {
        if (totalMagazines > 0)
        {
            isReloading = true;

            float reloadTime = 2f;
            Invoke("FinishReloading", reloadTime);
        }
        else
        {
            Debug.Log("Weapon is empty!");
        }
    }

    protected virtual void FinishReloading()
    {
        bulletsRemainingInMagazine = bulletsPerMagazine;
        totalMagazines--;

        isReloading = false;
    }
}