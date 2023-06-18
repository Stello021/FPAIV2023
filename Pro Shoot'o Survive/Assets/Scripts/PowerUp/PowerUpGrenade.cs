using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGrenade : PowerUp
{
    public override void PickUp(GameObject owner)
    {
        owner.GetComponent<PlayerController>().Grenades++;
        Destroy(gameObject);
    }


}
