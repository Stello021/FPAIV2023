using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMedikit : PowerUp
{
    [SerializeField] float hpRecovery = 50;
    public override void PickUp(GameObject owner)
    {
        PlayerLogic player = owner.GetComponent<PlayerLogic>();
        player.HP += hpRecovery;
        Destroy(gameObject);
    }
}
