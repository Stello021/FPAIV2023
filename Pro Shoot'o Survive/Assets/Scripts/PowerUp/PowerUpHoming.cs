using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHoming : PowerUp
{
    public override void PickUp(GameObject owner)
    {
        owner.GetComponent<ThirdPersonController>().activeHoming = true;
        Destroy(gameObject);
    }

}
