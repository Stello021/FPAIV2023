using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float raycastDistance;
    public Vector3 dir;

    public bool IsHoming;

    // homing variables
    public Transform target;
    //[SerializeField] float viewRadius = 300;
    //[SerializeField] LayerMask enemyMask;
    //[SerializeField] LayerMask obstacleMask;
    [SerializeField] float rotSpeed;
    //[SerializeField] float angleOfVision = 180;



    // Start is called before the first frame update
    void Start()
    {
        //dir = transform.forward;
        //SetTarget();
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (IsHoming) HomingMovement();
    //    else StandardMovement();
    //}

    private void Update()
    {
        if (!IsHoming)
        {
            StandardMovement();
        }
        else
        {
            HomingMovement();
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            if (hit.collider.tag == "Standard" || hit.collider.tag == "Ranged")
            {
                //enemy damage
                EnemyLogic enemy = hit.collider.gameObject.GetComponent<EnemyLogic>();
                enemy.currentHP = 0;
            }
            Destroy(gameObject);
        }
    }

    private void StandardMovement()
    {
        if (dir == Vector3.zero)
        {
            dir = transform.forward;
        }

        transform.localPosition += dir * bulletSpeed * Time.deltaTime;
    }

    private void HomingMovement() 
    {
        if (target != null)
        {
            Vector3 distance = target.position - transform.position;
            Quaternion rotationDir = Quaternion.LookRotation(distance);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, rotationDir, rotSpeed * Time.deltaTime);
            transform.position += distance.normalized * bulletSpeed * Time.deltaTime;
            transform.rotation = newRotation; 
        }
        else
        {
            StandardMovement();
        }
    }



    // set target for homing projectile (this method must be in the player)
    //public void SetTarget()
    //{
    //    Collider[] enemyTargets = Physics.OverlapSphere(transform.position, viewRadius, enemyMask);
    //    //Debug.Log("Detected enemies: " + enemyTargets.Length);
    //    if (enemyTargets.Length > 0)
    //    {
    //        Transform nextTarget = null;
    //        Vector3 lowestDist = Vector3.zero;

    //        for (int i = 0; i < enemyTargets.Length; i++)
    //        {
    //            Transform possibleTarget = enemyTargets[i].transform;
    //            Vector3 distToTarget = possibleTarget.position - transform.position;
    //            float angleToTarget = Vector3.Angle(transform.forward, distToTarget.normalized);
    //            //Debug.Log("ANGOLO: " + angleToTarget);
    //            // check if enemy is within angle of vision
    //            if (angleToTarget < angleOfVision * 0.5f)
    //            {
                    
    //                // check if enemy is NOT behind a wall
    //                if (!Physics.Raycast(transform.position, distToTarget.normalized, distToTarget.magnitude, obstacleMask))
    //                {
    //                    if (nextTarget == null)
    //                    {
    //                        lowestDist = distToTarget;
    //                        nextTarget = possibleTarget;
    //                    }
    //                    else if (distToTarget.magnitude < lowestDist.magnitude)
    //                    {
    //                        lowestDist = distToTarget;
    //                        nextTarget = possibleTarget;
    //                    }
    //                }
    //            }
    //        }

    //        target = nextTarget;
    //        Debug.Log(target);
    //    }
    //}
}
