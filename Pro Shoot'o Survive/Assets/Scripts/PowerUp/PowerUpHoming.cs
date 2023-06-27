using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHoming : PowerUp
{
    public override void PickUp(GameObject owner)
    {
        PlayerController player = owner.GetComponent<PlayerController>();
        player.ActivateHoming();
        Destroy(gameObject);
    }
}
