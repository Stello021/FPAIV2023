using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletLogic : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float raycastDistance;

    public bool IsHoming;

    // homing variables
    //public Transform target;
    //public float searchRadius = 300;
    //public List<Transform> targetsInRange = new List<Transform>();
    //public LayerMask objmask;
    //public float rotSpeed;



    // Start is called before the first frame update
    void Start()
    {
        //SetTarget();
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
        transform.position += new Vector3(0, 0, bulletSpeed) * Time.deltaTime;
    }

    private void HomingMovement() 
    {
        //Vector3 distance = target.position - transform.position;
        //Debug.Log(distance.magnitude);
        //Quaternion rotationDir = Quaternion.LookRotation(distance);
        //Quaternion newRotation = Quaternion.Lerp(transform.rotation, rotationDir, rotSpeed * Time.deltaTime);
        //transform.rotation = newRotation;
    }

    // homing stuff
    //public void SetTarget()
    //{
    //    this.targetsInRange.Clear();
    //    Collider[] targetsInRange = Physics.OverlapSphere(transform.position, searchRadius, objmask);

    //    if (targetsInRange.Length > 0)
    //    {
    //        Transform nextTarget = null;
    //        Vector3 lowestDist = Vector3.zero;

    //        for (int i = 0; i < targetsInRange.Length; i++)
    //        {
    //            Transform possibleTarget = targetsInRange[i].transform;
    //            Vector3 distToTarget = possibleTarget.position - transform.position;
    //            if (i == 0)
    //            {
    //                lowestDist = distToTarget;
    //                nextTarget = possibleTarget;
    //            }
    //            else if (distToTarget.magnitude < lowestDist.magnitude)
    //            {
    //                lowestDist = distToTarget;
    //                nextTarget = possibleTarget;
    //            }
    //        }

    //        target = nextTarget;
    //        Debug.Log(target);
    //    }
    //}
}
