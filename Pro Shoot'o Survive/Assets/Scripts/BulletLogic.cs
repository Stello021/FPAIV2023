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

    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (IsHoming) HomingMovement();
    //    else StandardMovement();
    //}

    private void Update()
    {
        if (IsHoming)
        {
            HomingMovement();
        }
        else
        {
            StandardMovement();
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
            Vector3 targetAdjust = new Vector3(target.position.x, target.position.y + 1.2f, target.position.z);
            Vector3 distance = targetAdjust - transform.position;
            Quaternion rotationDir = Quaternion.LookRotation(distance);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, rotationDir, rotSpeed * Time.deltaTime);
            transform.rotation = newRotation;
            transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        }
        else
        {
            StandardMovement();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    public IEnumerator WaitToEnableHoming()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        IsHoming = true;
    }

}
