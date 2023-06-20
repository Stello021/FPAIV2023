using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpArmor : PowerUp
{
    [SerializeField] float armorAmount = 25;
    public override void PickUp(GameObject owner)
    {
        PlayerLogic player = owner.GetComponent<PlayerLogic>();
        player.Armor += armorAmount;
        //player.UpdateArmorText();
        Destroy(gameObject);
    }
}
