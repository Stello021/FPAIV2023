using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    [Header("Homing variables")]
    [SerializeField] public bool IsHoming;
    public Transform target;
    [SerializeField] float rotSpeed;
    [SerializeField] float speedBeforeHoming;
    [SerializeField] float normalSpeed;

    //protected override void FixedUpdate()
    //{
    //    if (IsHoming)
    //    {
    //        HomingMovement();
    //    }
    //    else
    //    {
    //        StandardMovement();
    //    }
    //}

    protected override void Update()
    {
        base.Update();

        if (IsHoming)
        {
            HomingMovement();
        }
        else
        {
            StandardMovement();
        }
    }

    private void HomingMovement()
    {
        if (target != null)
        {
            Vector3 distance = target.position - transform.position;

            if (distance.magnitude <= 20f)
            {
                rotSpeed = 50;
            }

            Quaternion rotation = Quaternion.LookRotation(distance);
            Quaternion newRotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
            transform.rotation = newRotation;
            transform.position += transform.forward * bulletSpeed * Time.deltaTime;
            //rb.MoveRotation(newRotation);
            //rb.velocity = transform.forward * bulletSpeed * Time.fixedDeltaTime;
        }
        else
        {
            StandardMovement();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Standard") || other.CompareTag("Ranged"))
        {
            //enemy damage
            EnemyLogic enemy = other.gameObject.GetComponent<EnemyLogic>();
            enemy.ReceiveDamage(DamageDealt);
            Debug.Log("Danni al nemico:" + DamageDealt);
        }
        
        base.OnTriggerEnter(other);
    }


    
    public IEnumerator WaitToEnableHoming()
    {
        bulletSpeed = speedBeforeHoming;
        dir = transform.forward;
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        IsHoming = true;
        bulletSpeed = normalSpeed;
    }

}
