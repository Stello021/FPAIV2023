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
        // owner.activateHoming
        // quando il player sparerà finchè activateHoming è true
        // passa al bullet il fatto che deve essere homing
    }

    public void Homing()
    {
        // il bullet si fisserà allora come target il nemico più vicino
        // entro un raggio ragionevole
        // e nell'update
        // Vector3 distance = target.position - bullet.position
        // calcoliamo il vettore dal bullet al nemico target ogni frame 
        // e poi lo rotiamo
        // Quaternion rotationDir = Quaternion.LookRotation(distance)
        // Quaternion newRotation = Quaternion.Lerp(bullet.rotation, rotationDir, rotSpeed * Time.deltaTime);
        // Quaternion.RotateTowards(bullet.rotation, rotationDir, rotSpeed * Time.deltaTime)
    }

    public void SetTarget()
    {
        this.targetsInRange.Clear();
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, searchRadius, enemyMask);

        if (targetsInRange.Length > 0)
        {
            Transform nextTarget = null;
            Vector3 lowestDist = Vector3.zero;

            for (int i = 0; i < targetsInRange.Length; i++)
            {
                Transform possibleTarget = targetsInRange[i].transform;
                Vector3 distToTarget = possibleTarget.position - transform.position;
                if (i == 0)
                {
                    lowestDist = distToTarget;
                    nextTarget = possibleTarget;
                }
                else if (distToTarget.magnitude < lowestDist.magnitude)
                {
                    lowestDist = distToTarget;
                    nextTarget = possibleTarget;
                }
            }

            target = nextTarget;
        }



    }
}
