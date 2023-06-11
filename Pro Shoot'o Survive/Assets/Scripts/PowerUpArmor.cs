using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpArmor : PowerUp
{
    public float armorAmount;
    public override void PickUp(GameObject owner)
    {
        // owner.armor += 50;
        Debug.Log("Aumenta il parametro armor del player");
    }
}
