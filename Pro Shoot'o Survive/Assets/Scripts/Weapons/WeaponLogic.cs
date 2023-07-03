using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class WeaponLogic : MonoBehaviour
{
    public int bulletsPerMagazine;
    public int totalMagazines;
    public float reloadTime = 2f;

    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    protected int bulletsRemainingInMagazine;
    protected bool isReloading;

    public float damage;

    [SerializeField] List<AudioClip> shootClips;

    [SerializeField] TMP_Text bulletsPerMagazine_text;
    [SerializeField] TMP_Text bulletsRemainingInMagazine_text;

    [SerializeField] GameObject reloadUI;



    void Start()
    {
        bulletsRemainingInMagazine = bulletsPerMagazine;
        isReloading = false;
        if (reloadUI != null)
        {
            reloadUI.GetComponent<ReloadUI>().reloadTime = reloadTime;
        }
        if (bulletsPerMagazine_text != null && bulletsRemainingInMagazine_text != null)
        {
            bulletsPerMagazine_text.text = bulletsPerMagazine.ToString();
            bulletsRemainingInMagazine_text.text = bulletsRemainingInMagazine.ToString();
        }

        gameObject.tag = "Weapon";
    }

    public void Fire(Vector3 dir, bool isHoming = false, Transform target = null)
    {
        if (isReloading)
        {
            return;
        }

        GameObject bullet = WeaponManager.Instance.GetPlayerBullet();
        bullet.transform.position = bulletSpawnPoint.position;

        PlayerBullet bulletScript = bullet.GetComponent<PlayerBullet>();
        if (bulletScript != null)
        {
            if (!isHoming)
            {
                bulletScript.DamageDealt = damage;
                bulletScript.dir = dir;
            }
            else
            {
                bulletScript.IsHoming = isHoming;
                bulletScript.target = target;
                bulletScript.DamageDealt = damage;
                bullet.transform.rotation = bulletSpawnPoint.rotation;
            }
        }

        PlayShootClip();

        bulletsRemainingInMagazine--;
        bulletsRemainingInMagazine_text.text = bulletsRemainingInMagazine.ToString();
        if (bulletsRemainingInMagazine == 0)
        {
            Reload();
            reloadUI.SetActive(true);
        }

    }

    void Reload()
    {
        if (totalMagazines > 0)
        {
            isReloading = true;

            Invoke("FinishReloading", reloadTime);
        }
        else
        {
            Debug.Log("Weapon is empty!");
        }
    }

    void FinishReloading()
    {
        bulletsRemainingInMagazine = bulletsPerMagazine;
        totalMagazines--;
        bulletsRemainingInMagazine_text.text = bulletsRemainingInMagazine.ToString();


        isReloading = false;
    }

    private void PlayShootClip()
    {
        int randIndex = Random.Range(0, shootClips.Count);
        float randVolume = Random.Range(0.8f, 1.0f);
        AudioSource.PlayClipAtPoint(shootClips[randIndex], bulletSpawnPoint.position, randVolume);
    }
}