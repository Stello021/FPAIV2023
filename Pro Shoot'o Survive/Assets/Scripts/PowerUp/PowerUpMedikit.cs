using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMedikit : PowerUp
{
    [SerializeField] float hpRecovery = 50;
    public override void PickUp(GameObject owner)
    {
        PlayerController player = owner.GetComponent<PlayerController>();
        player.hp += hpRecovery;
        //player.UpdateHPText();
        Destroy(gameObject);

    }
}
