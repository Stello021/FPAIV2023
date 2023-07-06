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
    [SerializeField] private int wpnMaxAmmo;
    private int currentWpnClip;
    private int currentWpnAmmo;

    public float reloadTime = 2f;
    public float currentReloadTime;

    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    public bool IsReloading { get; private set; }

    public float damage;

    [SerializeField] private List<AudioClip> shootClips;

    [SerializeField] private TMP_Text wpnClipTextObject;
    [SerializeField] private TMP_Text wpnMagazineTextObject;

    [SerializeField] private GameObject reloadUI;
    public GameObject ReloadUI { get { return reloadUI; } }

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
        IsReloading = false;
        reloadUI.GetComponent<ReloadUI>().reloadTime = reloadTime;
    }

    public void InitUI_WeaponAmmo()
    {
        currentWpnClip = wpnClip;
        currentWpnAmmo = wpnMaxAmmo;

        wpnClipTextObject.text = wpnClip.ToString();
        wpnMagazineTextObject.text = hasInfiniteAmmo ? "∞" : currentWpnAmmo.ToString();
    }

    public void Fire(Vector3 dir, bool isHoming = false, Transform target = null)
    {
        if (IsReloading)
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
            if (currentWpnAmmo > 0 || hasInfiniteAmmo)
            {
                ReloadAutomatically();
            }

            else
            {
                playerController.SwitchCurrentWeapon();
            }
        }
    }

    public void ReloadManually()
    {
        if (currentWpnClip < wpnClip && (currentWpnAmmo > 0 || hasInfiniteAmmo))
        {
            ReloadAutomatically();
        }
    }

    private void ReloadAutomatically()
    {
        if (!IsReloading)
        {
            Invoke(nameof(FinishReloading), reloadTime);
        }

        IsReloading = true;
        reloadUI.SetActive(true);
    }

    private void FinishReloading()
    {
        if (!hasInfiniteAmmo)
        {
            if (currentWpnAmmo < wpnClip)
            {
                currentWpnClip = currentWpnAmmo;
                currentWpnAmmo = 0;
            }

            else
            {
                currentWpnAmmo -= (wpnClip - currentWpnClip);
                currentWpnClip = wpnClip;
            }

            wpnMagazineTextObject.text = currentWpnAmmo.ToString();
        }

        else
        {
            currentWpnClip = wpnClip;
        }

        wpnClipTextObject.text = currentWpnClip.ToString();

        IsReloading = false;
    }

    private void PlayShootClip()
    {
        int randIndex = Random.Range(0, shootClips.Count);
        float randVolume = Random.Range(0.8f, 1.0f);
        AudioSource.PlayClipAtPoint(shootClips[randIndex], bulletSpawnPoint.position, randVolume);
    }
}