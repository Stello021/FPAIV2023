using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpArmor : PowerUp
{
    [SerializeField] float armourAmount = 25;
    public override void PickUp(GameObject owner)
    {
        PlayerController player = owner.GetComponent<PlayerController>();
        player.armour += armourAmount;
        //player.UpdateArmourText();
        Destroy(gameObject);
    }
}
