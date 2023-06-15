using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] BulletLogic bullet;
    [SerializeField] int TotalAmmo;
    
    public int Ammo;

    public GameObject bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Ammo = TotalAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        //instantiateBullet(transform, Vector3.zero);
    }

    //[ContextMenu("Sparo Bullet")]
    public void instantiateBullet(Transform PlayerTransform, Vector3 offset)
    {
        if (Ammo > 0)
        {
            BulletLogic b = bullet;
            Instantiate(b, PlayerTransform.position + offset, PlayerTransform.rotation);
            Ammo--;
        }
        else
        {
            //Show that u need a reload on screen
        }
    }

    public void instantiateHomingBullet(Transform PlayerTransform, Vector3 offset)
    {
        if (Ammo > 0)
        {
            BulletLogic b = bullet;
            Instantiate(bullet, PlayerTransform.position + offset, PlayerTransform.rotation);
            Ammo--;
        }
        else
        {
            //Show that u need a reload on screen
        }
    }

    public void ReloadAmmo()
    {
        Ammo = TotalAmmo;
    }


    // Debug function
    [ContextMenu("Sparo Bullet Homing")]
    public void InstantiateHomingBullet()
    {
        if (Ammo > 0)
        {
            GameObject go = Instantiate(bulletPrefab, transform.position, transform.rotation); // Instantiate the bullet
            BulletLogic bullet = go.GetComponent<BulletLogic>();
            bullet.target = GameObject.FindGameObjectWithTag("Standard").transform;
            Debug.Log(bullet.target);
            StartCoroutine(bullet.WaitToEnableHoming());
        }
    }
}
