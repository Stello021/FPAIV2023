using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] BulletLogic bullet;
    [SerializeField] int TotalAmmo;
    
    public int Ammo;

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
            b.IsHoming = true;
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
}
