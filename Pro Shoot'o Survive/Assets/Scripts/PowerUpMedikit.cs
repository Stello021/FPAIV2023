using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMedikit : PowerUp
{
    public float hpRecovery;
    public override void PickUp(GameObject owner)
    {
        // owner.hp += 50;
        Debug.Log("Aumenta il parametro HP del player");
    }
}
