using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class WeaponLogic : MonoBehaviour
{
    [SerializeField] private bool hasInfiniteAmmo;
    [SerializeField] private bool isWeaponHanded;
    public bool IsWeaponHanded { get { return isWeaponHanded; } }

    [SerializeField] private int wpnClip;
    [SerializeField] private int wpnMagazine;
    private int currentWpnClip;
    private int currentWpnMagazine;

    public float reloadTime = 2f;
    public float currentReloadTime;

    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    private bool isReloading;

    public float damage;

    [SerializeField] private List<AudioClip> shootClips;

    [SerializeField] private TMP_Text wpnClipTextObject;
    [SerializeField] private TMP_Text wpnMagazineTextObject;

    [SerializeField] public GameObject reloadUI;

    public PlayerController playerController;

    void Start()
    {
        gameObject.tag = "Weapon";

        if (!isWeaponHanded)
        {
            return;
        }

        InitUI_WeaponAmmo();

        currentReloadTime = reloadTime;
        isReloading = false;
        reloadUI.GetComponent<ReloadUI>().reloadTime = reloadTime;
    }

    public void InitUI_WeaponAmmo()
    {
        currentWpnClip = wpnClip;
        currentWpnMagazine = wpnMagazine;

        wpnClipTextObject.text = wpnClip.ToString();
        wpnMagazineTextObject.text = hasInfiniteAmmo ? "∞" : currentWpnMagazine.ToString();
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
            bulletScript.DamageDealt = damage;

            if (!isHoming)
            {
                bulletScript.dir = dir;
            }
            else
            {
                bulletScript.IsHoming = isHoming;
                bulletScript.target = target;
                bullet.transform.rotation = bulletSpawnPoint.rotation;
            }
        }

        PlayShootClip();

        currentWpnClip--;
        wpnClipTextObject.text = currentWpnClip.ToString();

        if (currentWpnClip <= 0)
        {
            if (currentWpnMagazine > 0 || hasInfiniteAmmo)
            {
                Reload();
            }

            else
            {
                playerController.switchCurrentWeapon();

                //Debug.Log("Weapon is empty!");
            }
        }
    }

    void Reload()
    {
        isReloading = true;
        reloadUI.SetActive(true);

        Invoke("FinishReloading", reloadTime);
    }

    void FinishReloading()
    {
        if (!hasInfiniteAmmo)
        {
            if (currentWpnMagazine < wpnClip)
            {
                currentWpnClip = currentWpnMagazine;
                currentWpnMagazine = 0;
            }

            else
            {
                currentWpnMagazine -= (wpnClip - currentWpnClip);
                currentWpnClip = wpnClip;
            }

            wpnMagazineTextObject.text = currentWpnMagazine.ToString();
        }

        else
        {
            currentWpnClip = wpnClip;
        }

        wpnClipTextObject.text = currentWpnClip.ToString();

        isReloading = false;
    }

    private void PlayShootClip()
    {
        int randIndex = Random.Range(0, shootClips.Count);
        float randVolume = Random.Range(0.8f, 1.0f);
        AudioSource.PlayClipAtPoint(shootClips[randIndex], bulletSpawnPoint.position, randVolume);
    }
}