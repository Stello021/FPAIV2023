using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMedikit : PowerUp
{
    [SerializeField] float hpRecovery = 50;
    public override void PickUp(GameObject owner)
    {
        ThirdPersonController player = owner.GetComponent<ThirdPersonController>();
        player.hp += hpRecovery;
        //player.UpdateHPText();
        Destroy(gameObject);

    }
}
