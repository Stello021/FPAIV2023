using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpGrenade : PowerUp
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PickUp(GameObject owner)
    {
        //owner.GetComponent<ThirdPersonController>().grenades++;
        //Debug.Log("Granate: " + owner.GetComponent<ThirdPersonController>().grenades);
        Destroy(gameObject);
        
    }


}
