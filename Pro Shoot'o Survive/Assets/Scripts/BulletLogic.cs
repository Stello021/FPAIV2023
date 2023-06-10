using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float raycastDistance;

    public bool IsHoming;

    // homing variables
    public Transform target;
    [SerializeField] float viewRadius = 300;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] LayerMask obstacleMask;
    [SerializeField] float rotSpeed;
    [SerializeField] float angleOfVision = 180;



    // Start is called before the first frame update
    void Start()
    {
        SetTarget();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsHoming) HomingMovement();
        else StandardMovement();
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward,out hit, raycastDistance))
        {
            if(hit.collider.tag == "Enemy")
            {
                //enemy damaged
            }
            Destroy(gameObject);
        }
    }

    private void StandardMovement()
    {
        transform.localPosition += transform.forward * bulletSpeed * Time.deltaTime;
    }

    private void HomingMovement() 
    {
        Vector3 distance = target.position - transform.position;
        Debug.Log(distance.magnitude);
        Quaternion rotationDir = Quaternion.LookRotation(distance);
        Quaternion newRotation = Quaternion.Lerp(transform.rotation, rotationDir, rotSpeed * Time.deltaTime);
        transform.position += distance.normalized * bulletSpeed * Time.deltaTime;
        transform.rotation = newRotation;
    }



    // set target for homing projectile (this method must be in the player)
    public void SetTarget()
    {
        Collider[] validTargets = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);

        if (validTargets.Length > 0)
        {
            Transform nextTarget = null;
            Vector3 lowestDist = Vector3.zero;

            for (int i = 0; i < validTargets.Length; i++)
            {
                Transform possibleTarget = validTargets[i].transform;
                Vector3 distToTarget = possibleTarget.position - transform.position;
                float angleToTarget = Vector3.Angle(transform.forward, distToTarget.normalized);

                // check if enemy is within angle of vision
                if (angleToTarget < angleOfVision * 0.5f)
                {
                    // check if enemy is NOT behind a wall
                    if (!Physics.Raycast(transform.position, distToTarget.normalized, distToTarget.magnitude, obstacleMask))
                    {
                        if (nextTarget == null)
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
                }
            }

            target = nextTarget;
            Debug.Log(target);
        }
    }
}
