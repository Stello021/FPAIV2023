using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHoming : PowerUp
{
    public Transform target;

    public float searchRadius;

    public float viewAngle;

    public LayerMask enemyMask;
    public LayerMask obstacleMask;
    public List<Transform> targetsInRange = new List<Transform>();


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
        owner.GetComponent<ThirdPersonController>().activeHoming = true;
        Debug.Log("Sei homing");
        Destroy(gameObject);

        // quando il player sparerà finchè activateHoming è true
        // passa al bullet il fatto che deve essere homing
    }

    public void Homing()
    {
        // il bullet generato si fisserà allora come target il nemico più vicino
        // entro un raggio ragionevole
        // e nell'update
        // Vector3 distance = target.position - bullet.position
        // calcoliamo il vettore dal bullet al nemico target ogni frame 
        // e poi lo rotiamo
        // Quaternion rotationDir = Quaternion.LookRotation(distance)
        // Quaternion newRotation = Quaternion.Lerp(bullet.rotation, rotationDir, rotSpeed * Time.deltaTime);
        // Quaternion.RotateTowards(bullet.rotation, rotationDir, rotSpeed * Time.deltaTime)
        // e lo spostiamo nella direzione di distance
    }

}
