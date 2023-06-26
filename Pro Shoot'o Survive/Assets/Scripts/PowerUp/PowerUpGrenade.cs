using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGrenade : PowerUp
{
    public override void PickUp(GameObject owner)
    {
        PlayerController player = owner.GetComponent<PlayerController>();
        player.Grenades++;
        Destroy(gameObject);
    }


}
