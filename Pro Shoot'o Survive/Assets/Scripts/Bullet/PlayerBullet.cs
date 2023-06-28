using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    [Header("Homing variables")]
    [SerializeField] public bool IsHoming;
    public Transform target;
    [SerializeField] float rotSpeed;
    [SerializeField] float homingTimer;

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
            homingTimer -= Time.deltaTime;
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
            if (homingTimer <= 0)
            {
                Vector3 distance = target.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(distance);
                Quaternion newRotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
                transform.rotation = newRotation;
            }

            transform.position += transform.forward * bulletSpeed * Time.deltaTime;
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
            //Debug.Log("Danni al nemico:" + DamageDealt);
        }

        Destroy(gameObject);
    }

}
