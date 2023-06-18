using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpArmor : PowerUp
{
    [SerializeField] float armorAmount = 25;
    public override void PickUp(GameObject owner)
    {
        PlayerController player = owner.GetComponent<PlayerController>();
        player.Armor += armorAmount;
        //player.UpdateArmorText();
        Destroy(gameObject);
    }
}
