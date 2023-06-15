using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //PickUp();  da fare dentro al player questo, passandogli se stesso
        }
    }

    public virtual void PickUp(GameObject owner)
    {
        Debug.Log("Raccolto il powerup");
    }
}
