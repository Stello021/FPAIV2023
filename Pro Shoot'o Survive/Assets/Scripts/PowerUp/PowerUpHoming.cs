using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHoming : PowerUp
{
    public override void PickUp(GameObject owner)
    {
        owner.GetComponent<PlayerController>().activeHoming = true;
        Destroy(gameObject);
    }

}
