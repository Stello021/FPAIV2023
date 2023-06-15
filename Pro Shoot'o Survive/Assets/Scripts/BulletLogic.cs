using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float raycastDistance;
    public Vector3 dir = Vector3.forward;

    // homing variables
    [SerializeField] bool IsHoming;
    public Transform target;
    [SerializeField] float rotSpeed;

    public float DamageDealt;

    //[SerializeField] Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsHoming) HomingMovement();
        else StandardMovement();
    }

    private void Update()
    {
        //if (IsHoming)
        //{
        //    HomingMovement();
        //}
        //else
        //{
        //    StandardMovement();
        //}

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        //{
        //    if (hit.collider.tag == "Standard" || hit.collider.tag == "Ranged")
        //    {
        //        //enemy damage
        //        EnemyLogic enemy = hit.collider.gameObject.GetComponent<EnemyLogic>();
        //        enemy.currentHP -= DamageDealt;
        //    }
        //    Destroy(gameObject);
        //}
    }

    private void StandardMovement()
    {
        //rb.velocity = dir * bulletSpeed;
        
        transform.position += dir * bulletSpeed * Time.fixedDeltaTime;

    }

    private void HomingMovement() 
    {
        if (target != null)
        {
            Vector3 distance = target.position - transform.position;
            Quaternion rotationDir = Quaternion.LookRotation(distance);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, rotationDir, rotSpeed * Time.deltaTime);
            //rb.MoveRotation(newRotation);
            //rb.velocity = transform.forward * bulletSpeed * Time.fixedDeltaTime;

            transform.rotation = newRotation;
            transform.position += transform.forward * bulletSpeed * Time.fixedDeltaTime;
        }
        else
        {
            StandardMovement();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        //if (collision.collider.tag == "Standard" || collision.collider.tag == "Ranged")
        //{
        //    //enemy damage
        //    EnemyLogic enemy = collision.collider.gameObject.GetComponent<EnemyLogic>();
        //    enemy.currentHP -= DamageDealt;
        //}
        Debug.Log("Collision");
        Destroy(gameObject);
    }

    public IEnumerator WaitToEnableHoming()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        IsHoming = true;
    }

}
